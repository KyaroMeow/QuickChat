using System.Net.Http;
using System.Text;
using System.Text.Json;
using WebApplication2.DTO;

namespace QuickChatApp.WorkAPI
{
    internal class ChatApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public ChatApiClient(HttpClient httpClient, string baseUrl)
        {
            _httpClient = httpClient;
            _baseUrl = baseUrl.TrimEnd('/');
        }

        public async Task<IEnumerable<ChatDTO>> GetUserChatsAsync(int userId)
        {
            var url = $"{_baseUrl}/api/chats?userId={userId}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<ChatDTO>>(responseJson);
        }

        public async Task<ChatDTO> CreateChatAsync(ChatCreateDTO request)
        {
            var url = $"{_baseUrl}/api/chats";
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ChatDTO>(responseJson);
        }
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
    
    // Регистрация ViewModels
    services.AddTransient<MainWindowViewModel>();
    
    ServiceProvider = services.BuildServiceProvider();
    MainWindow = ServiceProvider.GetRequiredService<MainWindow>();
    MainWindow.Show();
}
*/
