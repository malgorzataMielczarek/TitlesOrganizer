// Ignore Spelling: Validator

using AutoMapper;
using FluentValidation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.Concrete.BookVMs
{
    public class BookVM
    {
        public List<IForListVM> Authors { get; set; } = new();

        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        public string? Edition { get; set; }

        public List<IForListVM> Genres { get; set; } = new();

        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [DisplayName("Volume number")]
        public int? NumberInSeries { get; set; }

        [DisplayName("Original language")]
        public string? OriginalLanguageCode { get; set; }

        [DisplayName("Original title")]
        public string? OriginalTitle { get; set; }

        [DisplayName("Book series")]
        public IForListVM? Series { get; set; }

        public string Title { get; set; } = string.Empty;
        public int? Year { get; set; }
    }

    public class BookVMValidator : AbstractValidator<BookVM>
    {
        public BookVMValidator()
        {
            RuleFor(b => b.Id).NotNull();
            RuleFor(b => b.Title).NotNull().NotEmpty().MaximumLength(225);
            RuleFor(b => b.OriginalTitle).MaximumLength(225);
            RuleFor(b => b.OriginalLanguageCode).Length(3).Unless(b => string.IsNullOrEmpty(b.OriginalLanguageCode));
            RuleForEach(b => b.OriginalLanguageCode).InclusiveBetween('A', 'Z');
            RuleFor(b => b.Year)
                .NotEqual(0)
                .LessThanOrEqualTo(DateTime.Now.Year);
            RuleFor(b => b.Edition).MaximumLength(25);
            RuleFor(b => b.Description).MaximumLength(2000);
        }
    }

    public class BookMappings : Profile
    {
        public BookMappings()
        {
            CreateMap<BookVM, Book>()
                .ForMember(dest => dest.Creators, opt => opt.Ignore())
                .ForMember(dest => dest.Series, opt => opt.Ignore())
                .ForMember(dest => dest.Genres, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.Authors, opt => opt.Ignore())
                .ForMember(dest => dest.Series, opt => opt.Ignore())
                .ForMember(dest => dest.Genres, opt => opt.Ignore());
        }
    }
}