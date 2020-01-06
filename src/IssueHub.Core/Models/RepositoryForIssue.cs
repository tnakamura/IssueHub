using System;
using Octokit;

namespace IssueHub.Models
{
    public sealed class RepositoryForIssue : Repository
    {
        public RepositoryForIssue(Issue issue)
            : base()
        {
            var segments = issue.Url
                .Replace("https://api.github.com/repos/", "")
                .Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            Owner = new RepositoryOwner(segments[0]);
            Name = segments[1];
            FullName = segments[0] + "/" + segments[1];
        }

        class RepositoryOwner : User
        {
            public RepositoryOwner(string login)
                : base()
            {
                Login = login;
            }
        }
    }
}
