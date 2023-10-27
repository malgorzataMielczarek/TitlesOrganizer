// Ignore Spelling: Validator

using FluentValidation.TestHelper;
using TitlesOrganizer.Application.ViewModels.BookVMs.UpdateVMs;

namespace TitlesOrganizer.Tests.ViewModels.BookVMs.UpdateVMs
{
    public class GenreValidatorTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        public void Validator_NullOrWhitespaceName_ShouldHaveError(string name)
        {
            var validator = new GenreValidator();
            var model = new GenreVM { Name = name };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(g => g.Name);
        }

        [Fact]
        public void Validator_TooLongName_ShouldHaveError()
        {
            var validator = new GenreValidator();
            var model = new GenreVM { Name = new string('a', 26) };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(g => g.Name);
        }

        [Fact]
        public void Validator_ValidName_ShouldNotHaveError()
        {
            var validator = new GenreValidator();
            var model = new GenreVM { Name = new string('a', 25) };

            var result = validator.TestValidate(model);

            result.ShouldNotHaveValidationErrorFor(g => g.Name);
        }
    }
}