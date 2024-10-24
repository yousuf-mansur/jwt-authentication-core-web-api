using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace APIWithJWTAuthentication.Data
{
    public class AppDbContext(DbContextOptions options):IdentityDbContext<ApplicationUser>(options)
    {
    }
}
