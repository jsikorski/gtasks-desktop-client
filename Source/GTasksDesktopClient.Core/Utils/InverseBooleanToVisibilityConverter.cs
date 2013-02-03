﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GTasksDesktopClient.Core.Utils
{
    public class InverseBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool bValue = false;

            if (value is bool)
                bValue = (bool) value;

            return bValue ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}