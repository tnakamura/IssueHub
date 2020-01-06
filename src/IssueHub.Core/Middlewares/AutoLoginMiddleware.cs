using IssueHub.Actions;
using IssueHub.States;
using IssueHub.Models;
using ReduxSharp;

namespace IssueHub.Middlewares
{
    public sealed class AutoLoginMiddleware : IMiddleware<AppState>
    {
        public void Invoke<TAction>(IStore<AppState> store, IDispatcher next, TAction action)
        {
            if (action is AutoLogIn)
            {
                AutoLogin(store, next);
            }
            else
            {
                next.Invoke(action);
            }
        }

        void AutoLogin(IStore<AppState> store, IDispatcher next)
        {
            var account = store.State.ApiClients.AccountsRepository
                .ResolveAsync()
                .GetAwaiter()
                .GetResult();
            if (account != null)
            {
                next.Invoke(new UserLoaded(new IssueHubUser(account.Login)));
                next.Invoke(new OAuthLoggedIn(account.AccessToken));
            }
        }
    }
}
