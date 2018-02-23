using AutoMapper;
using Consoles.Core;
using ReactiveUI;
using System;
using System.Threading.Tasks;

namespace UI.Wpf.Consoles
{
	public class ConsoleOptionsListViewModel : ReactiveObject, IConsoleOptionsListViewModel
	{
		private ReactiveCommand<IReactiveList<ConsoleOption>, IReactiveDerivedList<ConsoleOptionViewModel>> _loadItemsCommand;
		private IReactiveDerivedList<ConsoleOptionViewModel> _items;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsoleOptionsListViewModel()
		{
			var canInitialize = this.WhenAny(@this => @this.Items, items => {
				return items.Value == null;
			});

			InitializeItemsCommand = ReactiveCommand.CreateFromTask<IReactiveList<ConsoleOption>, IReactiveDerivedList<ConsoleOptionViewModel>>(async (consoleOptions) => await Task.Run(() =>
			{
				return consoleOptions.CreateDerivedCollection(
					filter: option => true,
					selector: option => Mapper.Map<ConsoleOptionViewModel>(option),
					scheduler: RxApp.MainThreadScheduler
				);
			}), canInitialize);

			InitializeItemsCommand.Subscribe(items =>
			{
				Items = items;
			});
		}

		public IReactiveDerivedList<ConsoleOptionViewModel> Items
		{
			get => _items;
			set => this.RaiseAndSetIfChanged(ref _items, value);
		}

		public ReactiveCommand<IReactiveList<ConsoleOption>, IReactiveDerivedList<ConsoleOptionViewModel>> InitializeItemsCommand
		{
			get => _loadItemsCommand;
			set => this.RaiseAndSetIfChanged(ref _loadItemsCommand, value);
		}
	}
}
