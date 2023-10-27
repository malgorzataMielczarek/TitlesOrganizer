using AutoMapper;
using FluentAssertions;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Tests.ViewModels.BookVMs
{
    public class BookVM_MappingExtensionsTests
    {
        [Fact]
        public void MapToBase_BookVM_Book()
        {
            var authors = new List<AuthorForListVM>()
            {
                new AuthorForListVM()
                {
                    Id = 1,
                    FullName = "Author1"
                },
                new AuthorForListVM()
                {
                    Id = 2,
                    FullName = "Author2"
                }
            };
            var genres = new List<GenreForListVM>()
            {
                new GenreForListVM()
                {
                    Id = 1,
                    Name = "Genre1"
                },
                new GenreForListVM()
                {
                    Id = 2,
                    Name = "Genre2"
                }
            };
            var bookVM = new BookVM()
            {
                Id = 1,
                Title = "Book Title",
                Authors = authors,
                Description = "Description",
                Edition = "I",
                Genres = genres,
                NumberInSeries = 1,
                OriginalLanguageCode = "ENG",
                OriginalTitle = "Book Original Title",
                Series = new SeriesForListVM() { Id = 1, Title = "Series1" },
                Year = 2001
            };
            var config = new MapperConfiguration(cfg => cfg.AddProfile<BookMappings>());
            IMapper mapper = config.CreateMapper();

            var result = bookVM.MapToBase(mapper);

            result.Should().NotBeNull().And.BeOfType<Book>();
            result.Id.Should().Be(bookVM.Id);
            result.Title.Should().Be(bookVM.Title);
            result.OriginalTitle.Should().Be(bookVM.OriginalTitle);
            result.OriginalLanguageCode.Should().Be(bookVM.OriginalLanguageCode);
            result.Year.Should().Be(bookVM.Year);
            result.Edition.Should().Be(bookVM.Edition);
            result.Description.Should().Be(bookVM.Description);
            result.Authors.Should().BeNullOrEmpty();
            result.Genres.Should().BeNullOrEmpty();
            result.Series.Should().BeNull();
            result.NumberInSeries.Should().Be(bookVM.NumberInSeries);
        }

        [Fact]
        public void MapFromBase_BookWithAllRelatedObjects_BookVM()
        {
            int countOfAuthors = 3, countOfGenres = 5;
            var book = Helpers.GetBook();
            book.Authors = Helpers.GetAuthorsList(countOfAuthors);
            book.Genres = Helpers.GetGenresList(countOfGenres);
            book.Series = Helpers.GetSeries();
            book.SeriesId = book.Series.Id;

            var config = new MapperConfiguration(cfg => cfg.AddProfile<BookMappings>());
            IMapper mapper = config.CreateMapper();

            var result = book.MapFromBase(mapper);

            result.Should().NotBeNull().And.BeOfType<BookVM>();
            result.Id.Should().Be(book.Id);
            result.Title.Should().Be(book.Title);
            result.OriginalTitle.Should().Be(book.OriginalTitle);
            result.OriginalLanguageCode.Should().Be(book.OriginalLanguageCode);
            result.Year.Should().Be(book.Year);
            result.Edition.Should().Be(book.Edition);
            result.Description.Should().Be(book.Description);
            result.Authors.Should().NotBeNull().And.HaveCount(countOfAuthors);
            result.Genres.Should().NotBeNull().And.HaveCount(countOfGenres);
            result.Series.Should().NotBeNull().And.BeOfType<SeriesForListVM>();
            result.NumberInSeries.Should().Be(book.NumberInSeries);
        }

        [Theory]
        [InlineData(3, 5)]
        [InlineData(5, 0)]
        [InlineData(0, 7)]
        [InlineData(0, 0)]
        public void MapFromBase_BookAndAuthorsAndLiteratureGenresAndBookSeries_BookVM(int countOfAuthors, int countOfGenres)
        {
            var book = Helpers.GetBook();
            var authors = Helpers.GetAuthorsList(countOfAuthors).AsQueryable();
            var genres = Helpers.GetGenresList(countOfGenres).AsQueryable();
            var series = Helpers.GetSeries();

            var config = new MapperConfiguration(cfg => cfg.AddProfile<BookMappings>());
            IMapper mapper = config.CreateMapper();

            var result = book.MapFromBase(mapper, authors, genres, series);

            result.Should().NotBeNull().And.BeOfType<BookVM>();
            result.Id.Should().Be(book.Id);
            result.Title.Should().Be(book.Title);
            result.OriginalTitle.Should().Be(book.OriginalTitle);
            result.OriginalLanguageCode.Should().Be(book.OriginalLanguageCode);
            result.Year.Should().Be(book.Year);
            result.Edition.Should().Be(book.Edition);
            result.Description.Should().Be(book.Description);
            result.Authors.Should().NotBeNull().And.HaveCount(countOfAuthors);
            result.Genres.Should().NotBeNull().And.HaveCount(countOfGenres);
            result.Series.Should().NotBeNull().And.BeOfType<SeriesForListVM>();
            result.NumberInSeries.Should().Be(book.NumberInSeries);
        }

        [Fact]
        public void MapFromBase_BookAndNullAuthorsAndNullLiteratureGenresAndNullBookSeries_BookVM()
        {
            var book = Helpers.GetBook();

            var config = new MapperConfiguration(cfg => cfg.AddProfile<BookMappings>());
            IMapper mapper = config.CreateMapper();

            var result = book.MapFromBase(mapper, null, null, null);

            result.Should().NotBeNull().And.BeOfType<BookVM>();
            result.Id.Should().Be(book.Id);
            result.Title.Should().Be(book.Title);
            result.OriginalTitle.Should().Be(book.OriginalTitle);
            result.OriginalLanguageCode.Should().Be(book.OriginalLanguageCode);
            result.Year.Should().Be(book.Year);
            result.Edition.Should().Be(book.Edition);
            result.Description.Should().Be(book.Description);
            result.Authors.Should().NotBeNull().And.BeEmpty();
            result.Genres.Should().NotBeNull().And.BeEmpty();
            result.Series.Should().BeNull();
            result.NumberInSeries.Should().Be(book.NumberInSeries);
        }
    }
}