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
		private readonly IConsolesRepository _consolesRepository = null;
		private readonly IConsolesProcessService _consolesProcessService = null;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public MapProfileConsoles(IConsolesRepository consolesRepository, IConsolesProcessService consolesProcessService)
		{
			_consolesRepository = consolesRepository ?? throw new ArgumentNullException(nameof(consolesRepository), nameof(MapProfileConsoles));
			_consolesProcessService = consolesProcessService ?? throw new ArgumentNullException(nameof(consolesProcessService), nameof(MapProfileConsoles));

			SetupMaps();
		}

		private void SetupMaps()
		{
			CreateMap<ArrangeOption, ArrangeOptionViewModel>().AfterMap((source, dest) =>
			{
				dest.Arrange = source;
				dest.Description = source.GetDisplayName();
				dest.Index = source.ChangeType<int>();

				switch (source)
				{
					case ArrangeOption.Grid:
						dest.Icon = PackIconKind.GridLarge;
						break;
					case ArrangeOption.Horizontally:
						dest.Icon = PackIconKind.ReorderHorizontal;
						break;
					case ArrangeOption.Vertically:
						dest.Icon = PackIconKind.ReorderVertical;
						break;
				}
			});

			CreateMap<ArrangeOptionViewModel, ArrangeOption>().AfterMap((source, dest) =>
			{
				dest = source.Arrange;
			});

			CreateMap<ConsoleEntity, ConsoleOptionViewModel>().ConstructUsing(source => new ConsoleOptionViewModel(_consolesProcessService));
			CreateMap<ConsoleOptionViewModel, ConsoleEntity>();
		}
	}
}
