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
                    Application.Current.FindResource("OutgoingMessageStyle") :
                    Application.Current.FindResource("IncomingMessageStyle");
            }
            return Application.Current.FindResource("IncomingMessageStyle");
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
                    Application.Current.FindResource("OutgoingMessageTextStyle") :
                    Application.Current.FindResource("MessageTextStyle");
            }
            return Application.Current.FindResource("MessageTextStyle");
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
                    Application.Current.FindResource("OutgoingMessageTimeStyle") :
                    Application.Current.FindResource("MessageTimeStyle");
            }
            return Application.Current.FindResource("MessageTimeStyle");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}