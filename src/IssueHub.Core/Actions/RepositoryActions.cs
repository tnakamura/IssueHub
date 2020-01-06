using System.Collections.Generic;
using IssueHub.States;
using Octokit;

namespace IssueHub.Actions
{
    public static class RepositoryActions
    {
        public static AsyncActionCreator<AppState> LoadRepositories()
        {
            return async (store) =>
            {
                store.Dispatch(new RepositoriesLoading());
                var repositories = await store.State
                    .ApiClients
                    .GitHubClient
                    .Repository
                    .GetAllForCurrent(
                        new RepositoryRequest()
                        {
                            Sort = RepositorySort.Updated,
                            Direction = SortDirection.Descending,
                        });
                store.Dispatch(new RepositoriesLoaded(repositories));
            };
        }
    }

    public readonly struct RepositoriesLoading { }

    public readonly struct RepositoriesLoaded
    {
        public readonly IReadOnlyList<Repository> Repositories;

        public RepositoriesLoaded(IReadOnlyList<Repository> repositories) =>
            Repositories = repositories;
    }

    public readonly struct SelectRepository
    {
        public readonly Repository Repository;

        public SelectRepository(Repository repository) =>
            Repository = repository;
    }
}
