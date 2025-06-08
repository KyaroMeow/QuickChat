using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using QuickChatApp.WorkAPI;
using WebApplication2.DTO;

namespace QuickChatApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text == textBox.Tag.ToString())
            {
                textBox.Text = "";
                textBox.Foreground = (Brush)textBox.FindResource("PrimaryColor");
			}
        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = textBox.Tag.ToString();
                textBox.Foreground = (Brush)textBox.FindResource("PrimaryColor");
			}
        }

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            PasswordHint.Visibility = Visibility.Collapsed;
        }


        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PasswordBox.Password))
            {
                PasswordHint.Visibility = Visibility.Visible;
            }
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на пустые поля
            if (string.IsNullOrEmpty(NameBox.Text) || NameBox.Text == "Имя" ||
                string.IsNullOrEmpty(LoginBox.Text) || LoginBox.Text == "Логин" ||
                string.IsNullOrEmpty(PasswordBox.Password))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            try
            {
                var userDto = new UserCreateDTO
                {
                    Username = NameBox.Text,
                    Login = LoginBox.Text,
                    Password = AuthApiClient.HashPassword(PasswordBox.Password)
                };

                // Вызов API для регистрации
                var user = await AuthApiClient.Instance.RegisterAsync(userDto);

                if (user != null)
                {
                    MessageBox.Show("Регистрация прошла успешно!");
                    NavigationService.Navigate(new Uri("/Pages/LoginPage.xaml", UriKind.Relative));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка регистрации: {ex.Message}");
            }
        }

        private void AvatarButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
