using System.Collections.Immutable;
using Octokit;

namespace IssueHub.States
{
    public sealed class AssigneesState
    {
        public AssigneesState() { }

        public AssigneesState(bool initialized, ImmutableList<User> assignees)
        {
            Initialized = initialized;
            Assignees = assignees;
        }

        public bool Initialized { get; private set; } = false;

        public ImmutableList<User> Assignees { get; private set; } = ImmutableList.Create<User>();
    }
}
