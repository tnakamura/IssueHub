using System.Collections.Immutable;
using Octokit;
using IssueHub.Models;

namespace IssueHub.States
{
    public sealed class IssuesState
    {
        public IssuesState() { }

        public IssuesState(
            bool initialized,
            bool isBusy,
            Repository repository,
            Favorite favorite,
            ImmutableList<Issue> issues,
            bool showAll)
        {
            Initialized = initialized;
            IsBusy = isBusy;
            Repository = repository;
            Favorite = favorite;
            Issues = issues;
            ShowAll = showAll;
        }

        public bool Initialized { get; private set; } = false;

        public bool IsBusy { get; private set; } = false;

        public bool ShowAll { get; private set; } = false;

        public Repository Repository { get; private set; }

        public Favorite Favorite { get; private set; }

        public ImmutableList<Issue> Issues { get; private set; } = ImmutableList.Create<Issue>();

        public bool IsFavorite => Favorite != null;
    }
}
