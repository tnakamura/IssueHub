using IssueHub.Reducers;
using ReduxSharp;

namespace IssueHub.States
{
    public sealed partial class AppState
    {
        public sealed class AppReducer : IReducer<AppState>
        {
            public AppState Invoke<TAction>(AppState state, TAction action)
            {
                state.ApiClients = ApiClientsReducer.Invoke(state.ApiClients, action);
                state.Login = LoginReducer.Invoke(state.Login, action);
                state.Repositories = RepositoriesReducer.Invoke(state.Repositories, action);
                state.Issues = IssuesReducer.Invoke(state.Issues, action);
                state.IssueComments = IssueCommentsReducer.Invoke(state.IssueComments, action);
                state.IssueForm = IssueFormReducer.Invoke(state.IssueForm, action);
                state.Labels = LabelsReducer.Invoke(state.Labels, action);
                state.Assignees = AssigneesReducer.Invoke(state.Assignees, action);
                state.Favorites = FavoritesReducer.Invoke(state.Favorites, action);
                state.Search = SearchReducer.Invoke(state.Search, action);
                return state;
            }
        }
    }
}
