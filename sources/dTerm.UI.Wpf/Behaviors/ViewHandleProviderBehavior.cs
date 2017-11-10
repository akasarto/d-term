using System;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Interop;

namespace dTerm.UI.Wpf.Behaviors
{
	public class ViewHandleProviderBehavior : Behavior<Window>
	{
		public static readonly DependencyProperty HandleProperty = DependencyProperty.Register("Handle", typeof(IntPtr), typeof(ViewHandleProviderBehavior), new PropertyMetadata(default(IntPtr)));

		public IntPtr Handle
		{
			get { return (IntPtr)GetValue(HandleProperty); }
			set { SetValue(HandleProperty, value); }
		}

		protected override void OnAttached()
		{
			AssociatedObject.Loaded += OnAssociatedWindowLoaded;
		}

		private void OnAssociatedWindowLoaded(object sender, RoutedEventArgs e)
		{
			var windowInteropHelper = new WindowInteropHelper(AssociatedObject);

			Handle = windowInteropHelper.Handle;
		}
	}
}
