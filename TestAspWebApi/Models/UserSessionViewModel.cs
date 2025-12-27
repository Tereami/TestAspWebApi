using System;

namespace TestAspWebApi.Models
{
    public class UserSessionViewModel
    {
        public int Id { get; set; }

        public string IpAddress { get; set; }

        public DateTime LoginTime { get; set; }

        public DateTime ExpiresTime { get; set; }

        public string WindowsVersion { get; set; }

        public string ClientAppVersion { get; set; }

        public bool Active { get; set; }
    }
}
