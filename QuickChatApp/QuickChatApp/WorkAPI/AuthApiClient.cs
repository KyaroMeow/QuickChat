using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebApplication2.DTO;

namespace QuickChatApp.WorkAPI
{
    internal class AuthApiClient
    {
        private static readonly Lazy<AuthApiClient> _instance = new Lazy<AuthApiClient>(() => new AuthApiClient());
        private readonly HttpClient _httpClient;

        public static AuthApiClient Instance => _instance.Value;

        private AuthApiClient()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5213/")
            };
        }

        public async Task<UserDTO?> RegisterAsync(UserCreateDTO userDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/register", userDto);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<UserDTO>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ошибка регистрации: {ex.Message}");
            }
        }

        public async Task<UserDTO?> LoginAsync(UserLoginDTO loginDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginDto);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<UserDTO>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ошибка входа: {ex.Message}");
            }
        }

        public static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}