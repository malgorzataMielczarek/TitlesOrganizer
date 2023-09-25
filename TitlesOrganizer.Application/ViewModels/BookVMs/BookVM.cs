// Ignore Spelling: Validator

using FluentValidation;

namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class BookVM
    {
        public int Id { get; set; }

        public required string Title { get; set; }

        public string? OriginalTitle { get; set; }
        public string? OriginalLanguageCode { get; set; }
        public int? Year { get; set; }
        public string? Edition { get; set; }
        public string? Description { get; set; }
    }

    public class BookValidator : AbstractValidator<BookVM>
    {
        public BookValidator()
        {
            RuleFor(x => x.Title).NotNull().NotEmpty().MaximumLength(450);
            RuleFor(x => x.OriginalTitle).MaximumLength(450);
            RuleFor(x => x.OriginalLanguageCode).Length(3).Must(lang => lang.ToLower().All(c => c >= 'a' && c <= 'z')).Unless(x => string.IsNullOrEmpty(x.OriginalLanguageCode));
            RuleFor(x => x.Year).GreaterThan(0).LessThanOrEqualTo(DateTime.Now.Year).When(x => x.Year.HasValue);
            RuleFor(x => x.Edition).MaximumLength(50);
            RuleFor(x => x.Description).MaximumLength(4000);
        }
    }
}