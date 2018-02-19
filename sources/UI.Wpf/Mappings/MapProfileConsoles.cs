using System;
using AutoMapper;
using Consoles.Core;
using UI.Wpf.Consoles;
using Sarto.Extensions;
using MaterialDesignThemes.Wpf;

namespace UI.Wpf.Mappings
{
	public class MapProfileConsoles : Profile
	{
		//
		private readonly IConsoleOptionsRepository _consolesRepository = null;
		private readonly IConsoleProcessService _consoleProcessService = null;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public MapProfileConsoles(IConsoleOptionsRepository consolesRepository, IConsoleProcessService consoleProcessService)
		{
			_consolesRepository = consolesRepository ?? throw new ArgumentNullException(nameof(consolesRepository), nameof(MapProfileConsoles));
			_consoleProcessService = consoleProcessService ?? throw new ArgumentNullException(nameof(consoleProcessService), nameof(MapProfileConsoles));

			SetupMaps();
		}

		private void SetupMaps()
		{
			CreateMap<IConsoleProcess, ConsoleProcessInstanceViewModel>().ConstructUsing(source => new ConsoleProcessInstanceViewModel(source));

			CreateMap<ProcessBasePath, ProcessBasePathViewModel>().AfterMap((source, dest) =>
			{
				dest.BasePath = source;
				dest.Description = source.GetDisplayName();
			});

			CreateMap<ConsoleArrangeOption, ConsoleArrangeOptionViewModel>().AfterMap((source, dest) =>
			{
				dest.Arrange = source;
				dest.Description = source.GetDisplayName();
				dest.OrderIndex = source.ChangeType<int>();

				switch (source)
				{
					case ConsoleArrangeOption.Grid:
						dest.IconKind = PackIconKind.GridLarge;
						break;
					case ConsoleArrangeOption.Horizontally:
						dest.IconKind = PackIconKind.ReorderHorizontal;
						break;
					case ConsoleArrangeOption.Vertically:
						dest.IconKind = PackIconKind.ReorderVertical;
						break;
				}
			});

			CreateMap<ConsoleArrangeOptionViewModel, ConsoleArrangeOption>().AfterMap((source, dest) =>
			{
				dest = source.Arrange;
			});

			CreateMap<ConsoleOption, ConsoleOptionViewModel>().ConstructUsing(source => new ConsoleOptionViewModel(_consoleProcessService));
			CreateMap<ConsoleOptionViewModel, ConsoleOption>();

			CreateMap<ConsoleOptionFormViewModel, ConsoleOptionViewModel>().ConstructUsing(source => new ConsoleOptionViewModel(_consoleProcessService));
			CreateMap<ConsoleOptionViewModel, ConsoleOptionFormViewModel>();
		}
	}
}
