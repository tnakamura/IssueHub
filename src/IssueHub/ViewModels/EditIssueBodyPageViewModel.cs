using IssueHub.Actions;
using IssueHub.States;
using Reactive.Bindings;
using ReduxSharp;
using Xamarin.Forms;

namespace IssueHub.ViewModels
{
    public sealed class EditIssueBodyPageViewModel : PageViewModel
    {
        readonly IStore<AppState> store;

        public EditIssueBodyPageViewModel(IStore<AppState> store)
            : base()
        {
            this.store = store;
            Body = new ReactivePropertySlim<string>(store.State.IssueForm.Body);
            ApplyCommand = new Command(Apply);
            CancelCommand = new Command(Cancel);
        }

        public IReactiveProperty<string> Body { get; }

        public Command ApplyCommand { get; }

        public Command CancelCommand { get; }

        void Apply()
        {
            store.Dispatch(new ChangeIssueBody(Body.Value));
            RaiseRequestPopModal();
        }

        void Cancel()
        {
            RaiseRequestPopModal();
        }
    }
}
