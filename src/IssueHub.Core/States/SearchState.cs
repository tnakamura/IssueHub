using System.Collections.Immutable;
using Octokit;

namespace IssueHub.States
{
    public sealed class SearchState
    {
        public SearchState() { }

        public SearchState(
            ImmutableList<Issue> issues,
            int totalCount,
            bool incompleteResults)
        {
            Issues = issues;
            TotalCount = totalCount;
            IncompleteResults = incompleteResults;
        }

        public bool IncompleteResults { get; private set; }

        public int TotalCount { get; private set; }

        public ImmutableList<Issue> Issues { get; private set; } = ImmutableList.Create<Issue>();
    }
}
