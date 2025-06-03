using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebApplication2.DTO;

namespace QuickChatApp.WorkAPI
{
    internal class MessageApiClient : IMessageApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public MessageApiClient(HttpClient httpClient, string baseUrl)
        {
            _httpClient = httpClient;
            _baseUrl = baseUrl.TrimEnd('/');
        }

        public async Task<IEnumerable<MessageDTO>> GetAllMessagesAsync()
        {
            var url = $"{_baseUrl}/api/Messages";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<MessageDTO>>(responseJson);
        }

        public async Task<MessageDTO> GetMessageByIdAsync(int id)
        {
            var url = $"{_baseUrl}/api/Messages/{id}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<MessageDTO>(responseJson);
        }

        public async Task<IEnumerable<MessageDTO>> GetMessagesByChatIdAsync(int chatId)
        {
            var url = $"{_baseUrl}/api/Messages/chat/{chatId}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<MessageDTO>>(responseJson);
        }

        public async Task<MessageDTO> CreateMessageAsync(MessageCreateDTO dto)
        {
            var url = $"{_baseUrl}/api/Messages";
            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<MessageDTO>(responseJson);
        }

        public async Task UpdateMessageAsync(int id, MessageUpdateDTO dto)
        {
            var url = $"{_baseUrl}/api/Messages/{id}";
            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(url, content);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteMessageAsync(int id)
        {
            var url = $"{_baseUrl}/api/Messages/{id}";
            var response = await _httpClient.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
        }
    }
    public interface IMessageApiClient
    {
        Task<IEnumerable<MessageDTO>> GetAllMessagesAsync();
        Task<MessageDTO> GetMessageByIdAsync(int id);
        Task<IEnumerable<MessageDTO>> GetMessagesByChatIdAsync(int chatId);
        Task<MessageDTO> CreateMessageAsync(MessageCreateDTO dto);
        Task UpdateMessageAsync(int id, MessageUpdateDTO dto);
        Task DeleteMessageAsync(int id);
    }
}
/*Интеграция с WPF
 * protected override void OnStartup(StartupEventArgs e)
{
    var services = new ServiceCollection();
    var baseUrl = "https://api.example.com";
    
    // Настройка HttpClient
    services.AddSingleton<HttpClient>(_ => new HttpClient
    {
        Timeout = TimeSpan.FromSeconds(30)
    });
    
    // Регистрация API клиентов
    services.AddSingleton<IAuthApiClient>(sp => 
        new AuthApiClient(sp.GetRequiredService<HttpClient>(), baseUrl));
        
    services.AddSingleton<IChatApiClient>(sp =>
        new ChatApiClient(sp.GetRequiredService<HttpClient>(), baseUrl));
        
    services.AddSingleton<IMessageApiClient>(sp =>
        new MessageApiClient(sp.GetRequiredService<HttpClient>(), baseUrl));
    
    // Регистрация ViewModels
    services.AddTransient<MainWindowViewModel>();
    services.AddTransient<ChatViewModel>();
    services.AddTransient<MessageViewModel>();
    
    ServiceProvider = services.BuildServiceProvider();
    MainWindow = ServiceProvider.GetRequiredService<MainWindow>();
    MainWindow.Show();
}
*/