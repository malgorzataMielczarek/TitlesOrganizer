// Ignore Spelling: Validator

using AutoMapper;
using FluentValidation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Base;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class AuthorVM
    {
        public IPartialList Books { get; set; } = new PartialList();

        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [DisplayName("Last name")]
        public string? LastName { get; set; }

        public string? Name { get; set; }
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