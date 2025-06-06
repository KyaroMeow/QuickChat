using System.Net.Http;
using System.Net.Http.Json;
using WebApplication2.DTO;

namespace QuickChatApp.WorkAPI
{
    internal class ChatApiClient
    {
        private static readonly Lazy<ChatApiClient> _instance =
            new Lazy<ChatApiClient>(() => new ChatApiClient());

        private readonly HttpClient _httpClient;

        public static ChatApiClient Instance => _instance.Value;

        private ChatApiClient()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5213/")
            };
        }

        // Получение чатов пользователя
        public async Task<List<ChatDTO>> GetUserChatsAsync(int userId)
        {
            var response = await _httpClient.GetAsync($"api/chats?userId={userId}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<ChatDTO>>();
        }

        // Создание нового чата
        public async Task<ChatDTO> CreateChatAsync(ChatCreateDTO chatDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/chats", chatDto);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<ChatDTO>();
        }
    }
}
