using Authorize.DBModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Authorize.Scripts {
  public class AuthorizeContext(DbContextOptions<AuthorizeContext> options) : IdentityDbContext<User>(options) {

        // protected override void OnConfiguring(DbContextOptionsBuilder optionsbuilder)
        // {
        //   string connectionString = "Server=127.0.0.1;User Id=mysql;Password=admin;Database=authorize;";
        //   optionsbuilder.UseMySql(
        //     connectionString,
        //     ServerVersion.AutoDetect(connectionString)
        //   );
        // }
    }
}
