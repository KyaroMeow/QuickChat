using System;
using System.Windows;
using System.Windows.Data;

namespace QuickChatApp.Converters
{
    public class MessageStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is int senderId && parameter is int currentUserId)
            {
                return senderId == currentUserId ?
                    Application.Current.Resources["OutgoingMessageStyle"] :
                    Application.Current.Resources["IncomingMessageStyle"];
            }
            return Application.Current.Resources["IncomingMessageStyle"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MessageTextStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is int senderId && parameter is int currentUserId)
            {
                return senderId == currentUserId ?
                    Application.Current.Resources["OutgoingMessageTextStyle"] :
                    Application.Current.Resources["MessageTextStyle"];
            }
            return Application.Current.Resources["MessageTextStyle"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MessageTimeStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is int senderId && parameter is int currentUserId)
            {
                return senderId == currentUserId ?
                    Application.Current.Resources["OutgoingMessageTimeStyle"] :
                    Application.Current.Resources["MessageTimeStyle"];
            }
            return Application.Current.Resources["MessageTimeStyle"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}