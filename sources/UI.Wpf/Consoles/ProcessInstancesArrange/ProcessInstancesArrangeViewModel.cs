using AutoMapper;
using ReactiveUI;
using System;
using System.Linq;

namespace UI.Wpf.Consoles
{
	public class ProcessInstancesArrangeViewModel : BaseViewModel
	{
		//
		private ReactiveList<ArrangeOption> _arrangeOptions;
		private IReactiveDerivedList<ArrangeOptionViewModel> _arrangeOptionViewModels;
		private ArrangeOptionViewModel _selectedArrange;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessInstancesArrangeViewModel()
		{
			var aOptions = Enum.GetValues(typeof(ArrangeOption)).Cast<ArrangeOption>();

			_arrangeOptions = new ReactiveList<ArrangeOption>(aOptions);

			ArrangeOptions = _arrangeOptions.CreateDerivedCollection(
				filter: noteEntity => true,
				selector: arrangeOption => Mapper.Map<ArrangeOptionViewModel>(arrangeOption),
				orderer: (noteX, noteY) => noteX.Index.CompareTo(noteY.Index)
			);

			this.WhenAnyValue(viewModel => viewModel.SelectedArrange).Subscribe(selectedArrange =>
			{
				if (selectedArrange != null)
				{
					System.Windows.MessageBox.Show($"Selected {selectedArrange.Arrange}");
				}
			});
		}

		/// <summary>
		/// Gets or sets the current arrange options list.
		/// </summary>
		public IReactiveDerivedList<ArrangeOptionViewModel> ArrangeOptions
		{
			get => _arrangeOptionViewModels;
			set => this.RaiseAndSetIfChanged(ref _arrangeOptionViewModels, value);
		}

		/// <summary>
		/// Gets or sets the currently selected arrange.
		/// </summary>
		public ArrangeOptionViewModel SelectedArrange
		{
			get => _selectedArrange;
			set => this.RaiseAndSetIfChanged(ref _selectedArrange, value);
		}

		public void Initialize()
		{
		}
	}
}
