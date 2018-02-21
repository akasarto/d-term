using System;
using AutoMapper;
using Notebook.Core;

namespace UI.Wpf.Mappings
{
	public class MapProfileNotebook : Profile
	{
		//
		private readonly INotebookRepository _notebookRepository = null;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public MapProfileNotebook(INotebookRepository notebookRepository)
		{
			_notebookRepository = notebookRepository ?? throw new ArgumentNullException(nameof(notebookRepository), nameof(MapProfileNotebook));

			SetupMaps();
		}

		private void SetupMaps()
		{
		}
	}
}
