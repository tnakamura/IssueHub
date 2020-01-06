using System.Collections.Immutable;
using IssueHub.Actions;
using IssueHub.States;

namespace IssueHub.Reducers
{
    static class SearchReducer
    {
        public static SearchState Invoke<TAction>(SearchState state, in TAction action)
        {
            switch (action)
            {
                case BeginSearch _:
                    return new SearchState();
                case IssueSearched a:
                    return new SearchState(
                        issues: ImmutableList.CreateRange(a.Issues),
                        totalCount: a.TotalCount,
                        incompleteResults: a.IncompleteResults);
                default:
                    return state;
            }
        }
    }
}
