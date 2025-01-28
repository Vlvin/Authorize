using Authorize.DBModels;
using Microsoft.EntityFrameworkCore;
// using Pomelo.EntityFrameworkCore;


namespace Authorize.Scripts {
  public class AuthorizeContext : DbContext {

      public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsbuilder)
        {
          string connectionString = "Server=127.0.0.1;User Id=mysql;Password=admin;Database=authorize;";
          optionsbuilder.UseMySql(
            connectionString,
            ServerVersion.AutoDetect(connectionString)
          );
        }
    }
}
