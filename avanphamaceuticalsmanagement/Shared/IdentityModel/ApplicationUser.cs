
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace avanphamaceuticalsmanagement.Shared.IdentityModel
{
    public class ApplicationUser : IdentityUser
    {
        [Column("ProfilePicture")]
        public string ProfilePicture { get; set; }
    }
}