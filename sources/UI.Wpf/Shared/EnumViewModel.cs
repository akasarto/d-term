using ReactiveUI;

namespace UI.Wpf
{
	public class EnumViewModel<TEnum> : ReactiveObject
	{
		private string _description;
		private TEnum _value;

		public string Description
		{
			get => _description;
			set => this.RaiseAndSetIfChanged(ref _description, value);
		}

		public TEnum Value
		{
			get => _value;
			set => this.RaiseAndSetIfChanged(ref _value, value);
		}
	}
}
