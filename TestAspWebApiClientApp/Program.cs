namespace TestAspWebApiClientApp
{
    public class Program
    {
        private const string baseUrl = "http://localhost:5041/";
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Welcome! Enter 1 to use Login-Pass. Enter 2 to use Tokens.");
            string? method = Console.ReadLine();



            if (method == "1")
            {
                await LoginPasswordConnection.Start(baseUrl);
            }
            else
            {
                await JwtConnection.Start(baseUrl);
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
