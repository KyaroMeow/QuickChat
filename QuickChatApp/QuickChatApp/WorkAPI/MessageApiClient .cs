using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebApplication2.DTO;

namespace QuickChatApp.WorkAPI
{
    internal class MessageApiClient
    {
        private static readonly Lazy<MessageApiClient> _instance =
            new Lazy<MessageApiClient>(() => new MessageApiClient());

        private readonly HttpClient _httpClient;

        public static MessageApiClient Instance => _instance.Value;

        private MessageApiClient()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5213/")
            };
        }

        // Получение всех сообщений
        public async Task<List<MessageDTO>> GetAllMessagesAsync()
        {
            var response = await _httpClient.GetAsync("api/Messages");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<MessageDTO>>();
        }

        // Получение сообщения по ID
        public async Task<MessageDTO> GetMessageAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/Messages/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<MessageDTO>();
        }

        // Получение сообщений чата
        public async Task<List<MessageDTO>> GetChatMessagesAsync(int chatId)
        {
            var response = await _httpClient.GetAsync($"api/Messages/chat/{chatId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<MessageDTO>>();
        }

        // Отправка сообщения
        public async Task<MessageDTO> SendMessageAsync(MessageCreateDTO messageDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Messages", messageDto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<MessageDTO>();
        }

        // Обновление статуса прочтения
        public async Task MarkAsReadAsync(int messageId, bool isRead = true)
        {
            var updateDto = new MessageUpdateDTO { IsRead = isRead };
            var response = await _httpClient.PutAsJsonAsync($"api/Messages/{messageId}", updateDto);
            response.EnsureSuccessStatusCode();
        }

        // Удаление сообщения
        public async Task DeleteMessageAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Messages/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}