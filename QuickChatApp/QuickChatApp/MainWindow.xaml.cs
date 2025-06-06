using QuickChatApp.WorkAPI;
using System.Windows;
using System.Windows.Navigation;
using WebApplication2.DTO;

namespace QuickChatApp
{
    public partial class MainWindow : NavigationWindow
	{
        private const int CurrentUserId = 1; // ID текущего пользователя
        private int _currentChatId = 1; // ID текущего чата
        public MainWindow()
        {
            InitializeComponent();
            LoadMessagesButton_Click();
            LoadUsers();
        }
        //Вывод всех пользователей
        private async void LoadUsers()
        {
            try
            {
                var users = await UserApiClient.Instance.GetUsersAsync();

                var usersText = string.Join("\n", users.Select(u =>
                    $"{u.Username} ({(u.IsOnline ? "Online" : "Offline")})"));

                MessageBox.Show(usersText, "Список пользователей");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка");
            }
        }
        //Регистрация
        private async void RegisterButton_Click()
        {
            try
            {
                var userDto = new UserCreateDTO
                {
                    Login = "Leon",
                    Username = "Test",
                    Password = "123456",
                    AvatarUrl = "https://example.com/avatar.png"
                };

                var registeredUser = await AuthApiClient.Instance.RegisterAsync(userDto);
                MessageBox.Show($"Зарегистрирован: {registeredUser.Username}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        // Логин
        private async void LoginButton_Click()
        {
            try
            {
                var loginDto = new UserLoginDTO
                {
                    Login = "Leon",
                    Password = "123456"
                };

                var user = await AuthApiClient.Instance.LoginAsync(loginDto);
                MessageBox.Show($"Добро пожаловать, {user.Username}!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }
        //загрузка чатов
        private async void LoadChatsButton_Click()
        {
            try
            {
                int currentUserId = 1; // ID текущего пользователя (можно заменить на реальный ID)
                var chats = await ChatApiClient.Instance.GetUserChatsAsync(currentUserId);

                // Формируем текст для MessageBox
                string chatsText = "Ваши чаты:\n\n" +
                    string.Join("\n", chats.Select(c =>
                        $"{c.Name} ({(c.IsGroup ? "Групповой" : "Личный")}) - Участники: {string.Join(", ", c.UserIds)}"));

                MessageBox.Show(chatsText, "Список чатов", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки чатов:\n{ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // Создание нового чата
        private async void CreateChatButton_Click()
        {
            try
            {
                var newChat = new ChatCreateDTO
                {
                    Name = "ИСП-8",
                    IsGroup = true,
                    UserIds = new List<int> {1, 2 } // ID участников чата
                };

                var createdChat = await ChatApiClient.Instance.CreateChatAsync(newChat);
                MessageBox.Show($"Создан чат: {createdChat.Name}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка создания чата: {ex.Message}");
            }
        }
        // Загрузка сообщений чата
        private async void LoadMessagesButton_Click()
        {
            try
            {
                var messages = await MessageApiClient.Instance.GetChatMessagesAsync(_currentChatId);

                string messagesText = $"Сообщения в чате (ID: {_currentChatId}):\n\n";
                messagesText += string.Join("\n---\n", messages.Select(m =>
                    $"[{m.SentAt:HH:mm}] {(m.SenderId == CurrentUserId ? "Вы" : "Отправитель " + m.SenderId)}: {m.Text}" +
                    $"{(m.IsRead ? "" : " (не прочитано)")}"));

                MessageBox.Show(messagesText, "История сообщений",
                              MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки сообщений:\n{ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Отправка нового сообщения
        private async void SendMessageButton_Click()
        {
            try
            {
                string messageText = Microsoft.VisualBasic.Interaction.InputBox(
                    "Введите текст сообщения:", "Новое сообщение", "");

                if (string.IsNullOrEmpty(messageText)) return;

                var newMessage = new MessageCreateDTO
                {
                    ChatId = _currentChatId,
                    SenderId = CurrentUserId,
                    Text = messageText
                };

                var sentMessage = await MessageApiClient.Instance.SendMessageAsync(newMessage);

                MessageBox.Show($"Сообщение отправлено!\nID: {sentMessage.Id}\nВремя: {sentMessage.SentAt:HH:mm:ss}",
                                "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка отправки сообщения:\n{ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Пометить сообщение как прочитанное
        private async void MarkAsReadButton_Click()
        {
            try
            {
                string messageIdStr = Microsoft.VisualBasic.Interaction.InputBox(
                    "Введите ID сообщения:", "Пометить как прочитанное", "");

                if (string.IsNullOrEmpty(messageIdStr)) return;

                if (int.TryParse(messageIdStr, out int messageId))
                {
                    await MessageApiClient.Instance.MarkAsReadAsync(messageId);
                    MessageBox.Show("Сообщение помечено как прочитанное", "Успех");
                }
                else
                {
                    MessageBox.Show("Некорректный ID сообщения", "Ошибка");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка:\n{ex.Message}", "Ошибка");
            }
        }
    }
}