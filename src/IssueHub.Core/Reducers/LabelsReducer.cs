using System.Collections.Immutable;
using IssueHub.Actions;
using IssueHub.States;

namespace IssueHub.Reducers
{
    static class LabelsReducer
    {
        public static LabelsState Invoke<TAction>(LabelsState state, in TAction action)
        {
            switch (action)
            {
                case NewIssue _:
                    return new LabelsState();
                case EditIssue _:
                    return new LabelsState();
                case LabelsLoaded a:
                    return new LabelsState(
                        initialized: true,
                        labels: ImmutableList.CreateRange(a.Labels));
                default:
                    return state;
            }
        }
    }
}
