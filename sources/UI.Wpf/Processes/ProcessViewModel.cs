using FluentValidation.Results;
using Processes.Core;
using ReactiveUI;
using System;
using System.Collections.Generic;
using UI.Wpf.Properties;

namespace UI.Wpf.Processes
{
	/// <summary>
	/// Process view model interface.
	/// </summary>
	public interface IProcessViewModel
	{
		Guid Id { get; set; }
		bool IsSupported { get; set; }
		string IsSupportedDescription { get; }
		bool IsValid { get; set; }
		string Name { get; set; }
		int OrderIndex { get; set; }
		string PicturePath { get; set; }
		string PicturePathDefault { get; }
		ProcessBasePath ProcessBasePath { get; set; }
		string ProcessBasePathDescription { get; set; }
		string ProcessExecutableName { get; set; }
		string ProcessStartupArgs { get; set; }
		ICollection<EnumViewModel<ProcessBasePath>> ProcessBasePathCollection { get; set; }
		void SetErrors(IEnumerable<ValidationFailure> validationFailures);
	}

	/// <summary>
	/// App process view model implementation.
	/// </summary>
	public class ProcessViewModel : ReactiveObjectWithValidation, IProcessViewModel
	{
		//
		private Guid _id;
		private bool _isSupported;
		private bool _isValid;
		private string _name;
		private int _orderIndex;
		private string _picturePath;
		private ProcessBasePath _processBasePath;
		private string _processBasePathDescription;
		private string _processExecutableName;
		private string _processStartupArgs;
		private ICollection<EnumViewModel<ProcessBasePath>> _processBasePathCollection;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessViewModel()
		{
		}

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
		/// Gets or sets the supported flag description.
		/// </summary>
		public string IsSupportedDescription => _isSupported ? "Supported" : "Not Supported";

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
		/// Gets the default icon path.
		/// </summary>
		public string PicturePathDefault => Resources.ProcessPicturePathDefault;

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
		/// Gets or sets the available process base path options.
		/// </summary>
		public ICollection<EnumViewModel<ProcessBasePath>> ProcessBasePathCollection
		{
			get => _processBasePathCollection;
			set => this.RaiseAndSetIfChanged(ref _processBasePathCollection, value);
		}

		/// <summary>
		/// Gets or sets the form validation state.
		/// </summary>
		public bool IsValid
		{
			get => _isValid;
			set => this.RaiseAndSetIfChanged(ref _isValid, value);
		}

		/// <summary>
		/// Validate the model.
		/// </summary>
		public void SetErrors(IEnumerable<ValidationFailure> validationFailures)
		{
			MergeErrors(validationFailures);

			IsValid = !HasErrors;
		}
	}
}
