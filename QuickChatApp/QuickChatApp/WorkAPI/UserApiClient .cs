using System.Net.Http;
using System.Net.Http.Json;
using WebApplication2.DTO;

namespace QuickChatApp.WorkAPI
{
    public class UserApiClient
    {
        private static readonly Lazy<UserApiClient> _instance =
        new Lazy<UserApiClient>(() => new UserApiClient());

        private readonly HttpClient _httpClient;

        // Приватный конструктор
        private UserApiClient()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5213/") // Укажите базовый URL API
            };
        }

        // Глобальная точка доступа
        public static UserApiClient Instance => _instance.Value;

        // Метод для получения пользователей
        public async Task<IEnumerable<UserDTO>> GetUsersAsync()
        {
            var response = await _httpClient.GetAsync("api/users");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<IEnumerable<UserDTO>>();
        }
    }
    
}