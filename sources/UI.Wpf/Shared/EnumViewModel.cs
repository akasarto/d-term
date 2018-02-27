using ReactiveUI;

namespace UI.Wpf
{
	public class EnumViewModel<TEnum> : ReactiveObject
	{
		private TEnum _enum;
		private string _description;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public EnumViewModel()
		{
		}

		/// <summary>
		/// Gets or sets the enum description.
		/// </summary>
		public string Description
		{
			get => _description;
			set => this.RaiseAndSetIfChanged(ref _description, value);
		}

		/// <summary>
		/// Gets or sets the enum value.
		/// </summary>
		public TEnum Value
		{
			get => _enum;
			set => this.RaiseAndSetIfChanged(ref _enum, value);
		}
	}
}
