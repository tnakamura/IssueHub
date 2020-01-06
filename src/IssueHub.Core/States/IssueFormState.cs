using System.Collections.Immutable;
using Octokit;

namespace IssueHub.States
{
    public sealed class IssueFormState
    {
        public IssueFormState() { }

        public IssueFormState(
            Issue editingIssue,
            string title,
            ImmutableList<User> assignees,
            ImmutableList<Label> labels,
            string body)
        {
            EditingIssue = editingIssue;
            Title = title;
            Labels = labels;
            Assignees = assignees;
            Body = body;
        }

        public Issue EditingIssue { get; private set; }

        public string Title { get; private set; }

        public ImmutableList<User> Assignees { get; private set; } = ImmutableList.Create<User>();

        public ImmutableList<Label> Labels { get; private set; } = ImmutableList.Create<Label>();

        public string Body { get; private set; }

        public bool IsNewIssue => EditingIssue == null;
    }
}
