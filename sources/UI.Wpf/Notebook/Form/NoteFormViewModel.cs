using ReactiveUI;
using System;

namespace UI.Wpf.Notebook
{
	public class NoteFormViewModel : BaseViewModel
	{
		//
		private Guid _id;
		private string _title;
		private string _description;
		private bool _isValid;

		//
		private readonly NoteFormViewModelValidator _noteFormViewModelValidator = null;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public NoteFormViewModel()
		{
			_noteFormViewModelValidator = new NoteFormViewModelValidator();

			this.WhenAnyValue(x => x.Title, x => x.Description).Subscribe((data) =>
			{
				Validate();
			});
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
		/// Gets or sets the title.
		/// </summary>
		public string Title
		{
			get => _title;
			set => this.RaiseAndSetIfChanged(ref _title, value);
		}

		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		public string Description
		{
			get => _description;
			set => this.RaiseAndSetIfChanged(ref _description, value);
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
		private void Validate()
		{
			var validationResult = _noteFormViewModelValidator.Validate(this);

			IsValid = !SetErrors(validationResult);
		}
	}
}
