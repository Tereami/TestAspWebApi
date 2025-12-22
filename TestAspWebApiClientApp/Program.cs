using System.Net.Http.Json;

namespace TestAspWebApiClientApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Welcome!");

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5041/");
            string login = string.Empty;
            string pass = string.Empty;
            string content = string.Empty;

            while (true)
            {
                Console.WriteLine("Your registered username:");
                login = Console.ReadLine()!;

                Console.WriteLine("Password:");
                pass = Console.ReadLine()!;

                Console.WriteLine("Попытка авторизации...");
                HttpResponseMessage loginResponse = await client.PostAsJsonAsync(
                    "api/login",
                    new { UserName = login, Password = pass });


                content = await loginResponse.Content.ReadAsStringAsync();

                if (!loginResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Ошибка авторизации: {loginResponse.StatusCode} {content}");
                    continue;
                }
                else
                    break;
            }
            Console.WriteLine("Success!");

            while (true)
            {
                Console.WriteLine("Введите команду или exit для выхода");

                string command = Console.ReadLine()!;
                if (command == "exit")
                    return;

                Console.WriteLine("Запрос отправлен...");
                HttpResponseMessage resp = await client.PostAsJsonAsync($"api/{command}",
                    new { UserName = login, Password = pass });


                content = await resp.Content.ReadAsStringAsync();

                if (!resp.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Ошибка: {resp.StatusCode} {content}");
                }
                else
                {
                    Console.WriteLine(content);
                }
            }
        }
    }
}
