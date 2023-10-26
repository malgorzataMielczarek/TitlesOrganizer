using FluentAssertions;
using TitlesOrganizer.Application.ViewModels.BookVMs.QueryVMs.DetailsVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Tests.Book.ViewModels.QueryVMs.DetailsVMs
{
    public class MappingExtensionsTests
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

        [Fact]
        public void MapToDetails_BookWithAllRelatedObjects_BookDetailsVM()
        {
            // Arrange
            int authorsCount = 2, genresCount = 3;
            var authorsList = Helpers.GetAuthorsList(authorsCount);
            var genresList = Helpers.GetGenresList(genresCount);
            var lang = new Domain.Models.Language() { Code = "ENG", Name = "English" };
            var book = new Domain.Models.Book()
            {
                Id = 1,
                Title = "Book Title",
                OriginalTitle = "Book Original Title",
                Authors = authorsList,
                Genres = genresList,
                Series = new Domain.Models.BookSeries() { Id = 1, Title = "Series Title", Books = Helpers.GetBooksList(3) },
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
            var authorsQueryable = Helpers.GetAuthorsList(authorsCount).AsQueryable();
            var genresQueryble = Helpers.GetGenresList(genresCount).AsQueryable();
            var series = new Domain.Models.BookSeries() { Id = 1, Title = "Series Title", Books = Helpers.GetBooksList(3) };
            var lang = new Domain.Models.Language() { Code = "ENG", Name = "English" };
            var book = new Domain.Models.Book()
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