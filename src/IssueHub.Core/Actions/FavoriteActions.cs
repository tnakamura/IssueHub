using System.Collections.Generic;
using IssueHub.Models;
using IssueHub.States;
using Octokit;

namespace IssueHub.Actions
{
    public static class FavoriteActions
    {
        public static AsyncActionCreator<AppState> LoadFavorites()
        {
            return async (store) =>
            {
                store.Dispatch(new FavoritesLoading());

                var favoritesRepository = store.State.ApiClients.FavoritesRepository;
                var favorites = await favoritesRepository.FindAllAsync();

                store.Dispatch(new FavoritesLoaded(favorites));
            };
        }

        public static AsyncActionCreator<AppState> LoadFavorite(string owner, string name)
        {
            return async (store) =>
            {
                var favoritesRepository = store.State.ApiClients.FavoritesRepository;
                var favorite = await favoritesRepository.FindByNameAsync(owner, name);

                store.Dispatch(new FavoriteLoaded(favorite));
            };
        }

        public static AsyncActionCreator<AppState> AddToFavorites(Repository repository)
        {
            return async (store) =>
            {
                var favorite = new Favorite(repository);

                var favoritesRepository = store.State.ApiClients.FavoritesRepository;
                await favoritesRepository.CreateAsync(favorite);

                store.Dispatch(new FavoriteAdded(favorite));
            };
        }

        public static AsyncActionCreator<AppState> RemoveFromFavorites(string owner, string name)
        {
            return async (store) =>
            {
                var favoritesRepository = store.State.ApiClients.FavoritesRepository;
                var favorite = await favoritesRepository.FindByNameAsync(owner, name);
                if (favorite != null)
                {
                    await favoritesRepository.DeleteAsync(favorite);
                    store.Dispatch(new FavoriteRemoved(favorite));
                }
            };
        }
    }

    public readonly struct FavoriteLoaded
    {
        public readonly Favorite Favorite;

        public FavoriteLoaded(Favorite favorite) => Favorite = favorite;
    }

    public readonly struct FavoriteAdded
    {
        public readonly Favorite Favorite;

        public FavoriteAdded(Favorite favorite) => Favorite = favorite;
    }

    public readonly struct FavoriteRemoved
    {
        public readonly Favorite Favorite;

        public FavoriteRemoved(Favorite favorite) => Favorite = favorite;
    }

    public readonly struct FavoritesLoading { }

    public readonly struct FavoritesLoaded
    {
        public readonly IReadOnlyList<Favorite> Favorites;

        public FavoritesLoaded(IReadOnlyList<Favorite> favorites) =>
            Favorites = favorites;
    }
}
