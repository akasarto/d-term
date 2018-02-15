using System;
using AutoMapper;
using Notebook.Core;
using UI.Wpf.Notebook;

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
			CreateMap<NoteEntity, NoteFormViewModel>();
			CreateMap<NoteFormViewModel, NoteEntity>();

			CreateMap<NoteEntity, NoteAddViewModel>().ConstructUsing(source => new NoteAddViewModel(_notebookRepository));
			CreateMap<NoteAddViewModel, NoteEntity>();

			CreateMap<NoteFormViewModel, NoteAddViewModel>().ConstructUsing(source => new NoteAddViewModel(_notebookRepository));
			CreateMap<NoteAddViewModel, NoteFormViewModel>();

			CreateMap<NoteEntity, NoteDetailsViewModel>().ConstructUsing(source => new NoteDetailsViewModel(_notebookRepository));
			CreateMap<NoteDetailsViewModel, NoteEntity>();

			CreateMap<NoteFormViewModel, NoteDetailsViewModel>().ConstructUsing(source => new NoteDetailsViewModel(_notebookRepository));
			CreateMap<NoteDetailsViewModel, NoteFormViewModel>();
		}
	}
}
