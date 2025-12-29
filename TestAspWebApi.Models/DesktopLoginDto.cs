namespace TestAspWebApi.Models
{
    public record DesktopLoginDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public string MachineId { get; set; }
        public string OsVersion { get; set; }
        public string ClientUserName { get; set; }
    }
}
