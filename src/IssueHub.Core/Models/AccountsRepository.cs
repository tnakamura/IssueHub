using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace IssueHub.Models
{
    public sealed class AccountsRepository : IAccountsRepository
    {
        const string AccessTokenKey = "GitHubAccessToken";

        const string LoginKey = "GitHubLogin";

        public Task DeleteAsync()
        {
            SecureStorage.Remove(AccessTokenKey);
            SecureStorage.Remove(LoginKey);
            return Task.CompletedTask;
        }

        public async Task<Account> ResolveAsync()
        {
            var accessToken = await SecureStorage.GetAsync(AccessTokenKey);
            var login = await SecureStorage.GetAsync(LoginKey);
            if (!string.IsNullOrEmpty(accessToken) &&
                !string.IsNullOrEmpty(login))
            {
                return new Account(
                    login: login,
                    accessToken: accessToken);
            }
            return null;
        }

        public async Task StoreAsync(Account account)
        {
            if (account == null) throw new ArgumentNullException(nameof(account));

            await SecureStorage.SetAsync(AccessTokenKey, account.AccessToken);
            await SecureStorage.SetAsync(LoginKey, account.Login);
        }
    }
}
