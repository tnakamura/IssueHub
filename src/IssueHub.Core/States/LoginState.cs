using Octokit;

namespace IssueHub.States
{
    public sealed class LoginState
    {
        public LoginState() { }

        public LoginState(bool loggedIn, string accessToken, User user)
        {
            LoggedIn = loggedIn;
            AccessToken = accessToken;
            User = user;
        }

        public bool LoggedIn { get; private set; } = false;

        public User User { get; private set; }

        public string AccessToken { get; private set; }
    }
}
