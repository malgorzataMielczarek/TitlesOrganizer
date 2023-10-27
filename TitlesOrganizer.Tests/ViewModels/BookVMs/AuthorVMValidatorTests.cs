// Ignore Spelling: Validator

using FluentValidation.TestHelper;
using TitlesOrganizer.Application.ViewModels.BookVMs;

namespace TitlesOrganizer.Tests.ViewModels.BookVMs
{
    public class AuthorVMValidatorTests
    {
        [Theory]
        [InlineData(null, null)]
        [InlineData(null, "")]
        [InlineData(null, "    ")]
        [InlineData("", null)]
        [InlineData("", "")]
        [InlineData("", "    ")]
        [InlineData("   ", null)]
        [InlineData("   ", "")]
        [InlineData("   ", "    ")]
        public void Validator_NullOrWhiteSpaceNameAndLastName_ShouldHaveError(string name, string lastName)
        {
            var validator = new AuthorVMValidator();
            var model = new AuthorVM() { Name = name, LastName = lastName };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(a => a.Name);
            result.ShouldHaveValidationErrorFor(a => a.LastName);
        }

        [Theory]
        [InlineData("test", null)]
        [InlineData("test", "")]
        [InlineData("test", "    ")]
        [InlineData(null, "test")]
        [InlineData("", "test")]
        [InlineData("   ", "test")]
        [InlineData("test", "test")]
        public void Validator_NotNullOrWhiteSpaceNameOrLastName_ShouldNotHaveError(string name, string lastName)
        {
            var validator = new AuthorVMValidator();
            var model = new AuthorVM() { Name = name, LastName = lastName };

            var result = validator.TestValidate(model);

            result.ShouldNotHaveValidationErrorFor(a => a.Name);
            result.ShouldNotHaveValidationErrorFor(a => a.LastName);
        }

        [Fact]
        public void Validator_TooLongName_ShouldHaveError()
        {
            var validator = new AuthorVMValidator();
            var model = new AuthorVM() { Name = new string('a', 26) };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(a => a.Name);
        }

        [Fact]
        public void Validator_TooLongLastName_ShouldHaveError()
        {
            var validator = new AuthorVMValidator();
            var model = new AuthorVM() { LastName = new string('a', 26) };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(a => a.LastName);
        }
    }
}