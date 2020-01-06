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
using Xamarin.Forms;

namespace IssueHub.ViewModels
{
    public sealed class SearchPageViewModel : PageViewModel
    {
        public SearchPageViewModel(IStore<AppState> store)
            : base()
        {
            Store = store;
            SearchCommand = new AsyncReactiveCommand(Keyword.Select(x => !string.IsNullOrEmpty(x)))
                .WithSubscribe(SearchAsync, AddDisposable);
            RefreshCommand = new AsyncReactiveCommand(Keyword.Select(x => !string.IsNullOrEmpty(x)))
                .WithSubscribe(RefreshAsync, AddDisposable);
            CancelCommand = new Command(Cancel);

            store.Select(x => x.Search)
                .DistinctUntilChanged(x => x.Issues)
                .Subscribe(x =>
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        Issues.Merge(x.Issues, IssueEqualityComparer.Default);
                    });
                });
        }

        public IStore<AppState> Store { get; }

        public ObservableCollection<Issue> Issues { get; } = new ObservableCollection<Issue>();

        public IReactiveProperty<string> Keyword { get; } = new ReactivePropertySlim<string>(string.Empty);

        public IReactiveProperty<bool> IsRefreshing { get; } = new ReactivePropertySlim<bool>(false);

        public IReactiveProperty<bool> IsLoading { get; } = new ReactivePropertySlim<bool>(false);

        public AsyncReactiveCommand SearchCommand { get; }

        public AsyncReactiveCommand RefreshCommand { get; }

        public Command CancelCommand { get; }

        void Cancel()
        {
            RaiseRequestPopModal();
        }

        Task SearchAsync() => SearchCoreAsync(IsLoading);

        Task RefreshAsync() => SearchCoreAsync(IsRefreshing);

        async Task SearchCoreAsync(IReactiveProperty<bool> isBusy)
        {
            try
            {
                isBusy.Value = true;
                await Store.DispatchAsync(
                    SearchActions.Search(
                        Keyword.Value));
            }
            finally
            {
                isBusy.Value = false;
            }
        }
    }
}
