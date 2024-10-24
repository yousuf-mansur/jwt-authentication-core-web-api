using Microsoft.AspNetCore.Identity;

namespace APIWithJWTAuthentication.Data
{
    public class ApplicationUser:IdentityUser
    {
        public string? Name { get; set; }
    }
}
