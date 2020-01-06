using Octokit;

namespace IssueHub.Models
{
    public sealed class IssueHubUser : User
    {
        public IssueHubUser(string login)
            : base()
        {
            Login = login;
        }

        public IssueHubUser(User user)
            : base()
        {
            Login = user.Login;
        }
    }
}
