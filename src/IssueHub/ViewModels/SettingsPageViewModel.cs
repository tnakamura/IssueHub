using System.Threading.Tasks;
using IssueHub.Actions;
using IssueHub.States;
using Reactive.Bindings;
using ReduxSharp;

namespace IssueHub.ViewModels
{
    public class SettingsPageViewModel : PageViewModel
    {
        readonly IStore<AppState> store;

        public SettingsPageViewModel(IStore<AppState> store)
            : base()
        {
            this.store = store;

            LogoutCommand = new AsyncReactiveCommand()
                .WithSubscribe(LogoutAsync);
        }

        public AsyncReactiveCommand LogoutCommand { get; }

        async Task LogoutAsync()
        {
            await store.DispatchAsync(LoginActions.Logout());
        }
    }
}
