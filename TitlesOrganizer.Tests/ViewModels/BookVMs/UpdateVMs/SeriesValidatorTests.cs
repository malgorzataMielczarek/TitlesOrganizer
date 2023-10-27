// Ignore Spelling: Validator

using FluentValidation.TestHelper;
using TitlesOrganizer.Application.ViewModels.BookVMs.UpdateVMs;

namespace TitlesOrganizer.Tests.ViewModels.BookVMs.UpdateVMs
{
    public class SeriesValidatorTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Validator_NullOrWhitespaceTitle_ShouldHaveError(string title)
        {
            var validator = new SeriesValidator();
            var model = new SeriesVM { Title = title };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(s => s.Title);
        }

        [Fact]
        public void Validator_TooLongTitle_ShouldHaveError()
        {
            var validator = new SeriesValidator();
            var model = new SeriesVM { Title = new string('a', 226) };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(s => s.Title);
        }

        [Fact]
        public void Validator_ValidTitle_ShouldNotHaveError()
        {
            var validator = new SeriesValidator();
            var model = new SeriesVM { Title = new string('a', 225) };

            var result = validator.TestValidate(model);

            result.ShouldNotHaveValidationErrorFor(s => s.Title);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Validator_NullOrWhitespaceOriginalTitle_ShouldNotHaveError(string originalTitle)
        {
            var validator = new SeriesValidator();
            var model = new SeriesVM { OriginalTitle = originalTitle };

            var result = validator.TestValidate(model);

            result.ShouldNotHaveValidationErrorFor(s => s.OriginalTitle);
        }

        [Fact]
        public void Validator_TooLongOriginalTitle_ShouldHaveError()
        {
            var validator = new SeriesValidator();
            var model = new SeriesVM { OriginalTitle = new string('a', 226) };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(s => s.OriginalTitle);
        }

        [Fact]
        public void Validator_ValidOriginalTitle_ShouldNotHaveError()
        {
            var validator = new SeriesValidator();
            var model = new SeriesVM { OriginalTitle = new string('a', 225) };

            var result = validator.TestValidate(model);

            result.ShouldNotHaveValidationErrorFor(s => s.OriginalTitle);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Validator_NullOrWhitespaceDescription_ShouldNotHaveError(string description)
        {
            var validator = new SeriesValidator();
            var model = new SeriesVM { Description = description };

            var result = validator.TestValidate(model);

            result.ShouldNotHaveValidationErrorFor(s => s.Description);
        }

        [Fact]
        public void Validator_TooLongDescription_ShouldHaveError()
        {
            var validator = new SeriesValidator();
            var model = new SeriesVM { Description = new string('a', 2001) };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(s => s.Description);
        }

        [Fact]
        public void Validator_ValidDescription_ShouldNotHaveError()
        {
            var validator = new SeriesValidator();
            var model = new SeriesVM { Description = new string('a', 2000) };

            var result = validator.TestValidate(model);

            result.ShouldNotHaveValidationErrorFor(s => s.Description);
        }
    }
}