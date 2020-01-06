using System.Collections.Immutable;
using Octokit;

namespace IssueHub.States
{
    public sealed class RepositoriesState
    {
        public RepositoriesState()
        {
        }

        public RepositoriesState(
            bool initialized,
            bool isBusy,
            ImmutableList<Repository> repositories)
        {
            Initialized = initialized;
            IsBusy = isBusy;
            Repositories = repositories;
        }

        public bool Initialized { get; private set; } = false;

        public bool IsBusy { get; private set; } = false;

        public ImmutableList<Repository> Repositories { get; private set; } = ImmutableList.Create<Repository>();
    }
}
