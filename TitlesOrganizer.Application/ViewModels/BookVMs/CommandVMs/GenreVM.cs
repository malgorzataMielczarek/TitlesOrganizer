// Ignore Spelling: Validator

using AutoMapper;
using FluentValidation;
using System.ComponentModel.DataAnnotations;
using TitlesOrganizer.Application.ViewModels.BookVMs.QueryVMs.ForListVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs.CommandVMs
{
    public class GenreVM
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public List<BookForListVM> Books { get; set; } = new List<BookForListVM>();

        [ScaffoldColumn(false)]
        public Paging BooksPaging { get; set; } = new Paging();
    }

    public class GenreValidator : AbstractValidator<GenreVM>
    {
        public GenreValidator()
        {
            RuleFor(g => g.Id).NotNull();
            RuleFor(g => g.Name).NotNull().NotEmpty().MaximumLength(25);
        }
    }

    public static partial class MappingExtensions
    {
        public static LiteratureGenre MapToBase(this GenreVM genreVM, IMapper mapper)
        {
            return mapper.Map<LiteratureGenre>(genreVM);
        }

        public static GenreVM MapFromBase(this LiteratureGenre genreWithBooks, IMapper mapper, Paging booksPaging)
        {
            var genreVM = mapper.Map<GenreVM>(genreWithBooks);
            booksPaging.Count = genreWithBooks.Books?.Count ?? 0;
            genreVM.Books = genreWithBooks.Books?
                .OrderBy(b => b.Title)
                .Skip(booksPaging.PageSize * (booksPaging.CurrentPage - 1))
                .Take(booksPaging.PageSize)
                .Map()
                ?? new List<BookForListVM>();
            genreVM.BooksPaging = booksPaging;

            return genreVM;
        }

        public static GenreVM MapFromBase(this LiteratureGenre genre, IMapper mapper, IQueryable<Book> books, Paging booksPaging)
        {
            var genreVM = mapper.Map<GenreVM>(genre);
            booksPaging.Count = books?.Count() ?? 0;
            genreVM.Books = books?
                .OrderBy(b => b.Title)
                .Map()
                .Skip(booksPaging.PageSize * (booksPaging.CurrentPage - 1))
                .Take(booksPaging.PageSize)
                .ToList() ?? new List<BookForListVM>();
            genreVM.BooksPaging = booksPaging;

            return genreVM;
        }
    }

    public class GenreMappings : Profile
    {
        public GenreMappings()
        {
            CreateMap<GenreVM, LiteratureGenre>()
                .ForMember(dest => dest.Books, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.Books, opt => opt.Ignore());
        }
    }
}