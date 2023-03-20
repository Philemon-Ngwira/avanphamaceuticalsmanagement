﻿
using Microsoft.AspNetCore.Identity;

namespace avanphamaceuticalsmanagement.Server.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string ProfilePicture { get; set; }
    }
}