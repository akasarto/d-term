using dTerm.Core;
using dTerm.Core.PathBuilders;
using dTerm.Core.ProcessStarters;
using dTerm.UI.Wpf.Infrastructure;
using dTerm.UI.Wpf.Models;
using dTerm.UI.Wpf.Services;
using dTerm.UI.Wpf.ViewModels;
using System.Collections.Generic;

namespace dTerm.UI.Wpf
{
	public static class Locator
	{
		public static ShellViewModel ShellViewModel => GetShellViewModelInstance();

		private static ShellViewModel GetShellViewModelInstance()
		{
			var consoleFactory = new ConsoleInstanceFactory();
			var consoleService = new ConsoleService(consoleFactory);

			var consoleDescriptors = new List<ConsoleDescriptor>()
			{
				new ConsoleDescriptor(ConsoleType.Cmd, new ConsoleProcessStartInfo(new SystemPathVarPathBuilder("/cmd.exe"))) { DisplayOrder = 1 },
				new ConsoleDescriptor(ConsoleType.GitBash, new ConsoleProcessStartInfo(new ProgramFilesFolderPathBuilder("/git/bin/sh.exe"), "--login -i")) { DisplayOrder = 2 },
				new ConsoleDescriptor(ConsoleType.PowerShell, new ConsoleProcessStartInfo(new SystemPathVarPathBuilder("/powershell.exe"))) { DisplayOrder = 3 },
				new ConsoleDescriptor(ConsoleType.UbuntuBash, new ConsoleProcessStartInfo(new System32FolderPathBuilder("/bash.exe"))) { DisplayOrder = 4 }
			};

			return new ShellViewModel(consoleDescriptors, consoleService);
		}
	}
}
