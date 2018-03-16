using Processes.Core;
using ReactiveUI;
using System;
using System.Reactive;
using System.Reactive.Linq;

namespace UI.Wpf.Processes
{
	//
	public interface IProcessInstanceViewModel
	{
		string Name { get; set; }
		bool IsElevated { get; set; }
		bool IsMinimized { get; set; }
		string MinimizedTooltip { get; }
		string PicturePath { get; set; }
		int ProcessId { get; }
		IntPtr ProcessMainWindowHandle { get; }
		IObservable<EventPattern<EventArgs>> ProcessTerminated { get; }
		void KillProcess();
	}

	//
	public class ProcessInstanceViewModel : ReactiveObject, IProcessInstanceViewModel
	{
		private readonly IProcess _process;
		private readonly IObservable<EventPattern<EventArgs>> _terminated;

		private string _name;
		private bool _isElevated;
		private bool _isMinimized;
		private string _picturePath;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessInstanceViewModel(IProcess process)
		{
			_process = process ?? throw new ArgumentNullException(nameof(process), nameof(ProcessInstanceViewModel));

			_terminated = Observable.FromEventPattern<EventHandler, EventArgs>(
				handler => _process.Exited += handler,
				handler => _process.Exited -= handler);
		}

		public string Name
		{
			get => _name;
			set => this.RaiseAndSetIfChanged(ref _name, value);
		}

		public bool IsElevated
		{
			get => _isElevated;
			set => this.RaiseAndSetIfChanged(ref _isElevated, value);
		}

		public bool IsMinimized
		{
			get => _isMinimized;
			set => this.RaiseAndSetIfChanged(ref _isMinimized, value);
		}

		public string PicturePath
		{
			get => _picturePath;
			set => this.RaiseAndSetIfChanged(ref _picturePath, value);
		}

		public int ProcessId => _process.Id;

		public IntPtr ProcessMainWindowHandle => Win32Api.GetProcessWindow(ProcessId);

		public IObservable<EventPattern<EventArgs>> ProcessTerminated => _terminated;

		public string MinimizedTooltip => Win32Api.GetWindowTitleClean(ProcessMainWindowHandle);

		public void KillProcess() => _process.Kill();
	}
}
