using MahApps.Metro.Controls;
using System;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Interop;

namespace dTerm.UI.Wpf.Behaviors
{
	public class WindowHandleProviderBehavior : Behavior<MetroWindow>
	{
		public static readonly DependencyProperty MyPropertyProperty = DependencyProperty.Register("WindowHandle", typeof(IntPtr), typeof(WindowHandleProviderBehavior), new PropertyMetadata(default(IntPtr)));

		public IntPtr WindowHandle
		{
			get { return (IntPtr)GetValue(MyPropertyProperty); }
			set { SetValue(MyPropertyProperty, value); }
		}

		protected override void OnAttached()
		{
			AssociatedObject.Loaded += OnAssociatedWindowLoaded;
		}

		private void OnAssociatedWindowLoaded(object sender, RoutedEventArgs e)
		{
			var windowInteropHelper = new WindowInteropHelper(AssociatedObject);

			WindowHandle = windowInteropHelper.Handle;
		}
	}
}
