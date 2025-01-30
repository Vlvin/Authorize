using Authorize.DBModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Authorize.Scripts {
  public class AuthorizeContext(DbContextOptions<AuthorizeContext> options) : IdentityDbContext<User>(options) 
    {
    }
}
