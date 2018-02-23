using Consoles.Core;
using ReactiveUI;
using System;

namespace UI.Wpf.Consoles
{
	public class ConsoleOptionViewModel : ReactiveObject, IConsoleOptionViewModel
	{
		private Guid _id;
		private bool _isSupported;
		private string _name;
		private int _orderIndex;
		private string _picturePath;
		private ProcessBasePath _processBasePath;
		private string _processBasePathDescription;
		private string _processExecutableName;
		private string _processStartupArgs;
		private DateTime _utcCreation;

		/// <summary>
		/// Gets or sets the id.
		/// </summary>
		public Guid Id
		{
			get => _id;
			set => this.RaiseAndSetIfChanged(ref _id, value);
		}

		/// <summary>
		/// Gets or sets the flag indicating if this option can be instantiated by the system.
		/// </summary>
		public bool IsSupported
		{
			get => _isSupported;
			set => this.RaiseAndSetIfChanged(ref _isSupported, value);
		}


		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		public string Name
		{
			get => _name;
			set => this.RaiseAndSetIfChanged(ref _name, value);
		}

		/// <summary>
		/// Gets or sets the order index.
		/// </summary>
		public int OrderIndex
		{
			get => _orderIndex;
			set => this.RaiseAndSetIfChanged(ref _orderIndex, value);
		}

		/// <summary>
		/// Gets or sets the icon path.
		/// </summary>
		public string PicturePath
		{
			get => _picturePath;
			set => this.RaiseAndSetIfChanged(ref _picturePath, value);
		}

		/// <summary>
		/// Gets or sets the process base path.
		/// </summary>
		public ProcessBasePath ProcessBasePath
		{
			get => _processBasePath;
			set => this.RaiseAndSetIfChanged(ref _processBasePath, value);
		}

		/// <summary>
		/// Gets or sets the base path description.
		/// </summary>
		public string ProcessBasePathDescription
		{
			get => _processBasePathDescription;
			set => this.RaiseAndSetIfChanged(ref _processBasePathDescription, value);
		}

		/// <summary>
		/// Gets or sets the process executable file name.
		/// </summary>
		public string ProcessExecutableName
		{
			get => _processExecutableName;
			set => this.RaiseAndSetIfChanged(ref _processExecutableName, value);
		}

		/// <summary>
		/// Gets or sets the process executable startup arguments.
		/// </summary>
		public string ProcessStartupArgs
		{
			get => _processStartupArgs;
			set => this.RaiseAndSetIfChanged(ref _processStartupArgs, value);
		}

		/// <summary>
		/// Gets or sets the UTC creation data and time.
		/// </summary>
		public DateTime UTCCreation
		{
			get => _utcCreation;
			set => this.RaiseAndSetIfChanged(ref _utcCreation, value);
		}
	}
}
