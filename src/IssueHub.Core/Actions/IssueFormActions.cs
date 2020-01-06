using System.Collections.Generic;
using IssueHub.States;
using Octokit;

namespace IssueHub.Actions
{
    public static class IssueFormActions
    {
        public static AsyncActionCreator<AppState> LoadLabels(string owner, string name)
        {
            return async (store) =>
            {
                store.Dispatch(new LabelsLoading());

                var labels = await store.State
                    .ApiClients
                    .GitHubClient
                    .Issue
                    .Labels
                    .GetAllForRepository(owner, name);

                store.Dispatch(new LabelsLoaded(labels));
            };
        }

        public static AsyncActionCreator<AppState> LoadAssignees(string owner, string name)
        {
            return async (store) =>
            {
                store.Dispatch(new AssigneesLoading());

                var assignees = await store.State
                    .ApiClients
                    .GitHubClient
                    .Issue
                    .Assignee
                    .GetAllForRepository(owner, name);

                store.Dispatch(new AssigneesLoaded(assignees));
            };
        }

        public static AsyncActionCreator<AppState> CreateIssue(
            string owner,
            string name,
            string title,
            string body,
            IReadOnlyList<User> assignees,
            IReadOnlyList<Label> labels)
        {
            return async (store) =>
            {
                var newIssue = new Octokit.NewIssue(title)
                {
                    Body = body,
                };
                foreach (var assignee in assignees)
                {
                    newIssue.Assignees.Add(assignee.Login);
                }
                foreach (var label in labels)
                {
                    newIssue.Labels.Add(label.Name);
                }

                var createdIssue = await store.State
                    .ApiClients
                    .GitHubClient
                    .Issue
                    .Create(
                        owner: owner,
                        name: name,
                        newIssue: newIssue);

                store.Dispatch(new IssueCreated(createdIssue));
            };
        }

        public static AsyncActionCreator<AppState> UpdateIssue(
            string owner,
            string name,
            Issue issue,
            string title,
            string body,
            IReadOnlyList<User> assignees,
            IReadOnlyList<Label> labels)
        {
            return async (store) =>
            {
                var issueUpdate = issue.ToUpdate();
                issueUpdate.Title = title;
                issueUpdate.Body = body;

                issueUpdate.ClearAssignees();
                foreach (var assignee in assignees)
                {
                    issueUpdate.Assignees.Add(assignee.Login);
                }

                issueUpdate.ClearLabels();
                foreach (var label in labels)
                {
                    issueUpdate.Labels.Add(label.Name);
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

    public readonly struct IssueUpdated
    {
        public readonly Issue Issue;

        public IssueUpdated(Issue issue) => Issue = issue;
    }

    public readonly struct IssueCreated
    {
        public readonly Issue Issue;

        public IssueCreated(Issue issue) => Issue = issue;
    }

    public readonly struct AssigneesLoading { }

    public readonly struct AssigneesLoaded
    {
        public readonly IReadOnlyList<User> Assignees;

        public AssigneesLoaded(IReadOnlyList<User> assignees) =>
            Assignees = assignees;
    }

    public readonly struct LabelsLoading { }

    public readonly struct LabelsLoaded
    {
        public readonly IReadOnlyList<Label> Labels;

        public LabelsLoaded(IReadOnlyList<Label> labels) =>
            Labels = labels;
    }

    public readonly struct SelectLabel
    {
        public readonly Label Label;

        public SelectLabel(Label label) => Label = label;
    }

    public readonly struct DeselectLabel
    {
        public readonly Label Label;

        public DeselectLabel(Label label) => Label = label;
    }

    public readonly struct SelectAssignee
    {
        public readonly User Assignee;

        public SelectAssignee(User assignee) => Assignee = assignee;
    }

    public readonly struct DeselectAssignee
    {
        public readonly User Assignee;

        public DeselectAssignee(User assignee) => Assignee = assignee;
    }

    public readonly struct NewIssue { }

    public readonly struct EditIssue
    {
        public readonly Issue Issue;

        public EditIssue(Issue issue) => Issue = issue;
    }

    public readonly struct ChangeIssueBody
    {
        public readonly string Body;

        public ChangeIssueBody(string body) => Body = body;
    }

    public readonly struct ChangeIssueTitle
    {
        public readonly string Title;

        public ChangeIssueTitle(string title) => Title = title;
    }
}
