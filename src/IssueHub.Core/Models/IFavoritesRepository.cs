using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

namespace IssueHub.Models
{
    public interface IFavoritesRepository
    {
        Task<IReadOnlyList<Favorite>> FindAllAsync();

        Task<Favorite> FindAsync(string id);

        Task<Favorite> FindByNameAsync(string owner, string name);

		Task<Favorite> FindByRepositoryIdAsync(long repositoryId);

        Task CreateAsync(Favorite favorite);

        Task UpdateAsync(Favorite favorite);

        Task DeleteAsync(Favorite favorite);
    }
}
