using Processes.Core;
using ReactiveUI;
using System;
using System.Reactive;
using System.Reactive.Linq;

namespace UI.Wpf.Consoles
{
	//
	public interface IConsoleInstanceViewModel
	{
		string Name { get; set; }
		bool IsConsole { get; set; }
		int ProcessId { get; }
		IConsoleHwndHost ProcessHost { get; }
		IntPtr ProcessMainModuleHandle { get; }
		IntPtr ProcessMainWindowHandle { get; }
		uint ProcessThreadId { get; }
		IObservable<EventPattern<EventArgs>> ProcessExited { get; }
		void TerminateProcess();
	}

	//
	public class ConsoleInstanceViewModel : ReactiveObject, IConsoleInstanceViewModel
	{
		//
		private readonly IProcess _process;
		private readonly IConsoleHwndHostFactory _processHostFactory;
		private readonly IObservable<EventPattern<EventArgs>> _terminated;

		//
		private string _name;
		private bool _isConsole;
		private IConsoleHwndHost _processHost;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsoleInstanceViewModel(IProcess process, IConsoleHwndHostFactory processHostFactory)
		{
			_process = process ?? throw new ArgumentNullException(nameof(process), nameof(ConsoleInstanceViewModel));
			_processHostFactory = processHostFactory ?? throw new ArgumentNullException(nameof(processHostFactory), nameof(ConsoleInstanceViewModel));

			_terminated = Observable.FromEventPattern<EventHandler, EventArgs>(
				handler => _process.Exited += handler,
				handler => _process.Exited -= handler);
		}

		public string Name
		{
			get => _name;
			set => this.RaiseAndSetIfChanged(ref _name, value);
		}

		public bool IsConsole
		{
			get => _isConsole;
			set => this.RaiseAndSetIfChanged(ref _isConsole, value);
		}

		public IConsoleHwndHost ProcessHost
		{
			get
			{
				if (_processHost == null)
				{
					_processHost = _processHostFactory.Create(_process);
				}

				return _processHost;
			}
		}

		public int ProcessId => _process.Id;

		public IntPtr ProcessMainModuleHandle => _process.MainModuleHandle;

		public IntPtr ProcessMainWindowHandle => _process.MainWindowHandle;

		public uint ProcessThreadId => _process.ThreadId;

		public IObservable<EventPattern<EventArgs>> ProcessExited => _terminated;

		public void TerminateProcess()
		{
			_process.Stop();
		}
	}
}
