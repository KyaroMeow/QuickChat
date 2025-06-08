using QuickChatApp.WorkAPI;
using System.Security.Policy;
using System.Windows;
using System.Windows.Navigation;
using WebApplication2.DTO;

namespace QuickChatApp
{
    public partial class MainWindow : NavigationWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            ConnectChatHub();
        }
        void ConnectChatHub()
        {
            ChatHubConnection chatHubConnection = new ChatHubConnection("http://localhost:5213/chatHub");
            chatHubConnection.StartAsync();
        }
       
    }
}