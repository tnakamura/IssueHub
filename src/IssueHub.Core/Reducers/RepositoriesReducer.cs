using System.Collections.Immutable;
using IssueHub.Actions;
using IssueHub.States;

namespace IssueHub.Reducers
{
    static class RepositoriesReducer
    {
        public static RepositoriesState Invoke<TAction>(RepositoriesState state, in TAction action)
        {
            switch (action)
            {
                case RepositoriesLoading _:
                    return new RepositoriesState(
                        initialized: state.Initialized,
                        isBusy: true,
                        repositories: state.Repositories);
                case RepositoriesLoaded a:
                    return new RepositoriesState(
                        initialized: true,
                        isBusy: false,
                        repositories: ImmutableList.CreateRange(a.Repositories));
                default:
                    return state;
            }
        }
    }
}
