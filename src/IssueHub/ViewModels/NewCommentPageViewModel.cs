using System.Reactive.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using IssueHub.Actions;
using IssueHub.States;
using Reactive.Bindings;
using ReduxSharp;
using Xamarin.Forms;
using R = IssueHub.Properties.Resources;

namespace IssueHub.ViewModels
{
    public sealed class NewCommentPageViewModel : PageViewModel
    {
        readonly IStore<AppState> store;

        readonly IUserDialogs dialogs;

        public NewCommentPageViewModel(
            IStore<AppState> store,
            IUserDialogs dialogs)
            : base()
        {
            this.store = store;
            this.dialogs = dialogs;

            Body = new ReactivePropertySlim<string>(string.Empty);

            CancelCommand = new Command(Cancel);
            CommentCommand = new AsyncReactiveCommand(Body.Select(x => !string.IsNullOrEmpty(x)))
                .WithSubscribe(CommentAsync, AddDisposable);
        }

        public IReactiveProperty<string> Body { get; }

        public Command CancelCommand { get; }

        public AsyncReactiveCommand CommentCommand { get; }

        async Task CommentAsync()
        {
            using (var dialog = dialogs.Loading(title: R.Sending, maskType: MaskType.Black))
            {
                dialog.Show();
                await store.DispatchAsync(
                    IssueCommentActions.PostComment(
                        store.State.Issues.Repository.Owner.Login,
                        store.State.Issues.Repository.Name,
                        store.State.IssueComments.Issue.Number,
                        Body.Value));
                dialog.Hide();
            }
            Body.Value = string.Empty;
            RaiseRequestPopModal();
        }

        void Cancel()
        {
            RaiseRequestPopModal();
        }
    }
}
