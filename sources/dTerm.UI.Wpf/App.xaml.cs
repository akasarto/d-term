using dTerm.Core;
using dTerm.Core.PathBuilders;
using dTerm.Core.ProcessStarters;
using dTerm.UI.Wpf.Infrastructure;
using dTerm.UI.Wpf.Models;
using dTerm.UI.Wpf.Services;
using dTerm.UI.Wpf.ViewModels;
using dTerm.UI.Wpf.Views;
using SimpleInjector;
using System;
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
			var consoleInstanceFactory = new ConsoleInstanceFactory();
			var consoleService = new ConsoleService(consoleInstanceFactory);

			var consoleDescriptors = new List<ConsoleDescriptor>()
			{
				new ConsoleDescriptor(ConsoleType.Cmd, new ConsoleProcessStartInfo(new SystemPathVarPathBuilder("/cmd.exe"))) { DisplayOrder = 1 },
				new ConsoleDescriptor(ConsoleType.GitBash, new ConsoleProcessStartInfo(new ProgramFilesFolderPathBuilder("/git/bin/sh.exe"), "--login -i")) { DisplayOrder = 2 },
				new ConsoleDescriptor(ConsoleType.PowerShell, new ConsoleProcessStartInfo(new SystemPathVarPathBuilder("/powershell.exe"))) { DisplayOrder = 3 },
				new ConsoleDescriptor(ConsoleType.UbuntuBash, new ConsoleProcessStartInfo(new System32FolderPathBuilder("/bash.exe"))) { DisplayOrder = 4 }
			};

			Current.MainWindow = new ShellView(new ShellViewModel(consoleDescriptors, consoleService));

			Current.MainWindow.Show();
		}

		private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs args)
		{
			//ToDo: Log
		}

		private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			//ToDo: Log
		}
	}
}
