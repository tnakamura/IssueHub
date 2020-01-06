using System.Collections.Generic;
using IssueHub.Actions;
using IssueHub.Reducers;
using IssueHub.States;
using Xunit;
using Octokit;

namespace IssueHub.Tests.Reducers
{
    public class RepositoriesReducerTest
    {
        [Fact]
        public void Test_RepositoriesLoading()
        {
            var state = new RepositoriesState();

            var actual = RepositoriesReducer.Invoke(state, new RepositoriesLoading());

            Assert.True(actual.IsBusy);
            Assert.False(actual.Initialized);
            Assert.Empty(actual.Repositories);
        }

        [Fact]
        public void Test_RepositoriesLoaded()
        {
            var state = new RepositoriesState();

            var actual = RepositoriesReducer.Invoke(
                state,
                new RepositoriesLoaded(
                    new List<Repository>()
                    {
                        new TestRepository("tnakamura", "IssueHub"),
                    }));

            Assert.False(actual.IsBusy);
            Assert.True(actual.Initialized);
            Assert.Single(actual.Repositories);
            Assert.Equal("tnakamura/IssueHub", actual.Repositories[0].FullName);
        }

        class TestRepository : Repository
        {
            public TestRepository(string owner, string name)
                : base()
            {
                FullName = $"{owner}/{name}";
                Name = name;
            }
        }
    }
}
