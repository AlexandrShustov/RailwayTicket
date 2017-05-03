using System.Data.Entity;
using Domain.Entities;

namespace DAL
{
    public class ApplicationDbContext : DbContext
    {
        internal ApplicationDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        internal IDbSet<User> Users { get; set; }
        internal IDbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new RoleConfiguration());
        }
    }
}