using System;
using System.Globalization;
using System.Windows.Data;

namespace UI.Wpf.Notebook
{
	public class ListItemWidthConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			var actualWidth = System.Convert.ToDouble(values[0].ToString());

			return actualWidth;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}
