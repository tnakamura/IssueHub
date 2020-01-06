using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

namespace IssueHub.Models
{
    public interface IAccountsRepository
    {
        Task<Account> ResolveAsync();

        Task StoreAsync(Account account);

        Task DeleteAsync();
    }
}
