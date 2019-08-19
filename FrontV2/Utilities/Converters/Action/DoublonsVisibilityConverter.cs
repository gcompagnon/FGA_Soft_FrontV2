using System;
using System.Globalization;
using System.Windows.Data;

namespace FrontV2.Converters
{
    class DoublonsVisibilityConverter : IValueConverter
    {

        public object Convert(object value, Type type, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                Boolean val = (Boolean)value;
                if (val)
                {
                    return System.Windows.Visibility.Visible;
                }
                else
                {
                    return System.Windows.Visibility.Hidden;
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value is bool)
            {
                Boolean val = (Boolean)value;
                if (val)
                {
                    return System.Windows.Visibility.Visible;
                }
                else
                {
                    return System.Windows.Visibility.Hidden;
                }
            }

            return null;
        }
    }
}
