using AutoMapper;
using ReactiveUI;
using System;
using System.Linq;

namespace UI.Wpf.Consoles
{
	public class ConsoleProcessInstancesArrangeViewModel : BaseViewModel
	{
		//
		private ReactiveList<ConsoleArrangeOption> _arrangeOptions;
		private IReactiveDerivedList<ConsoleArrangeOptionViewModel> _arrangeOptionViewModels;
		private ConsoleArrangeOptionViewModel _selectedArrange;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsoleProcessInstancesArrangeViewModel(MaterialDesignThemes.Wpf.ISnackbarMessageQueue test)
		{
			var aOptions = Enum.GetValues(typeof(ConsoleArrangeOption)).Cast<ConsoleArrangeOption>();

			_arrangeOptions = new ReactiveList<ConsoleArrangeOption>(aOptions);

			ArrangeOptions = _arrangeOptions.CreateDerivedCollection(
				filter: noteEntity => true,
				selector: arrangeOption => Mapper.Map<ConsoleArrangeOptionViewModel>(arrangeOption),
				orderer: (noteX, noteY) => noteX.OrderIndex.CompareTo(noteY.OrderIndex),
				scheduler: RxApp.MainThreadScheduler
			);

			this.WhenAnyValue(viewModel => viewModel.SelectedArrange).Subscribe(selectedArrange =>
			{
				if (selectedArrange == null)
				{
					return;
				}

				test.Enqueue("Urrp!");

				MessageBus.Current.SendMessage(new ConsoleArrangeChangedMessage(selectedArrange.Arrange));
			});
		}

		/// <summary>
		/// Gets or sets the current arrange options list.
		/// </summary>
		public IReactiveDerivedList<ConsoleArrangeOptionViewModel> ArrangeOptions
		{
			get => _arrangeOptionViewModels;
			set => this.RaiseAndSetIfChanged(ref _arrangeOptionViewModels, value);
		}

		/// <summary>
		/// Gets or sets the currently selected arrange.
		/// </summary>
		public ConsoleArrangeOptionViewModel SelectedArrange
		{
			get => _selectedArrange;
			set => this.RaiseAndSetIfChanged(ref _selectedArrange, value);
		}
	}
}
