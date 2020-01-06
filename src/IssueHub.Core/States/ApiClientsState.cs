using System;
using System.Threading.Tasks;
using IssueHub.Models;
using Octokit;

namespace IssueHub.States
{
    public sealed class ApiClientsState
    {
        public ApiClientsState(
            IGitHubClient gitHubClient,
            IAccountsRepository accountsRepository,
            IFavoritesRepository favoritesRepository)
        {
            FavoritesRepository = favoritesRepository;
            GitHubClient = gitHubClient ?? new GitHubClient(new ProductHeaderValue("IssueHub"));
            AccountsRepository = accountsRepository ?? new AccountsRepository();
        }

        public IGitHubClient GitHubClient { get; private set; }

        public IAccountsRepository AccountsRepository { get; private set; }

        public IFavoritesRepository FavoritesRepository { get; private set; }
    }
}
