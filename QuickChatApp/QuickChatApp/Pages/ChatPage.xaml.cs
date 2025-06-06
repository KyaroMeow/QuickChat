using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WebApplication2.DTO;

namespace QuickChatApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для ChatPage.xaml
    /// </summary>
    public partial class ChatPage : Page
    {
        public ObservableCollection<Contact> Contacts { get; set; }
        private UserDTO _currentUser;

        public ChatPage(UserDTO currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            Contacts = new ObservableCollection<Contact>();
            DataContext = this;

            LoadUserData();
            LoadContactsAsync();

            MessageInput.GotFocus += MessageInput_GotFocus;
            MessageInput.LostFocus += MessageInput_LostFocus;
            SidebarPopup.IsOpen = false;
        }

        private void LoadUserData()
        {
            // Установка данных текущего пользователя
            if (_currentUser != null)
            {
                // Здесь можно загрузить аватар, если он есть
                // Для примера просто установим имя пользователя
                UserNameTextBlock.Text = _currentUser.Username;
                UserStatusTextBlock.Text = _currentUser.IsOnline ? "online" : "offline";
            }
        }

        private async void LoadContactsAsync()
        {
            try
            {
                // Здесь должен быть запрос к API для получения контактов
                // Пока используем заглушку, но с реальными данными пользователя
                var contacts = await GetContactsFromApi(_currentUser.Id);

                Contacts.Clear();
                foreach (var contact in contacts)
                {
                    Contacts.Add(new Contact
                    {
                        Name = contact.Username,
                        LastMessage = contact.IsOnline ? "online" : "offline",
                        AvatarColor = GetRandomColorBrush(),
                        IsOnline = contact.IsOnline
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки контактов: {ex.Message}");
            }
        }

        private async Task<List<UserDTO>> GetContactsFromApi(int userId)
        {
            // Заглушка - в реальности здесь должен быть запрос к API
            // Например: return await ChatApiClient.Instance.GetUserContactsAsync(userId);

            // Временная заглушка с тестовыми данными
            return new List<UserDTO>
            {
                new UserDTO { Id = 2, Username = "Анна Петрова", IsOnline = true },
                new UserDTO { Id = 3, Username = "Михаил Козлов", IsOnline = false },
                new UserDTO { Id = 4, Username = "Елена Смирнова", IsOnline = true }
            };
        }

        private Brush GetRandomColorBrush()
        {
            var colors = new[]
            {
                Brushes.LightBlue,
                Brushes.LightGreen,
                Brushes.LightSalmon,
                Brushes.LightPink,
                Brushes.LightGoldenrodYellow
            };

            var random = new Random();
            return colors[random.Next(colors.Length)];
        }

        public class Contact
        {
            public string Name { get; set; }
            public string LastMessage { get; set; }
            public Brush AvatarColor { get; set; }
            public bool IsOnline { get; set; }
        }
        private void MessageInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (MessageInput.Text == "Введите сообщение...")
            {
                MessageInput.Text = "";
            }
        }
        // Открытие popup при нажатии на "Список контактов" в сайдбаре
        private void ContactsListButton_Click(object sender, RoutedEventArgs e)
        {
            ContactsPopup.IsOpen = true;
        }

        // Закрытие popup
        private void CloseContactsPopup_Click(object sender, RoutedEventArgs e)
        {
            ContactsPopup.IsOpen = false;
        }
        // Обработчик кнопки выхода в сайдбаре
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LogoutConfirmationPopup.IsOpen = true;
        }

        // Подтверждение выхода
        private void ConfirmLogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LogoutConfirmationPopup.IsOpen = false;
            NavigationService.Navigate(new Uri("/Pages/LoginPage.xaml", UriKind.Relative));
        }
        // Открытие попапа (вызовите этот метод из кнопки в сайдбаре)
        private void OpenProfileEditPopup_Click(object sender, RoutedEventArgs e)
        {
            ProfileEditPopup.IsOpen = true;
        }

        // Закрытие попапа
        private void CloseProfileEditPopup_Click(object sender, RoutedEventArgs e)
        {
            ProfileEditPopup.IsOpen = false;
        }

        // Применение изменений
        private void ApplyProfileChanges_Click(object sender, RoutedEventArgs e)
        {
            // Здесь будет логика сохранения изменений
            // Пока просто закроем попап
            ProfileEditPopup.IsOpen = false;

            // Можно добавить уведомление об успешном сохранении
            MessageBox.Show("Изменения сохранены!", "Успех",
                           MessageBoxButton.OK, MessageBoxImage.Information);
        }
        // Отмена выхода
        private void CancelLogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LogoutConfirmationPopup.IsOpen = false;
        }
        private void MessageInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(MessageInput.Text))
            {
                MessageInput.Text = "Введите сообщение...";
            }
        }
        private void CloseSidebarButton_Click(object sender, RoutedEventArgs e)
        {
            SidebarPopup.IsOpen = false;
        }
        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            SidebarPopup.IsOpen = true;
        }



        private void OverlayGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Закрываем сайдбар при клике на затемненную область
            var element = e.OriginalSource as FrameworkElement;
            if (element?.Name != "SidebarPopup" && SidebarPopup.IsOpen)
            {
                SidebarPopup.IsOpen = false;
            }
        }
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(MessageInput.Text) && MessageInput.Text != "Введите сообщение...")
            {
                // Создаем новое сообщение
                Border messageBorder = new Border();
                messageBorder.Style = (Style)FindResource("OutgoingMessageStyle");

                StackPanel stackPanel = new StackPanel();
                TextBlock messageText = new TextBlock();
                messageText.Text = MessageInput.Text;
                messageText.Style = (Style)FindResource("OutgoingMessageTextStyle");

                TextBlock timeText = new TextBlock();
                timeText.Text = DateTime.Now.ToString("HH:mm");
                timeText.Style = (Style)FindResource("OutgoingMessageTimeStyle");

                stackPanel.Children.Add(messageText);
                stackPanel.Children.Add(timeText);
                messageBorder.Child = stackPanel;

                MessagesPanel.Children.Add(messageBorder);
                MessageInput.Text = "";

                // Прокрутка вниз
                MessagesScrollViewer.ScrollToBottom();
            }
        }
    }
}
