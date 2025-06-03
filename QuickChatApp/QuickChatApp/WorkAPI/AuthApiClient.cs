using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using WebApplication2.DTO;

namespace QuickChatApp.WorkAPI
{
    internal class AuthApiClient
    {
        private static readonly Lazy<AuthApiClient> _instance =
        new Lazy<AuthApiClient>(() => new AuthApiClient());

        private readonly HttpClient _httpClient;
        private string _baseUrl;

        // Закрытый конструктор (чтобы нельзя было создать экземпляр извне)
        private AuthApiClient()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        // Публичный доступ к единственному экземпляру
        public static AuthApiClient Instance => _instance.Value;

        // Метод для установки базового URL (если он может меняться)
        public void Configure(string baseUrl)
        {
            _baseUrl = baseUrl.TrimEnd('/');
        }

        public async Task<UserDTO> RegisterAsync(UserCreateDTO request)
        {
            if (string.IsNullOrEmpty(_baseUrl))
                throw new InvalidOperationException("Base URL is not configured. Call Configure() first.");

            var url = $"{_baseUrl}/api/auth/register";
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<UserDTO>(responseJson);
        }

        public async Task<UserDTO> LoginAsync(UserLoginDTO request)
        {
            if (string.IsNullOrEmpty(_baseUrl))
                throw new InvalidOperationException("Base URL is not configured. Call Configure() first.");

            var url = $"{_baseUrl}/api/auth/login";
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<UserDTO>(responseJson);
        }
    }
}
/*Интеграция с WPF
 * class Program
{
    static async Task Main(string[] args)
    {
        // Получаем единственный экземпляр
        var apiClient = AuthApiClient.Instance;
        
        // Настраиваем базовый URL (один раз при старте приложения)
        apiClient.Configure("http://your-api-domain.com");

        // Регистрация
        try
        {
            var registerRequest = new RegisterRequest
            {
                Login = "user123",
                Username = "John Doe",
                Password = "securepassword123",
                AvatarUrl = "https://example.com/avatar.jpg"
            };

            var registerResponse = await apiClient.RegisterAsync(registerRequest);
            Console.WriteLine($"Регистрация успешна: {JsonSerializer.Serialize(registerResponse)}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ошибка: {e.Message}");
        }

        // Авторизация
        try
        {
            var loginRequest = new LoginRequest
            {
                Login = "user123",
                Password = "securepassword123"
            };

            var loginResponse = await apiClient.LoginAsync(loginRequest);
            Console.WriteLine($"Авторизация успешна: {JsonSerializer.Serialize(loginResponse)}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ошибка: {e.Message}");
        }
    }
}
*/
