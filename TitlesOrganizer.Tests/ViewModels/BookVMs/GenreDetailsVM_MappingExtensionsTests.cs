using FluentAssertions;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;

namespace TitlesOrganizer.Tests.ViewModels.BookVMs
{
    public class GenreDetailsVM_MappingExtensionsTests
    {
        [Fact]
        public void MapToDetails_LiteratureGenreAndBooksAndSeriesAndAuthors_GenreDetailsVM()
        {
            // Arrange
            int booksCount = 5, seriesCount = 2, authorsCount = 7;
            Paging booksPaging = new Paging() { CurrentPage = 1, PageSize = booksCount }, seriesPaging = new Paging() { CurrentPage = 1, PageSize = seriesCount }, authorsPaging = new Paging() { CurrentPage = 1, PageSize = authorsCount };
            var genre = Helpers.GetGenre();
            var booksQueryable = Helpers.GetBooksList(booksCount).AsQueryable();
            var seriesQueryable = Helpers.GetSeriesList(seriesCount).AsQueryable();
            var authorsQueryable = Helpers.GetAuthorsList(authorsCount).AsQueryable();

            // Act
            var result = genre.MapToDetails(booksQueryable, booksPaging, seriesQueryable, seriesPaging, authorsQueryable, authorsPaging);

            // Assert
            result.Should().NotBeNull().And.BeOfType<GenreDetailsVM>();
            result.Id.Should().Be(genre.Id);
            result.Title.Should().Be(genre.Name);
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
            result.Authors.Values.Should().NotBeNull().And.HaveCount(authorsCount);
            result.Authors.Paging.Should().Be(authorsPaging);
            result.Authors.Paging.CurrentPage.Should().Be(1);
            result.Authors.Paging.PageSize.Should().Be(authorsCount);
            result.Authors.Paging.Count.Should().Be(authorsCount);
        }
    }
}