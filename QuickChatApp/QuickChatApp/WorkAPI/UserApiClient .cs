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
    internal class UserApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public UserApiClient(HttpClient httpClient, string baseUrl)
        {
            _httpClient = httpClient;
            _baseUrl = baseUrl.TrimEnd('/');
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var url = $"{_baseUrl}/api/users";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<UserDTO>>(responseJson);
        }

        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            var url = $"{_baseUrl}/api/users/{id}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<UserDTO>(responseJson);
        }
    }
}
/*Интеграция в WPF
 * public IServiceProvider ServiceProvider { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        var services = new ServiceCollection();
        var baseUrl = "https://api.example.com";
        
        // Настройка HttpClient
        services.AddSingleton(new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(30)
        });
        
        // Регистрация API клиента
        services.AddSingleton(provider => 
            new UserApiClient(
                provider.GetRequiredService<HttpClient>(),
                baseUrl));
        
        // Регистрация ViewModel
        services.AddTransient<UserViewModel>();
        
        ServiceProvider = services.BuildServiceProvider();
        
        // Создание и отображение главного окна
        var mainWindow = new MainWindow
        {
            DataContext = ServiceProvider.GetRequiredService<UserViewModel>()
        };
        mainWindow.Show();
    }
*/
