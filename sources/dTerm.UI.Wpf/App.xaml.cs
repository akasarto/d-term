using dTerm.UI.Wpf.Views;
using System;
using System.Windows;
using System.Windows.Threading;

namespace dTerm.UI.Wpf
{
	public partial class App : Application
	{
		public App()
		{
			AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
		}

		private void Application_Exit(object sender, ExitEventArgs e)
		{
		}

		private void Application_Startup(object sender, StartupEventArgs e)
		{
			SetupAndShowMainWindow();
		}

		private void SetupAndShowMainWindow()
		{
			Current.MainWindow = new ShellView();

			Current.MainWindow.Show();
		}

		private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs args)
		{
			//ToDo: Log
			args.Handled = true;
		}

		private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			//ToDo: Log
		}
	}
}
