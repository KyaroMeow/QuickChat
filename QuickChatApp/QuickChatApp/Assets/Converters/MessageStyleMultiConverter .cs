using System;
using System.Windows;
using System.Windows.Data;

namespace QuickChatApp.Converters
{
    public class MessageStyleMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is int senderId && values[1] is int currentUserId)
            {
                return senderId == currentUserId ?
                    Application.Current.FindResource("OutgoingMessageStyle") :
                    Application.Current.FindResource("IncomingMessageStyle");
            }
            return Application.Current.FindResource("OutgoingMessageStyle");
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}