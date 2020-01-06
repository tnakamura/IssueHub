using System.Collections.Generic;
using System.Linq;
using IssueHub.Models;
using IssueHub.States;
using Octokit;

namespace IssueHub.Actions
{
    public static class SearchActions
    {
        public static AsyncActionCreator<AppState> Search(string keyword)
        {
            return async store =>
            {
                var result = await store.State
                    .ApiClients
                    .GitHubClient
                    .Search
                    .SearchIssues(new SearchIssuesRequest(keyword)
                    {
                        SortField = IssueSearchSort.Updated,
                        Order = SortDirection.Descending,
                        State = ItemState.Open,
                        User = store.State.Login.User?.Login,
                    });

                var issues = result.Items.Select(x => new SearchedIssue(x));

                store.Dispatch(
                    new IssueSearched(
                        issues,
                        result.TotalCount,
                        result.IncompleteResults));
            };
        }
    }

    public readonly struct BeginSearch { }

    public readonly struct IssueSearched
    {
        public readonly bool IncompleteResults;

        public readonly int TotalCount;

        public readonly IEnumerable<Issue> Issues;

        public IssueSearched(IEnumerable<Issue> issues, int totalCount, bool incompleteResults)
        {
            Issues = issues;
            TotalCount = totalCount;
            IncompleteResults = incompleteResults;
        }
    }
}
