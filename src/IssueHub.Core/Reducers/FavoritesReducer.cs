using IssueHub.Actions;
using IssueHub.States;
using IssueHub.Models;
using System.Collections.Immutable;

namespace IssueHub.Reducers
{
    static class FavoritesReducer
    {
        public static FavoritesState Invoke<TAction>(FavoritesState state, in TAction action)
        {
            switch (action)
            {
                case FavoritesLoaded a:
                    return new FavoritesState(
                        favorites: ImmutableList.CreateRange(a.Favorites),
						initialized: true);
                case FavoriteAdded a:
                    return new FavoritesState(
                        favorites: state.Favorites.Add(a.Favorite),
						initialized: state.Initialized);
                case FavoriteRemoved a:
                    return new FavoritesState(
                        favorites: state.Favorites.Remove(a.Favorite),
						initialized: state.Initialized);
                default:
                    return state;
            }
        }
    }
}
