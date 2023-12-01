// Ignore Spelling: Validator

using AutoMapper;
using FluentValidation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Common;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.Concrete.BookVMs
{
    public class SeriesVM
    {
        public IPartialListVM Books { get; set; } = new PartialListVM();

        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [DisplayName("Original title")]
        public string? OriginalTitle { get; set; }

        public string Title { get; set; } = string.Empty;
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