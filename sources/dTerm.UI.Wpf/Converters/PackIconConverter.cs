using MaterialDesignThemes.Wpf;
using System;
using System.Globalization;
using System.Windows.Data;

namespace dTerm.UI.Wpf.Converters
{
    public class PackIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!Enum.TryParse<PackIconKind>((value ?? string.Empty).ToString(), ignoreCase: true, out var kind))
            {
                kind = PackIconKind.QuestionMark;
            }

            return kind;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Keep the exception as we want to know if this ever gets used.
            throw new NotImplementedException();
        }
    }
}
