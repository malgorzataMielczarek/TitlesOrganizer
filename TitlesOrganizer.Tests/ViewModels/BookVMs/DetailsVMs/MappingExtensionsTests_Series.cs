using FluentAssertions;
using TitlesOrganizer.Application.ViewModels.BookVMs.DetailsVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;

namespace TitlesOrganizer.Tests.ViewModels.BookVMs.DetailsVMs
{
    public class MappingExtensionsTests_Series
    {
        [Fact]
        public void MapToDetails_BookSeriesWithAllRelatedObjects_SeriesDetailsVM()
        {
            // Arrange
            int booksCount = 5, genresCount = 2, authorsCount = 3;
            Paging booksPaging = new Paging() { CurrentPage = 1, PageSize = booksCount };
            var series = Helpers.GetSeries();
            var booksList = Helpers.GetBooksList(booksCount);
            var authorsList = Helpers.GetAuthorsList(authorsCount);
            var genresList = Helpers.GetGenresList(genresCount);

            foreach (var book in booksList)
            {
                book.Genres = genresList;
                book.Authors = authorsList;
                book.Series = series;
                book.SeriesId = series.Id;
            }

            series.Books = booksList;

            // Act
            var result = series.MapToDetails(booksPaging);

            // Assert
            result.Should().NotBeNull().And.BeOfType<SeriesDetailsVM>();
            result.Id.Should().Be(series.Id);
            result.Title.Should().Be(series.Title);
            result.OriginalTitle.Should().Be(series.OriginalTitle);
            result.Description.Should().Be(series.Description);
            result.Books.Should().NotBeNull().And.HaveCount(booksCount);
            result.BooksPaging.Should().Be(booksPaging);
            result.BooksPaging.CurrentPage.Should().Be(1);
            result.BooksPaging.PageSize.Should().Be(booksCount);
            result.BooksPaging.Count.Should().Be(booksCount);
            result.Authors.Should().NotBeNull().And.HaveCount(authorsCount);
            result.Genres.Should().NotBeNull().And.HaveCount(genresCount);
        }

        [Fact]
        public void MapToDetails_BookSeriesAndBooksAndAuthorsAndGenres_SeriesDetailsVM()
        {
            // Arrange
            int booksCount = 5, genresCount = 2, authorsCount = 3;
            Paging booksPaging = new Paging() { CurrentPage = 1, PageSize = booksCount };
            var series = Helpers.GetSeries();
            var booksQueryable = Helpers.GetBooksList(booksCount).AsQueryable();
            var authorsQueryable = Helpers.GetAuthorsList(authorsCount).AsQueryable();
            var genresQueryable = Helpers.GetGenresList(genresCount).AsQueryable();

            // Act
            var result = series.MapToDetails(booksQueryable, booksPaging, authorsQueryable, genresQueryable);

            // Assert
            result.Should().NotBeNull().And.BeOfType<SeriesDetailsVM>();
            result.Id.Should().Be(series.Id);
            result.Title.Should().Be(series.Title);
            result.OriginalTitle.Should().Be(series.OriginalTitle);
            result.Description.Should().Be(series.Description);
            result.Books.Should().NotBeNull().And.HaveCount(booksCount);
            result.BooksPaging.Should().Be(booksPaging);
            result.BooksPaging.CurrentPage.Should().Be(1);
            result.BooksPaging.PageSize.Should().Be(booksCount);
            result.BooksPaging.Count.Should().Be(booksCount);
            result.Authors.Should().NotBeNull().And.HaveCount(authorsCount);
            result.Genres.Should().NotBeNull().And.HaveCount(genresCount);
        }
    }
}