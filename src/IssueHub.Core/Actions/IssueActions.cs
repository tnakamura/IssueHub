using System.Collections.Generic;
using IssueHub.States;
using System.Linq;
using IssueHub.Models;
using Octokit;

namespace IssueHub.Actions
{
    public static class IssueActions
    {
        public static AsyncActionCreator<AppState> LoadRepositoryIssues(
            string owner,
            string name,
            bool showAll = false)
        {
            return async (store) =>
            {
                store.Dispatch(new IssuesLoading());
                var issues = await store.State
                    .ApiClients
                    .GitHubClient
                    .Issue
                    .GetAllForRepository(
                        owner: owner,
                        name: name,
                        request: new RepositoryIssueRequest
                        {
                            State = showAll ? ItemStateFilter.All : ItemStateFilter.Open,
                            SortProperty = IssueSort.Updated,
                            SortDirection = SortDirection.Descending,
                        });
                store.Dispatch(new IssuesLoaded(issues, showAll));
            };
        }

        public static AsyncActionCreator<AppState> LoadCreatedIssues(
            bool showAll = false)
        {
            return async (store) =>
            {
                store.Dispatch(new IssuesLoading());
                var issues = await store.State
                    .ApiClients
                    .GitHubClient
                    .Issue
                    .GetAllForCurrent(
                        request: new RepositoryIssueRequest
                        {
                            Filter = IssueFilter.Created,
                            State = showAll ? ItemStateFilter.All : ItemStateFilter.Open,
                            SortProperty = IssueSort.Updated,
                            SortDirection = SortDirection.Descending,
                        });
                var createdIssues = issues.Select(x => new CreatedIssue(x));
                store.Dispatch(new IssuesLoaded(createdIssues, showAll));
            };
        }

        public static AsyncActionCreator<AppState> LoadAssignedIssues(
            bool showAll = false)
        {
            return async (store) =>
            {
                store.Dispatch(new IssuesLoading());
                var issues = await store.State
                    .ApiClients
                    .GitHubClient
                    .Issue
                    .GetAllForCurrent(
                        request: new RepositoryIssueRequest
                        {
                            Filter = IssueFilter.Assigned,
                            State = showAll ? ItemStateFilter.All : ItemStateFilter.Open,
                            SortProperty = IssueSort.Updated,
                            SortDirection = SortDirection.Descending,
                        });
                var assignedIssues = issues.Select(x => new AssignedIssue(x));
                store.Dispatch(new IssuesLoaded(assignedIssues, showAll));
            };
        }

        public static AsyncActionCreator<AppState> CloseIssue(
            string owner,
            string name,
            Issue issue) =>
            ChangeIssueState(owner, name, issue, ItemState.Closed);

        public static AsyncActionCreator<AppState> OpenIssue(
            string owner,
            string name,
            Issue issue) =>
            ChangeIssueState(owner, name, issue, ItemState.Open);

        static AsyncActionCreator<AppState> ChangeIssueState(
            string owner,
            string name,
            Issue issue,
            ItemState state)
        {
            return async (store) =>
            {
                var issueUpdate = new IssueUpdate
                {
                    Title = issue.Title,
                    Body = issue.Body,
                    State = state,
                };
                foreach (var assignee in issue.Assignees)
                {
                    issueUpdate.AddAssignee(assignee.Login);
                }
                foreach (var label in issue.Labels)
                {
                    issueUpdate.AddLabel(label.Name);
                }

                var updatedIssue = await store.State
                    .ApiClients
                    .GitHubClient
                    .Issue
                    .Update(
                        owner: owner,
                        name: name,
                        number: issue.Number,
                        issueUpdate: issueUpdate);

                store.Dispatch(new IssueUpdated(updatedIssue));
            };
        }
    }

    public readonly struct ChangeItemStateFilter
    {
        public readonly bool ShowAll;

        public ChangeItemStateFilter(bool showAll) => ShowAll = showAll;
    }

    public readonly struct IssuesLoading { }

    public readonly struct IssuesLoaded
    {
        public readonly IEnumerable<Issue> Issues;

        public readonly bool ShowAll;

        public IssuesLoaded(IEnumerable<Issue> issues, bool showAll)
        {
            Issues = issues;
            ShowAll = showAll;
        }
    }

    public readonly struct SelectIssue
    {
        public readonly Issue Issue;

        public SelectIssue(Issue issue) => Issue = issue;
    }

    public readonly struct SelectCreatedIssue
    {
        public readonly Issue Issue;

        public SelectCreatedIssue(Issue issue) => Issue = issue;
    }

    public readonly struct SelectAssignedIssue
    {
        public readonly Issue Issue;

        public SelectAssignedIssue(Issue issue) => Issue = issue;
    }

    public readonly struct SelectSearchedIssue
    {
        public readonly Issue Issue;

        public SelectSearchedIssue(Issue issue) => Issue = issue;
    }

    public readonly struct SelectSmartList { }
}
