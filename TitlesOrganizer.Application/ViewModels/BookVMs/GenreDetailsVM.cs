using AutoMapper;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Base;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class GenreDetailsVM : BaseDetailsVM
    {
        public IPartialList Authors { get; set; } = new PartialList();
        public IPartialList Books { get; set; } = new PartialList();
        public IPartialList Series { get; set; } = new PartialList();
    }

    public class GenreDetailsMappings : Profile
    {
        public GenreDetailsMappings()
        {
            CreateMap<LiteratureGenre, GenreDetailsVM>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(source => source.Name))
                .ForMember(dest => dest.Books, opt => opt.Ignore());
        }
    }
}