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

		//Методы из xaml-элементов!!!!!!!
		private void LoginButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
		{

		}

		private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
		{

		}

		private void TextBox_GotFocus(object sender, RoutedEventArgs e)
		{

		}

		private void TextBox_LostFocus(object sender, RoutedEventArgs e)
		{

		}

		private void RegisterHyperlink_Click(object sender, RoutedEventArgs e)
		{

		}

		private void UsernameBox_TextChanged(object sender, TextChangedEventArgs e)
		{

        }
    }
}
