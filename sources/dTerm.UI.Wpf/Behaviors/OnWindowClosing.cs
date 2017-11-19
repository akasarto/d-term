using System.Windows;

namespace dTerm.UI.Wpf.Behaviors
{
	public class OnWindowClosing
	{
		public static readonly DependencyProperty CallMethodProperty = DependencyProperty.RegisterAttached("CallMethod", typeof(string), typeof(OnWindowClosing), new PropertyMetadata(null, OnCallMethodPropertyChanged));

		public static string GetCallMethod(DependencyObject obj)
		{
			return (string)obj.GetValue(CallMethodProperty);
		}

		public static void SetCallMethod(DependencyObject obj, string value)
		{
			obj.SetValue(CallMethodProperty, value);
		}

		private static void OnCallMethodPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var window = d as Window;

			if (window == null)
				return;

			window.Closing += (sender, args) =>
			{
				var model = window.DataContext;
				var methodName = e.NewValue.ToString();
				var modelMethod = model?.GetType().GetMethod(methodName);

				modelMethod?.Invoke(model, null);
			};
		}
	}
}
