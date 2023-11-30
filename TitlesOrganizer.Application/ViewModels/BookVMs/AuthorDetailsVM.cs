using AutoMapper;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Base;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class AuthorDetailsVM : BaseDetailsVM
    {
        public IPartialList Books { get; set; } = new PartialList();
        public IPartialList Genres { get; set; } = new PartialList();
        public IPartialList Series { get; set; } = new PartialList();
    }

    public class AuthorDetailsMappings : Profile
    {
        public AuthorDetailsMappings()
        {
            CreateMap<Author, AuthorDetailsVM>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(source => $"{source.Name} {source.LastName}"))
                .ForMember(dest => dest.Books, opt => opt.Ignore());
        }
    }
}