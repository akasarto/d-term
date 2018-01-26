using App.Consoles.Core;
using App.Consoles.Service;
using ReactiveUI;
using System;

namespace UI.Wpf.ViewModels
{
	public class ShellViewModel : ReactiveObject
	{
		private readonly IConsoleProcessService _consoleProcessService = null;

		public ShellViewModel(IConsoleProcessService consoleProcessService)
		{
			_consoleProcessService = consoleProcessService ?? throw new ArgumentNullException(nameof(consoleProcessService), nameof(ShellViewModel));

			CreateConsole = ReactiveCommand.Create(CreateConsoleExecute);
		}

		public ReactiveCommand CreateConsole { get; protected set; }

		private void CreateConsoleExecute()
		{
			var instance = _consoleProcessService.Create(new ProcessDescriptor() { FilePath = @"/cmd.exe", PathType = PathType.SystemPathVar });

			instance.Start();

#warning test
			ShowWindow(instance.MainWindowHandle, new IntPtr(5));
		}

		[System.Runtime.InteropServices.DllImport("user32.dll", ExactSpelling = true)]
		internal static extern bool ShowWindow(IntPtr hwnd, IntPtr nCmdShow);
	}
}
