using System;
using System.Threading.Tasks;
using ReduxSharp;

namespace IssueHub
{
    public static class StoreEx
    {
        public static async ValueTask DispatchAsync<TState>(
            this IStore<TState> store,
            AsyncActionCreator<TState> asyncActionCreator)
        {
            if (asyncActionCreator == null) throw new ArgumentNullException(nameof(asyncActionCreator));
            await asyncActionCreator(store).ConfigureAwait(false);
        }
    }

    public delegate ValueTask AsyncActionCreator<TState>(IStore<TState> store);
}
