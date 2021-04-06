using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Server.Dal.Contract;

namespace Server.Dal.Provider
{
    internal class SafeExecuteProvider : ISafeExecuteProvider
    {
        private readonly IDataExceptionManager _dataExceptionManager;

        public SafeExecuteProvider(IDataExceptionManager dataExceptionManager)
        {
            _dataExceptionManager =
                dataExceptionManager ?? throw new ArgumentNullException(nameof(dataExceptionManager));
        }

        public Task<T> SafeExecuteAsync<T>(
            IDataProvider provider,
            Func<IDataProvider, CancellationToken, Task<T>> func,
            IsolationLevel level = IsolationLevel.RepeatableRead,
            int retryCount = 3,
            CancellationToken token = default)
        {
            return SafeExecuteAsync(WrapperTaskT, func, provider, level, retryCount, token);
        }

        public Task SafeExecuteAsync(
            IDataProvider provider,
            Func<IDataProvider, CancellationToken, Task> func,
            IsolationLevel level = IsolationLevel.RepeatableRead,
            int retryCount = 3,
            CancellationToken token = default)
        {
            return SafeExecuteAsync(WrapperTask<VoidStruct>, func, provider, level, retryCount, token);
        }

        private async Task<T> SafeExecuteAsync<T, TArg>(
            Func<IDataProvider, TArg, CancellationToken, Task<T>> func,
            TArg arg,
            IDataProvider provider,
            IsolationLevel level = IsolationLevel.RepeatableRead,
            int retryCount = 3,
            CancellationToken token = default)
        {
            T result;
            var count = 0;
            while (true)
            {
                try
                {
                    using var transaction = provider.Transaction(level);
                    result = await func(provider, arg, token).ConfigureAwait(false);
                    transaction.Complete();
                    break;
                }
                catch (System.Exception exception)
                {
                    Reset(provider);

                    if (_dataExceptionManager.IsRepeatAction(exception) && ++count >= retryCount) throw;

                    await Task.Delay(TimeSpan.FromSeconds(1), token).ConfigureAwait(false);
                }
            }

            return result;
        }

        private static void Reset(IDataProvider provider)
        {
            provider.Reset();
        }

        [StructLayout(LayoutKind.Auto)]
        private readonly struct VoidStruct
        {
        }

        private static async Task<T> WrapperTask<T>(
            IDataProvider dp,
            Func<IDataProvider, CancellationToken, Task> f,
            CancellationToken t)
        {
            await f(dp, t);
            return default;
        }

        private static Task<T> WrapperTaskT<T>(
            IDataProvider dp,
            Func<IDataProvider, CancellationToken, Task<T>> f,
            CancellationToken t)
        {
            return f(dp, t);
        }
    }
}