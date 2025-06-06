using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace QuickChatApp.Assets.Converters
{

	public class MessageStyleConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is int senderId)
			{
				// Получаем текущий пользователь ID из параметра или контекста
				int currentUserId = GetCurrentUserId(parameter);

				if (senderId == currentUserId)
				{
					// Исходящее сообщение (от текущего пользователя)
					return Application.Current.FindResource("OutgoingMessageStyle");
				}
				else
				{
					// Входящее сообщение (от другого пользователя)
					return Application.Current.FindResource("IncomingMessageStyle");
				}
			}

			// По умолчанию входящее сообщение
			return Application.Current.FindResource("IncomingMessageStyle");
		}

		private int GetCurrentUserId(object parameter)
		{
			// Пытаемся получить из разных источников
			if (parameter is int userId)
				return userId;

			if (parameter is string userIdStr && int.TryParse(userIdStr, out int parsedId))
				return parsedId;

			// Получаем из DataContext главного окна
			if (Application.Current.MainWindow?.Content is Frame frame &&
				frame.Content is Pages.ChatPage chatPage)
			{
				return chatPage.CurrentUserId;
			}

			return -1; // Невалидный ID
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	// Конвертер для стиля текста сообщения
	public class MessageTextStyleConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is int senderId)
			{
				int currentUserId = GetCurrentUserId(parameter);

				if (senderId == currentUserId)
				{
					// Текст исходящего сообщения (белый текст)
					return Application.Current.FindResource("OutgoingMessageTextStyle");
				}
				else
				{
					// Текст входящего сообщения (обычный цвет)
					return Application.Current.FindResource("MessageTextStyle");
				}
			}

			return Application.Current.FindResource("MessageTextStyle");
		}

		private int GetCurrentUserId(object parameter)
		{
			if (parameter is int userId)
				return userId;

			if (parameter is string userIdStr && int.TryParse(userIdStr, out int parsedId))
				return parsedId;

			if (Application.Current.MainWindow?.Content is Frame frame &&
				frame.Content is Pages.ChatPage chatPage)
			{
				return chatPage.CurrentUserId;
			}

			return -1;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	// Конвертер для стиля времени сообщения
	public class MessageTimeStyleConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is int senderId)
			{
				int currentUserId = GetCurrentUserId(parameter);

				if (senderId == currentUserId)
				{
					// Время исходящего сообщения (светлый цвет)
					return Application.Current.FindResource("OutgoingMessageTimeStyle");
				}
				else
				{
					// Время входящего сообщения (обычный цвет)
					return Application.Current.FindResource("MessageTimeStyle");
				}
			}

			return Application.Current.FindResource("MessageTimeStyle");
		}

		private int GetCurrentUserId(object parameter)
		{
			if (parameter is int userId)
				return userId;

			if (parameter is string userIdStr && int.TryParse(userIdStr, out int parsedId))
				return parsedId;

			if (Application.Current.MainWindow?.Content is Frame frame &&
				frame.Content is Pages.ChatPage chatPage)
			{
				return chatPage.CurrentUserId;
			}

			return -1;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}