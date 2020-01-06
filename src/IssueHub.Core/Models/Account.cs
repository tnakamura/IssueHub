namespace IssueHub.Models
{
    public sealed class Account
    {
        public Account(string login, string accessToken)
        {
            Login = login;
            AccessToken = accessToken;
        }

        public string Login { get; private set; }

        public string AccessToken { get; private set; }
    }
}
