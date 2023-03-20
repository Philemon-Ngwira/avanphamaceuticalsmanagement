
using Microsoft.AspNetCore.Identity;

namespace avanphamaceuticalsmanagement.Shared.IdentityModel
{
    public class CustomUser : IdentityUser
    {
        public string ProfilePicture { get; set; }
    }
}
