using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuickChatApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для ChatPage.xaml
    /// </summary>
    public partial class ChatPage : Page
    {
        public ChatPage()
        {
            InitializeComponent();
            MessageInput.GotFocus += MessageInput_GotFocus;
            MessageInput.LostFocus += MessageInput_LostFocus;
            SidebarPopup.IsOpen = false; // Скрываем меню при загрузке
        }

        private void MessageInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (MessageInput.Text == "Введите сообщение...")
            {
                MessageInput.Text = "";
            }
        }

        private void MessageInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(MessageInput.Text))
            {
                MessageInput.Text = "Введите сообщение...";
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
