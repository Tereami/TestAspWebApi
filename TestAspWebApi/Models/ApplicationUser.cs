using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace TestAspWebApi.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime RegisteredAt { get; set; } = DateTime.Now;

        public ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
