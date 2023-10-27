using FluentAssertions;
using TitlesOrganizer.Application.ViewModels.BookVMs.DetailsVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Tests.ViewModels.BookVMs.DetailsVMs
{
    public class MappingExtensionsTests_Genre
    {
        [Fact]
        public void MapToDetails_LiteratureGenreWithAllRelatedObjects_GenreDetailsVM()
        {
            // Arrange
            int booksCount = 5, seriesCount = 1, authorsCount = 3;
            Paging booksPaging = new Paging() { CurrentPage = 1, PageSize = booksCount }, seriesPaging = new Paging() { CurrentPage = 1, PageSize = seriesCount }, authorsPaging = new Paging() { CurrentPage = 1, PageSize = authorsCount };
            var genre = Helpers.GetGenre();
            var booksList = Helpers.GetBooksList(booksCount);
            var authorsList = Helpers.GetAuthorsList(authorsCount);
            var series = Helpers.GetSeries();

            foreach (var book in booksList)
            {
                book.Genres = new List<LiteratureGenre>() { genre };
                book.Authors = authorsList;
                book.Series = series;
                book.SeriesId = series.Id;
            }

            genre.Books = booksList;

            // Act
            var result = genre.MapToDetails(booksPaging, seriesPaging, authorsPaging);

            // Assert
            result.Should().NotBeNull().And.BeOfType<GenreDetailsVM>();
            result.Id.Should().Be(genre.Id);
            result.Name.Should().Be(genre.Name);
            result.Books.Should().NotBeNull().And.HaveCount(booksCount);
            result.BooksPaging.Should().Be(booksPaging);
            result.BooksPaging.CurrentPage.Should().Be(1);
            result.BooksPaging.PageSize.Should().Be(booksCount);
            result.BooksPaging.Count.Should().Be(booksCount);
            result.Series.Should().NotBeNull().And.HaveCount(seriesCount);
            result.SeriesPaging.Should().Be(seriesPaging);
            result.SeriesPaging.CurrentPage.Should().Be(1);
            result.SeriesPaging.PageSize.Should().Be(seriesCount);
            result.SeriesPaging.Count.Should().Be(seriesCount);
            result.Authors.Should().NotBeNull().And.HaveCount(authorsCount);
            result.AuthorsPaging.Should().Be(authorsPaging);
            result.AuthorsPaging.CurrentPage.Should().Be(1);
            result.AuthorsPaging.PageSize.Should().Be(authorsCount);
            result.AuthorsPaging.Count.Should().Be(authorsCount);
        }

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
            result.Name.Should().Be(genre.Name);
            result.Books.Should().NotBeNull().And.HaveCount(booksCount);
            result.BooksPaging.Should().Be(booksPaging);
            result.BooksPaging.CurrentPage.Should().Be(1);
            result.BooksPaging.PageSize.Should().Be(booksCount);
            result.BooksPaging.Count.Should().Be(booksCount);
            result.Series.Should().NotBeNull().And.HaveCount(seriesCount);
            result.SeriesPaging.Should().Be(seriesPaging);
            result.SeriesPaging.CurrentPage.Should().Be(1);
            result.SeriesPaging.PageSize.Should().Be(seriesCount);
            result.SeriesPaging.Count.Should().Be(seriesCount);
            result.Authors.Should().NotBeNull().And.HaveCount(authorsCount);
            result.AuthorsPaging.Should().Be(authorsPaging);
            result.AuthorsPaging.CurrentPage.Should().Be(1);
            result.AuthorsPaging.PageSize.Should().Be(authorsCount);
            result.AuthorsPaging.Count.Should().Be(authorsCount);
        }
    }
}