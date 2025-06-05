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
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
			SetPlaceholders();

		}
		private void SetPlaceholders()
		{
			// Установка placeholder для логина
			UsernameBox.Text = "Логин";
			UsernameBox.Foreground = Brushes.Gray;

			// Установка placeholder для пароля (через событие Loaded)
			PasswordBox.Loaded += (s, e) => SetPasswordPlaceholder();
		}
		private void SetPasswordPlaceholder()
		{
			if (string.IsNullOrEmpty(PasswordBox.Password))
			{
				PasswordBox.Background = new VisualBrush(new TextBlock()
				{
					Text = "Пароль",
					Foreground = Brushes.Gray,
					FontSize = 14,
					Margin = new Thickness(5, 0, 0, 0),
					VerticalAlignment = VerticalAlignment.Center
				})
				{ Stretch = Stretch.None, AlignmentX = AlignmentX.Left };
			}
		}
        private void RegisterHyperlink_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/RegisterPage.xaml", UriKind.Relative));
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Пример проверки логина и пароля
            if (string.IsNullOrEmpty(UsernameBox.Text) || string.IsNullOrEmpty(PasswordBox.Password))
            {
                MessageBox.Show("Введите логин и пароль");
                return;
            }
   
            // Здесь можно добавить проверку данных с сервера или локальной базы данных
            //NavigationService.Navigate(new Uri("/Pages/ChatPage.xaml", UriKind.Relative));
        }
        //Методы из xaml-элементов!!!!!!!

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
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text == textBox.Tag.ToString())
            {
                textBox.Text = "";
                textBox.Foreground = Brushes.Black;
            }
        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = textBox.Tag.ToString();
                textBox.Foreground = Brushes.Gray;
            }
        }

        private void UsernameBox_TextChanged(object sender, TextChangedEventArgs e)
		{

        }
    }
}
