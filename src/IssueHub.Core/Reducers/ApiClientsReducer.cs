using IssueHub.Actions;
using IssueHub.States;
using Octokit;

namespace IssueHub.Reducers
{
    static class ApiClientsReducer
    {
        public static ApiClientsState Invoke<TAction>(ApiClientsState state, in TAction action)
        {
            switch (action)
            {
                case OAuthLoggedIn a:
                    return new ApiClientsState(
                        gitHubClient: new GitHubClient(
                            new ProductHeaderValue("IssueHub"))
                        {
                            Credentials = new Credentials(
                                a.AccessToken),
                        },
                        accountsRepository: state.AccountsRepository,
                        favoritesRepository: state.FavoritesRepository);
                case PersonalAccessTokenLogin a:
                    return new ApiClientsState(
                        gitHubClient: new GitHubClient(
                            new ProductHeaderValue("IssueHub"))
                        {
                            Credentials = new Credentials(
                                a.Login,
                                a.AccessToken),
                        },
                        accountsRepository: state.AccountsRepository,
                        favoritesRepository: state.FavoritesRepository);
                default:
                    return state;
            }
        }
    }
}
