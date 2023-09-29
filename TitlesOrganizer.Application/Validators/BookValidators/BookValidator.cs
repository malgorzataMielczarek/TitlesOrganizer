// Ignore Spelling: Validator Validators

using FluentValidation;
using TitlesOrganizer.Application.ViewModels.BookVMs;

namespace TitlesOrganizer.Application.Validators.BookValidators
{
    public class BookValidator : AbstractValidator<BookVM>
    {
        public BookValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Title).NotNull().NotEmpty().MaximumLength(225);
            RuleFor(x => x.OriginalTitle).MaximumLength(225);
            RuleFor(x => x.OriginalLanguageCode).Length(3).Must(lang => lang.ToLower().All(c => c >= 'a' && c <= 'z')).Unless(x => x.OriginalLanguageCode is null);
            RuleFor(x => x.Year).GreaterThan(0).LessThanOrEqualTo(DateTime.Now.Year);
            RuleFor(x => x.Edition).MaximumLength(25);
            RuleFor(x => x.Description).MaximumLength(2000);
        }
    }
}