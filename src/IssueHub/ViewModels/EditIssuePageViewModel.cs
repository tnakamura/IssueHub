using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using IssueHub.Actions;
using IssueHub.States;
using Reactive.Bindings;
using ReduxSharp;
using Xamarin.Essentials;
using Xamarin.Forms;
using R = IssueHub.Properties.Resources;

namespace IssueHub.ViewModels
{
    public class EditIssuePageViewModel : PageViewModel
    {
        readonly IUserDialogs dialogs;

        readonly ReactivePropertySlim<string> labelsText = new ReactivePropertySlim<string>();

        readonly ReactivePropertySlim<string> assigneesText = new ReactivePropertySlim<string>();

        public EditIssuePageViewModel(
            IStore<AppState> store,
            IUserDialogs dialogs)
            : base()
        {
            this.dialogs = dialogs;

            Store = store;
            PageTitle = store.State.IssueForm.EditingIssue != null
                ? string.Format(R.EditIssueNumberFormat, store.State.IssueForm.EditingIssue.Number)
                : R.NewIssue;
            LabelsText = labelsText.ToReadOnlyReactivePropertySlim();
            AssigneesText = assigneesText.ToReadOnlyReactivePropertySlim();
            Title = new ReactivePropertySlim<string>(store.State.IssueForm.Title);
            Body = new ReactivePropertySlim<string>(store.State.IssueForm.Body);

            CancelCommand = new Command(Cancel);
            SubmitCommand = new AsyncReactiveCommand(Title.Select(x => !string.IsNullOrEmpty(x)))
                .WithSubscribe(Submit, AddDisposable);

            AddDisposable(
                Title.Subscribe(x =>
                {
                    store.Dispatch(new ChangeIssueTitle(x));
                }));
            AddDisposable(
                store.Select(x => x.IssueForm)
                    .DistinctUntilChanged()
                    .Subscribe(x =>
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            Body.Value = x.Body;
                            assigneesText.Value = string.Join(
                                ", ",
                                x.Assignees.Select(y => y.Login));
                            labelsText.Value = string.Join(
                                ", ",
                                x.Labels.Select(y => y.Name));
                        });
                    }));
        }

        public IStore<AppState> Store { get; }

        public IReactiveProperty<string> Title { get; }

        public IReactiveProperty<string> Body { get; }

        public IReadOnlyReactiveProperty<string> AssigneesText { get; }

        public IReadOnlyReactiveProperty<string> LabelsText { get; }

        public Command CancelCommand { get; }

        public AsyncReactiveCommand SubmitCommand { get; }

        void Cancel()
        {
            RaiseRequestPopModal();
        }

        async Task Submit()
        {
            using (var dialog = dialogs.Loading(title: R.Sending, maskType: MaskType.Black))
            {
                dialog.Show();
                if (Store.State.IssueForm.IsNewIssue)
                {
                    await Store.DispatchAsync(
                        IssueFormActions.CreateIssue(
                            owner: Store.State.Issues.Repository.Owner.Login,
                            name: Store.State.Issues.Repository.Name,
                            title: Title.Value,
                            body: Body.Value,
                            assignees: Store.State.IssueForm.Assignees,
                            labels: Store.State.IssueForm.Labels));
                }
                else
                {
                    await Store.DispatchAsync(
                        IssueFormActions.UpdateIssue(
                            owner: Store.State.Issues.Repository.Owner.Login,
                            name: Store.State.Issues.Repository.Name,
                            issue: Store.State.IssueForm.EditingIssue,
                            title: Title.Value,
                            body: Body.Value,
                            assignees: Store.State.IssueForm.Assignees,
                            labels: Store.State.IssueForm.Labels));
                }
                dialog.Hide();
            }
            RaiseRequestPopModal();
        }
    }
}
