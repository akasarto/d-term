using FluentValidation.Results;
using Processes.Core;
using ReactiveUI;
using System;
using System.Collections.Generic;
using UI.Wpf.Properties;

namespace UI.Wpf.Processes
{
	//
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

	//
	public class ProcessViewModel : ReactiveObjectWithValidation, IProcessViewModel
	{
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

		public Guid Id
		{
			get => _id;
			set => this.RaiseAndSetIfChanged(ref _id, value);
		}

		public bool IsSupported
		{
			get => _isSupported;
			set => this.RaiseAndSetIfChanged(ref _isSupported, value);
		}

		public string IsSupportedDescription => _isSupported ? Resources.Supported : Resources.NotSupported;

		public string Name
		{
			get => _name;
			set => this.RaiseAndSetIfChanged(ref _name, value);
		}

		public int OrderIndex
		{
			get => _orderIndex;
			set => this.RaiseAndSetIfChanged(ref _orderIndex, value);
		}

		public string PicturePath
		{
			get => _picturePath;
			set => this.RaiseAndSetIfChanged(ref _picturePath, value);
		}

		public string PicturePathDefault => Resources.ProcessPicturePathDefault;

		public ProcessBasePath ProcessBasePath
		{
			get => _processBasePath;
			set => this.RaiseAndSetIfChanged(ref _processBasePath, value);
		}

		public string ProcessBasePathDescription
		{
			get => _processBasePathDescription;
			set => this.RaiseAndSetIfChanged(ref _processBasePathDescription, value);
		}

		public string ProcessExecutableName
		{
			get => _processExecutableName;
			set => this.RaiseAndSetIfChanged(ref _processExecutableName, value);
		}

		public string ProcessStartupArgs
		{
			get => _processStartupArgs;
			set => this.RaiseAndSetIfChanged(ref _processStartupArgs, value);
		}

		public ICollection<EnumViewModel<ProcessBasePath>> ProcessBasePathCollection
		{
			get => _processBasePathCollection;
			set => this.RaiseAndSetIfChanged(ref _processBasePathCollection, value);
		}

		public bool IsValid
		{
			get => _isValid;
			set => this.RaiseAndSetIfChanged(ref _isValid, value);
		}

		public void SetErrors(IEnumerable<ValidationFailure> validationFailures)
		{
			MergeErrors(validationFailures);

			IsValid = !HasErrors;
		}
	}
}
