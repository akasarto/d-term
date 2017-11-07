using dTerm.Core.DataBus;
using dTerm.Core.Processes;
using dTerm.UI.Wpf.Factories;
using dTerm.UI.Wpf.Models;
using dTerm.UI.Wpf.ViewModels;
using dTerm.UI.Wpf.Views;
using NLog;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;

namespace dTerm.UI.Wpf
{
	public partial class App : Application
	{
		private ILogger _logger = null;
		private Container _container = null;

		public App()
		{
			_logger = LogManager.GetCurrentClassLogger();
			_container = Initializer.CreateContainer();
		}

		private void Application_Exit(object sender, ExitEventArgs e)
		{
			DataBus.Dispose();
		}

		private void Application_Startup(object sender, StartupEventArgs e)
		{
			SetupAndShowMainWindow();
		}

		private void SetupAndShowMainWindow()
		{
			var processFactory = new TermConsoleProcessFactory();

			var consoleOptions = new List<ConsoleOption>()
			{
				new ConsoleOption(Core.Entities.ConsoleType.Cmd, new SystemPathProcessStartInfoBuilder("/cmd.exe")) { DisplayOrder = 1 },
				new ConsoleOption(Core.Entities.ConsoleType.GitBash, new ProgramFilesFolderProcessStartInfoBuilder("git/bin/bash.exe")) { DisplayOrder = 2 },
				new ConsoleOption(Core.Entities.ConsoleType.PowerShell, new SystemPathProcessStartInfoBuilder("/powershell.exe")) { DisplayOrder = 3 },
				new ConsoleOption(Core.Entities.ConsoleType.UbuntuBash, new System32FolderProcessStartInfoBuilder("/bash.exe")) { DisplayOrder = 4 }
			};

			Current.MainWindow = new ShellView(new ShellViewModel(processFactory, consoleOptions));

			Current.MainWindow.Closing += (sender, args) =>
			{
				var mainWindow = sender as Window;

				foreach (Window window in mainWindow.OwnedWindows)
				{
					try
					{
						window.Close();
					}
					catch (Exception ex)
					{
						_logger.Fatal(ex);
					}
				}
			};

			Current.MainWindow.Show();
		}

		private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs args)
		{
			_logger.Fatal(args.Exception);
		}
	}
}
