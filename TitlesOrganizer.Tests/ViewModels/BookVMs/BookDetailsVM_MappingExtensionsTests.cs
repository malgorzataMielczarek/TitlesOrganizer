using FluentAssertions;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Domain.Models;
using TitlesOrganizer.Tests.Helpers;

namespace TitlesOrganizer.Tests.ViewModels.BookVMs
{
    public class BookDetailsVM_MappingExtensionsTests
    {
        [Fact]
        public void MapToDetails_BookWithAllRelatedObjects_BookDetailsVM()
        {
            // Arrange
            int authorsCount = 2, genresCount = 3;
            var authorsList = BookModuleHelpers.GetAuthorsList(authorsCount);
            var genresList = BookModuleHelpers.GetGenresList(genresCount);
            var lang = new Language() { Code = "ENG", Name = "English" };
            var book = new Book()
            {
                Id = 1,
                Title = "Book Title",
                OriginalTitle = "Book Original Title",
                Authors = authorsList,
                Genres = genresList,
                Series = new BookSeries() { Id = 1, Title = "Series Title", Books = BookModuleHelpers.GetBooksList(3) },
                SeriesId = 1,
                OriginalLanguageCode = lang.Code,
                OriginalLanguage = lang,
                Year = 2001,
                NumberInSeries = 1,
                Edition = "I",
                Description = "Description"
            };
            book.Series.Books.Remove(book.Series.Books.First());
            book.Series.Books.Add(book);

            // Act
            var result = book.MapToDetails();

            // Assert
            result.Should().NotBeNull().And.BeOfType<BookDetailsVM>();
            result.Id.Should().Be(book.Id);
            result.Title.Should().NotBeNullOrWhiteSpace().And.Be(book.Title);
            result.OriginalTitle.Should().Be(book.OriginalTitle);
            result.OriginalLanguage.Should().Be(lang.Name);
            result.Description.Should().Be(book.Description);
            result.Edition.Should().Be(book.Edition);
            result.Year.Should().Be("2001");
            result.Series.Should().NotBeNull();
            result.InSeries.Should().NotBeNullOrWhiteSpace().And.Be("1 of 3 in ");
            result.Authors.Should().NotBeNull().And.HaveCount(authorsCount);
            result.Genres.Should().NotBeNull().And.HaveCount(genresCount);
        }

        [Fact]
        public void MapToDetails_BookAndLanguageAndAuthorsAndGenresAndSeries_BookDetailsVM()
        {
            // Arrange
            int authorsCount = 2, genresCount = 3;
            var authorsQueryable = BookModuleHelpers.GetAuthorsList(authorsCount);
            var genresQueryble = BookModuleHelpers.GetGenresList(genresCount);
            var series = new BookSeries() { Id = 1, Title = "Series Title", Books = BookModuleHelpers.GetBooksList(3) };
            var lang = new Language() { Code = "ENG", Name = "English" };
            var book = new Book()
            {
                Id = 1,
                Title = "Book Title",
                OriginalTitle = "Book Original Title",
                SeriesId = 1,
                OriginalLanguageCode = lang.Code,
                Year = 2001,
                NumberInSeries = 1,
                Edition = "I",
                Description = "Description"
            };
            series.Books.Remove(series.Books.First());
            series.Books.Add(book);

            // Act
            var result = book.MapToDetails(lang, authorsQueryable, genresQueryble, series);

            // Assert
            result.Should().NotBeNull().And.BeOfType<BookDetailsVM>();
            result.Id.Should().Be(book.Id);
            result.Title.Should().NotBeNullOrWhiteSpace().And.Be(book.Title);
            result.OriginalTitle.Should().Be(book.OriginalTitle);
            result.OriginalLanguage.Should().Be(lang.Name);
            result.Description.Should().Be(book.Description);
            result.Edition.Should().Be(book.Edition);
            result.Year.Should().Be("2001");
            result.Series.Should().NotBeNull();
            result.InSeries.Should().NotBeNullOrWhiteSpace().And.Be("1 of 3 in ");
            result.Authors.Should().NotBeNull().And.HaveCount(authorsCount);
            result.Genres.Should().NotBeNull().And.HaveCount(genresCount);
        }
    }
}