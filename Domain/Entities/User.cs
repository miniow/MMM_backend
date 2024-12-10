using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class User : IdentityUser
    {
        public string? Initials { get; set; }
    }
}
