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
		int Pid { get; }
		string Name { get; set; }
		IProcessHost Host { get; }
		IntPtr MainWindowHandle { get; }
		IObservable<EventPattern<EventArgs>> Terminated { get; }
	}

	//
	public class ProcessInstanceViewModel : ReactiveObject, IProcessInstanceViewModel
	{
		//
		private readonly IProcess _process;
		private readonly IProcessHostFactory _processHostFactory;
		private readonly IObservable<EventPattern<EventArgs>> _terminated;

		//
		private string _name;
		private IProcessHost _processHost;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessInstanceViewModel(IProcess process, IProcessHostFactory processHostFactory)
		{
			_process = process ?? throw new ArgumentNullException(nameof(process), nameof(ProcessInstanceViewModel));
			_processHostFactory = processHostFactory ?? throw new ArgumentNullException(nameof(processHostFactory), nameof(ProcessInstanceViewModel));

			_terminated = Observable.FromEventPattern<EventHandler, EventArgs>(
				handler => _process.Exited += handler,
				handler => _process.Exited -= handler);
		}

		public int Pid => _process.Id;

		public string Name
		{
			get => _name;
			set => this.RaiseAndSetIfChanged(ref _name, value);
		}

		public IProcessHost Host
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

		public IntPtr MainWindowHandle => _process.MainWindowHandle;

		public IObservable<EventPattern<EventArgs>> Terminated => _terminated;
	}
}
