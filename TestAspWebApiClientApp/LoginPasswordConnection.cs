using System.Net.Http.Json;

namespace TestAspWebApiClientApp
{
    public static class LoginPasswordConnection
    {
        private static readonly string apiUrl = "apiv1";

        public async static Task Start(string baseUrl)
        {
            Uri baseUri = new Uri(baseUrl);
            HttpClient client = new HttpClient();
            client.BaseAddress = baseUri;
            string login = string.Empty;
            string pass = string.Empty;
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


                string content = await loginResponse.Content.ReadAsStringAsync();

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

            while (true)
            {
                Console.WriteLine("Введите команду date или exit для выхода");

                string command = Console.ReadLine()!;
                if (command == "exit")
                    return;

                Console.WriteLine("Запрос отправлен...");
                HttpResponseMessage resp = await client.PostAsJsonAsync($"{apiUrl}/{command}",
                    new { UserName = login, Password = pass });


                string content = await resp.Content.ReadAsStringAsync();

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
