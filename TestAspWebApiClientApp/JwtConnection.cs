using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using TestAspWebApi.Models;

namespace TestAspWebApiClientApp
{
    record Tokens
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }

    public static class JwtConnection
    {
        private static readonly string apiUrl = "apiv2";

        private static string tokenJsonFilePath = "";

        public static async Task Start(string baseUrl)
        {
            string appDataFlder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "TestAspWebApiClientApp");
            if (!Directory.Exists(appDataFlder))
            {
                Directory.CreateDirectory(appDataFlder);
                Console.WriteLine($"Создана папка: {appDataFlder}");
            }
            tokenJsonFilePath = Path.Combine(appDataFlder, "tokens.json");


            HttpClient client = new HttpClient() { BaseAddress = new Uri(baseUrl) };

            Tokens tokens;

            if (File.Exists(tokenJsonFilePath))
            {
                Console.WriteLine($"Найден файл {tokenJsonFilePath}");
                string readFile = await File.ReadAllTextAsync(tokenJsonFilePath);
                tokens = JsonSerializer.Deserialize<Tokens>(readFile)!;
            }
            else
            {
                Console.WriteLine($"Не найден файл сохраненных токенов. Новый вход...");
                tokens = await LoginAsync(client);
            }

            tokens = await TryAuthorizedRequest(client, tokens);


            Console.WriteLine($"Успешный вход!");

            while (true)
            {
                Console.WriteLine("Введите команду date или exit для выхода");

                string command = Console.ReadLine()!;
                if (command == "exit")
                    return;

                if (command == "date")
                {
                    tokens = await TryAuthorizedRequest(client, tokens);
                    await GetDateAsync(client);
                }
            }
        }

        static async Task<Tokens> LoginAsync(HttpClient client)
        {
            string login = string.Empty;
            string pass = string.Empty;

            DesktopLoginDto loginDto = new DesktopLoginDto
            {
                UserName = login,
                Password = pass,
                MachineId = MachineIdGenerator.Get(),
                OsVersion = Environment.OSVersion.ToString(),
                ClientUserName = Environment.UserName,
            };


            while (true)
            {
                Console.WriteLine("Имя пользователя");
                login = Console.ReadLine()!;

                Console.WriteLine("Password:");
                pass = Console.ReadLine()!;

                Console.WriteLine("Попытка авторизации по логину и паролю...");
                HttpResponseMessage loginResponse = await client.PostAsJsonAsync(
                    $"{apiUrl}/login",
                    loginDto);

                string content = await loginResponse.Content.ReadAsStringAsync();

                if (loginResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Получен ответ: {content}");
                    Tokens? tokens = await loginResponse.Content.ReadFromJsonAsync<Tokens>();

                    SaveTokens(tokens!);
                    SetAccessToken(client, tokens!);

                    Console.WriteLine("Успешно, токены загружены!");
                    return tokens!;
                }
                else
                {
                    Console.WriteLine($"Ошибка авторизации: {loginResponse.StatusCode} {content}");
                    continue;
                }
            }
        }

        static async Task<bool> RefreshAsync(HttpClient client, Tokens tokens)
        {
            Console.WriteLine("Обновление access token...");
            HttpResponseMessage response = await client.PostAsJsonAsync($"{apiUrl}/refresh", tokens.RefreshToken);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Ошибка: {response.StatusCode}");
                return false;
            }


            JsonElement json = await response.Content.ReadFromJsonAsync<JsonElement>();
            string newAccessToken = json.GetProperty("accessToken").GetString()!;
            Console.WriteLine($"New access token: {newAccessToken}");
            tokens.AccessToken = newAccessToken;

            SaveTokens(tokens);
            SetAccessToken(client, tokens);
            return true;
        }

        static async Task<Tokens> TryAuthorizedRequest(HttpClient client, Tokens tokens)
        {
            Console.WriteLine("Проверка запроса, требующего авторизации....");
            SetAccessToken(client, tokens);

            HttpResponseMessage response = await client.GetAsync($"{apiUrl}/date");
            Console.WriteLine(response.StatusCode);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                Console.WriteLine("Токен недействителен! Пробую обновить...");
                bool refreshResult = await RefreshAsync(client, tokens);
                if (!refreshResult)
                {
                    Console.WriteLine("Требуется повторный вход");
                    tokens = await LoginAsync(client);
                }
            }
            return tokens;
        }

        static async Task GetDateAsync(HttpClient client)
        {
            HttpResponseMessage response = await client.GetAsync($"{apiUrl}/date");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
                return;
            }

            string content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(content);
        }

        static void SetAccessToken(HttpClient client, Tokens tokens)
        {
            string accessToken = tokens.AccessToken;
            client.DefaultRequestHeaders.Authorization =
                       new AuthenticationHeaderValue("Bearer", accessToken);
            Console.WriteLine($"В headers установлен access токен: {accessToken}");
        }

        static async void SaveTokens(Tokens t)
        {
            string tokensJson = JsonSerializer.Serialize(t, new JsonSerializerOptions
            {
                WriteIndented = true,
            });
            await File.WriteAllTextAsync(tokenJsonFilePath, tokensJson);
            Console.WriteLine($"Токен {tokensJson} сохранен в файл: {tokenJsonFilePath}");
        }


    }
}
