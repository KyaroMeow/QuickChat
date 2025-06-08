using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using QuickChatApp.WorkAPI;
using WebApplication2.DTO;

namespace QuickChatApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для ChatPage.xaml
    /// </summary>
    public partial class ChatPage : Page
    {

       
        public ObservableCollection<Contact> Contacts { get; set; }
        public ObservableCollection<MessageDTO> Messages { get; set; }
        private UserDTO _currentUser;
        private int _currentChatId = -1; // Текущий выбранный чат
        private ChatHubConnection chatHubConnection;
        public static readonly DependencyProperty CurrentUserIdProperty =
        DependencyProperty.Register("CurrentUserId", typeof(int), typeof(ChatPage));
        public ChatPage(UserDTO currentUser)
        {
            InitializeComponent();
            chatHubConnection = new ChatHubConnection("http://localhost:5213/chatHub");
            Contacts = new ObservableCollection<Contact>();
            Messages = new ObservableCollection<MessageDTO>();
            _currentUser = currentUser;

            // Устанавливаем CurrentUserId
            CurrentUserId = _currentUser.Id;

            DataContext = this;
            chatHubConnection.StartAsync();
            LoadContactsAsync();

            MessageInput.GotFocus += MessageInput_GotFocus;
            MessageInput.LostFocus += MessageInput_LostFocus;
            SidebarPopup.IsOpen = false;
        }
        public int CurrentUserId
        {
            get { return (int)GetValue(CurrentUserIdProperty); }
            set { SetValue(CurrentUserIdProperty, value);}
        }

        private async void LoadContactsAsync()
        {
            try
            {
                // Получаем список пользователей (контактов) через API
                var chats = await ChatApiClient.Instance.GetUserChatsAsync(CurrentUserId);
                var allUserIds = chats
            .SelectMany(chat => chat.UserIds)
            .Distinct()
            .Where(id => id != CurrentUserId)
            .ToList();
                var chatIds = chats.Select(c => c.Id).ToList();
                chatHubConnection.JoinAllUserChatsAsync(chatIds);
                var users = await UserApiClient.Instance.GetUsersByIdsAsync(allUserIds);
                
                Contacts.Clear();
                foreach (var contact in users)
                {
                    Contacts.Add(new Contact
                    {
                        Id = contact.Id,
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

        private async void LoadChatMessages(int chatId)
        {
            try
            {
                Messages.Clear();
                var messages = await MessageApiClient.Instance.GetChatMessagesAsync(chatId);

                foreach (var message in messages.OrderBy(m => m.SentAt))
                {
                    Messages.Add(message);
                }

                // Прокрутка к последнему сообщению
                MessagesScrollViewer.ScrollToBottom();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки сообщений: {ex.Message}");
            }
        }

        private async void CreateOrSelectChat(int contactId)
        {
            try
            {
                // Проверяем, есть ли уже чат с этим пользователем
                var userChats = await ChatApiClient.Instance.GetUserChatsAsync(_currentUser.Id);
                var existingChat = userChats.FirstOrDefault(c =>
                     c.UserIds.Contains(contactId));

                if (existingChat != null)
                {
                    _currentChatId = existingChat.Id;
                    ChatTitleTextBlock.Text = Contacts.First(c => c.Id == contactId).Name;
                    LoadChatMessages(_currentChatId);
                }
                else
                {
                    // Создаем новый чат
                    var newChat = await ChatApiClient.Instance.CreateChatAsync(new ChatCreateDTO
                    {
                        UserIds = new List<int> { _currentUser.Id, contactId }
                    });

                    _currentChatId = newChat.Id;
                    ChatTitleTextBlock.Text = Contacts.First(c => c.Id == contactId).Name;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка создания/выбора чата: {ex.Message}");
            }
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentChatId == -1)
            {
                MessageBox.Show("Выберите чат для отправки сообщения");
                return;
            }

            if (!string.IsNullOrEmpty(MessageInput.Text) && MessageInput.Text != "Введите сообщение...")
            {
                try
                {
                    var messageDto = new MessageCreateDTO
                    {
                        ChatId = _currentChatId,
                        SenderId = _currentUser.Id,
                        Text = MessageInput.Text
                    };

                    var sentMessage = await MessageApiClient.Instance.SendMessageAsync(messageDto);
                    Messages.Add(sentMessage);

                    MessageInput.Text = "";
                    MessagesScrollViewer.ScrollToBottom();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка отправки сообщения: {ex.Message}");
                }
            }
        }

        // Обработчик выбора контакта из списка
        private void ContactItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is Contact contact)
            {
                CreateOrSelectChat(contact.Id);
                ContactsPopup.IsOpen = false;
            }
        }

        // Обновленный класс Contact
        public class Contact
        {
            public int Id { get; set; }
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
    }
}
