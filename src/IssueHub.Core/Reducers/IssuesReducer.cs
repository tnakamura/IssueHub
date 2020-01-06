using System.Collections.Immutable;
using IssueHub.Actions;
using IssueHub.Models;
using IssueHub.States;
using Octokit;

namespace IssueHub.Reducers
{
    static class IssuesReducer
    {
        public static IssuesState Invoke<TAction>(IssuesState state, in TAction action)
        {
            switch (action)
            {
                case SelectRepository a:
                    return new IssuesState(
                        initialized: false,
                        isBusy: false,
                        repository: a.Repository,
                        favorite: null,
                        issues: ImmutableList.Create<Issue>(),
                        showAll: state.ShowAll);
                case SelectSmartList _:
                    return new IssuesState(
                        initialized: false,
                        isBusy: false,
                        repository: null,
                        favorite: null,
                        issues: ImmutableList.Create<Issue>(),
                        showAll: state.ShowAll);
                case SelectCreatedIssue a:
                    return new IssuesState(
                        initialized: state.Initialized,
                        isBusy: state.IsBusy,
                        repository: new RepositoryForIssue(a.Issue),
                        favorite: state.Favorite,
                        issues: state.Issues,
                        showAll: state.ShowAll);
                case SelectAssignedIssue a:
                    return new IssuesState(
                        initialized: state.Initialized,
                        isBusy: state.IsBusy,
                        repository: new RepositoryForIssue(a.Issue),
                        favorite: state.Favorite,
                        issues: state.Issues,
                        showAll: state.ShowAll);
                case SelectSearchedIssue a:
                    return new IssuesState(
                        initialized: state.Initialized,
                        isBusy: state.IsBusy,
                        repository: new RepositoryForIssue(a.Issue),
                        favorite: state.Favorite,
                        issues: state.Issues,
                        showAll: state.ShowAll);
                case IssuesLoading _:
                    return new IssuesState(
                        initialized: state.Initialized,
                        isBusy: true,
                        repository: state.Repository,
                        favorite: state.Favorite,
                        issues: state.Issues,
                        showAll: state.ShowAll);
                case IssuesLoaded a:
                    return new IssuesState(
                        initialized: true,
                        isBusy: false,
                        repository: state.Repository,
                        favorite: state.Favorite,
                        issues: ImmutableList.CreateRange(a.Issues),
                        showAll: a.ShowAll);
                case IssueCreated a:
                    return new IssuesState(
                        initialized: state.Initialized,
                        isBusy: state.IsBusy,
                        repository: state.Repository,
                        favorite: state.Favorite,
                        issues: state.Issues.Insert(0, a.Issue),
                        showAll: state.ShowAll);
                case IssueUpdated a:
                    return HandleIssueUpdated(state, a);
                case FavoriteLoaded a:
                    return new IssuesState(
                        initialized: state.Initialized,
                        isBusy: state.IsBusy,
                        repository: state.Repository,
                        favorite: a.Favorite,
                        issues: state.Issues,
                        showAll: state.ShowAll);
                case ChangeItemStateFilter a:
                    return new IssuesState(
                        initialized: state.Initialized,
                        isBusy: state.IsBusy,
                        repository: state.Repository,
                        favorite: state.Favorite,
                        issues: state.Issues,
                        showAll: a.ShowAll);
                default:
                    return state;
            }
        }

        static IssuesState HandleIssueUpdated(IssuesState state, in IssueUpdated action)
        {
            var issueId = action.Issue.Id;
            var builder = state.Issues.ToBuilder();
            builder.RemoveAll(x => x.Id == issueId);
            builder.Insert(0, action.Issue);
            return new IssuesState(
                initialized: state.Initialized,
                isBusy: state.IsBusy,
                repository: state.Repository,
                favorite: state.Favorite,
                issues: builder.ToImmutable(),
                showAll: state.ShowAll);
        }
    }
}
