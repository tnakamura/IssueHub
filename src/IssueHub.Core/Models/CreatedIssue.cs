using Octokit;

namespace IssueHub.Models
{
    public sealed class CreatedIssue : Issue
    {
        public CreatedIssue(Issue issue) : base(
              url: issue.Url,
              htmlUrl: issue.HtmlUrl,
              commentsUrl: issue.CommentsUrl,
              eventsUrl: issue.EventsUrl,
              number: issue.Number,
              state: issue.State.Value,
              title: issue.Title,
              body: issue.Body,
              closedBy: issue.ClosedBy,
              user: issue.User,
              labels: issue.Labels,
              assignee: issue.Assignee,
              assignees: issue.Assignees,
              milestone: issue.Milestone,
              comments: issue.Comments,
              pullRequest: issue.PullRequest,
              closedAt: issue.ClosedAt,
              createdAt: issue.CreatedAt,
              updatedAt: issue.UpdatedAt,
              id: issue.Id,
              nodeId: issue.NodeId,
              locked: issue.Locked,
              repository: issue.Repository,
              reactions: issue.Reactions)
        {
            if (issue.Repository == null)
            {
                Repository = new RepositoryForIssue(issue);
            }
        }
    }
}
