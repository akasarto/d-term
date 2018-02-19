using System;
using Consoles.Core;
using ReactiveUI;

namespace UI.Wpf.Consoles
{
	public class ConsoleOptionFormViewModel : BaseViewModel
	{
		private Guid _id;
		private string _name;
		private int _orderIndex;
		private string _picturePath;
		private BasePath _processBasePath;
		private string _processExecutableName;
		private string _processStartupArgs;
		private bool _isValid;

		//
		private readonly ConsoleOptionFormViewModelValidator _consoleOptionFormViewModelValidator = null;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsoleOptionFormViewModel()
		{
			_consoleOptionFormViewModelValidator = new ConsoleOptionFormViewModelValidator();
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
		/// Gets or sets the name.
		/// </summary>
		public string PicturePath
		{
			get => _picturePath;
			set => this.RaiseAndSetIfChanged(ref _picturePath, value);
		}

		/// <summary>
		/// Gets or sets the base path for the process exe filename.
		/// </summary>
		public BasePath ProcessBasePath
		{
			get => _processBasePath;
			set => this.RaiseAndSetIfChanged(ref _processBasePath, value);
		}

		/// <summary>
		/// Gets or sets the process exe file name.
		/// </summary>
		public string ProcessExecutableName
		{
			get => _processExecutableName;
			set => this.RaiseAndSetIfChanged(ref _processExecutableName, value);
		}

		/// <summary>
		/// Gets or sets the process exe startup arguments.
		/// </summary>
		public string ProcessStartupArgs
		{
			get => _processStartupArgs;
			set => this.RaiseAndSetIfChanged(ref _processStartupArgs, value);
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
		public void Validate()
		{
			var validationResult = _consoleOptionFormViewModelValidator.Validate(this);

			IsValid = !SetErrors(validationResult);
		}
	}
}
