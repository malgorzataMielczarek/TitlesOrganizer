// Ignore Spelling: Validator

using AutoMapper;
using FluentValidation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TitlesOrganizer.Application.ViewModels.BookVMs.ForListVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs.UpdateVMs
{
    public class AuthorVM
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        public string? Name { get; set; }

        [DisplayName("Last name")]
        public string? LastName { get; set; }

        public List<BookForListVM> Books { get; set; } = new List<BookForListVM>();

        [ScaffoldColumn(false)]
        public Paging BooksPaging { get; set; } = new Paging();
    }

    public class AuthorValidator : AbstractValidator<AuthorVM>
    {
        public AuthorValidator()
        {
            RuleFor(a => a.Id).NotNull();
            RuleFor(a => a.Name).MaximumLength(25);
            RuleFor(a => a.LastName).MaximumLength(25);
            RuleFor(a => a.Name).NotNull().NotEmpty().When(a => string.IsNullOrWhiteSpace(a.LastName));
            RuleFor(a => a.LastName).NotNull().NotEmpty().When(a => string.IsNullOrWhiteSpace(a.Name));
        }
    }

    public static partial class MappingExtensions
    {
        public static Author MapToBase(this AuthorVM authorVM, IMapper mapper)
        {
            return mapper.Map<Author>(authorVM);
        }

        public static AuthorVM MapFromBase(this Author authorWithBooks, IMapper mapper, Paging booksPaging)
        {
            var authorVM = mapper.Map<AuthorVM>(authorWithBooks);
            booksPaging.Count = authorWithBooks.Books.Count;
            authorVM.Books = authorWithBooks.Books
                .OrderBy(b => b.Title)
                .Skip(booksPaging.PageSize * (booksPaging.CurrentPage - 1))
                .Take(booksPaging.PageSize)
                .Map();
            authorVM.BooksPaging = booksPaging;

            return authorVM;
        }

        public static AuthorVM MapFromBase(this Author author, IMapper mapper, IQueryable<Book> books, Paging booksPaging)
        {
            var authorVM = mapper.Map<AuthorVM>(author);
            booksPaging.Count = books?.Count() ?? 0;
            authorVM.Books = books?
                .OrderBy(b => b.Title)
                .Map()
                .Skip(booksPaging.PageSize * (booksPaging.CurrentPage - 1))
                .Take(booksPaging.PageSize)
                .ToList() ?? new List<BookForListVM>();
            authorVM.BooksPaging = booksPaging;

            return authorVM;
        }
    }

    public class AuthorMappings : Profile
    {
        public AuthorMappings()
        {
            CreateMap<AuthorVM, Author>()
                .ForMember(dest => dest.Books, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.Books, opt => opt.Ignore());
        }
    }
}