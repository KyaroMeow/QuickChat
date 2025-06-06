using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using WebApplication2.DTO;

namespace QuickChatApp.WorkAPI
{
    internal class AuthApiClient
    {
        private static readonly Lazy<AuthApiClient> _instance =
           new Lazy<AuthApiClient>(() => new AuthApiClient());

        private readonly HttpClient _httpClient;

        public static AuthApiClient Instance => _instance.Value;

        private AuthApiClient()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5213/")
            };
        }

        // Регистрация пользователя
        public async Task<UserDTO?> RegisterAsync(UserCreateDTO userDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", userDto);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<UserDTO>();

            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Ошибка регистрации: {error}");
        }

        // Авторизация пользователя
        public async Task<UserDTO?> LoginAsync(UserLoginDTO loginDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginDto);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<UserDTO>();

            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Ошибка входа: {error}");
        }

        // Хэширование пароля (аналогично серверу)
        public static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
