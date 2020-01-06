using System;
using Octokit;
using SQLite;

namespace IssueHub.Models
{
    [Table("favorites")]
    public class Favorite
    {
        public Favorite()
        {
        }

        public Favorite(Repository repository)
        {
            Id = Ulid.NewUlid().ToString();
            RepositoryId = repository.Id;
            Name = repository.Name;
            FullName = repository.FullName;
            Owner = repository.Owner.Login;
            Private = repository.Private;
        }

        [PrimaryKey]
        [Column("id")]
        public string Id { get; set; }

        [NotNull]
        [Indexed(Name = "favorites_repository_id", Unique = true)]
        [Column("repository_id")]
        public long RepositoryId { get; set; }

        [NotNull]
        [Indexed(Name = "favorites_owner_name", Order = 1)]
        [Column("name")]
        public string Name { get; set; }

        [NotNull]
        [Indexed(Name = "favorites_owner_name", Order = 0)]
        [Column("owner")]
        public string Owner { get; set; }

        [NotNull]
        [Column("full_name")]
        public string FullName { get; set; }

        [NotNull]
        [Column("private")]
        public bool Private { get; set; }

        public Repository ToGitHubRepository()
        {
            return new FavoritedRepository(this);
        }

        class FavoritedRepository : Repository
        {
            public FavoritedRepository(Favorite favorite)
            {
                Id = favorite.RepositoryId;
                Name = favorite.Name;
                FullName = favorite.FullName;
                Private = favorite.Private;
                Owner = new FavoritedUser(favorite);
            }
        }

        class FavoritedUser : User
        {
            public FavoritedUser(Favorite favorite)
            {
                Login = favorite.Owner;
            }
        }
    }
}
