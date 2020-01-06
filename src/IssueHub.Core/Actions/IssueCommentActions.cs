using System;
using System.Collections.Generic;
using System.Text;
using Octokit;
using IssueHub.States;

namespace IssueHub.Actions
{
    public static class IssueCommentActions
    {
        public static AsyncActionCreator<AppState> LoadIssueComments(
            string owner,
            string name,
            int number)
        {
            return async (store) =>
            {
                store.Dispatch(new IssueCommentsLoading());
                var comments = await store.State
                    .ApiClients
                    .GitHubClient
                    .Issue
                    .Comment
                    .GetAllForIssue(
                        owner,
                        name,
                        number);
                store.Dispatch(new IssueCommentsLoaded(comments));
            };
        }

        public static AsyncActionCreator<AppState> PostComment(
            string owner,
            string name,
            int number,
            string comment)
        {
            return async (store) =>
            {
                var createdComment = await store.State
                    .ApiClients
                    .GitHubClient
                    .Issue
                    .Comment
                    .Create(
                        owner,
                        name,
                        number,
                        comment);
                store.Dispatch(new CommentCreated(createdComment));
            };
        }
    }

    public readonly struct CommentCreated
    {
        public readonly IssueComment Comment;

        public CommentCreated(IssueComment comment) => Comment = comment;
    }

    public readonly struct IssueCommentsLoading { }

    public readonly struct IssueCommentsLoaded
    {
        public readonly IReadOnlyList<IssueComment> Comments;

        public IssueCommentsLoaded(IReadOnlyList<IssueComment> comments) =>
            Comments = comments;
    }
}
