using Microsoft.AspNetCore.Identity;

namespace UserManagement.Authetication
{
    public class AppUser:IdentityUser<Guid>
    {
        public string? Name { get; set; }
        public string? IdNumber { get; set; }
    }
}
