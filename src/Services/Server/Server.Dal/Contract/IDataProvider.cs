using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Dex.Ef.Contracts.Entities;

namespace Server.Dal.Contract
{
    public interface IDataProvider
    {
        TransactionScope Transaction();
        TransactionScope Transaction(IsolationLevel isolationLevel);

        IQueryable<T> Get<T>() where T : class;
        IQueryable<T> Get<T>(Expression<Func<T, bool>> predicate) where T : class;

        Task<T> Insert<T>(T entity, CancellationToken cancellationToken = default) where T : class;
        Task BatchInsert<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default) where T : class;

        Task<T> Update<T>(T entity, bool ignoreSystemProps = true, CancellationToken cancellationToken = default) where T : class;
        Task BatchUpdate<T>(IEnumerable<T> entities, bool ignoreSystemProps = true, CancellationToken cancellationToken = default) where T : class;

        Task Delete<T>(T entity, CancellationToken cancellationToken = default) where T : class;
        Task BatchDelete<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default) where T : class;

        Task DeleteById<T, TKey>(TKey id, CancellationToken cancellationToken = default) where T : class, IEntity<TKey> where TKey : IComparable;
        Task BatchDeleteByIds<T, TKey>(IEnumerable<TKey> ids, CancellationToken cancellationToken = default) where T : class, IEntity<TKey> where TKey : IComparable;

        Task SetDelete<T, TKey>(TKey id, CancellationToken cancellationToken = default) where T : class, IDeletable, IEntity<TKey> where TKey : IComparable;
        Task BatchSetDelete<T, TKey>(IEnumerable<TKey> ids, CancellationToken cancellationToken = default) where T : class, IDeletable, IEntity<TKey> where TKey : IComparable;

        internal void Reset();
    }
}