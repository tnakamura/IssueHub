using System.Collections.Immutable;
using IssueHub.Models;

namespace IssueHub.States
{
    public sealed class FavoritesState
    {
        public FavoritesState() { }

        public FavoritesState(
            ImmutableList<Favorite> favorites,
            bool initialized)
        {
            Favorites = favorites;
            Initialized = initialized;
        }

        public bool Initialized { get; private set; } = false;

        public ImmutableList<Favorite> Favorites { get; private set; } = ImmutableList.Create<Favorite>();
    }
}
