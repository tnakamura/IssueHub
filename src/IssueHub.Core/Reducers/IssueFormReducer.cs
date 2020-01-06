using System;
using System.Collections.Immutable;
using IssueHub.Actions;
using IssueHub.States;
using Octokit;

namespace IssueHub.Reducers
{
    static class IssueFormReducer
    {
        public static IssueFormState Invoke<TAction>(IssueFormState state, in TAction action)
        {
            switch (action)
            {
                case Actions.NewIssue _:
                    return new IssueFormState();
                case EditIssue a:
                    return new IssueFormState(
                        editingIssue: a.Issue,
                        title: a.Issue.Title,
                        assignees: ImmutableList.CreateRange(
                            a.Issue.Assignees ?? Array.Empty<User>()),
                        labels: ImmutableList.CreateRange(a.Issue.Labels),
                        body: a.Issue.Body);
                case SelectLabel a:
                    return new IssueFormState(
                        editingIssue: state.EditingIssue,
                        title: state.Title,
                        assignees: state.Assignees,
                        labels: state.Labels.Add(a.Label),
                        body: state.Body);
                case DeselectLabel a:
                    return new IssueFormState(
                        editingIssue: state.EditingIssue,
                        title: state.Title,
                        assignees: state.Assignees,
                        labels: state.Labels.RemoveAll(x => x.Name == a.Label.Name),
                        body: state.Body);
                case SelectAssignee a:
                    return new IssueFormState(
                        editingIssue: state.EditingIssue,
                        title: state.Title,
                        assignees: state.Assignees.Add(a.Assignee),
                        labels: state.Labels,
                        body: state.Body);
                case DeselectAssignee a:
                    return new IssueFormState(
                        editingIssue: state.EditingIssue,
                        title: state.Title,
                        assignees: state.Assignees.RemoveAll(x => x.Login == a.Assignee.Login),
                        labels: state.Labels,
                        body: state.Body);
                case ChangeIssueBody a:
                    return new IssueFormState(
                        editingIssue: state.EditingIssue,
                        title: state.Title,
                        assignees: state.Assignees,
                        labels: state.Labels,
                        body: a.Body);
                case ChangeIssueTitle a:
                    return new IssueFormState(
                        editingIssue: state.EditingIssue,
                        title: a.Title,
                        assignees: state.Assignees,
                        labels: state.Labels,
                        body: state.Body);
                default:
                    return state;
            }
        }
    }
}
