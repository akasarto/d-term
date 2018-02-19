using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace UI.Wpf.Shared
{
	public class IsValidGeometryConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var pathData = value?.ToString() ?? string.Empty;

			try
			{
				return !Geometry.Parse(pathData).IsEmpty();
			}
			catch
			{
				return false;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
	}
}
