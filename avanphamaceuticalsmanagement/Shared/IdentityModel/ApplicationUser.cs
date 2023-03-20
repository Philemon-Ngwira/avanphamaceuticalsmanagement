
using Microsoft.AspNetCore.Identity;

namespace avanphamaceuticalsmanagement.Shared.IdentityModel
{
    public class ApplicationUser : IdentityUser
    {
        public string ProfilePicture { get; set; }
    }
}