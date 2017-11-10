using dTerm.Core.DataBus;
using dTerm.Core.Entities;
using dTerm.Core.Processes;
using dTerm.UI.Wpf.Infrastructure;
using dTerm.UI.Wpf.Models;
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
			DataBus.Dispose();
		}

		private void Application_Startup(object sender, StartupEventArgs e)
		{
			SetupAndShowMainWindow();
		}

		private void SetupAndShowMainWindow()
		{
			var factory = new ConsoleInstanceFactory();

			var consoleDescriptors = new List<ConsoleDescriptor>()
			{
				new ConsoleDescriptor(ConsoleType.Cmd, new SystemPathProcessStartInfoBuilder("/cmd.exe")) { DisplayOrder = 1 },
				new ConsoleDescriptor(ConsoleType.GitBash, new ProgramFilesFolderProcessStartInfoBuilder("/git/bin/bash.exe")) { DisplayOrder = 2 },
				new ConsoleDescriptor(ConsoleType.PowerShell, new SystemPathProcessStartInfoBuilder("/powershell.exe")) { DisplayOrder = 3 },
				new ConsoleDescriptor(ConsoleType.UbuntuBash, new System32FolderProcessStartInfoBuilder("/bash.exe")) { DisplayOrder = 4 }
			};

			Current.MainWindow = new ShellView(new ShellViewModel(consoleDescriptors, factory));

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
