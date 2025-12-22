using System.Net.Http.Json;

namespace TestAspWebApiClientApp
{
    public class Program
    {
        private const string baseUrl = "http://localhost:5041/";
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Welcome! Enter 1 to use Cookies. Enter 2 to use JWT.");
            string? method = Console.ReadLine();
            string apiUrl = "apiv1";
            if (method == "2")
                apiUrl = "apiv2";


            Uri baseUri = new Uri(baseUrl);
            HttpClient client = new HttpClient();
            client.BaseAddress = baseUri;
            string login = string.Empty;
            string pass = string.Empty;
            string content = string.Empty;
            HttpResponseMessage loginResponse;

            while (true)
            {
                Console.WriteLine("Имя пользователя");
                login = Console.ReadLine()!;

                Console.WriteLine("Password:");
                pass = Console.ReadLine()!;

                Console.WriteLine("Попытка авторизации...");
                loginResponse = await client.PostAsJsonAsync(
                    $"{apiUrl}/login",
                    new { UserName = login, Password = pass });


                content = await loginResponse.Content.ReadAsStringAsync();

                if (!loginResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Ошибка авторизации: {loginResponse.StatusCode} {content}");
                    continue;
                }
                else
                {
                    Console.WriteLine($"Получен ответ: {content}");
                    break;
                }
            }
            Console.WriteLine("Success!");


            if (method == "1")
            {
                await CookieConnection.Start(client, apiUrl, login, pass);
            }
            else
            {
                await JwtConnection.Start(client, apiUrl, loginResponse);
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
