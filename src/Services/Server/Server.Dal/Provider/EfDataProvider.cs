using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Dex.Ef.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using Server.Dal.Base;
using Server.Dal.Contract;

namespace Server.Dal.Provider
{
    public class EfDataProvider : IDataProvider
    {
        private readonly ResetDbContext _dbContext;
        private readonly IDataExceptionManager _exceptionManager;

        public EfDataProvider(ResetDbContext connection, IDataExceptionManager exceptionManager)
        {
            _dbContext = connection ?? throw new ArgumentNullException(nameof(connection));
            _exceptionManager = exceptionManager ?? throw new ArgumentNullException(nameof(exceptionManager));
        }

        public TransactionScope Transaction()
        {
            return Transaction(IsolationLevel.ReadCommitted);
        }

        public TransactionScope Transaction(IsolationLevel isolationLevel)
        {
            var ambientLevel = System.Transactions.Transaction.Current?.IsolationLevel;
            var txOptions = new TransactionOptions
            {
                IsolationLevel = ambientLevel == null
                    ? isolationLevel
                    : (IsolationLevel) Math.Min((int) ambientLevel, (int) isolationLevel)
            };
            return new TransactionScope(TransactionScopeOption.Required, txOptions,
                TransactionScopeAsyncFlowOption.Enabled);
        }

        public IQueryable<T> Get<T>() where T : class
        {
            return _dbContext.Set<T>().AsQueryable().AsNoTracking();
        }

        public IQueryable<T> Get<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return _dbContext.Set<T>().AsQueryable().Where(predicate).AsNoTracking();
        }

        public Task<T> Insert<T>(T entity, CancellationToken cancellationToken = default) where T : class
        {
            return ExecuteCommand(async state =>
            {
                Add(state.entity);
                await _dbContext.SaveChangesAsync(state.cancellationToken).ConfigureAwait(false);
                return state.entity;
            }, (entity, cancellationToken));
        }

        public Task BatchInsert<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default)
            where T : class
        {
            return ExecuteCommand(state =>
            {
                foreach (var entity in state.entities)
                {
                    Add(entity);
                }

                return _dbContext.SaveChangesAsync(state.cancellationToken);
            }, (entities, cancellationToken));
        }

        public Task<T> Update<T>(T entity, bool ignoreSystemProps = true, CancellationToken cancellationToken = default)
            where T : class
        {
            return ExecuteCommand(async state =>
            {
                UpdateEntity(state.entity);
                await _dbContext.SaveChangesAsync(state.cancellationToken).ConfigureAwait(false);
                return state.entity;
            }, (entity, cancellationToken));
        }

        public Task BatchUpdate<T>(IEnumerable<T> entities, bool ignoreSystemProps = true,
            CancellationToken cancellationToken = default) where T : class
        {
            return ExecuteCommand(state =>
            {
                foreach (var entity in state.entities)
                {
                    UpdateEntity(entity);
                }

                return _dbContext.SaveChangesAsync(state.cancellationToken);
            }, (entities, cancellationToken));
        }

        public Task Delete<T>(T entity, CancellationToken cancellationToken = default) where T : class
        {
            return ExecuteCommand(state =>
            {
                _dbContext.Set<T>().Remove(state.entity);
                return _dbContext.SaveChangesAsync(state.cancellationToken);
            }, (entity, cancellationToken));
        }

        public Task BatchDelete<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default)
            where T : class
        {
            return ExecuteCommand(state =>
            {
                _dbContext.Set<T>().RemoveRange(state.entities);
                return _dbContext.SaveChangesAsync(state.cancellationToken);
            }, (entities, cancellationToken));
        }

        public Task DeleteById<T, TKey>(TKey id, CancellationToken cancellationToken = default)
            where T : class, IEntity<TKey> 
            where TKey : IComparable
        {
            return ExecuteCommand(async state =>
            {
                var entity = await _dbContext.Set<T>()
                    .Where(t => state.id.Equals(t.Id))
                    .SingleAsync(state.cancellationToken)
                    .ConfigureAwait(false);
                _dbContext.Set<T>().Remove(entity);
                return await _dbContext.SaveChangesAsync(state.cancellationToken).ConfigureAwait(false);
            }, (id, cancellationToken));
        }

        public Task BatchDeleteByIds<T, TKey>(IEnumerable<TKey> ids, CancellationToken cancellationToken = default)
            where T : class, IEntity<TKey> 
            where TKey : IComparable
        {
            return ExecuteCommand(async state =>
            {
                var entity = await _dbContext.Set<T>()
                    .Where(t => state.ids.Contains(t.Id))
                    .ToArrayAsync(state.cancellationToken)
                    .ConfigureAwait(false);
                _dbContext.Set<T>().RemoveRange(entity);
                return await _dbContext.SaveChangesAsync(state.cancellationToken).ConfigureAwait(false);
            }, (ids, cancellationToken));
        }

        public Task SetDelete<T, TKey>(TKey id, CancellationToken cancellationToken = default)
            where T : class, IDeletable, IEntity<TKey> 
            where TKey : IComparable
        {
            return ExecuteCommand(async state =>
            {
                var entity = await _dbContext
                    .Set<T>().Where(t => state.id.Equals(t.Id))
                    .SingleAsync(state.cancellationToken)
                    .ConfigureAwait(false);
                entity.DeletedUtc = DateTime.UtcNow;
                UpdateEntity(entity);
                return await _dbContext.SaveChangesAsync(state.cancellationToken).ConfigureAwait(false);
            }, (id, cancellationToken));
        }

        public Task BatchSetDelete<T, TKey>(IEnumerable<TKey> ids, CancellationToken cancellationToken = default)
            where T : class, IDeletable, IEntity<TKey>
            where TKey : IComparable
        {
            return ExecuteCommand(async state =>
            {
                var entities = await _dbContext.Set<T>()
                    .Where(t => state.ids.Contains(t.Id))
                    .ToArrayAsync(state.cancellationToken)
                    .ConfigureAwait(false);

                foreach (var entity in entities)
                {
                    entity.DeletedUtc = DateTime.UtcNow;
                    UpdateEntity(entity);
                }

                return await _dbContext.SaveChangesAsync(state.cancellationToken).ConfigureAwait(false);
            }, (ids, cancellationToken));
        }

        void IDataProvider.Reset()
        {
            _dbContext.Reset();
        }

        private void Add<T>(T entity) where T : class
        {
            if (entity is ICreatedUtc createdUtc)
            {
                createdUtc.CreatedUtc = DateTime.UtcNow;
            }

            if (entity is IUpdatedUtc updatedUtc)
            {
                updatedUtc.UpdatedUtc = DateTime.UtcNow;
            }

            _dbContext.Set<T>().Add(entity);
        }

        private void UpdateEntity<T>(T entity) where T : class
        {
            if (entity is IUpdatedUtc updatedUtc)
            {
                updatedUtc.UpdatedUtc = DateTime.UtcNow;
            }

            var entityEntry = _dbContext.Entry(entity);
            entityEntry.State = EntityState.Modified;
        }

        private async Task<T> ExecuteCommand<T, TState>(Func<TState, Task<T>> func, TState state)
        {
            try
            {
                return await func(state).ConfigureAwait(false);
            }
            catch (System.Exception exception)
            {
                throw _exceptionManager.Normalize(exception);
            }
        }
    }
}