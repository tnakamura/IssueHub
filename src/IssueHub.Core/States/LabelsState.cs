using System.Collections.Immutable;
using Octokit;

namespace IssueHub.States
{
    public sealed class LabelsState
    {
        public LabelsState() { }

        public LabelsState(bool initialized, ImmutableList<Label> labels)
        {
            Initialized = initialized;
            Labels = labels;
        }

        public bool Initialized { get; private set; } = false;

        public ImmutableList<Label> Labels { get; private set; } = ImmutableList.Create<Label>();
    }
}
