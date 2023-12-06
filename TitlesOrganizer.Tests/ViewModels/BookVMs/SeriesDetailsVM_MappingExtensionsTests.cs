using FluentAssertions;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Tests.Helpers;

namespace TitlesOrganizer.Tests.ViewModels.BookVMs
{
    public class SeriesDetailsVM_MappingExtensionsTests
    {
        [Fact]
        public void MapToDetails_BookSeriesAndBooksAndAuthorsAndGenres_SeriesDetailsVM()
        {
            // Arrange
            int booksCount = 5, genresCount = 2, authorsCount = 3;
            Paging booksPaging = new Paging() { CurrentPage = 1, PageSize = booksCount };
            var series = BookModuleHelpers.GetSeries();
            var booksQueryable = BookModuleHelpers.GetBooksList(booksCount).AsQueryable();
            var authorsQueryable = BookModuleHelpers.GetAuthorsList(authorsCount).AsQueryable();
            var genresQueryable = BookModuleHelpers.GetGenresList(genresCount).AsQueryable();

            // Act
            var result = series.MapToDetails(booksQueryable, booksPaging, authorsQueryable, genresQueryable);

            // Assert
            result.Should().NotBeNull().And.BeOfType<SeriesDetailsVM>();
            result.Id.Should().Be(series.Id);
            result.Title.Should().Be(series.Title);
            result.OriginalTitle.Should().Be(series.OriginalTitle);
            result.Description.Should().Be(series.Description);
            result.Books.Values.Should().NotBeNull().And.HaveCount(booksCount);
            result.Books.Paging.Should().Be(booksPaging);
            result.Books.Paging.CurrentPage.Should().Be(1);
            result.Books.Paging.PageSize.Should().Be(booksCount);
            result.Books.Paging.Count.Should().Be(booksCount);
            result.Authors.Should().NotBeNull().And.HaveCount(authorsCount);
            result.Genres.Should().NotBeNull().And.HaveCount(genresCount);
        }
    }
}