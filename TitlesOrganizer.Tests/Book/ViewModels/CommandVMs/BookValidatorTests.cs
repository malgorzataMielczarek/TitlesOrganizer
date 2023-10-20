// Ignore Spelling: Validator

using FluentValidation.TestHelper;
using TitlesOrganizer.Application.ViewModels.BookVMs.CommandVMs;

namespace TitlesOrganizer.Tests.Book.ViewModels.CommandVMs
{
    public class BookValidatorTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Validator_NullOrWhiteSpaceTitle_ShouldHaveError(string title)
        {
            var validator = new BookValidator();
            var model = new BookVM() { Title = title };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(b => b.Title);
        }

        [Fact]
        public void Validator_TooLongTitle_ShouldHaveError()
        {
            var validator = new BookValidator();
            var model = new BookVM() { Title = new string('a', 226) };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(b => b.Title);
        }

        [Fact]
        public void Validator_ValidTitle_ShouldNotHaveError()
        {
            var validator = new BookValidator();
            var model = new BookVM() { Title = new string('a', 225) };

            var result = validator.TestValidate(model);

            result.ShouldNotHaveValidationErrorFor(b => b.Title);
        }

        [Fact]
        public void Validator_TooLongOriginalTitle_ShouldHaveError()
        {
            var validator = new BookValidator();
            var model = new BookVM() { OriginalTitle = new string('a', 226) };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(b => b.OriginalTitle);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Validator_NullOrEmptyOriginalTitle_ShouldNotHaveError(string originalTitle)
        {
            var validator = new BookValidator();
            var model = new BookVM() { OriginalTitle = originalTitle };

            var result = validator.TestValidate(model);

            result.ShouldNotHaveValidationErrorFor(b => b.OriginalTitle);
        }

        [Fact]
        public void Validator_ValidOriginalTitle_ShouldNotHaveError()
        {
            var validator = new BookValidator();
            var model = new BookVM() { OriginalTitle = new string('a', 225) };

            var result = validator.TestValidate(model);

            result.ShouldNotHaveValidationErrorFor(b => b.OriginalTitle);
        }

        [Fact]
        public void Validator_TooLongOriginalLanguageCode_ShouldHaveError()
        {
            var validator = new BookValidator();
            var model = new BookVM() { OriginalLanguageCode = "ENGL" };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(b => b.OriginalLanguageCode);
        }

        [Fact]
        public void Validator_TooShortOriginalLanguageCode_ShouldHaveError()
        {
            var validator = new BookValidator();
            var model = new BookVM() { OriginalLanguageCode = "EN" };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(b => b.OriginalLanguageCode);
        }

        [Theory]
        [InlineData("EN9")]
        [InlineData("EŃG")]
        [InlineData("eng")]
        [InlineData("eNG")]
        [InlineData("EN.")]
        [InlineData("EN?")]
        [InlineData(";EN")]
        [InlineData("E-N")]
        [InlineData("E N")]
        [InlineData("E N G")]
        public void Validator_NotLargeLatinLettersOnlyOriginalLanguageCode_ShouldHaveError(string languageCode)
        {
            var validator = new BookValidator();
            var model = new BookVM() { OriginalLanguageCode = languageCode };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(b => b.OriginalLanguageCode);
        }

        [Fact]
        public void Validator_ValidOriginalLanguageCode_ShouldNotHaveError()
        {
            var validator = new BookValidator();
            var model = new BookVM() { OriginalLanguageCode = "AZM" };

            var result = validator.TestValidate(model);

            result.ShouldNotHaveValidationErrorFor(b => b.OriginalLanguageCode);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Validator_NullOrEmptyOriginalLanguageCode_ShouldNotHaveError(string languageCode)
        {
            var validator = new BookValidator();
            var model = new BookVM() { OriginalLanguageCode = languageCode };

            var result = validator.TestValidate(model);

            result.ShouldNotHaveValidationErrorFor(b => b.OriginalLanguageCode);
        }

        [Fact]
        public void Validator_DefaultIntValueForYear_ShouldHaveError()
        {
            var validator = new BookValidator();
            var model = new BookVM() { Year = (int)default };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(b => b.Year);
        }

        [Fact]
        public void Validator_FeatureYear_ShouldHaveError()
        {
            var validator = new BookValidator();
            var model = new BookVM() { Year = DateTime.Now.Year + 1 };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(b => b.Year);
        }

        [Fact]
        public void Validator_CurrentYear_ShouldNotHaveError()
        {
            var validator = new BookValidator();
            var model = new BookVM() { Year = DateTime.Now.Year };

            var result = validator.TestValidate(model);

            result.ShouldNotHaveValidationErrorFor(b => b.Year);
        }

        [Fact]
        public void Validator_NullYear_ShouldNotHaveError()
        {
            var validator = new BookValidator();
            var model = new BookVM() { Year = null };

            var result = validator.TestValidate(model);

            result.ShouldNotHaveValidationErrorFor(b => b.Year);
        }
    }
}