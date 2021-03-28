using MaterialDesignThemes.Wpf;
using System;
using System.Globalization;
using System.Windows.Data;

namespace dTerm.UI.Wpf.Converters
{
    public class IconValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!Enum.TryParse<PackIconKind>(value.ToString(), ignoreCase: true, out var packIcon))
            {
                return null;
            }

            return packIcon;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}
