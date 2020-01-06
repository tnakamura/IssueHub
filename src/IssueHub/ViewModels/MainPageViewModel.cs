using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using IssueHub.Actions;
using IssueHub.Models;
using IssueHub.States;
using IssueHub.Utils;
using Octokit;
using Reactive.Bindings;
using ReduxSharp;
using ValueTaskSupplement;
using Xamarin.Essentials;
using R = IssueHub.Properties.Resources;

namespace IssueHub.ViewModels
{
    public sealed class MainPageViewModel : PageViewModel
    {
        public IStore<AppState> Store { get; }

        public MainPageViewModel(IStore<AppState> store)
            : base()
        {
            Store = store;

            InitializeCommand = new AsyncReactiveCommand(Store.Select(x => x.Login.LoggedIn))
                .WithSubscribe(InitializeAsync, AddDisposable);
            RefreshCommand = new AsyncReactiveCommand(Store.Select(x => x.Login.LoggedIn))
                .WithSubscribe(RefreshAsync, AddDisposable);

            AddDisposable(
                store.DistinctUntilChanged(x => (x.Repositories.Repositories, x.Favorites.Favorites))
                    .Subscribe(x =>
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            Initialized.Value = x.Repositories.Initialized;
                            Sections.Merge(ToViewModels(x));
                        });
                    }));
        }

        static IEnumerable<ListGroupViewModel> ToViewModels(AppState state)
        {
            yield return new ListGroupViewModel(
                R.SmartLists,
                new List<ListItemViewModel>()
                {
                    new SmartListViewModel(
                        R.Created,
                        Octicons.Glyph.IssueOpened),
                    new SmartListViewModel(
                        R.Assigned,
                        Octicons.Glyph.Person),
                });
            if (0 < state.Favorites.Favorites.Count)
            {
                yield return new ListGroupViewModel(
                    R.Favorites,
                    state.Favorites.Favorites.Select(x => new FavoriteViewModel(x)));
            }
            if (0 < state.Repositories.Repositories.Count)
            {
                yield return new ListGroupViewModel(
                    R.Repositories,
                    state.Repositories.Repositories.Select(x => new RepositoryViewModel(x)));
            }
        }

        public IReactiveProperty<bool> Initialized { get; } = new ReactivePropertySlim<bool>(false);

        public IReactiveProperty<bool> IsRefreshing { get; } = new ReactivePropertySlim<bool>(false);

        public IReactiveProperty<bool> IsLoading { get; } = new ReactivePropertySlim<bool>(false);

        public ObservableCollection<ListGroupViewModel> Sections { get; } = new ObservableCollection<ListGroupViewModel>();

        public AsyncReactiveCommand InitializeCommand { get; }

        public AsyncReactiveCommand RefreshCommand { get; }

        Task InitializeAsync() => LoadCoreAsync(IsLoading);

        Task RefreshAsync() => LoadCoreAsync(IsRefreshing);

        async Task LoadCoreAsync(IReactiveProperty<bool> isBusy)
        {
            try
            {
                isBusy.Value = true;
                await ValueTaskEx.WhenAll(
                    Store.DispatchAsync(RepositoryActions.LoadRepositories()),
                    Store.DispatchAsync(FavoriteActions.LoadFavorites()));
            }
            finally
            {
                isBusy.Value = false;
            }
        }
    }

    public abstract class ListItemViewModel
    {
        protected ListItemViewModel() { }

        public string FullName { get; protected set; }
    }

    public sealed class ListGroupViewModel : ObservableCollection<ListItemViewModel>
    {
        public ListGroupViewModel(string title)
            : base()
        {
            Title = title;
        }

        public ListGroupViewModel(string title, IEnumerable<ListItemViewModel> lists)
            : base(lists)
        {
            Title = title;
        }

        public string Title { get; private set; }
    }

    public sealed class SmartListViewModel : ListItemViewModel
    {
        public SmartListViewModel(string fullName, Octicons.Glyph glyph)
            : base()
        {
            FullName = fullName;
            Glyph = Octicons.GetGlyphString(glyph);
        }

        public string Glyph { get; }
    }

    public sealed class RepositoryViewModel : ListItemViewModel
    {
        readonly Repository repository;

        public RepositoryViewModel(Repository repository)
            : base()
        {
            this.repository = repository;
            FullName = repository.FullName;
        }

        public bool Private => repository.Private;

        public Repository ToRepository() => repository;
    }

    public sealed class FavoriteViewModel : ListItemViewModel
    {
        readonly Favorite favorite;

        public FavoriteViewModel(Favorite favorite)
            : base()
        {
            this.favorite = favorite;
            FullName = favorite.FullName;
        }

        public bool Private => favorite.Private;

        public Repository ToRepository() => favorite.ToGitHubRepository();
    }
}
