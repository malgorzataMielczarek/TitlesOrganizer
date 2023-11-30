using AutoMapper;
using System.ComponentModel;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Base;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class SeriesDetailsVM : BaseDetailsVM
    {
        public List<IForListVM> Authors { get; set; } = new();

        public IPartialList Books { get; set; } = new PartialList();

        public string Description { get; set; } = string.Empty;

        public List<IForListVM> Genres { get; set; } = new();

        [DisplayName("Original title")]
        public string OriginalTitle { get; set; } = string.Empty;
    }

    public class SeriesDetailsMappings : Profile
    {
        public SeriesDetailsMappings()
        {
            CreateMap<BookSeries, SeriesDetailsVM>()
                .ForMember(dest => dest.Books, opt => opt.Ignore());
        }
    }
}