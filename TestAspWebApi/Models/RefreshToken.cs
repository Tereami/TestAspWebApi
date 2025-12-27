using System;

namespace TestAspWebApi.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }

        public string Token { get; set; } = Guid.NewGuid().ToString();

        public DateTime ExpiresAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool Revoked { get; set; } = false;

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
