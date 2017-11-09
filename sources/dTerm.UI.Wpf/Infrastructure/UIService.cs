using System;
using System.Windows;

namespace dTerm.UI.Wpf.Infrastructure
{
	public static class UIService
	{
		public static void Invoke(Action action)
		{
			var dispatcher = Application.Current?.Dispatcher;

			if (dispatcher == null || dispatcher.CheckAccess())
			{
				action();
				return;
			}

			dispatcher.Invoke(action);
		}
	}
}
