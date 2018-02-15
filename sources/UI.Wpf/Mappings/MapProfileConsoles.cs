using System;
using AutoMapper;
using Consoles.Core;
using UI.Wpf.Consoles;
using Sarto.Extensions;

namespace UI.Wpf.Mappings
{
	public class MapProfileConsoles : Profile
	{
		//
		private readonly IConsolesRepository _consolesRepository = null;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public MapProfileConsoles(IConsolesRepository consolesRepository)
		{
			_consolesRepository = consolesRepository ?? throw new ArgumentNullException(nameof(consolesRepository), nameof(MapProfileConsoles));

			SetupMaps();
		}

		private void SetupMaps()
		{
			CreateMap<ArrangeOption, ArrangeOptionViewModel>().AfterMap((source, dest) =>
			{
				dest.Arrange = source;
				dest.Description = source.GetDisplayName();
				dest.Index = source.ChangeType<int>();
			});

			CreateMap<ArrangeOptionViewModel, ArrangeOption>().AfterMap((source, dest) =>
			{
				dest = source.Arrange;
			});

			CreateMap<ConsoleEntity, ConsoleOptionViewModel>();
			CreateMap<ConsoleOptionViewModel, ConsoleEntity>();
		}
	}
}
