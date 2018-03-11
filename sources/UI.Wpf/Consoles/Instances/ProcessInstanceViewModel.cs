﻿using Processes.Core;
using ReactiveUI;
using System;
using System.Reactive;
using System.Reactive.Linq;

namespace UI.Wpf.Consoles
{
	//
	public interface IProcessInstanceViewModel
	{
		string Name { get; set; }
		int ProcessId { get; }
		IntPtr ProcessMainWindowHandle { get; }
		IObservable<EventPattern<EventArgs>> ProcessTerminated { get; }
		void TerminateProcess();
	}

	//
	public class ProcessInstanceViewModel : ReactiveObject, IProcessInstanceViewModel
	{
		private readonly IProcess _process;
		private readonly IObservable<EventPattern<EventArgs>> _terminated;

		private string _name;

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

		public int ProcessId => _process.Id;

		public IntPtr ProcessMainWindowHandle => _process.MainWindowHandle;

		public IObservable<EventPattern<EventArgs>> ProcessTerminated => _terminated;

		public void TerminateProcess()
		{
			_process.Stop();
		}
	}
}
