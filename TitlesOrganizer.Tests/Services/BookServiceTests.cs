// Ignore Spelling: Upsert

using AutoMapper;
using FluentAssertions;
using Moq;
using TitlesOrganizer.Application.Services;
using TitlesOrganizer.Application.ViewModels.Base;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Interfaces;
using TitlesOrganizer.Domain.Models;
using TitlesOrganizer.Tests.ViewModels.BookVMs;

namespace TitlesOrganizer.Tests.Services
{
    public class BookServiceTests
    {
        [Fact]
        public void Delete_ExistingBookId()
        {
            var book = new Book() { Id = 1, Title = "Title" };
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBook(book.Id)).Returns(book);
            commandsRepo.Setup(r => r.Delete(book));
            var mapper = new Mock<IMapper>().Object;
            var lang = new Mock<ILanguageRepository>().Object;

            var service = new BookServiceForTest(commandsRepo.Object, queriesRepo.Object, lang, mapper);
            service.Delete(book.Id);

            queriesRepo.Verify(r => r.GetBook(book.Id), Times.Once);
            commandsRepo.Verify(r => r.Delete(book), Times.Once);
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void Delete_NotExistingBookId()
        {
            var book = new Book() { Id = 1, Title = "Title" };
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBook(book.Id)).Returns((Book?)null);
            commandsRepo.Setup(r => r.Delete(book));
            var mapper = new Mock<IMapper>().Object;
            var lang = new Mock<ILanguageRepository>().Object;

            var service = new BookServiceForTest(commandsRepo.Object, queriesRepo.Object, lang, mapper);
            service.Delete(book.Id);

            queriesRepo.Verify(r => r.GetBook(book.Id), Times.Once);
            commandsRepo.Verify(r => r.Delete(book), Times.Never);
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void Get_ExistingBookId()
        {
            var book = Helpers.GetBook(1);
            book.OriginalLanguageCode = "ENG";
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBookWithAuthorsGenresAndSeries(book.Id)).Returns(book);
            IMapper mapper = new Mock<IMapper>().Object;
            var langRepo = new Mock<ILanguageRepository>();
            langRepo.Setup(r => r.GetAllLanguages())
                .Returns(new[]
                {
                    new Language() { Code = "ENG", Name = "English" },
                    new Language() { Code = "PLN", Name = "Polish" }
                }.AsQueryable());

            var service = new BookServiceForTest(commandsRepo.Object, queriesRepo.Object, langRepo.Object, mapper);
            var result = service.Get(book.Id);

            queriesRepo.Verify(r => r.GetBookWithAuthorsGenresAndSeries(book.Id), Times.Once());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            langRepo.Verify(r => r.GetAllLanguages(), Times.Once);
            result.Should().NotBeNull().And.BeOfType<BookVM>();
            result.Id.Should().Be(book.Id);
        }

        [Fact]
        public void Get_NotExistingBookId()
        {
            int bookId = 1;
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBookWithAuthorsGenresAndSeries(bookId)).Returns((Book?)null);
            IMapper mapper = new Mock<IMapper>().Object;
            var lang = new Mock<ILanguageRepository>().Object;

            var service = new BookServiceForTest(commandsRepo.Object, queriesRepo.Object, lang, mapper);
            var result = service.Get(bookId);

            queriesRepo.Verify(r => r.GetBookWithAuthorsGenresAndSeries(bookId), Times.Once());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            result.Should().NotBeNull().And.BeOfType<BookVM>();
            result.Id.Should().Be(default);
        }

        [Fact]
        public void GetDetails_ExistingBookIdWithSeriesAndLanguage()
        {
            // Arrange
            var seriesId = 1;
            var book = Helpers.GetBook(1);
            book.SeriesId = seriesId;
            book.OriginalLanguageCode = "ENG";

            var a1 = Helpers.GetAuthor(1);
            var a2 = Helpers.GetAuthor(2);
            var a3 = Helpers.GetAuthor(3);
            var a4 = Helpers.GetAuthor(4);
            a1.Books.Add(Helpers.GetBook(1));
            a2.Books.Add(Helpers.GetBook(2));
            a3.Books.Add(Helpers.GetBook(2));
            a3.Books.Add(Helpers.GetBook(1));
            var authors = new List<Author>() { a1, a2, a3, a4 }.AsQueryable();

            BookSeries series = Helpers.GetSeries(seriesId);
            var seriesBooks = new List<Book>() { Helpers.GetBook(1), Helpers.GetBook(2), Helpers.GetBook(3) };
            series.Books = seriesBooks;

            var g1 = Helpers.GetGenre(1);
            var g2 = Helpers.GetGenre(2);
            var g3 = Helpers.GetGenre(3);
            var g4 = Helpers.GetGenre(4);
            g1.Books = new List<Book>() { Helpers.GetBook(1) };
            g2.Books = new List<Book>() { Helpers.GetBook(2) };
            g3.Books = new List<Book>() { Helpers.GetBook(2), Helpers.GetBook(1) };
            var genres = new List<LiteratureGenre>() { g1, g2, g3, g4 }.AsQueryable();

            var languages = new List<Language>()
            {
                new Language() { Code="PLN", Name="Polish" },
                new Language() { Code="ENG", Name="English" }
            }.AsQueryable();

            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBook(book.Id)).Returns(book);
            queriesRepo.Setup(r => r.GetAllAuthorsWithBooks()).Returns(authors);
            queriesRepo.Setup(r => r.GetAllLiteratureGenresWithBooks()).Returns(genres);
            queriesRepo.Setup(r => r.GetBookSeriesWithBooks(seriesId)).Returns(series);
            var langRepo = new Mock<ILanguageRepository>();
            langRepo.Setup(r => r.GetAllLanguages()).Returns(languages);
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookServiceForTest(commandsRepo.Object, queriesRepo.Object, langRepo.Object, mapper);

            // Act
            var result = service.GetDetails(book.Id);

            // Assert
            queriesRepo.Verify(r => r.GetBook(book.Id), Times.Once());
            queriesRepo.Verify(r => r.GetAllAuthorsWithBooks(), Times.Once());
            queriesRepo.Verify(r => r.GetAllLiteratureGenresWithBooks(), Times.Once());
            queriesRepo.Verify(r => r.GetBookSeriesWithBooks(seriesId), Times.Once());
            langRepo.Verify(r => r.GetAllLanguages(), Times.Once());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            langRepo.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeOfType<BookDetailsVM>();
            result.Id.Should().Be(book.Id);
            result.Title.Should().Be(book.Title);
            result.OriginalLanguage.Should().Be("English");
            service.Authors.Should()
                .NotBeNullOrEmpty().And
                .HaveCount(2).And
                .Equal(a1, a3).And
                .NotContain(new[] { a2, a4 });
            service.Books.Should()
                .NotBeNullOrEmpty().And
                .HaveCount(3).And
                .Equal(seriesBooks);
            service.Genres.Should()
                .NotBeNullOrEmpty().And
                .HaveCount(2).And
                .Equal(g1, g3).And
                .NotContain(new[] { g2, g4 });
            service.Series.Should()
                .NotBeNull().And
                .Be(series);
        }

        [Fact]
        public void GetDetails_ExistingBookIdWithoutSeriesAndLanguage()
        {
            // Arrange
            var book = Helpers.GetBook(1);
            book.SeriesId = null;
            book.OriginalLanguageCode = null;

            var a1 = Helpers.GetAuthor(1);
            var a2 = Helpers.GetAuthor(2);
            var a3 = Helpers.GetAuthor(3);
            var a4 = Helpers.GetAuthor(4);
            a1.Books.Add(Helpers.GetBook(1));
            a2.Books.Add(Helpers.GetBook(2));
            a3.Books.Add(Helpers.GetBook(2));
            a3.Books.Add(Helpers.GetBook(1));
            var authors = new List<Author>() { a1, a2, a3, a4 }.AsQueryable();

            var g1 = Helpers.GetGenre(1);
            var g2 = Helpers.GetGenre(2);
            var g3 = Helpers.GetGenre(3);
            var g4 = Helpers.GetGenre(4);
            g1.Books = new List<Book>() { Helpers.GetBook(1) };
            g2.Books = new List<Book>() { Helpers.GetBook(2) };
            g3.Books = new List<Book>() { Helpers.GetBook(2), Helpers.GetBook(1) };
            var genres = new List<LiteratureGenre>() { g1, g2, g3, g4 }.AsQueryable();

            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBook(book.Id)).Returns(book);
            queriesRepo.Setup(r => r.GetAllAuthorsWithBooks()).Returns(authors);
            queriesRepo.Setup(r => r.GetAllLiteratureGenresWithBooks()).Returns(genres);
            queriesRepo.Setup(r => r.GetBookSeriesWithBooks(It.IsAny<int>()));
            var langRepo = new Mock<ILanguageRepository>();
            langRepo.Setup(r => r.GetAllLanguages());
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookServiceForTest(commandsRepo.Object, queriesRepo.Object, langRepo.Object, mapper);

            // Act
            var result = service.GetDetails(book.Id);

            // Assert
            queriesRepo.Verify(r => r.GetBook(book.Id), Times.Once());
            queriesRepo.Verify(r => r.GetAllAuthorsWithBooks(), Times.Once());
            queriesRepo.Verify(r => r.GetAllLiteratureGenresWithBooks(), Times.Once());
            queriesRepo.Verify(r => r.GetBookSeriesWithBooks(It.IsAny<int>()), Times.Never());
            langRepo.Verify(r => r.GetAllLanguages(), Times.Never());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            langRepo.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeOfType<BookDetailsVM>();
            result.Id.Should().Be(book.Id);
            result.Title.Should().Be(book.Title);
            result.OriginalLanguage.Should().Be(string.Empty);
            service.Authors.Should()
                .NotBeNullOrEmpty().And
                .HaveCount(2).And
                .Equal(a1, a3).And
                .NotContain(new[] { a2, a4 });
            service.Books.Should().BeNull();
            service.Genres.Should()
                .NotBeNullOrEmpty().And
                .HaveCount(2).And
                .Equal(g1, g3).And
                .NotContain(new[] { g2, g4 });
            service.Series.Should().BeNull();
        }

        [Fact]
        public void GetDetails_NotExistingBookId()
        {
            int bookId = 1;
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBook(bookId)).Returns((Book?)null);
            queriesRepo.Setup(r => r.GetAllAuthorsWithBooks());
            queriesRepo.Setup(r => r.GetAllLiteratureGenresWithBooks());
            queriesRepo.Setup(r => r.GetBookSeriesWithBooks(It.IsAny<int>()));
            var langRepo = new Mock<ILanguageRepository>();
            langRepo.Setup(r => r.GetAllLanguages());
            IMapper mapper = new Mock<IMapper>().Object;
            var service = new BookServiceForTest(commandsRepo.Object, queriesRepo.Object, langRepo.Object, mapper);

            var result = service.GetDetails(bookId);

            // Assert
            queriesRepo.Verify(r => r.GetBook(bookId), Times.Once());
            queriesRepo.Verify(r => r.GetAllAuthorsWithBooks(), Times.Never());
            queriesRepo.Verify(r => r.GetAllLiteratureGenresWithBooks(), Times.Never());
            queriesRepo.Verify(r => r.GetBookSeriesWithBooks(It.IsAny<int>()), Times.Never());
            langRepo.Verify(r => r.GetAllLanguages(), Times.Never());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            langRepo.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeOfType<BookDetailsVM>();
            result.Id.Should().Be(default);
            result.Title.Should().Be(string.Empty);
            result.OriginalLanguage.Should().Be(string.Empty);
            service.Authors.Should().BeNull();
            service.Books.Should().BeNull();
            service.Genres.Should().BeNull();
            service.Series.Should().BeNull();
        }

        [Theory]
        [InlineData(5, 5, 1, SortByEnum.Ascending, "Title")]
        [InlineData(6, 3, 2, SortByEnum.Descending, null)]
        public void GetList(int count, int pageSize, int pageNo, SortByEnum sort, string? search)
        {
            var books = Helpers.GetBooksList(count).AsQueryable();
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetAllBooks()).Returns(books);
            var mapper = new Mock<IMapper>().Object;
            var lang = new Mock<ILanguageRepository>().Object;
            var service = new BookServiceForTest(commandsRepo.Object, queriesRepo.Object, lang, mapper);

            var result = service.GetList(sort, pageSize, pageNo, search);

            queriesRepo.Verify(r => r.GetAllBooks(), Times.Once());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            service.Books.Should().NotBeNull().And.HaveCount(count).And.Equal(books);
            result.Should().NotBeNull().And.BeOfType<ListBookForListVM>();
            result.Paging.Should().NotBeNull();
            result.Paging.PageSize.Should().Be(pageSize);
            result.Paging.CurrentPage.Should().Be(pageNo);
            result.Paging.Count.Should().Be(count);
            result.Filtering.Should().NotBeNull();
            result.Filtering.SearchString.Should().NotBeNull().And.Be(search ?? string.Empty);
            result.Filtering.SortBy.Should().Be(sort);
        }

        [Theory]
        [InlineData(5, 1, SortByEnum.Ascending, "Name")]
        [InlineData(3, 2, SortByEnum.Descending, null)]
        public void GetListForAuthor_ExistingAuthor(int pageSize, int pageNo, SortByEnum sort, string? search)
        {
            var a1 = Helpers.GetAuthor(1);
            var a2 = Helpers.GetAuthor(2);
            var b1 = Helpers.GetBook(1);
            var b2 = Helpers.GetBook(2);
            var b3 = Helpers.GetBook(3);
            var b4 = Helpers.GetBook(4);
            b1.Authors = new List<Author>() { a1, a2 };
            b2.Authors = new List<Author>() { a1 };
            b3.Authors = new List<Author>() { a2 };
            b4.Authors = new List<Author>();
            var books = new List<Book> { b1, b2, b3, b4 }.AsQueryable();
            var author = Helpers.GetAuthor(a1.Id);
            var commands = new Mock<IBookCommandsRepository>();
            var queries = new Mock<IBookModuleQueriesRepository>();
            queries.Setup(q => q.GetAuthor(a1.Id)).Returns(author);
            queries.Setup(q => q.GetAllBooksWithAuthorsGenresAndSeries()).Returns(books);
            var mapper = new Mock<IMapper>().Object;
            var lang = new Mock<ILanguageRepository>().Object;
            var service = new BookServiceForTest(commands.Object, queries.Object, lang, mapper);

            var result = service.GetListForAuthor(a1.Id, sort, pageSize, pageNo, search);

            result.Should()
                .NotBeNull().And
                .BeOfType<ListBookForAuthorVM>();
            queries.Verify(
                q => q.GetAuthor(a1.Id),
                Times.Once);
            queries.Verify(
                q => q.GetAllBooksWithAuthorsGenresAndSeries(),
                Times.Once);
            queries.VerifyNoOtherCalls();
            commands.VerifyNoOtherCalls();
            result.Paging.Should().NotBeNull();
            result.Paging.CurrentPage.Should().Be(pageNo);
            result.Paging.PageSize.Should().Be(pageSize);
            result.Filtering.Should().NotBeNull();
            result.Filtering.SearchString.Should()
                .NotBeNull().And
                .Be(search ?? string.Empty);
            result.Filtering.SortBy.Should().Be(sort);
            service.Books.Should()
                .NotBeNull().And
                .Equal(books);
            service.Authors.Should()
                .NotBeNullOrEmpty().And
                .ContainSingle(a => a.Equals(author));
        }

        [Theory]
        [InlineData(5, 1, SortByEnum.Ascending, "Name")]
        [InlineData(3, 2, SortByEnum.Descending, null)]
        public void GetListForAuthor_NotExistingAuthor(int pageSize, int pageNo, SortByEnum sort, string? search)
        {
            var a1 = Helpers.GetAuthor(1);
            var a2 = Helpers.GetAuthor(2);
            int authorId = 3;
            var b1 = Helpers.GetBook(1);
            var b2 = Helpers.GetBook(2);
            var b3 = Helpers.GetBook(3);
            var b4 = Helpers.GetBook(4);
            b1.Authors = new List<Author>() { a1, a2 };
            b2.Authors = new List<Author>() { a1 };
            b3.Authors = new List<Author>() { a2 };
            b4.Authors = new List<Author>();
            var books = new List<Book> { b1, b2, b3, b4 }.AsQueryable();
            var commands = new Mock<IBookCommandsRepository>();
            var queries = new Mock<IBookModuleQueriesRepository>();
            queries.Setup(q => q.GetAuthor(authorId))
                .Returns((Author?)null);
            queries.Setup(q => q.GetAllBooksWithAuthorsGenresAndSeries())
                .Returns(books);
            var mapper = new Mock<IMapper>().Object;
            var lang = new Mock<ILanguageRepository>().Object;
            var service = new BookServiceForTest(commands.Object, queries.Object, lang, mapper);

            var result = service.GetListForAuthor(authorId, sort, pageSize, pageNo, search);

            result.Should()
                .NotBeNull().And
                .BeOfType<ListBookForAuthorVM>();
            queries.Verify(
                q => q.GetAuthor(authorId),
                Times.Once);
            queries.Verify(
                q => q.GetAllBooksWithAuthorsGenresAndSeries(),
                Times.Once);
            queries.VerifyNoOtherCalls();
            commands.VerifyNoOtherCalls();
            result.Paging.Should().NotBeNull();
            result.Paging.CurrentPage.Should().Be(pageNo);
            result.Paging.PageSize.Should().Be(pageSize);
            result.Filtering.Should().NotBeNull();
            result.Filtering.SearchString.Should()
                .NotBeNull().And
                .Be(search ?? string.Empty);
            result.Filtering.SortBy.Should().Be(sort);
            service.Books.Should()
                .NotBeNull().And
                .Equal(books);
            service.Authors.Should()
                .NotBeNullOrEmpty().And
                .NotContainNulls().And
                .ContainSingle(a => a.Id == default);
        }

        [Theory]
        [InlineData(5, 1, SortByEnum.Ascending, "Name")]
        [InlineData(3, 2, SortByEnum.Descending, null)]
        public void GetListForGenre_ExistingGenre(int pageSize, int pageNo, SortByEnum sort, string? search)
        {
            var g1 = Helpers.GetGenre(1);
            var g2 = Helpers.GetGenre(2);
            var b1 = Helpers.GetBook(1);
            var b2 = Helpers.GetBook(2);
            var b3 = Helpers.GetBook(3);
            var b4 = Helpers.GetBook(4);
            b1.Genres = new List<LiteratureGenre>() { g1, g2 };
            b2.Genres = new List<LiteratureGenre>() { g1 };
            b3.Genres = new List<LiteratureGenre>() { g2 };
            b4.Genres = new List<LiteratureGenre>();
            var books = new List<Book> { b1, b2, b3, b4 }.AsQueryable();
            var genre = Helpers.GetGenre(g1.Id);
            var commands = new Mock<IBookCommandsRepository>();
            var queries = new Mock<IBookModuleQueriesRepository>();
            queries.Setup(q => q.GetLiteratureGenre(g1.Id))
                .Returns(genre);
            queries.Setup(q => q.GetAllBooksWithAuthorsGenresAndSeries())
                .Returns(books);
            var mapper = new Mock<IMapper>().Object;
            var lang = new Mock<ILanguageRepository>().Object;
            var service = new BookServiceForTest(commands.Object, queries.Object, lang, mapper);

            var result = service.GetListForGenre(g1.Id, sort, pageSize, pageNo, search);

            result.Should()
                .NotBeNull().And
                .BeOfType<ListBookForGenreVM>();
            queries.Verify(
                q => q.GetLiteratureGenre(g1.Id),
                Times.Once);
            queries.Verify(
                q => q.GetAllBooksWithAuthorsGenresAndSeries(),
                Times.Once);
            queries.VerifyNoOtherCalls();
            commands.VerifyNoOtherCalls();
            result.Paging.Should().NotBeNull();
            result.Paging.CurrentPage.Should().Be(pageNo);
            result.Paging.PageSize.Should().Be(pageSize);
            result.Filtering.Should().NotBeNull();
            result.Filtering.SearchString.Should()
                .NotBeNull().And
                .Be(search ?? string.Empty);
            result.Filtering.SortBy.Should().Be(sort);
            service.Books.Should()
                .NotBeNull().And
                .Equal(books);
            service.Genres.Should()
                .NotBeNullOrEmpty().And
                .ContainSingle(g => g.Equals(genre));
        }

        [Theory]
        [InlineData(5, 1, SortByEnum.Ascending, "Name")]
        [InlineData(3, 2, SortByEnum.Descending, null)]
        public void GetListForGenre_NotExistingGenre(int pageSize, int pageNo, SortByEnum sort, string? search)
        {
            var g1 = Helpers.GetGenre(1);
            var g2 = Helpers.GetGenre(2);
            int genreId = 3;
            var b1 = Helpers.GetBook(1);
            var b2 = Helpers.GetBook(2);
            var b3 = Helpers.GetBook(3);
            var b4 = Helpers.GetBook(4);
            b1.Genres = new List<LiteratureGenre>() { g1, g2 };
            b2.Genres = new List<LiteratureGenre>() { g1 };
            b3.Genres = new List<LiteratureGenre>() { g2 };
            b4.Genres = new List<LiteratureGenre>();
            var books = new List<Book> { b1, b2, b3, b4 }.AsQueryable();
            var commands = new Mock<IBookCommandsRepository>();
            var queries = new Mock<IBookModuleQueriesRepository>();
            queries.Setup(q => q.GetLiteratureGenre(genreId))
                .Returns((LiteratureGenre?)null);
            queries.Setup(q => q.GetAllBooksWithAuthorsGenresAndSeries())
                .Returns(books);
            var mapper = new Mock<IMapper>().Object;
            var lang = new Mock<ILanguageRepository>().Object;
            var service = new BookServiceForTest(commands.Object, queries.Object, lang, mapper);

            var result = service.GetListForGenre(genreId, sort, pageSize, pageNo, search);

            result.Should()
                .NotBeNull().And
                .BeOfType<ListBookForGenreVM>();
            queries.Verify(
                q => q.GetLiteratureGenre(genreId),
                Times.Once);
            queries.Verify(
                q => q.GetAllBooksWithAuthorsGenresAndSeries(),
                Times.Once);
            queries.VerifyNoOtherCalls();
            commands.VerifyNoOtherCalls();
            result.Paging.Should().NotBeNull();
            result.Paging.CurrentPage.Should().Be(pageNo);
            result.Paging.PageSize.Should().Be(pageSize);
            result.Filtering.Should().NotBeNull();
            result.Filtering.SearchString.Should()
                .NotBeNull().And
                .Be(search ?? string.Empty);
            result.Filtering.SortBy.Should().Be(sort);
            service.Books.Should()
                .NotBeNull().And
                .Equal(books);
            service.Genres.Should()
                .NotBeNullOrEmpty().And
                .NotContainNulls().And
                .ContainSingle(g => g.Id == default);
        }

        [Theory]
        [InlineData(5, 1, SortByEnum.Ascending, "Name")]
        [InlineData(3, 2, SortByEnum.Descending, null)]
        public void GetListForSeries_ExistingSeries(int pageSize, int pageNo, SortByEnum sort, string? search)
        {
            var s1 = Helpers.GetSeries(1);
            var s2 = Helpers.GetSeries(2);
            var b1 = Helpers.GetBook(1);
            var b2 = Helpers.GetBook(2);
            var b3 = Helpers.GetBook(3);
            var b4 = Helpers.GetBook(4);
            b1.Series = s1;
            b1.SeriesId = s1.Id;
            b2.Series = s1;
            b2.SeriesId = s1.Id;
            b3.Series = s2;
            b3.SeriesId = s2.Id;
            b4.Series = null;
            b4.SeriesId = null;
            var books = new List<Book> { b1, b2, b3, b4 }.AsQueryable();
            var series = Helpers.GetSeries(s1.Id);
            var commands = new Mock<IBookCommandsRepository>();
            var queries = new Mock<IBookModuleQueriesRepository>();
            queries.Setup(q => q.GetBookSeries(s1.Id))
                .Returns(series);
            queries.Setup(q => q.GetAllBooks())
                .Returns(books);
            var mapper = new Mock<IMapper>().Object;
            var lang = new Mock<ILanguageRepository>().Object;
            var service = new BookServiceForTest(commands.Object, queries.Object, lang, mapper);

            var result = service.GetListForSeries(s1.Id, sort, pageSize, pageNo, search);

            result.Should()
                .NotBeNull().And
                .BeOfType<ListBookForSeriesVM>();
            queries.Verify(
                q => q.GetBookSeries(s1.Id),
                Times.Once);
            queries.Verify(
                q => q.GetAllBooks(),
                Times.Once);
            queries.VerifyNoOtherCalls();
            commands.VerifyNoOtherCalls();
            result.Paging.Should().NotBeNull();
            result.Paging.CurrentPage.Should().Be(pageNo);
            result.Paging.PageSize.Should().Be(pageSize);
            result.Filtering.Should().NotBeNull();
            result.Filtering.SearchString.Should()
                .NotBeNull().And
                .Be(search ?? string.Empty);
            result.Filtering.SortBy.Should().Be(sort);
            service.Books.Should()
                .NotBeNull().And
                .Equal(books);
            service.Series.Should().Be(series);
        }

        [Theory]
        [InlineData(5, 1, SortByEnum.Ascending, "Name")]
        [InlineData(3, 2, SortByEnum.Descending, null)]
        public void GetListForSeries_NotExistingSeries(int pageSize, int pageNo, SortByEnum sort, string? search)
        {
            var s1 = Helpers.GetSeries(1);
            var s2 = Helpers.GetSeries(2);
            var seriesId = 3;
            var b1 = Helpers.GetBook(1);
            var b2 = Helpers.GetBook(2);
            var b3 = Helpers.GetBook(3);
            var b4 = Helpers.GetBook(4);
            b1.Series = s1;
            b1.SeriesId = s1.Id;
            b2.Series = s1;
            b2.SeriesId = s1.Id;
            b3.Series = s2;
            b3.SeriesId = s2.Id;
            b4.Series = null;
            b4.SeriesId = null;
            var books = new List<Book> { b1, b2, b3, b4 }.AsQueryable();
            var commands = new Mock<IBookCommandsRepository>();
            var queries = new Mock<IBookModuleQueriesRepository>();
            queries.Setup(q => q.GetBookSeries(seriesId))
                .Returns((BookSeries?)null);
            queries.Setup(q => q.GetAllBooks())
                .Returns(books);
            var mapper = new Mock<IMapper>().Object;
            var lang = new Mock<ILanguageRepository>().Object;
            var service = new BookServiceForTest(commands.Object, queries.Object, lang, mapper);

            var result = service.GetListForSeries(seriesId, sort, pageSize, pageNo, search);

            result.Should()
                .NotBeNull().And
                .BeOfType<ListBookForSeriesVM>();
            queries.Verify(
                q => q.GetBookSeries(seriesId),
                Times.Once);
            queries.Verify(
                q => q.GetAllBooks(),
                Times.Once);
            queries.VerifyNoOtherCalls();
            commands.VerifyNoOtherCalls();
            result.Paging.Should().NotBeNull();
            result.Paging.CurrentPage.Should().Be(pageNo);
            result.Paging.PageSize.Should().Be(pageSize);
            result.Filtering.Should().NotBeNull();
            result.Filtering.SearchString.Should()
                .NotBeNull().And
                .Be(search ?? string.Empty);
            result.Filtering.SortBy.Should().Be(sort);
            service.Books.Should()
                .NotBeNull().And
                .Equal(books);
            service.Series.Should().NotBeNull();
            service.Series!.Id.Should().Be(default);
        }

        [Fact]
        public void GetPartialListForAuthor_ExistingAuthor()
        {
            var author = Helpers.GetAuthor(1);
            var b1 = Helpers.GetBook(1);
            var b2 = Helpers.GetBook(2);
            var b3 = Helpers.GetBook(3);
            var b4 = Helpers.GetBook(4);
            author.Books = new Book[] { b1, b2, b3, b4 };
            var commands = new Mock<IBookCommandsRepository>();
            var queries = new Mock<IBookModuleQueriesRepository>();
            queries.Setup(q => q.GetAuthorWithBooks(author.Id))
                .Returns(author);
            var mapper = new Mock<IMapper>().Object;
            var lang = new Mock<ILanguageRepository>().Object;
            var service = new BookServiceForTest(commands.Object, queries.Object, lang, mapper);
            int pageSize = 2, pageNo = 3;

            var result = service.GetPartialListForAuthor(author.Id, pageSize, pageNo);

            result.Should()
                .NotBeNull().And
                .BeOfType<PartialList<Book>>();
            queries.Verify(
                q => q.GetAuthorWithBooks(author.Id),
                Times.Once);
            queries.VerifyNoOtherCalls();
            commands.VerifyNoOtherCalls();
            result.Paging.Should().NotBeNull();
            result.Paging.CurrentPage.Should().Be(pageNo);
            result.Paging.PageSize.Should().Be(pageSize);
            service.Books.Should()
                .NotBeNullOrEmpty().And
                .Equal(author.Books);
        }

        [Fact]
        public void GetPartialListForAuthor_NotExistingAuthor()
        {
            var authorId = 1;
            var commands = new Mock<IBookCommandsRepository>();
            var queries = new Mock<IBookModuleQueriesRepository>();
            queries.Setup(q => q.GetAuthorWithBooks(authorId))
                .Returns((Author?)null);
            var mapper = new Mock<IMapper>().Object;
            var lang = new Mock<ILanguageRepository>().Object;
            var service = new BookServiceForTest(commands.Object, queries.Object, lang, mapper);
            int pageSize = 2, pageNo = 3;

            var result = service.GetPartialListForAuthor(authorId, pageSize, pageNo);

            result.Should()
                .NotBeNull().And
                .BeOfType<PartialList<Book>>();
            queries.Verify(
                q => q.GetAuthorWithBooks(authorId),
                Times.Once);
            queries.VerifyNoOtherCalls();
            commands.VerifyNoOtherCalls();
            result.Paging.Should().NotBeNull();
            result.Paging.CurrentPage.Should().Be(1);
            result.Paging.PageSize.Should().Be(pageSize);
            result.Values.Should().NotBeNull().And.BeEmpty();
            service.Books.Should().BeNull();
        }

        [Fact]
        public void GetPartialListForGenre_ExistingGenre()
        {
            var genre = Helpers.GetGenre(1);
            var b1 = Helpers.GetBook(1);
            var b2 = Helpers.GetBook(2);
            var b3 = Helpers.GetBook(3);
            var b4 = Helpers.GetBook(4);
            genre.Books = new Book[] { b1, b2, b3, b4 };
            var commands = new Mock<IBookCommandsRepository>();
            var queries = new Mock<IBookModuleQueriesRepository>();
            queries.Setup(q => q.GetLiteratureGenreWithBooks(genre.Id))
                .Returns(genre);
            var mapper = new Mock<IMapper>().Object;
            var lang = new Mock<ILanguageRepository>().Object;
            var service = new BookServiceForTest(commands.Object, queries.Object, lang, mapper);
            int pageSize = 2, pageNo = 3;

            var result = service.GetPartialListForGenre(genre.Id, pageSize, pageNo);

            result.Should()
                .NotBeNull().And
                .BeOfType<PartialList<Book>>();
            queries.Verify(
                q => q.GetLiteratureGenreWithBooks(genre.Id),
                Times.Once);
            queries.VerifyNoOtherCalls();
            commands.VerifyNoOtherCalls();
            result.Paging.Should().NotBeNull();
            result.Paging.CurrentPage.Should().Be(pageNo);
            result.Paging.PageSize.Should().Be(pageSize);
            service.Books.Should()
                .NotBeNullOrEmpty().And
                .Equal(genre.Books);
        }

        [Fact]
        public void GetPartialListForGenre_NotExistingGenre()
        {
            var genreId = 1;
            var commands = new Mock<IBookCommandsRepository>();
            var queries = new Mock<IBookModuleQueriesRepository>();
            queries.Setup(q => q.GetLiteratureGenreWithBooks(genreId))
                .Returns((LiteratureGenre?)null);
            var mapper = new Mock<IMapper>().Object;
            var lang = new Mock<ILanguageRepository>().Object;
            var service = new BookServiceForTest(commands.Object, queries.Object, lang, mapper);
            int pageSize = 2, pageNo = 3;

            var result = service.GetPartialListForGenre(genreId, pageSize, pageNo);

            result.Should()
                .NotBeNull().And
                .BeOfType<PartialList<Book>>();
            queries.Verify(
                q => q.GetLiteratureGenreWithBooks(genreId),
                Times.Once);
            queries.VerifyNoOtherCalls();
            commands.VerifyNoOtherCalls();
            result.Paging.Should().NotBeNull();
            result.Paging.CurrentPage.Should().Be(1);
            result.Paging.PageSize.Should().Be(pageSize);
            result.Values.Should().NotBeNull().And.BeEmpty();
            service.Books.Should().BeNull();
        }

        [Fact]
        public void SelectAuthors_ExistingBookIdAndExistingAuthorsIds()
        {
            Author author1 = new Author() { Id = 1 };
            Author author2 = new Author() { Id = 2 };
            Author author3 = new Author() { Id = 3 };
            var book = new Book()
            {
                Id = 1,
                Title = "Title",
                Authors = new List<Author>() { author1 }
            };
            var authorsIds = new[] { author2.Id, author3.Id };
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBook(book.Id)).Returns(book);
            queriesRepo.Setup(r => r.GetAuthor(author2.Id))
                .Returns(author2);
            queriesRepo.Setup(r => r.GetAuthor(author3.Id))
                .Returns(author3);
            commandsRepo.Setup(r => r.UpdateBookAuthorsRelation(book));
            IMapper mapper = new Mock<IMapper>().Object;
            var lang = new Mock<ILanguageRepository>().Object;

            var service = new BookServiceForTest(commandsRepo.Object, queriesRepo.Object, lang, mapper);
            service.SelectAuthors(book.Id, authorsIds);

            queriesRepo.Verify(r => r.GetBook(book.Id), Times.Once());
            queriesRepo.Verify(r => r.GetAuthor(author2.Id), Times.Once());
            queriesRepo.Verify(r => r.GetAuthor(author3.Id), Times.Once());
            book.Authors.Should().HaveCount(2);
            book.Authors.Should().Contain(author2).And.Contain(author3).And.NotContain(author1);
            commandsRepo.Verify(r => r.UpdateBookAuthorsRelation
            (
                It.Is<Book>(b =>
                    b.Equals(book) &&
                    b.Authors != null && b.Authors.Count == 2 &&
                    b.Authors.Contains(author2) && b.Authors.Contains(author3) && !b.Authors.Contains(author1))
            ), Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectAuthors_ExistingBookIdAndNotExistingAuthorsIds()
        {
            var book = new Book()
            {
                Id = 1,
                Title = "Title",
                Authors = new List<Author>() { new Author() { Id = 1 } }
            };
            var authorsIds = new[] { 2, 3 };
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBook(book.Id)).Returns(book);
            queriesRepo.Setup(r => r.GetAuthor(2)).Returns((Author?)null);
            queriesRepo.Setup(r => r.GetAuthor(3)).Returns((Author?)null);
            commandsRepo.Setup(r => r.UpdateBookAuthorsRelation(book));
            IMapper mapper = new Mock<IMapper>().Object;
            var lang = new Mock<ILanguageRepository>().Object;

            var service = new BookServiceForTest(commandsRepo.Object, queriesRepo.Object, lang, mapper);
            service.SelectAuthors(book.Id, authorsIds);

            queriesRepo.Verify(r => r.GetBook(book.Id), Times.Once());
            queriesRepo.Verify(r => r.GetAuthor(2), Times.Once());
            queriesRepo.Verify(r => r.GetAuthor(3), Times.Once());
            book.Authors.Should().NotBeNull().And.BeEmpty();
            commandsRepo.Verify(r => r.UpdateBookAuthorsRelation
            (
                It.Is<Book>(b =>
                    b.Equals(book) &&
                    b.Authors != null && b.Authors.Count == 0)
            ), Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectAuthors_ExistingBookIdAndEmptyList()
        {
            var book = new Book()
            {
                Id = 1,
                Title = "Title",
                Authors = new List<Author>() { new Author() { Id = 1 } }
            };
            var authorsIds = Array.Empty<int>();
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBook(book.Id)).Returns(book);
            queriesRepo.Setup(r => r.GetAuthor(It.IsAny<int>()));
            commandsRepo.Setup(r => r.UpdateBookAuthorsRelation(book));
            IMapper mapper = new Mock<IMapper>().Object;
            var lang = new Mock<ILanguageRepository>().Object;

            var service = new BookServiceForTest(commandsRepo.Object, queriesRepo.Object, lang, mapper);
            service.SelectAuthors(book.Id, authorsIds);

            queriesRepo.Verify(r => r.GetBook(book.Id), Times.Once());
            queriesRepo.Verify(r => r.GetAuthor(It.IsAny<int>()), Times.Never());
            book.Authors.Should().NotBeNull().And.BeEmpty();
            commandsRepo.Verify(r => r.UpdateBookAuthorsRelation
            (
                It.Is<Book>(b =>
                    b.Equals(book) &&
                    b.Authors != null && b.Authors.Count == 0)
            ), Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectAuthors_NotExistingBookIdAndAuthorsIds()
        {
            int bookId = 1;
            Author author = new Author { Id = 1 };
            var authorsIds = new[] { author.Id };
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBook(bookId)).Returns((Book?)null);
            queriesRepo.Setup(r => r.GetAuthor(author.Id)).Returns(author);
            commandsRepo.Setup(r => r.UpdateBookAuthorsRelation(It.IsAny<Book>()));
            IMapper mapper = new Mock<IMapper>().Object;
            var lang = new Mock<ILanguageRepository>().Object;

            var service = new BookServiceForTest(commandsRepo.Object, queriesRepo.Object, lang, mapper);
            service.SelectAuthors(bookId, authorsIds);

            queriesRepo.Verify(r => r.GetBook(bookId), Times.Once());
            queriesRepo.Verify(r => r.GetAuthor(It.IsAny<int>()), Times.Never());
            commandsRepo.Verify(r => r.UpdateBookAuthorsRelation(It.IsAny<Book>()), Times.Never());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectGenres_ExistingBookIdAndExistingLiteratureGenresIds()
        {
            var genre1 = new LiteratureGenre() { Id = 1, Name = "Name1" };
            var genre2 = new LiteratureGenre() { Id = 2, Name = "Name2" };
            var genre3 = new LiteratureGenre() { Id = 3, Name = "Name3" };
            var book = new Book()
            {
                Id = 1,
                Title = "Title",
                Genres = new List<LiteratureGenre>() { genre1 }
            };
            var genresIds = new[] { genre2.Id, genre3.Id };
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBook(book.Id)).Returns(book);
            queriesRepo.Setup(r => r.GetLiteratureGenre(genre2.Id)).Returns(genre2);
            queriesRepo.Setup(r => r.GetLiteratureGenre(genre3.Id)).Returns(genre3);
            commandsRepo.Setup(r => r.UpdateBookGenresRelation(book));
            IMapper mapper = new Mock<IMapper>().Object;
            var lang = new Mock<ILanguageRepository>().Object;

            var service = new BookServiceForTest(commandsRepo.Object, queriesRepo.Object, lang, mapper);
            service.SelectGenres(book.Id, genresIds);

            queriesRepo.Verify(r => r.GetBook(book.Id), Times.Once());
            queriesRepo.Verify(r => r.GetLiteratureGenre(genre2.Id), Times.Once());
            queriesRepo.Verify(r => r.GetLiteratureGenre(genre3.Id), Times.Once());
            book.Genres.Should().HaveCount(2).And.Contain(genre2).And.Contain(genre3).And.NotContain(genre1);
            commandsRepo.Verify(r => r.UpdateBookGenresRelation
            (
                It.Is<Book>(b =>
                    b.Equals(book) &&
                    b.Genres != null && b.Genres.Count == 2 &&
                    b.Genres.Contains(genre2) && b.Genres.Contains(genre3) && !b.Genres.Contains(genre1))
            ), Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectGenres_ExistingBookIdAndNotExistingLiteratureGenresIds()
        {
            var genre1 = new LiteratureGenre() { Id = 1, Name = "Name1" };
            var book = new Book()
            {
                Id = 1,
                Title = "Title",
                Genres = new List<LiteratureGenre>() { genre1 }
            };
            var genresIds = new[] { 2, 3 };
            var commendsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBook(book.Id)).Returns(book);
            queriesRepo.Setup(r => r.GetLiteratureGenre(2)).Returns((LiteratureGenre?)null);
            queriesRepo.Setup(r => r.GetLiteratureGenre(3)).Returns((LiteratureGenre?)null);
            commendsRepo.Setup(r => r.UpdateBookGenresRelation(book));
            IMapper mapper = new Mock<IMapper>().Object;
            var lang = new Mock<ILanguageRepository>().Object;

            var service = new BookServiceForTest(commendsRepo.Object, queriesRepo.Object, lang, mapper);
            service.SelectGenres(book.Id, genresIds);

            queriesRepo.Verify(r => r.GetBook(book.Id), Times.Once());
            queriesRepo.Verify(r => r.GetLiteratureGenre(2), Times.Once());
            queriesRepo.Verify(r => r.GetLiteratureGenre(3), Times.Once());
            book.Genres.Should().NotBeNull().And.BeEmpty();
            commendsRepo.Verify(r => r.UpdateBookGenresRelation
            (
                It.Is<Book>(b =>
                    b.Equals(book) &&
                    b.Genres != null && b.Genres.Count == 0)
            ), Times.Once());
            commendsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectGenres_ExistingBookIdAndEmptyList()
        {
            var genre1 = new LiteratureGenre() { Id = 1, Name = "Name1" };
            var book = new Book()
            {
                Id = 1,
                Title = "Title",
                Genres = new List<LiteratureGenre>() { genre1 }
            };
            var genresIds = Array.Empty<int>();
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBook(book.Id)).Returns(book);
            queriesRepo.Setup(r => r.GetLiteratureGenre(It.IsAny<int>()));
            commandsRepo.Setup(r => r.UpdateBookGenresRelation(book));
            IMapper mapper = new Mock<IMapper>().Object;
            var lang = new Mock<ILanguageRepository>().Object;

            var service = new BookServiceForTest(commandsRepo.Object, queriesRepo.Object, lang, mapper);
            service.SelectGenres(book.Id, genresIds);

            queriesRepo.Verify(r => r.GetBook(book.Id), Times.Once());
            queriesRepo.Verify(r => r.GetLiteratureGenre(It.IsAny<int>()), Times.Never());
            book.Genres.Should().NotBeNull().And.BeEmpty();
            commandsRepo.Verify(r => r.UpdateBookGenresRelation
            (
                It.Is<Book>(b =>
                    b.Equals(book) &&
                    b.Genres != null && b.Genres.Count == 0)
            ), Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectGenres_NotExistingBookIdAndLiteratureGenresIds()
        {
            int bookId = 1;
            var genre1 = new LiteratureGenre() { Id = 1, Name = "Name1" };
            var genresIds = new[] { genre1.Id };
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBook(bookId)).Returns((Book?)null);
            queriesRepo.Setup(r => r.GetLiteratureGenre(genre1.Id)).Returns(genre1);
            commandsRepo.Setup(r => r.UpdateBookGenresRelation(It.IsAny<Book>()));
            IMapper mapper = new Mock<IMapper>().Object;
            var lang = new Mock<ILanguageRepository>().Object;

            var service = new BookService(commandsRepo.Object, queriesRepo.Object, lang, mapper);
            service.SelectGenres(bookId, genresIds);

            queriesRepo.Verify(r => r.GetBook(bookId), Times.Once());
            queriesRepo.Verify(r => r.GetLiteratureGenre(genre1.Id), Times.Never());
            commandsRepo.Verify(r => r.UpdateBookGenresRelation(It.IsAny<Book>()), Times.Never());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectSeries_ExistingBookIdAndExistingBookSeriesId()
        {
            var series1 = new BookSeries() { Id = 1, Title = "Title1" };
            var series2 = new BookSeries() { Id = 2, Title = "Title2" };
            var book = new Book()
            {
                Id = 1,
                Title = "Title",
                Series = series1,
                SeriesId = series1.Id
            };
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBook(book.Id)).Returns(book);
            queriesRepo.Setup(r => r.GetBookSeries(series2.Id)).Returns(series2);
            commandsRepo.Setup(r => r.UpdateBookSeriesRelation(book));
            IMapper mapper = new Mock<IMapper>().Object;
            var lang = new Mock<ILanguageRepository>().Object;

            var service = new BookServiceForTest(commandsRepo.Object, queriesRepo.Object, lang, mapper);
            service.SelectSeries(book.Id, series2.Id);

            queriesRepo.Verify(r => r.GetBook(book.Id), Times.Once());
            queriesRepo.Verify(r => r.GetBookSeries(series2.Id), Times.Once());
            book.Series.Should().Be(series2);
            book.SeriesId.Should().Be(series2.Id);
            commandsRepo.Verify(r => r.UpdateBookSeriesRelation
            (
                It.Is<Book>(b =>
                    b.Equals(book) &&
                    b.Series == series2 &&
                    b.SeriesId == series2.Id)
            ), Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectSeries_ExistingBookIdAndNotExistingBookSeriesId()
        {
            var series1 = new BookSeries() { Id = 1, Title = "Title1" };
            var book = new Book()
            {
                Id = 1,
                Title = "Title",
                Series = series1,
                SeriesId = series1.Id
            };
            int newSeriesId = 2;
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBook(book.Id)).Returns(book);
            queriesRepo.Setup(r => r.GetBookSeries(newSeriesId)).Returns((BookSeries?)null);
            commandsRepo.Setup(r => r.UpdateBookSeriesRelation(book));
            IMapper mapper = new Mock<IMapper>().Object;
            var lang = new Mock<ILanguageRepository>().Object;

            var service = new BookServiceForTest(commandsRepo.Object, queriesRepo.Object, lang, mapper);
            service.SelectSeries(book.Id, newSeriesId);

            queriesRepo.Verify(r => r.GetBook(book.Id), Times.Once());
            queriesRepo.Verify(r => r.GetBookSeries(newSeriesId), Times.Once());
            book.Series.Should().BeNull();
            book.SeriesId.Should().BeNull();
            commandsRepo.Verify(r => r.UpdateBookSeriesRelation
            (
                It.Is<Book>(b =>
                    b.Equals(book) &&
                    b.Series == null &&
                    b.SeriesId == null)
            ), Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectSeries_ExistingBookIdAndNull()
        {
            var series1 = new BookSeries() { Id = 1, Title = "Title1" };
            var book = new Book()
            {
                Id = 1,
                Title = "Title",
                Series = series1,
                SeriesId = series1.Id
            };
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBook(book.Id)).Returns(book);
            queriesRepo.Setup(r => r.GetBookSeries(It.IsAny<int>()));
            commandsRepo.Setup(r => r.UpdateBookSeriesRelation(book));
            IMapper mapper = new Mock<IMapper>().Object;
            var lang = new Mock<ILanguageRepository>().Object;

            var service = new BookServiceForTest(commandsRepo.Object, queriesRepo.Object, lang, mapper);
            service.SelectSeries(book.Id, null);

            queriesRepo.Verify(r => r.GetBook(book.Id), Times.Once());
            queriesRepo.Verify(r => r.GetBookSeries(It.IsAny<int>()), Times.Never());
            book.Series.Should().BeNull();
            book.SeriesId.Should().BeNull();
            commandsRepo.Verify(r => r.UpdateBookSeriesRelation
            (
                It.Is<Book>(b =>
                    b.Equals(book) &&
                    b.Series == null &&
                    b.SeriesId == null)
            ), Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectSeries_NotExistingBookIdAndBookSeriesId()
        {
            int bookId = 1;
            var series1 = new BookSeries() { Id = 1, Title = "Title1" };
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBook(bookId)).Returns((Book?)null);
            queriesRepo.Setup(r => r.GetBookSeries(series1.Id)).Returns(series1);
            commandsRepo.Setup(r => r.UpdateBookSeriesRelation(It.IsAny<Book>()));
            IMapper mapper = new Mock<IMapper>().Object;
            var lang = new Mock<ILanguageRepository>().Object;

            var service = new BookServiceForTest(commandsRepo.Object, queriesRepo.Object, lang, mapper);
            service.SelectSeries(bookId, series1.Id);

            queriesRepo.Verify(r => r.GetBook(bookId), Times.Once());
            queriesRepo.Verify(r => r.GetBookSeries(series1.Id), Times.Never());
            commandsRepo.Verify(r => r.UpdateBookSeriesRelation(It.IsAny<Book>()), Times.Never());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void Upsert_NewBook_NewId()
        {
            var book = new BookVM() { Title = "Title" };
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            commandsRepo.Setup(r => r.Insert(It.IsAny<Book>()))
                .Returns(1);
            commandsRepo.Setup(r => r.Update(It.IsAny<Book>()));
            var mapper = new Mock<IMapper>().Object;
            var lang = new Mock<ILanguageRepository>().Object;
            var service = new BookServiceForTest(commandsRepo.Object, queriesRepo.Object, lang, mapper);

            var result = service.Upsert(book);

            commandsRepo.Verify(
                r => r.Insert(It.Is<Book>(b => b.Id == default)),
                Times.Once());
            commandsRepo.Verify(
                r => r.Update(It.IsAny<Book>()),
                Times.Never());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
            result.Should().Be(1);
        }

        [Fact]
        public void Upsert_ExistingBook_BookId()
        {
            var book = new BookVM() { Id = 1, Title = "Title" };
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            commandsRepo.Setup(r => r.Insert(It.IsAny<Book>()))
                .Returns(1);
            commandsRepo.Setup(r => r.Update(It.IsAny<Book>()));
            var mapper = new Mock<IMapper>().Object;
            var lang = new Mock<ILanguageRepository>().Object;
            var service = new BookServiceForTest(commandsRepo.Object, queriesRepo.Object, lang, mapper);

            var result = service.Upsert(book);

            commandsRepo.Verify(
                r => r.Insert(It.IsAny<Book>()),
                Times.Never());
            commandsRepo.Verify(
                r => r.Update(It.Is<Book>(b => b.Id == 1)),
                Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
            result.Should().Be(1);
        }
    }

    internal class BookServiceForTest : BookService
    {
        public IEnumerable<Author>? Authors { get; private set; }
        public IEnumerable<Book>? Books { get; private set; }
        public BookSeries? Series { get; private set; }
        public IEnumerable<LiteratureGenre>? Genres { get; private set; }

        public BookServiceForTest(IBookCommandsRepository bookCommandsRepository, IBookModuleQueriesRepository bookModuleQueriesRepository, ILanguageRepository languageRepository, IMapper mapper) : base(bookCommandsRepository, bookModuleQueriesRepository, languageRepository, mapper)
        {
        }

        protected override Book Map(BookVM book)
        {
            return new Book()
            {
                Id = book.Id,
                Title = book.Title
            };
        }

        protected override BookVM Map(Book book)
        {
            return new BookVM()
            {
                Id = book.Id
            };
        }

        protected override BookDetailsVM MapToDetails(Book book, Language? language, IEnumerable<Author> authors, IEnumerable<LiteratureGenre> genres, BookSeries? series, IEnumerable<Book>? booksInSeries)
        {
            Authors = authors;
            Books = booksInSeries;
            Genres = genres;
            Series = series;

            return new BookDetailsVM()
            {
                Id = book.Id,
                Title = book.Title,
                OriginalLanguage = language?.Name ?? string.Empty
            };
        }

        protected override ListBookForListVM MapToList(IQueryable<Book> books, SortByEnum sortBy, int pageSize, int pageNo, string searchString)
        {
            Books = books;

            return new ListBookForListVM()
            {
                Paging = new Paging()
                {
                    Count = books.Count(),
                    CurrentPage = pageNo,
                    PageSize = pageSize
                },
                Filtering = new Filtering()
                {
                    SortBy = sortBy,
                    SearchString = searchString
                }
            };
        }

        protected override ListBookForAuthorVM MapForAuthor(IQueryable<Book> books, Author author, SortByEnum sortBy, int pageSize, int pageNo, string searchString)
        {
            Books = books;
            Authors = new List<Author>() { author }.AsQueryable();

            return new ListBookForAuthorVM()
            {
                Paging = new Paging()
                {
                    Count = books.Count(),
                    CurrentPage = pageNo,
                    PageSize = pageSize
                },
                Filtering = new Filtering()
                {
                    SortBy = sortBy,
                    SearchString = searchString
                }
            };
        }

        protected override ListBookForGenreVM MapForGenre(IQueryable<Book> books, LiteratureGenre genre, SortByEnum sortBy, int pageSize, int pageNo, string searchString)
        {
            Books = books;
            Genres = new List<LiteratureGenre>() { genre }.AsQueryable();

            return new ListBookForGenreVM()
            {
                Paging = new Paging()
                {
                    Count = books.Count(),
                    CurrentPage = pageNo,
                    PageSize = pageSize
                },
                Filtering = new Filtering()
                {
                    SortBy = sortBy,
                    SearchString = searchString
                }
            };
        }

        protected override ListBookForSeriesVM MapForSeries(IQueryable<Book> books, BookSeries series, SortByEnum sortBy, int pageSize, int pageNo, string searchString)
        {
            Books = books;
            Series = series;

            return new ListBookForSeriesVM()
            {
                Paging = new Paging()
                {
                    Count = books.Count(),
                    CurrentPage = pageNo,
                    PageSize = pageSize
                },
                Filtering = new Filtering()
                {
                    SortBy = sortBy,
                    SearchString = searchString
                }
            };
        }

        protected override PartialList<Book> MapToPartialList(ICollection<Book> books, int pageSize, int pageNo)
        {
            Books = books.AsQueryable();

            return new PartialList<Book>()
            {
                Paging = new Paging()
                {
                    Count = books.Count,
                    CurrentPage = pageNo,
                    PageSize = pageSize
                }
            };
        }
    }
}