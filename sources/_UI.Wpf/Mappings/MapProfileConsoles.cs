using System;
using AutoMapper;
using Consoles.Core;

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
		}
	}
}
