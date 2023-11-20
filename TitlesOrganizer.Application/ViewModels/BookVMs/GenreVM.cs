// Ignore Spelling: Validator

using AutoMapper;
using FluentValidation;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Base;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class GenreVM : IUpdateVM<LiteratureGenre>
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public IPartialList<Book> Books { get; set; } = new PartialList<Book>();
    }

    public class GenreVMValidator : AbstractValidator<GenreVM>
    {
        public GenreVMValidator()
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
            if (genreWithBooks.Books == null)
            {
                booksPaging.Count = 0;
                booksPaging.CurrentPage = 1;
            }
            else
            {
                genreVM.Books.Values = genreWithBooks.Books.OrderBy(b => b.Title).MapToList(ref booksPaging);
            }

            genreVM.Books.Paging = booksPaging;

            return genreVM;
        }

        public static GenreVM MapFromBase(this LiteratureGenre genre, IMapper mapper, IQueryable<Book> books, Paging booksPaging)
        {
            var genreVM = mapper.Map<GenreVM>(genre);
            if (books == null)
            {
                booksPaging.Count = 0;
                booksPaging.CurrentPage = 1;
            }
            else
            {
                genreVM.Books.Values = books.OrderBy(b => b.Title).MapToList(ref booksPaging).ToList();
            }

            genreVM.Books.Paging = booksPaging;

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