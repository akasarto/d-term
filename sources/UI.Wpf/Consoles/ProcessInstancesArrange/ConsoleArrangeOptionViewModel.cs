using MaterialDesignThemes.Wpf;
using ReactiveUI;

namespace UI.Wpf.Consoles
{
	public class ConsoleArrangeOptionViewModel : BaseViewModel
	{
		private ConsoleArrangeOption _arrange;
		private string _description;
		private int _orderIndex;
		private PackIconKind _iconKind;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsoleArrangeOptionViewModel()
		{
		}

		/// <summary>
		/// Gets or sets the arrange.
		/// </summary>
		public ConsoleArrangeOption Arrange
		{
			get => _arrange;
			set => this.RaiseAndSetIfChanged(ref _arrange, value);
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
		/// Gets or sets the icon kind.
		/// </summary>
		public PackIconKind IconKind
		{
			get => _iconKind;
			set => this.RaiseAndSetIfChanged(ref _iconKind, value);
		}

		/// <summary>
		/// Gets or sets the order index.
		/// </summary>
		public int OrderIndex
		{
			get => _orderIndex;
			set => this.RaiseAndSetIfChanged(ref _orderIndex, value);
		}
	}
}
