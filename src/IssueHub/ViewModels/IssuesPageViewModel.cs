using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Octokit;
using Reactive.Bindings;
using ReduxSharp;
using ValueTaskSupplement;
using Xamarin.Essentials;
using IssueHub.Utils;
using IssueHub.States;
using IssueHub.Actions;

namespace IssueHub.ViewModels
{
    public sealed class IssuesPageViewModel : PageViewModel
    {
        public IStore<AppState> Store { get; }

        public IssuesPageViewModel(IStore<AppState> store)
            : base()
        {
            Store = store;
            PageTitle = store.State.Issues.Repository.Name;
            Issues = new ObservableCollection<Issue>(store.State.Issues.Issues);

            ShowClosedIssues = new ReactivePropertySlim<bool>(
                store.State.Issues.ShowAll);

            InitializeCommand = new AsyncReactiveCommand()
                .WithSubscribe(InitializeAsync, AddDisposable);
            RefreshCommand = new AsyncReactiveCommand()
                .WithSubscribe(RefreshAsync, AddDisposable);
            ToggleClosedIssuesCommand = new AsyncReactiveCommand()
                .WithSubscribe(ToggleClosedIssuesAsync, AddDisposable);
            ToggleFavoriteCommand = new AsyncReactiveCommand()
                .WithSubscribe(ToggleFavoriteAsync, AddDisposable);

            AddDisposable(
                store.Select(x => x.Issues)
                    .DistinctUntilChanged(x => x.Issues)
                    .Subscribe(x =>
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            IsInitialized.Value = x.Initialized;
                            IsFavorite.Value = x.IsFavorite;
                            ShowClosedIssues.Value = x.ShowAll;
                            Issues.Merge(x.Issues, IssueEqualityComparer.Default);
                        });
                    }));
        }

        public IReactiveProperty<bool> ShowClosedIssues { get; }

        public ObservableCollection<Issue> Issues { get; }

        public IReactiveProperty<bool> IsInitialized { get; } = new ReactivePropertySlim<bool>(false);

        public IReactiveProperty<bool> IsFavorite { get; } = new ReactivePropertySlim<bool>(false);

        public IReactiveProperty<bool> IsRefreshing { get; } = new ReactivePropertySlim<bool>(false);

        public IReactiveProperty<bool> IsLoading { get; } = new ReactivePropertySlim<bool>(false);

        public AsyncReactiveCommand InitializeCommand { get; }

        public AsyncReactiveCommand RefreshCommand { get; }

        public AsyncReactiveCommand ToggleClosedIssuesCommand { get; }

        public AsyncReactiveCommand ToggleFavoriteCommand { get; }

        async Task InitializeAsync()
        {
            try
            {
                IsLoading.Value = true;
                await ValueTaskEx.WhenAll(
                    Store.DispatchAsync(
                        IssueActions.LoadRepositoryIssues(
                            Store.State.Issues.Repository.Owner.Login,
                            Store.State.Issues.Repository.Name,
                            Store.State.Issues.ShowAll)),
                    Store.DispatchAsync(
                        FavoriteActions.LoadFavorite(
                            Store.State.Issues.Repository.Owner.Login,
                            Store.State.Issues.Repository.Name)));
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
                    IssueActions.LoadRepositoryIssues(
                        Store.State.Issues.Repository.Owner.Login,
                        Store.State.Issues.Repository.Name,
                        showAll));
            }
            finally
            {
                IsRefreshing.Value = false;
            }
        }

        async Task ToggleFavoriteAsync()
        {
            if (Store.State.Issues.IsFavorite)
            {
                await Store.DispatchAsync(
                    FavoriteActions.RemoveFromFavorites(
                        Store.State.Issues.Repository.Owner.Login,
                        Store.State.Issues.Repository.Name));
            }
            else
            {
                await Store.DispatchAsync(
                    FavoriteActions.AddToFavorites(
                        Store.State.Issues.Repository));
            }
        }
    }

    internal sealed class IssueEqualityComparer : IEqualityComparer<Issue>
    {
        public static readonly IssueEqualityComparer Default = new IssueEqualityComparer();

        IssueEqualityComparer() { }

        public bool Equals(Issue x, Issue y)
        {
            return x.Id == y.Id &&
                x.Number == y.Number &&
                x.UpdatedAt == y.UpdatedAt;
        }

        public int GetHashCode(Issue obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
