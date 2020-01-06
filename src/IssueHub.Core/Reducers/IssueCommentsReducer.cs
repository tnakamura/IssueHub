using System.Runtime.CompilerServices;
using System.Collections.Immutable;
using IssueHub.Actions;
using IssueHub.States;
using Octokit;

namespace IssueHub.Reducers
{
    static class IssueCommentsReducer
    {
        public static IssueCommentsState Invoke<TAction>(IssueCommentsState state, in TAction action)
        {
            switch (action)
            {
                case SelectIssue a:
                    return HandleSelectIssue(a.Issue);
                case SelectCreatedIssue a:
                    return HandleSelectIssue(a.Issue);
                case SelectAssignedIssue a:
                    return HandleSelectIssue(a.Issue);
                case SelectSearchedIssue a:
                    return HandleSelectIssue(a.Issue);
                case IssueCommentsLoading _:
                    return new IssueCommentsState(
                        initialized: state.Initialized,
                        isBusy: true,
                        issue: state.Issue,
                        comments: state.Comments);
                case IssueCommentsLoaded a:
                    return new IssueCommentsState(
                        initialized: true,
                        isBusy: false,
                        issue: state.Issue,
                        comments: ImmutableList.CreateRange(a.Comments));
                case CommentCreated a:
                    return new IssueCommentsState(
                        initialized: state.Initialized,
                        isBusy: false,
                        issue: state.Issue,
                        comments: state.Comments.Add(a.Comment));
                case IssueUpdated a when state.Issue.Number == a.Issue.Number:
                    return new IssueCommentsState(
                        initialized: state.Initialized,
                        isBusy: false,
                        issue: a.Issue,
                        comments: state.Comments);
                default:
                    return state;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static IssueCommentsState HandleSelectIssue(Issue issue)
        {
            // コメントが 0 件なら初期化済みとして扱い、
            // API を呼び出さない。
            return new IssueCommentsState(
                initialized: issue.Comments == 0,
                isBusy: false,
                issue: issue,
                comments: ImmutableList.Create<IssueComment>());
        }
    }
}
