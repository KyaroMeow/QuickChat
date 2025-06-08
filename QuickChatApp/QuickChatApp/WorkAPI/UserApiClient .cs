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

        // Метод для получения пользователей по логину
        public async Task<IEnumerable<UserDTO>> GetUserLoginAsync(string? login = null)
        {
            string requestUri = "api/users";

            if (!string.IsNullOrEmpty(login))
            {
                requestUri += $"?login={Uri.EscapeDataString(login)}";
            }

            var response = await _httpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<IEnumerable<UserDTO>>();
        }
        public async Task<UserDTO> GetUserIdAsync(int id)
        {
            string requestUri = $"api/users/{id}";

            // Выполнение GET-запроса к API
            var response = await _httpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode(); // Бросает исключение, если статус не успешный

            // Десериализация ответа в UserDTO
            var userDto = await response.Content.ReadFromJsonAsync<UserDTO>();

            return userDto;
        }
        public async Task<List<UserDTO>> GetUsersByIdsAsync(IEnumerable<int> userIds)
        {
            // Создаем список задач для получения каждого пользователя
            var userTasks = userIds.Select(id => GetUserIdAsync(id)).ToList();

            // Ожидаем завершения всех задач
            var users = await Task.WhenAll(userTasks);

            // Фильтруем результаты, чтобы удалить null (если какие-то пользователи не найдены)
            return users.Where(u => u != null).ToList();
        }
    }
    
}