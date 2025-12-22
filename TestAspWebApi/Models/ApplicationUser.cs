using Microsoft.AspNetCore.Identity;
using System;

namespace TestAspWebApi.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime RegisteredAt { get; set; } = DateTime.Now;
    }
}
