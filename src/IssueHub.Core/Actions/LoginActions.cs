using System;
using System.Linq;
using IssueHub.States;
using Octokit;

namespace IssueHub.Actions
{
    public static class LoginActions
    {
        public static AsyncActionCreator<AppState> ReceiveAccessToken(
            string accessToken)
        {
            return async store =>
            {
                store.Dispatch(new OAuthLoggedIn(accessToken));

                var user = await store.State
                    .ApiClients
                    .GitHubClient
                    .User
                    .Current();
                store.Dispatch(new UserLoaded(user));

                await store.State
                    .ApiClients
                    .AccountsRepository
                    .StoreAsync(
                        new Models.Account(user.Login, accessToken));
            };
        }

        public static AsyncActionCreator<AppState> Logout()
        {
            return async (store) =>
            {
                await store.State
                    .ApiClients
                    .AccountsRepository
                    .DeleteAsync();
                store.Dispatch(new Logout());
            };
        }

        public static AsyncActionCreator<AppState> LoadUser()
        {
            return async (store) =>
            {
                var user = await store.State
                    .ApiClients
                    .GitHubClient
                    .User
                    .Current();
                store.Dispatch(new UserLoaded(user));
            };
        }
    }

    public readonly struct UserLoaded
    {
        public readonly User User;

        public UserLoaded(User user) => User = user;
    }

    public readonly struct AutoLogIn { }

    public readonly struct OAuthLoggedIn
    {
        public readonly string AccessToken;

        public OAuthLoggedIn(string accessToken) => AccessToken = accessToken;
    }

    public readonly struct PersonalAccessTokenLogin
    {
        public readonly string Login;

        public readonly string AccessToken;

        public PersonalAccessTokenLogin(string userName, string accessToken)
        {
            Login = userName;
            AccessToken = accessToken;
        }
    }

    public readonly struct Logout { }
}
