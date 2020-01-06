using IssueHub.Actions;
using IssueHub.States;

namespace IssueHub.Reducers
{
    static class LoginReducer
    {
        public static LoginState Invoke<TAction>(LoginState state, in TAction action)
        {
            switch (action)
            {
                case OAuthLoggedIn a:
                    return new LoginState(
                        loggedIn: true,
                        accessToken: a.AccessToken,
                        user: state.User);
                case PersonalAccessTokenLogin a:
                    return new LoginState(
                        loggedIn: true,
                        accessToken: a.AccessToken,
                        user: state.User);
                case Logout _:
                    return new LoginState();
                case UserLoaded a:
                    return new LoginState(
                        loggedIn: state.LoggedIn,
                        accessToken: state.AccessToken,
                        user: a.User);
                default:
                    return state;
            }
        }
    }
}
