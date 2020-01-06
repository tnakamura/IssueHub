using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using IssueHub.Actions;
using IssueHub.States;
using IssueHub.Utils;
using Octokit;
using Reactive.Bindings;
using ReduxSharp;
using Xamarin.Essentials;
using R = IssueHub.Properties.Resources;

namespace IssueHub.ViewModels
{
    public sealed class IssueCommentsPageViewModel : PageViewModel
    {
        readonly IUserDialogs dialogs;

        public IStore<AppState> Store { get; }

        public IssueCommentsPageViewModel(
            IStore<AppState> store,
            IUserDialogs dialogs)
            : base()
        {
            this.dialogs = dialogs;

            Store = store;
            PageTitle = $"#{store.State.IssueComments.Issue.Number}";
            IssueComments = new ObservableCollection<IssueCommentViewModelBase>(
                ToViewModels(store.State.IssueComments));

            InitializeCommand = new AsyncReactiveCommand()
                .WithSubscribe(InitializeAsync, AddDisposable);
            RefreshCommand = new AsyncReactiveCommand()
                .WithSubscribe(RefreshAsync, AddDisposable);
            CloseIssueCommand = new AsyncReactiveCommand()
                .WithSubscribe(CloseIssueAsync, AddDisposable);
            ReopenIssueCommand = new AsyncReactiveCommand()
                .WithSubscribe(ReopenIssueAsync, AddDisposable);

            AddDisposable(
                store.Select(x => x.IssueComments)
                    .DistinctUntilChanged(x => (x.Issue, x.Comments, x.Initialized))
                    .Subscribe(x =>
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            IsInitialized.Value = x.Initialized;
                            IssueComments.Merge(
                                ToViewModels(x),
                                IssueCommentEqualityComparer.Default);
                        });
                    }));
        }

        static IEnumerable<IssueCommentViewModelBase> ToViewModels(IssueCommentsState state)
        {
            yield return new IssueTitleViewModel(state.Issue);
            yield return new IssueAssigneesViewModel(state.Issue);
            yield return new IssueLabelsViewModel(state.Issue);
            yield return new IssueBodyViewModel(state.Issue);
            foreach (var comment in state.Comments)
            {
                yield return new IssueCommentViewModel(comment);
            }
        }

        public AsyncReactiveCommand InitializeCommand { get; }

        public AsyncReactiveCommand RefreshCommand { get; }

        public AsyncReactiveCommand CloseIssueCommand { get; }

        public AsyncReactiveCommand ReopenIssueCommand { get; }

        public IReactiveProperty<bool> IsInitialized { get; } = new ReactivePropertySlim<bool>(false);

        public IReactiveProperty<bool> IsRefreshing { get; } = new ReactivePropertySlim<bool>(false);

        public IReactiveProperty<bool> IsLoading { get; } = new ReactivePropertySlim<bool>(false);

        public Issue Issue => Store.State.IssueComments.Issue;

        public ObservableCollection<IssueCommentViewModelBase> IssueComments { get; }

        Task InitializeAsync() => LoadCoreAsync(IsLoading);

        Task RefreshAsync() => LoadCoreAsync(IsRefreshing);

        async Task LoadCoreAsync(IReactiveProperty<bool> isBusy)
        {
            try
            {
                isBusy.Value = true;
                await Store.DispatchAsync(
                    IssueCommentActions.LoadIssueComments(
                        Store.State.Issues.Repository.Owner.Login,
                        Store.State.Issues.Repository.Name,
                        Store.State.IssueComments.Issue.Number));
            }
            finally
            {
                isBusy.Value = false;
            }
        }

        async Task CloseIssueAsync()
        {
            using (var dialog = dialogs.Loading(title: R.Closing, maskType: MaskType.Black))
            {
                dialog.Show();
                await Store.DispatchAsync(
                    IssueActions.CloseIssue(
                        Store.State.Issues.Repository.Owner.Login,
                        Store.State.Issues.Repository.Name,
                        Store.State.IssueComments.Issue));
                dialog.Hide();
            }
        }

        async Task ReopenIssueAsync()
        {
            using (var dialog = dialogs.Loading(title: R.Opening, maskType: MaskType.Black))
            {
                dialog.Show();
                await Store.DispatchAsync(
                    IssueActions.OpenIssue(
                        Store.State.Issues.Repository.Owner.Login,
                        Store.State.Issues.Repository.Name,
                        Store.State.IssueComments.Issue));
                dialog.Hide();
            }
        }
    }

    public abstract class IssueCommentViewModelBase : ViewModelBase
    {
        protected IssueCommentViewModelBase()
            : base()
        {
        }
    }

    public sealed class IssueCommentViewModel : IssueCommentViewModelBase
    {
        readonly IssueComment comment;

        public IssueCommentViewModel(IssueComment comment)
            : base()
        {
            this.comment = comment;
        }

        public int Id => comment.Id;

        public string Body => comment.Body;

        public User User => comment.User;

        public DateTimeOffset CreatedAt => comment.CreatedAt;

        public DateTimeOffset? UpdatedAt => comment.UpdatedAt;
    }

    public sealed class IssueBodyViewModel : IssueCommentViewModelBase
    {
        readonly Issue issue;

        public IssueBodyViewModel(Issue issue)
            : base()
        {
            this.issue = issue;
        }

        public int Id => issue.Id;

        public string Body => issue.Body;

        public User User => issue.User;

        public DateTimeOffset CreatedAt => issue.CreatedAt;

        public DateTimeOffset? UpdatedAt => issue.UpdatedAt;
    }

    public sealed class IssueTitleViewModel : IssueCommentViewModelBase
    {
        readonly Issue issue;

        public IssueTitleViewModel(Issue issue)
            : base()
        {
            this.issue = issue;
        }

        public int Id => issue.Id;

        public string Title => issue.Title;

        public StringEnum<ItemState> State => issue.State;

        public DateTimeOffset? UpdatedAt => issue.UpdatedAt;
    }

    public sealed class IssueAssigneesViewModel : IssueCommentViewModelBase
    {
        readonly Issue issue;

        public IssueAssigneesViewModel(Issue issue)
            : base()
        {
            this.issue = issue;
            Assignees = string.Join(", ", issue.Assignees.Select(x => x.Login));
        }

        public int Id => issue.Id;

        public string Assignees { get; }

        public DateTimeOffset? UpdatedAt => issue.UpdatedAt;
    }

    public sealed class IssueLabelsViewModel : IssueCommentViewModelBase
    {
        readonly Issue issue;

        public IssueLabelsViewModel(Issue issue)
            : base()
        {
            this.issue = issue;
            Labels = string.Join(", ", issue.Labels.Select(x => x.Name));
        }

        public int Id => issue.Id;

        public string Labels { get; }

        public DateTimeOffset? UpdatedAt => issue.UpdatedAt;
    }

    internal sealed class IssueCommentEqualityComparer : IEqualityComparer<IssueCommentViewModelBase>
    {
        internal static readonly IssueCommentEqualityComparer Default = new IssueCommentEqualityComparer();

        IssueCommentEqualityComparer() { }

        public bool Equals(IssueCommentViewModelBase x, IssueCommentViewModelBase y)
        {
            if (x is IssueCommentViewModel commentX && y is IssueCommentViewModel commentY)
            {
                return commentX.Id == commentY.Id &&
                    commentX.UpdatedAt == commentY.UpdatedAt;
            }
            else if (x is IssueTitleViewModel titleX && y is IssueTitleViewModel titleY)
            {
                return titleX.Id == titleY.Id &&
                    titleX.UpdatedAt == titleY.UpdatedAt;
            }
            else if (x is IssueBodyViewModel bodyX && y is IssueBodyViewModel bodyY)
            {
                return bodyX.Id == bodyY.Id &&
                    bodyX.UpdatedAt == bodyY.UpdatedAt;
            }
            else if (x is IssueAssigneesViewModel assigneesX && y is IssueAssigneesViewModel assigneesY)
            {
                return assigneesX.Id == assigneesY.Id &&
                    assigneesX.UpdatedAt == assigneesY.UpdatedAt;
            }
            else if (x is IssueLabelsViewModel labelsX && y is IssueLabelsViewModel labelsY)
            {
                return labelsX.Id == labelsY.Id &&
                    labelsX.UpdatedAt == labelsY.UpdatedAt;
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(IssueCommentViewModelBase obj)
        {
            return obj.GetHashCode();
        }
    }
}
