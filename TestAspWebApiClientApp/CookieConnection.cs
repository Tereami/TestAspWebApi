using System.Net.Http.Json;

namespace TestAspWebApiClientApp
{
    public static class CookieConnection
    {
        public async static Task Start(HttpClient client, string apiUrl, string login, string pass)
        {
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
