// Ignore Spelling: Validator

using AutoMapper;
using FluentValidation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Base;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class SeriesVM : IUpdateVM<BookSeries>
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        [DisplayName("Original title")]
        public string? OriginalTitle { get; set; }

        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        public IPartialList<Book> Books { get; set; } = new PartialList<Book>();
    }

    public class SeriesVMValidator : AbstractValidator<SeriesVM>
    {
        public SeriesVMValidator()
        {
            RuleFor(s => s.Id).NotNull();
            RuleFor(s => s.Title).NotNull().NotEmpty().MaximumLength(225);
            RuleFor(s => s.OriginalTitle).MaximumLength(225);
            RuleFor(s => s.Description).MaximumLength(2000);
        }
    }

    public static partial class MappingExtensions
    {
        public static BookSeries MapToBase(this SeriesVM seriesVM, IMapper mapper)
        {
            return mapper.Map<BookSeries>(seriesVM);
        }

        public static SeriesVM MapFromBase(this BookSeries seriesWithBooks, IMapper mapper, Paging booksPaging)
        {
            var seriesVM = mapper.Map<SeriesVM>(seriesWithBooks);
            seriesVM.Books.Values = seriesWithBooks.Books.MapToList(ref booksPaging);
            seriesVM.Books.Paging = booksPaging;

            return seriesVM;
        }

        public static SeriesVM MapFromBase(this BookSeries series, IMapper mapper, IQueryable<Book> books, Paging booksPaging)
        {
            var seriesVM = mapper.Map<SeriesVM>(series);
            if (books == null)
            {
                booksPaging.Count = 0;
                booksPaging.CurrentPage = 1;
            }
            else
            {
                seriesVM.Books.Values = books.MapToList(ref booksPaging).ToList();
            }

            seriesVM.Books.Paging = booksPaging;

            return seriesVM;
        }
    }

    public class SeriesMappings : Profile
    {
        public SeriesMappings()
        {
            CreateMap<SeriesVM, BookSeries>()
                .ForMember(dest => dest.Books, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.Books, opt => opt.Ignore());
        }
    }
}