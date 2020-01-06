using System.Collections.Immutable;
using Octokit;

namespace IssueHub.States
{
    public sealed class IssueCommentsState
    {
        public IssueCommentsState() { }

        public IssueCommentsState(
            bool initialized,
            bool isBusy,
            Issue issue,
            ImmutableList<IssueComment> comments)
        {
            Initialized = initialized;
            IsBusy = isBusy;
            Issue = issue;
            Comments = comments;
        }

        public bool Initialized { get; private set; } = false;

        public bool IsBusy { get; private set; } = false;

        public Issue Issue { get; private set; }

        public ImmutableList<IssueComment> Comments { get; private set; } = ImmutableList.Create<IssueComment>();
    }
}
