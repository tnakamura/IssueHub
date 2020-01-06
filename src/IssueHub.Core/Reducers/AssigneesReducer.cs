using System.Collections.Immutable;
using IssueHub.Actions;
using IssueHub.States;

namespace IssueHub.Reducers
{
    static class AssigneesReducer
    {
        public static AssigneesState Invoke<TAction>(AssigneesState state, in TAction action)
        {
            switch (action)
            {
                case NewIssue _:
                    return new AssigneesState();
                case EditIssue _:
                    return new AssigneesState();
                case AssigneesLoaded a:
                    return new AssigneesState(
                        initialized: true,
                        assignees: ImmutableList.CreateRange(a.Assignees));
                default:
                    return state;
            }
        }
    }
}
