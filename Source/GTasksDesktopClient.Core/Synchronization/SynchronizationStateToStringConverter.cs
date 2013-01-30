using System;
using System.Globalization;
using System.Windows.Data;

namespace GTasksDesktopClient.Core.Synchronization
{
    public class SynchronizationStateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var state = (SynchronizationState) value;

            switch (state)
            {
                case SynchronizationState.Idle:
                    return "gotowa";
                case SynchronizationState.Connecting:
                    return "łączenie...";
                default:
                    return "wystąpił nieznany błąd";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}