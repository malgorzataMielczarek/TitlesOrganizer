// Ignore Spelling: Validator

using AutoMapper;
using FluentValidation;
using System.ComponentModel;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Base;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class AuthorVM : IUpdateVM<Author>
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        [DisplayName("Last name")]
        public string? LastName { get; set; }

        public IPartialList<Book> Books { get; set; } = new PartialList<Book>();
    }

    public class AuthorVMValidator : AbstractValidator<AuthorVM>
    {
        public AuthorVMValidator()
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
            authorVM.Books.Values = authorWithBooks.Books.OrderBy(b => b.Title).MapToList(ref booksPaging);
            authorVM.Books.Paging = booksPaging;

            return authorVM;
        }

        public static AuthorVM MapFromBase(this Author author, IMapper mapper, IQueryable<Book> books, Paging booksPaging)
        {
            var authorVM = mapper.Map<AuthorVM>(author);
            if (books == null)
            {
                booksPaging.Count = 0;
                booksPaging.CurrentPage = 1;
            }
            else
            {
                authorVM.Books.Values = books.OrderBy(b => b.Title).MapToList(ref booksPaging).ToList();
            }

            authorVM.Books.Paging = booksPaging;

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