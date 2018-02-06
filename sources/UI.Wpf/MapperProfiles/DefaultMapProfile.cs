using AutoMapper;
using Notebook.Core;
using UI.Wpf.Notebook;

namespace UI.Wpf.MapperProfiles
{
	public class DefaultMapProfile : Profile
	{
		public DefaultMapProfile()
		{
			CreateMap<Note, NoteListItemViewModel>();
			CreateMap<NoteListItemViewModel, Note>();
		}
	}
}
