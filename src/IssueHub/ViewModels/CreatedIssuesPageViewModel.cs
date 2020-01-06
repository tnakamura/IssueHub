using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
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
    public sealed class CreatedIssuesPageViewModel : PageViewModel
    {
        public IStore<AppState> Store { get; }

        public CreatedIssuesPageViewModel(IStore<AppState> store)
            : base()
        {
            Store = store;
            PageTitle = R.Created;
            Issues = new ObservableCollection<Issue>(store.State.Issues.Issues);

            ShowClosedIssues = new ReactivePropertySlim<bool>(
                store.State.Issues.ShowAll);

            InitializeCommand = new AsyncReactiveCommand()
                .WithSubscribe(InitializeAsync, AddDisposable);
            RefreshCommand = new AsyncReactiveCommand()
                .WithSubscribe(RefreshAsync, AddDisposable);
            ToggleClosedIssuesCommand = new AsyncReactiveCommand()
                .WithSubscribe(ToggleClosedIssuesAsync, AddDisposable);

            AddDisposable(
                store.Select(x => x.Issues)
                    .DistinctUntilChanged(x => x.Issues)
                    .Subscribe(x =>
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            IsInitialized.Value = x.Initialized;
                            ShowClosedIssues.Value = x.ShowAll;
                            Issues.Merge(x.Issues, IssueEqualityComparer.Default);
                        });
                    }));
        }

        public IReactiveProperty<bool> ShowClosedIssues { get; }

        public ObservableCollection<Issue> Issues { get; }

        public IReactiveProperty<bool> IsInitialized { get; } = new ReactivePropertySlim<bool>(false);

        public IReactiveProperty<bool> IsRefreshing { get; } = new ReactivePropertySlim<bool>(false);

        public IReactiveProperty<bool> IsLoading { get; } = new ReactivePropertySlim<bool>(false);

        public AsyncReactiveCommand InitializeCommand { get; }

        public AsyncReactiveCommand RefreshCommand { get; }

        public AsyncReactiveCommand ToggleClosedIssuesCommand { get; }

        async Task InitializeAsync()
        {
            try
            {
                IsLoading.Value = true;
                await Store.DispatchAsync(
                    IssueActions.LoadCreatedIssues(
                        Store.State.Issues.ShowAll));
            }
            finally
            {
                IsLoading.Value = false;
            }
        }

        Task RefreshAsync() =>
            RefreshCoreAsync(ShowClosedIssues.Value);

        Task ToggleClosedIssuesAsync() =>
            RefreshCoreAsync(!ShowClosedIssues.Value);

        async Task RefreshCoreAsync(bool showAll)
        {
            try
            {
                IsRefreshing.Value = true;
                await Store.DispatchAsync(
                    IssueActions.LoadCreatedIssues(
                        showAll));
            }
            finally
            {
                IsRefreshing.Value = false;
            }
        }
    }
}
