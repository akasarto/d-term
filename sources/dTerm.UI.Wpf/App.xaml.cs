using dTerm.Core.DataBus;
using dTerm.Core.Entities;
using dTerm.Core.Processes;
using dTerm.UI.Wpf.Factories;
using dTerm.UI.Wpf.Models;
using dTerm.UI.Wpf.ViewModels;
using dTerm.UI.Wpf.Views;
using SimpleInjector;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;

namespace dTerm.UI.Wpf
{
	public partial class App : Application
	{
		private Container _container = null;

		public App()
		{
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
			var consoleProcessFactory = new TermConsoleProcessFactory();

			var consoleOptions = new List<ConsoleOption>()
			{
				new ConsoleOption(ConsoleType.Cmd, new SystemPathProcessStartInfoBuilder("/cmd.exe")) { DisplayOrder = 1 },
				new ConsoleOption(ConsoleType.GitBash, new ProgramFilesFolderProcessStartInfoBuilder("/git/bin/bash.exe")) { DisplayOrder = 2 },
				new ConsoleOption(ConsoleType.PowerShell, new SystemPathProcessStartInfoBuilder("/powershell.exe")) { DisplayOrder = 3 },
				new ConsoleOption(ConsoleType.UbuntuBash, new System32FolderProcessStartInfoBuilder("/bash.exe")) { DisplayOrder = 4 }
			};

			Current.MainWindow = new ShellView(new ShellViewModel(consoleProcessFactory, consoleOptions));

			Current.MainWindow.Show();
		}

		private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs args)
		{
			//ToDo: Log
		}
	}
}
