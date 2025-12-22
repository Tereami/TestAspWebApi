using System.Net.Http.Json;


namespace TestAspWebApiClientApp
{
    record TokenResponse(string Token);

    public static class JwtConnection
    {
        public static async Task Start(HttpClient client, string apiUrl, HttpResponseMessage response)
        {
            TokenResponse? json = await response.Content.ReadFromJsonAsync<TokenResponse>();
            if (json == null)
                throw new Exception("FAILED TO READ RESPONSE TOKEN!");

            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", json.Token);

            while (true)
            {
                Console.WriteLine("Введите команду date или exit для выхода");

                string command = Console.ReadLine()!;
                if (command == "exit")
                    return;

                Console.WriteLine("Запрос отправлен...");
                var cmdResponse = await client.GetAsync($"{apiUrl}/{command}");


                string content = await cmdResponse.Content.ReadAsStringAsync();

                if (!cmdResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Ошибка: {cmdResponse.StatusCode} {content}");
                }
                else
                {
                    Console.WriteLine(content);
                }
            }
        }
    }
}
