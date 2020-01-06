using IssueHub.Models;
using Octokit;

namespace IssueHub.States
{
    public sealed partial class AppState
    {
        public AppState(IFavoritesRepository favoritesRepository)
            : this(favoritesRepository, null, null)
        {
        }

        public AppState(
            IFavoritesRepository favoritesRepository,
            IGitHubClient gitHubClient,
            IAccountsRepository accountsRepository)
        {
            ApiClients = new ApiClientsState(
                gitHubClient: gitHubClient,
                accountsRepository: accountsRepository,
                favoritesRepository: favoritesRepository);
            Login = new LoginState();
            Repositories = new RepositoriesState();
            Favorites = new FavoritesState();
            Issues = new IssuesState();
            IssueComments = new IssueCommentsState();
            IssueForm = new IssueFormState();
            Labels = new LabelsState();
            Assignees = new AssigneesState();
            Search = new SearchState();
        }

        public ApiClientsState ApiClients { get; private set; }

        public LoginState Login { get; private set; }

        public RepositoriesState Repositories { get; private set; }

        public FavoritesState Favorites { get; private set; }

        public IssuesState Issues { get; private set; }

        public IssueCommentsState IssueComments { get; private set; }

        public IssueFormState IssueForm { get; private set; }

        public LabelsState Labels { get; private set; }

        public AssigneesState Assignees { get; private set; }

        public SearchState Search { get; private set; }
    }
}
