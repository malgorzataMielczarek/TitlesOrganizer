using FluentAssertions;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;

namespace TitlesOrganizer.Tests.ViewModels.BookVMs
{
    public class AuthorDetailsVM_MappingExtensionsTests
    {
        [Fact]
        public void MapToDetails_AuthorWithAllRelatedObjects_AuthorDetailsVM()
        {
            // Arrange
            int booksCount = 5, seriesCount = booksCount / 2, genresCount = booksCount * 2;
            Paging booksPaging = new Paging() { CurrentPage = 1, PageSize = booksCount }, seriesPaging = new Paging() { CurrentPage = 1, PageSize = seriesCount }, genresPaging = new Paging() { CurrentPage = 1, PageSize = genresCount };
            var author = Helpers.GetAuthor();
            var booksList = Helpers.GetBooksList(booksCount);
            var seriesList = Helpers.GetSeriesList(seriesCount);
            foreach (var series in seriesList)
            {
                var book = booksList.ElementAtOrDefault(series.Id);
                if (book != null)
                {
                    book.SeriesId = series.Id;
                    book.Series = series;
                }

                book = booksList.ElementAtOrDefault(series.Id + seriesCount);
                if (book != null)
                {
                    book.SeriesId = series.Id;
                    book.Series = series;
                }
            }

            var genresList = Helpers.GetGenresList(genresCount);
            foreach (var book in booksList)
            {
                int i = book.Id - 1;
                book.Genres = genresList.GetRange(i, genresCount - i);
            }

            author.Books = booksList;

            // Act
            var result = author.MapToDetails(booksPaging, seriesPaging, genresPaging);

            // Assert
            result.Should().NotBeNull().And.BeOfType<AuthorDetailsVM>();
            result.Id.Should().Be(author.Id);
            result.FullName.Should().Be(author.Name + " " + author.LastName);
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
            result.Genres.Should().NotBeNull().And.HaveCount(genresCount);
            result.GenresPaging.Should().Be(genresPaging);
            result.GenresPaging.CurrentPage.Should().Be(1);
            result.GenresPaging.PageSize.Should().Be(genresCount);
            result.GenresPaging.Count.Should().Be(genresCount);
        }

        [Fact]
        public void MapToDetails_AuthorAndBooksAndSeriesAndGenres_AuthorDetailsVM()
        {
            // Arrange
            int booksCount = 5, seriesCount = booksCount / 2, genresCount = booksCount * 2;
            Paging booksPaging = new Paging() { CurrentPage = 1, PageSize = booksCount }, seriesPaging = new Paging() { CurrentPage = 1, PageSize = seriesCount }, genresPaging = new Paging() { CurrentPage = 1, PageSize = genresCount };
            var author = Helpers.GetAuthor();
            var booksQueryable = Helpers.GetBooksList(booksCount).AsQueryable();
            var seriesQueryable = Helpers.GetSeriesList(seriesCount).AsQueryable();
            var genresQueryable = Helpers.GetGenresList(genresCount).AsQueryable();

            // Act
            var result = author.MapToDetails(booksQueryable, booksPaging, seriesQueryable, seriesPaging, genresQueryable, genresPaging);

            // Assert
            result.Should().NotBeNull().And.BeOfType<AuthorDetailsVM>();
            result.Id.Should().Be(author.Id);
            result.FullName.Should().Be(author.Name + " " + author.LastName);
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
            result.Genres.Should().NotBeNull().And.HaveCount(genresCount);
            result.GenresPaging.Should().Be(genresPaging);
            result.GenresPaging.CurrentPage.Should().Be(1);
            result.GenresPaging.PageSize.Should().Be(genresCount);
            result.GenresPaging.Count.Should().Be(genresCount);
        }
    }
}