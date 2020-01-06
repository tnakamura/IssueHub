using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using ValueTaskSupplement;

namespace IssueHub.Models
{
    public sealed class FavoritesRepository : IFavoritesRepository
    {
        readonly AsyncLazy<SQLiteAsyncConnection> connection;

        public FavoritesRepository(AsyncLazy<SQLiteAsyncConnection> connection)
        {
            this.connection = connection;
        }

        public async Task CreateAsync(Favorite favorite)
        {
            await (await connection).InsertAsync(favorite);
        }

        public async Task DeleteAsync(Favorite favorite)
        {
            await (await connection).DeleteAsync(favorite);
        }

        public async Task<IReadOnlyList<Favorite>> FindAllAsync()
        {
            return await (await connection).Table<Favorite>()
                .ToListAsync();
        }

        public async Task<Favorite> FindAsync(string id)
        {
            return await (await connection).Table<Favorite>()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Favorite> FindByNameAsync(string owner, string name)
        {
            return await (await connection).Table<Favorite>()
                .Where(x => x.Owner == owner)
                .Where(x => x.Name == name)
                .FirstOrDefaultAsync();
        }

        public async Task<Favorite> FindByRepositoryIdAsync(long repositoryId)
        {
            return await (await connection).Table<Favorite>()
                .Where(x => x.RepositoryId == repositoryId)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(Favorite favorite)
        {
            await (await connection).UpdateAsync(favorite);
        }
    }
}
