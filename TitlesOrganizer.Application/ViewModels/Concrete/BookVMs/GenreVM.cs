// Ignore Spelling: Validator

using AutoMapper;
using FluentValidation;
using System.ComponentModel.DataAnnotations;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Common;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.Concrete.BookVMs
{
    public class GenreVM
    {
        public IPartialListVM Books { get; set; } = new PartialListVM();

        [ScaffoldColumn(false)]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
    }

    public class GenreVMValidator : AbstractValidator<GenreVM>
    {
        public GenreVMValidator()
        {
            RuleFor(g => g.Id).NotNull();
            RuleFor(g => g.Name).NotNull().NotEmpty().MaximumLength(25);
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