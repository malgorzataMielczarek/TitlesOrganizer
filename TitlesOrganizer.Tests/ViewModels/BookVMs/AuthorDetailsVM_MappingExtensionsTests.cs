using FluentAssertions;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Tests.Helpers;

namespace TitlesOrganizer.Tests.ViewModels.BookVMs
{
    public class AuthorDetailsVM_MappingExtensionsTests
    {
        [Fact]
        public void MapToDetails_AuthorAndBooksAndSeriesAndGenres_AuthorDetailsVM()
        {
            // Arrange
            int booksCount = 5, seriesCount = booksCount / 2, genresCount = booksCount * 2;
            Paging booksPaging = new Paging() { CurrentPage = 1, PageSize = booksCount }, seriesPaging = new Paging() { CurrentPage = 1, PageSize = seriesCount }, genresPaging = new Paging() { CurrentPage = 1, PageSize = genresCount };
            var author = BookModuleHelpers.GetAuthor();
            var booksQueryable = BookModuleHelpers.GetBooksList(booksCount).AsQueryable();
            var seriesQueryable = BookModuleHelpers.GetSeriesList(seriesCount).AsQueryable();
            var genresQueryable = BookModuleHelpers.GetGenresList(genresCount).AsQueryable();

            // Act
            var result = author.MapToDetails(booksQueryable, booksPaging, seriesQueryable, seriesPaging, genresQueryable, genresPaging);

            // Assert
            result.Should().NotBeNull().And.BeOfType<AuthorDetailsVM>();
            result.Id.Should().Be(author.Id);
            result.Title.Should().Be(author.Name + " " + author.LastName);
            result.Books.Values.Should().NotBeNull().And.HaveCount(booksCount);
            result.Books.Paging.Should().Be(booksPaging);
            result.Books.Paging.CurrentPage.Should().Be(1);
            result.Books.Paging.PageSize.Should().Be(booksCount);
            result.Books.Paging.Count.Should().Be(booksCount);
            result.Series.Values.Should().NotBeNull().And.HaveCount(seriesCount);
            result.Series.Paging.Should().Be(seriesPaging);
            result.Series.Paging.CurrentPage.Should().Be(1);
            result.Series.Paging.PageSize.Should().Be(seriesCount);
            result.Series.Paging.Count.Should().Be(seriesCount);
            result.Genres.Values.Should().NotBeNull().And.HaveCount(genresCount);
            result.Genres.Paging.Should().Be(genresPaging);
            result.Genres.Paging.CurrentPage.Should().Be(1);
            result.Genres.Paging.PageSize.Should().Be(genresCount);
            result.Genres.Paging.Count.Should().Be(genresCount);
        }
    }
}