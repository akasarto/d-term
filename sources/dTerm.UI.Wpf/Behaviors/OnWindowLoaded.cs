using System.Windows;
using System.Windows.Interop;

namespace dTerm.UI.Wpf.Behaviors
{
	public class OnWindowLoaded
	{
		public static readonly DependencyProperty CallMethodProperty = DependencyProperty.RegisterAttached("CallMethod", typeof(string), typeof(OnWindowLoaded), new PropertyMetadata(null, OnCallMethodPropertyChanged));

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

			window.Loaded += (sender, args) =>
			{
				var model = window.DataContext;
				var methodName = e.NewValue.ToString();
				var interopHelper = new WindowInteropHelper(window);
				var modelMethod = model?.GetType().GetMethod(methodName);

				modelMethod?.Invoke(model, new object[] { interopHelper.Handle });
			};
		}
	}
}
