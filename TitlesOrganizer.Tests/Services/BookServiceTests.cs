// Ignore Spelling: Upsert

using AutoMapper;
using FluentAssertions;
using Moq;
using TitlesOrganizer.Application.Mappings.Abstract;
using TitlesOrganizer.Application.Services;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Common;
using TitlesOrganizer.Application.ViewModels.Concrete.BookVMs;
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
            queriesRepo.Setup(r => r.GetBook(book.Id))
                .Returns(book);
            commandsRepo.Setup(r => r.Delete(book));
            var mappings = new Mock<IBookVMsMappings>();
            var lang = new Mock<ILanguageRepository>().Object;
            var service = new BookService(commandsRepo.Object, queriesRepo.Object, lang, mappings.Object);

            service.Delete(book.Id);

            queriesRepo.Verify(
                r => r.GetBook(book.Id),
                Times.Once);
            commandsRepo.Verify(
                r => r.Delete(book),
                Times.Once);
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
            mappings.VerifyNoOtherCalls();
        }

        [Fact]
        public void Delete_NotExistingBookId()
        {
            var book = new Book() { Id = 1, Title = "Title" };
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBook(book.Id))
                .Returns((Book?)null);
            commandsRepo.Setup(r => r.Delete(book));
            var mappings = new Mock<IBookVMsMappings>();
            var lang = new Mock<ILanguageRepository>().Object;
            var service = new BookService(commandsRepo.Object, queriesRepo.Object, lang, mappings.Object);

            service.Delete(book.Id);

            queriesRepo.Verify(
                r => r.GetBook(book.Id),
                Times.Once);
            commandsRepo.Verify(
                r => r.Delete(book),
                Times.Never);
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
            mappings.VerifyNoOtherCalls();
        }

        [Fact]
        public void Get_ExistingBookIdWithSeries()
        {
            var book = Helpers.GetBook(1);
            var authors = Helpers.GetAuthorsList(2);
            var genres = Helpers.GetGenresList(3);
            var series = Helpers.GetSeries(2);
            book.Authors = authors;
            var mappedAuthors = authors.Select(a => new ForListVM() { Id = a.Id }).ToList<IForListVM>();
            book.Genres = genres;
            var mappedGenres = genres.Select(g => new ForListVM() { Id = g.Id }).ToList<IForListVM>();
            book.Series = series;
            book.SeriesId = series.Id;
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBookWithAuthorsGenresAndSeries(book.Id))
                .Returns(book);
            var mappings = new Mock<IBookVMsMappings>();
            mappings.Setup(m => m.Map<Book, BookVM>(book))
                .Returns(new BookVM() { Id = book.Id });
            mappings.Setup(m => m.Map(authors))
                .Returns(mappedAuthors);
            mappings.Setup(m => m.Map(genres))
                .Returns(mappedGenres);
            mappings.Setup(m => m.Map(series))
                .Returns(new ForListVM() { Id = series.Id });
            var lang = new Mock<ILanguageRepository>().Object;
            var service = new BookService(commandsRepo.Object, queriesRepo.Object, lang, mappings.Object);

            var result = service.Get(book.Id);

            queriesRepo.Verify(
                r => r.GetBookWithAuthorsGenresAndSeries(book.Id),
                Times.Once());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            mappings.Verify(
                m => m.Map<Book, BookVM>(book),
                Times.Once());
            mappings.Verify(
                m => m.Map(authors),
                Times.Once());
            mappings.Verify(
                m => m.Map(genres),
                Times.Once());
            mappings.Verify(
                m => m.Map(series),
                Times.Once());
            mappings.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeOfType<BookVM>();
            result.Id.Should().Be(book.Id);
            result.Authors.Should()
                .BeAssignableTo<List<IForListVM>>().And
                .Equal(mappedAuthors);
            result.Genres.Should()
                .BeAssignableTo<List<IForListVM>>().And
                .Equal(mappedGenres);
            result.Series.Should()
                .NotBeNull().And
                .BeAssignableTo<IForListVM?>();
            result.Series!.Id.Should().Be(series.Id);
        }

        [Fact]
        public void Get_ExistingBookIdWithoutSeries()
        {
            var book = Helpers.GetBook(1);
            var authors = Helpers.GetAuthorsList(2);
            var genres = Helpers.GetGenresList(3);
            book.Authors = authors;
            var mappedAuthors = authors.Select(a => new ForListVM() { Id = a.Id }).ToList<IForListVM>();
            book.Genres = genres;
            var mappedGenres = genres.Select(g => new ForListVM() { Id = g.Id }).ToList<IForListVM>();
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBookWithAuthorsGenresAndSeries(book.Id))
                .Returns(book);
            var mappings = new Mock<IBookVMsMappings>();
            mappings.Setup(m => m.Map<Book, BookVM>(book))
                .Returns(new BookVM() { Id = book.Id });
            mappings.Setup(m => m.Map(authors))
                .Returns(mappedAuthors);
            mappings.Setup(m => m.Map(genres))
                .Returns(mappedGenres);
            mappings.Setup(m => m.Map(It.IsAny<BookSeries>()));
            var lang = new Mock<ILanguageRepository>().Object;
            var service = new BookService(commandsRepo.Object, queriesRepo.Object, lang, mappings.Object);

            var result = service.Get(book.Id);

            queriesRepo.Verify(
                r => r.GetBookWithAuthorsGenresAndSeries(book.Id),
                Times.Once());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            mappings.Verify(
                m => m.Map<Book, BookVM>(book),
                Times.Once());
            mappings.Verify(
                m => m.Map(authors),
                Times.Once());
            mappings.Verify(
                m => m.Map(genres),
                Times.Once());
            mappings.Verify(
                m => m.Map(It.IsAny<BookSeries>()),
                Times.Never());
            mappings.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeOfType<BookVM>();
            result.Id.Should().Be(book.Id);
            result.Authors.Should()
                .BeAssignableTo<List<IForListVM>>().And
                .Equal(mappedAuthors);
            result.Genres.Should()
                .BeAssignableTo<List<IForListVM>>().And
                .Equal(mappedGenres);
            result.Series.Should().BeNull();
        }

        [Fact]
        public void Get_NotExistingBookId()
        {
            int bookId = 1;
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBookWithAuthorsGenresAndSeries(bookId))
                .Returns((Book?)null);
            var mappings = new Mock<IBookVMsMappings>();
            mappings.Setup(m => m.Map<Book, BookVM>(It.IsAny<Book>()));
            mappings.Setup(m => m.Map(It.IsAny<IEnumerable<Author>>()));
            mappings.Setup(m => m.Map(It.IsAny<IEnumerable<LiteratureGenre>>()));
            mappings.Setup(m => m.Map(It.IsAny<BookSeries>()));
            var lang = new Mock<ILanguageRepository>().Object;
            var service = new BookService(commandsRepo.Object, queriesRepo.Object, lang, mappings.Object);

            var result = service.Get(bookId);

            queriesRepo.Verify(
                r => r.GetBookWithAuthorsGenresAndSeries(bookId),
                Times.Once());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            mappings.Verify(
                m => m.Map<Book, BookVM>(It.IsAny<Book>()),
                Times.Never());
            mappings.Verify(
                m => m.Map(It.IsAny<IEnumerable<Author>>()),
                Times.Never());
            mappings.Verify(
                m => m.Map(It.IsAny<IEnumerable<LiteratureGenre>>()),
                Times.Never());
            mappings.Verify(
                m => m.Map(It.IsAny<BookSeries>()),
                Times.Never());
            mappings.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeOfType<BookVM>();
            result.Id.Should().Be(default);
            result.Authors.Should()
                .BeAssignableTo<List<IForListVM>>().And
                .NotBeNull().And
                .BeEmpty();
            result.Genres.Should()
                .BeAssignableTo<List<IForListVM>>().And
                .NotBeNull().And
                .BeEmpty();
            result.Series.Should().BeNull();
        }

        [Theory]
        [InlineData(null, "Part of ")]
        [InlineData(1, "1 of 3 in ")]
        public void GetDetails_ExistingBookWithSeriesAndLanguage(int? numberInSeries, string inSeries)
        {
            // Arrange
            var book = Helpers.GetBook(1);
            var series = Helpers.GetSeries(1);
            book.Series = series;
            book.SeriesId = series.Id;
            book.NumberInSeries = numberInSeries;
            book.OriginalLanguageCode = "ENG";
            var authors = Helpers.GetAuthorsList(2);
            book.Authors = authors;
            var mappedAuthors = authors.Select(a => new ForListVM() { Id = a.Id }).ToList<IForListVM>();
            var genres = Helpers.GetGenresList(2);
            book.Genres = genres;
            var mappedGenres = genres.Select(g => new ForListVM() { Id = g.Id }).ToList<IForListVM>();
            var languages = new[]
            {
                new Language() { Code="PLN", Name="Polish" },
                new Language() { Code="ENG", Name="English" }
            }.AsQueryable();
            var seriesWithBooks = Helpers.GetSeries(series.Id);
            var seriesBooks = Helpers.GetBooksList(3);
            seriesWithBooks.Books = seriesBooks;

            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBookWithAuthorsGenresAndSeries(book.Id))
                .Returns(book);
            queriesRepo.Setup(r => r.GetBookSeriesWithBooks(series.Id))
                .Returns(seriesWithBooks);
            var langRepo = new Mock<ILanguageRepository>();
            langRepo.Setup(r => r.GetAllLanguages())
                .Returns(languages);
            var mappings = new Mock<IBookVMsMappings>();
            mappings.Setup(m => m.Map<Book, BookDetailsVM>(book))
                .Returns(new BookDetailsVM() { Id = book.Id });
            mappings.Setup(m => m.Map(authors))
                .Returns(mappedAuthors);
            mappings.Setup(m => m.Map(genres))
                .Returns(mappedGenres);
            mappings.Setup(m => m.Map(series))
                .Returns(new ForListVM() { Id = series.Id });

            var service = new BookService(commandsRepo.Object, queriesRepo.Object, langRepo.Object, mappings.Object);

            // Act
            var result = service.GetDetails(book.Id);

            // Assert
            queriesRepo.Verify(
                r => r.GetBookWithAuthorsGenresAndSeries(book.Id),
                Times.Once());
            langRepo.Verify(
                r => r.GetAllLanguages(),
                Times.Once());
            queriesRepo.Verify(
                r => r.GetBookSeriesWithBooks(series.Id),
                Times.Once());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            langRepo.VerifyNoOtherCalls();
            mappings.Verify(
                m => m.Map<Book, BookDetailsVM>(book),
                Times.Once());
            mappings.Verify(
                m => m.Map(authors),
                Times.Once());
            mappings.Verify(
                m => m.Map(genres),
                Times.Once());
            mappings.Verify(
                m => m.Map(series),
                Times.Once());
            mappings.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeOfType<BookDetailsVM>();
            result.Id.Should().Be(book.Id);
            result.Authors.Should()
                .BeAssignableTo<List<IForListVM>>().And
                .Equal(mappedAuthors);
            result.Genres.Should()
                .BeAssignableTo<List<IForListVM>>().And
                .Equal(mappedGenres);
            result.Series.Should()
                .NotBeNull().And
                .BeAssignableTo<IForListVM?>();
            result.Series!.Id.Should().Be(series.Id);
            result.OriginalLanguage.Should().Be("English");
            result.InSeries.Should().Be(inSeries);
        }

        [Fact]
        public void GetDetails_ExistingBookWithoutSeriesAndLanguage()
        {// Arrange
            var book = Helpers.GetBook(1);
            book.Series = null;
            book.SeriesId = null;
            book.OriginalLanguageCode = null;
            var authors = Helpers.GetAuthorsList(2);
            book.Authors = authors;
            var mappedAuthors = authors.Select(a => new ForListVM() { Id = a.Id }).ToList<IForListVM>();
            var genres = Helpers.GetGenresList(2);
            book.Genres = genres;
            var mappedGenres = genres.Select(g => new ForListVM() { Id = g.Id }).ToList<IForListVM>();

            var commandsRepo = new Mock<IBookCommandsRepository>();

            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBookWithAuthorsGenresAndSeries(book.Id))
                .Returns(book);
            queriesRepo.Setup(r => r.GetBookSeriesWithBooks(It.IsAny<int>()));

            var langRepo = new Mock<ILanguageRepository>();
            langRepo.Setup(r => r.GetAllLanguages());

            var mappings = new Mock<IBookVMsMappings>();
            mappings.Setup(m => m.Map<Book, BookDetailsVM>(book))
                .Returns(new BookDetailsVM() { Id = book.Id });
            mappings.Setup(m => m.Map(authors))
                .Returns(mappedAuthors);
            mappings.Setup(m => m.Map(genres))
                .Returns(mappedGenres);
            mappings.Setup(m => m.Map(It.IsAny<BookSeries>()));

            var service = new BookService(commandsRepo.Object, queriesRepo.Object, langRepo.Object, mappings.Object);

            // Act
            var result = service.GetDetails(book.Id);

            // Assert
            queriesRepo.Verify(
                r => r.GetBookWithAuthorsGenresAndSeries(book.Id),
                Times.Once());
            langRepo.Verify(
                r => r.GetAllLanguages(),
                Times.Never());
            queriesRepo.Verify(
                r => r.GetBookSeriesWithBooks(It.IsAny<int>()),
                Times.Never());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            langRepo.VerifyNoOtherCalls();
            mappings.Verify(
                m => m.Map<Book, BookDetailsVM>(book),
                Times.Once());
            mappings.Verify(
                m => m.Map(authors),
                Times.Once());
            mappings.Verify(
                m => m.Map(genres),
                Times.Once());
            mappings.Verify(
                m => m.Map(It.IsAny<BookSeries>()),
                Times.Never());
            mappings.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeOfType<BookDetailsVM>();
            result.Id.Should().Be(book.Id);
            result.Authors.Should()
                .BeAssignableTo<List<IForListVM>>().And
                .Equal(mappedAuthors);
            result.Genres.Should()
                .BeAssignableTo<List<IForListVM>>().And
                .Equal(mappedGenres);
            result.OriginalLanguage.Should()
                .NotBeNull().And
                .BeEmpty();
            result.InSeries.Should()
                .NotBeNull().And
                .BeEmpty();
            result.Series.Should().BeNull();
        }

        [Fact]
        public void GetDetails_NotExistingBookId()
        {
            var bookId = 1;
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBookWithAuthorsGenresAndSeries(bookId))
                .Returns((Book?)null);
            queriesRepo.Setup(r => r.GetBookSeriesWithBooks(It.IsAny<int>()));
            var langRepo = new Mock<ILanguageRepository>();
            langRepo.Setup(r => r.GetAllLanguages());
            var mappings = new Mock<IBookVMsMappings>();
            mappings.Setup(m => m.Map<Book, BookDetailsVM>(It.IsAny<Book>()));
            mappings.Setup(m => m.Map(It.IsAny<IEnumerable<Author>>()));
            mappings.Setup(m => m.Map(It.IsAny<IEnumerable<LiteratureGenre>>()));
            mappings.Setup(m => m.Map(It.IsAny<BookSeries>()));
            var service = new BookService(commandsRepo.Object, queriesRepo.Object, langRepo.Object, mappings.Object);

            var result = service.GetDetails(bookId);

            // Assert
            queriesRepo.Verify(
                r => r.GetBookWithAuthorsGenresAndSeries(bookId),
                Times.Once());
            langRepo.Verify(
                r => r.GetAllLanguages(),
                Times.Never());
            queriesRepo.Verify(
                r => r.GetBookSeriesWithBooks(It.IsAny<int>()),
                Times.Never());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            langRepo.VerifyNoOtherCalls();
            mappings.Verify(
                m => m.Map<Book, BookDetailsVM>(It.IsAny<Book>()),
                Times.Never());
            mappings.Verify(
                m => m.Map(It.IsAny<IEnumerable<Author>>()),
                Times.Never());
            mappings.Verify(
                m => m.Map(It.IsAny<IEnumerable<LiteratureGenre>>()),
                Times.Never());
            mappings.Verify(
                m => m.Map(It.IsAny<BookSeries>()),
                Times.Never());
            mappings.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeOfType<BookDetailsVM>();
            result.Id.Should().Be(default);
            result.Authors.Should()
                .BeAssignableTo<List<IForListVM>>().And
                .NotBeNull().And
                .BeEmpty();
            result.Genres.Should()
                .BeAssignableTo<List<IForListVM>>().And
                .NotBeNull().And
                .BeEmpty();
            result.OriginalLanguage.Should()
                .NotBeNull().And
                .BeEmpty();
            result.InSeries.Should()
                .NotBeNull().And
                .BeEmpty();
            result.Series.Should().BeNull();
        }

        [Theory]
        [InlineData("Title")]
        [InlineData(null)]
        public void GetList(string? search)
        {
            int count = 3, pageSize = 2, pageNo = 2;
            SortByEnum sort = SortByEnum.Descending;
            var books = Helpers.GetBooksList(count).AsQueryable();
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            Paging paging = null;
            Filtering filtering = null;
            queriesRepo.Setup(r => r.GetAllBooks())
                .Returns(books);
            var mappings = new Mock<IBookVMsMappings>();
            mappings.Setup(m => m.Map(books, It.IsAny<Paging>(), It.IsAny<Filtering>()))
                .Callback<IQueryable<Book>, Paging, Filtering>((b, p, f) =>
                {
                    paging = p;
                    filtering = f;
                })
                .Returns(new ListVM());
            var lang = new Mock<ILanguageRepository>().Object;
            var service = new BookService(commandsRepo.Object, queriesRepo.Object, lang, mappings.Object);

            var result = service.GetList(sort, pageSize, pageNo, search);

            queriesRepo.Verify(
                r => r.GetAllBooks(),
                Times.Once());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            mappings.Verify(
                m => m.Map(books, It.IsAny<Paging>(), It.IsAny<Filtering>()),
                Times.Once());
            mappings.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeOfType<ListVM>();
            paging.Should().NotBeNull();
            paging!.PageSize.Should().Be(pageSize);
            paging.CurrentPage.Should().Be(pageNo);
            paging.Count.Should().Be(default);
            filtering.Should().NotBeNull();
            filtering!.SearchString.Should()
                .NotBeNull().And
                .Be(search ?? string.Empty);
            filtering.SortBy.Should().Be(sort);
        }

        [Theory]
        [InlineData("Name")]
        [InlineData(null)]
        public void GetListForAuthor_ExistingAuthor(string? search)
        {
            int pageSize = 4, pageNo = 2;
            SortByEnum sort = SortByEnum.Descending;
            var a1 = Helpers.GetAuthor(1);
            var a2 = Helpers.GetAuthor(2);
            var b1 = Helpers.GetBook(1);
            var b2 = Helpers.GetBook(2);
            var b3 = Helpers.GetBook(3);
            var b4 = Helpers.GetBook(4);
            b1.Authors = [a1, a2];
            b2.Authors = [a1];
            b3.Authors = [a2];
            b4.Authors = [];
            var books = new List<Book> { b1, b2, b3, b4 }.AsQueryable();
            var author = Helpers.GetAuthor(a1.Id);
            var commands = new Mock<IBookCommandsRepository>();
            var queries = new Mock<IBookModuleQueriesRepository>();
            queries.Setup(q => q.GetAuthor(a1.Id))
                .Returns(author);
            queries.Setup(q => q.GetAllBooksWithAuthorsGenresAndSeries())
                .Returns(books);
            Paging paging = null;
            Filtering filtering = null;
            var mappings = new Mock<IBookVMsMappings>();
            mappings.Setup(m => m.MapToDoubleListForItem(books, author, It.IsAny<Paging>(), It.IsAny<Filtering>()))
                .Callback<IQueryable<Book>, Author, Paging, Filtering>((b, a, p, f) =>
                {
                    paging = p;
                    filtering = f;
                })
                .Returns(new DoubleListForItemVM());
            var lang = new Mock<ILanguageRepository>().Object;
            var service = new BookService(commands.Object, queries.Object, lang, mappings.Object);

            var result = service.GetListForAuthor(a1.Id, sort, pageSize, pageNo, search);

            result.Should()
                .NotBeNull().And
                .BeOfType<DoubleListForItemVM>();
            queries.Verify(
                q => q.GetAuthor(a1.Id),
                Times.Once);
            queries.Verify(
                q => q.GetAllBooksWithAuthorsGenresAndSeries(),
                Times.Once);
            queries.VerifyNoOtherCalls();
            commands.VerifyNoOtherCalls();
            mappings.Verify(
                m => m.MapToDoubleListForItem(books, author, It.IsAny<Paging>(), It.IsAny<Filtering>()),
                Times.Once());
            mappings.VerifyNoOtherCalls();
            paging.Should().NotBeNull();
            paging!.CurrentPage.Should().Be(pageNo);
            paging.PageSize.Should().Be(pageSize);
            filtering.Should().NotBeNull();
            filtering!.SearchString.Should()
                .NotBeNull().And
                .Be(search ?? string.Empty);
            filtering.SortBy.Should().Be(sort);
        }

        [Theory]
        [InlineData("Name")]
        [InlineData(null)]
        public void GetListForAuthor_NotExistingAuthor(string? search)
        {
            int pageSize = 4, pageNo = 2;
            SortByEnum sort = SortByEnum.Descending;
            var a1 = Helpers.GetAuthor(1);
            var a2 = Helpers.GetAuthor(2);
            int authorId = 3;
            var b1 = Helpers.GetBook(1);
            var b2 = Helpers.GetBook(2);
            var b3 = Helpers.GetBook(3);
            var b4 = Helpers.GetBook(4);
            b1.Authors = [a1, a2];
            b2.Authors = [a1];
            b3.Authors = [a2];
            b4.Authors = [];
            var books = new List<Book> { b1, b2, b3, b4 }.AsQueryable();
            var commands = new Mock<IBookCommandsRepository>();
            var queries = new Mock<IBookModuleQueriesRepository>();
            queries.Setup(q => q.GetAuthor(authorId))
                .Returns((Author?)null);
            queries.Setup(q => q.GetAllBooksWithAuthorsGenresAndSeries())
                .Returns(books);
            Paging paging = null;
            Filtering filtering = null;
            var mappings = new Mock<IBookVMsMappings>();
            mappings.Setup(m => m.MapToDoubleListForItem(books, It.Is<Author>(a => a.Id == default), It.IsAny<Paging>(), It.IsAny<Filtering>()))
                .Callback<IQueryable<Book>, Author, Paging, Filtering>((b, a, p, f) =>
                {
                    paging = p;
                    filtering = f;
                })
                .Returns(new DoubleListForItemVM());
            var lang = new Mock<ILanguageRepository>().Object;
            var service = new BookService(commands.Object, queries.Object, lang, mappings.Object);

            var result = service.GetListForAuthor(authorId, sort, pageSize, pageNo, search);

            result.Should()
                .NotBeNull().And
                .BeOfType<DoubleListForItemVM>();
            queries.Verify(
                q => q.GetAuthor(authorId),
                Times.Once);
            queries.Verify(
                q => q.GetAllBooksWithAuthorsGenresAndSeries(),
                Times.Once);
            queries.VerifyNoOtherCalls();
            commands.VerifyNoOtherCalls();
            mappings.Verify(
                m => m.MapToDoubleListForItem(books, It.Is<Author>(a => a.Id == default), It.IsAny<Paging>(), It.IsAny<Filtering>()),
                Times.Once());
            mappings.VerifyNoOtherCalls();
            paging.Should().NotBeNull();
            paging!.CurrentPage.Should().Be(pageNo);
            paging.PageSize.Should().Be(pageSize);
            filtering.Should().NotBeNull();
            filtering!.SearchString.Should()
                .NotBeNull().And
                .Be(search ?? string.Empty);
            filtering.SortBy.Should().Be(sort);
        }

        [Theory]
        [InlineData("Name")]
        [InlineData(null)]
        public void GetListForGenre_ExistingLiteratureGenre(string? search)
        {
            int pageSize = 4, pageNo = 2;
            SortByEnum sort = SortByEnum.Descending;
            var g1 = Helpers.GetGenre(1);
            var g2 = Helpers.GetGenre(2);
            var b1 = Helpers.GetBook(1);
            var b2 = Helpers.GetBook(2);
            var b3 = Helpers.GetBook(3);
            var b4 = Helpers.GetBook(4);
            b1.Genres = [g1, g2];
            b2.Genres = [g1];
            b3.Genres = [g2];
            b4.Genres = [];
            var books = new List<Book> { b1, b2, b3, b4 }.AsQueryable();
            var genre = Helpers.GetGenre(g1.Id);
            var commands = new Mock<IBookCommandsRepository>();
            var queries = new Mock<IBookModuleQueriesRepository>();
            queries.Setup(q => q.GetLiteratureGenre(g1.Id))
                .Returns(genre);
            queries.Setup(q => q.GetAllBooksWithAuthorsGenresAndSeries())
                .Returns(books);
            Paging paging = null;
            Filtering filtering = null;
            var mappings = new Mock<IBookVMsMappings>();
            mappings.Setup(m => m.MapToDoubleListForItem(books, genre, It.IsAny<Paging>(), It.IsAny<Filtering>()))
                .Callback<IQueryable<Book>, LiteratureGenre, Paging, Filtering>((b, a, p, f) =>
                {
                    paging = p;
                    filtering = f;
                })
                .Returns(new DoubleListForItemVM());
            var lang = new Mock<ILanguageRepository>().Object;
            var service = new BookService(commands.Object, queries.Object, lang, mappings.Object);

            var result = service.GetListForGenre(g1.Id, sort, pageSize, pageNo, search);

            result.Should()
                .NotBeNull().And
                .BeOfType<DoubleListForItemVM>();
            queries.Verify(
                q => q.GetLiteratureGenre(g1.Id),
                Times.Once);
            queries.Verify(
                q => q.GetAllBooksWithAuthorsGenresAndSeries(),
                Times.Once);
            queries.VerifyNoOtherCalls();
            commands.VerifyNoOtherCalls();
            mappings.Verify(
                m => m.MapToDoubleListForItem(books, genre, It.IsAny<Paging>(), It.IsAny<Filtering>()),
                Times.Once());
            mappings.VerifyNoOtherCalls();
            paging.Should().NotBeNull();
            paging!.CurrentPage.Should().Be(pageNo);
            paging.PageSize.Should().Be(pageSize);
            filtering.Should().NotBeNull();
            filtering!.SearchString.Should()
                .NotBeNull().And
                .Be(search ?? string.Empty);
            filtering.SortBy.Should().Be(sort);
        }

        [Theory]
        [InlineData("Name")]
        [InlineData(null)]
        public void GetListForGenre_NotExistingLiteratureGenre(string? search)
        {
            int pageSize = 4, pageNo = 2;
            SortByEnum sort = SortByEnum.Descending;
            var g1 = Helpers.GetGenre(1);
            var g2 = Helpers.GetGenre(2);
            int genreId = 3;
            var b1 = Helpers.GetBook(1);
            var b2 = Helpers.GetBook(2);
            var b3 = Helpers.GetBook(3);
            var b4 = Helpers.GetBook(4);
            b1.Genres = [g1, g2];
            b2.Genres = [g1];
            b3.Genres = [g2];
            b4.Genres = [];
            var books = new List<Book> { b1, b2, b3, b4 }.AsQueryable();
            var commands = new Mock<IBookCommandsRepository>();
            var queries = new Mock<IBookModuleQueriesRepository>();
            queries.Setup(q => q.GetLiteratureGenre(genreId))
                .Returns((LiteratureGenre?)null);
            queries.Setup(q => q.GetAllBooksWithAuthorsGenresAndSeries())
                .Returns(books);
            Paging paging = null;
            Filtering filtering = null;
            var mappings = new Mock<IBookVMsMappings>();
            mappings.Setup(m => m.MapToDoubleListForItem(books, It.Is<LiteratureGenre>(g => g.Id == default), It.IsAny<Paging>(), It.IsAny<Filtering>()))
                .Callback<IQueryable<Book>, Author, Paging, Filtering>((b, g, p, f) =>
                {
                    paging = p;
                    filtering = f;
                })
                .Returns(new DoubleListForItemVM());
            var lang = new Mock<ILanguageRepository>().Object;
            var service = new BookService(commands.Object, queries.Object, lang, mappings.Object);

            var result = service.GetListForGenre(genreId, sort, pageSize, pageNo, search);

            result.Should()
                .NotBeNull().And
                .BeOfType<DoubleListForItemVM>();
            queries.Verify(
                q => q.GetLiteratureGenre(genreId),
                Times.Once);
            queries.Verify(
                q => q.GetAllBooksWithAuthorsGenresAndSeries(),
                Times.Once);
            queries.VerifyNoOtherCalls();
            commands.VerifyNoOtherCalls();
            mappings.Verify(
                m => m.MapToDoubleListForItem(books, It.Is<LiteratureGenre>(g => g.Id == default), It.IsAny<Paging>(), It.IsAny<Filtering>()),
                Times.Once());
            mappings.VerifyNoOtherCalls();
            paging.Should().NotBeNull();
            paging!.CurrentPage.Should().Be(pageNo);
            paging.PageSize.Should().Be(pageSize);
            filtering.Should().NotBeNull();
            filtering!.SearchString.Should()
                .NotBeNull().And
                .Be(search ?? string.Empty);
            filtering.SortBy.Should().Be(sort);
        }

        [Theory]
        [InlineData("Title")]
        [InlineData(null)]
        public void GetListForSeries_ExistingBookSeries(string? search)
        {
            int pageSize = 4, pageNo = 2;
            SortByEnum sort = SortByEnum.Descending;
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
            var mappings = new Mock<IBookVMsMappings>();
            Paging paging = null;
            Filtering filtering = null;
            mappings.Setup(m => m.MapToListForItem(books, series, It.IsAny<Paging>(), It.IsAny<Filtering>()))
                .Callback<IQueryable<Book>, BookSeries, Paging, Filtering>((a, b, p, f) =>
                {
                    paging = p;
                    filtering = f;
                })
                .Returns(new ListForItemVM());
            var lang = new Mock<ILanguageRepository>().Object;
            var service = new BookService(commands.Object, queries.Object, lang, mappings.Object);

            var result = service.GetListForSeries(s1.Id, sort, pageSize, pageNo, search);

            result.Should()
                .NotBeNull().And
                .BeOfType<ListForItemVM>();
            queries.Verify(
                q => q.GetBookSeries(s1.Id),
                Times.Once);
            queries.Verify(
                q => q.GetAllBooks(),
                Times.Once);
            queries.VerifyNoOtherCalls();
            commands.VerifyNoOtherCalls();
            mappings.Verify(
                m => m.MapToListForItem(books, series, It.IsAny<Paging>(), It.IsAny<Filtering>()),
                Times.Once());
            mappings.VerifyNoOtherCalls();
            paging.Should().NotBeNull();
            paging!.CurrentPage.Should().Be(pageNo);
            paging.PageSize.Should().Be(pageSize);
            filtering.Should().NotBeNull();
            filtering!.SearchString.Should()
                .NotBeNull().And
                .Be(search ?? string.Empty);
            filtering.SortBy.Should().Be(sort);
        }

        [Theory]
        [InlineData("Title")]
        [InlineData(null)]
        public void GetListForSeries_NotExistingBookSeries(string? search)
        {
            int pageSize = 4, pageNo = 2;
            SortByEnum sort = SortByEnum.Descending;
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
            var mappings = new Mock<IBookVMsMappings>();
            Paging paging = null;
            Filtering filtering = null;
            mappings.Setup(m => m.MapToListForItem(books, It.Is<BookSeries>(s => s.Id == default), It.IsAny<Paging>(), It.IsAny<Filtering>()))
                .Callback<IQueryable<Book>, BookSeries, Paging, Filtering>((a, b, p, f) =>
                {
                    paging = p;
                    filtering = f;
                })
                .Returns(new ListForItemVM());
            var lang = new Mock<ILanguageRepository>().Object;
            var service = new BookService(commands.Object, queries.Object, lang, mappings.Object);

            var result = service.GetListForSeries(seriesId, sort, pageSize, pageNo, search);

            result.Should()
                .NotBeNull().And
                .BeOfType<ListForItemVM>();
            queries.Verify(
                q => q.GetBookSeries(seriesId),
                Times.Once);
            queries.Verify(
                q => q.GetAllBooks(),
                Times.Once);
            queries.VerifyNoOtherCalls();
            commands.VerifyNoOtherCalls();
            mappings.Verify(
                m => m.MapToListForItem(books, It.Is<BookSeries>(s => s.Id == default), It.IsAny<Paging>(), It.IsAny<Filtering>()),
                Times.Once());
            mappings.VerifyNoOtherCalls();
            paging.Should().NotBeNull();
            paging!.CurrentPage.Should().Be(pageNo);
            paging.PageSize.Should().Be(pageSize);
            filtering.Should().NotBeNull();
            filtering!.SearchString.Should()
                .NotBeNull().And
                .Be(search ?? string.Empty);
            filtering.SortBy.Should().Be(sort);
        }

        [Fact]
        public void GetPartialListForAuthor_ExistingAuthor()
        {
            int pageSize = 2, pageNo = 3;
            var author = Helpers.GetAuthor(1);
            var books = Helpers.GetBooksList(4);
            author.Books = books;
            var commands = new Mock<IBookCommandsRepository>();
            var queries = new Mock<IBookModuleQueriesRepository>();
            queries.Setup(q => q.GetAuthorWithBooks(author.Id))
                .Returns(author);
            var mappings = new Mock<IBookVMsMappings>();
            mappings.Setup(m => m.Map(books, It.IsAny<Paging>()))
                .Returns(new PartialListVM());
            var lang = new Mock<ILanguageRepository>().Object;
            var service = new BookService(commands.Object, queries.Object, lang, mappings.Object);

            var result = service.GetPartialListForAuthor(author.Id, pageSize, pageNo);

            result.Should()
                .NotBeNull().And
                .BeOfType<PartialListVM>();
            queries.Verify(
                q => q.GetAuthorWithBooks(author.Id),
                Times.Once);
            queries.VerifyNoOtherCalls();
            commands.VerifyNoOtherCalls();
            mappings.Verify(
                m => m.Map(books, It.Is<Paging>(p => p.PageSize == pageSize && p.CurrentPage == pageNo)),
                Times.Once());
            mappings.VerifyNoOtherCalls();
        }

        [Fact]
        public void GetPartialListForAuthor_NotExistingAuthor()
        {
            int pageSize = 2, pageNo = 3, authorId = 1;
            var commands = new Mock<IBookCommandsRepository>();
            var queries = new Mock<IBookModuleQueriesRepository>();
            queries.Setup(q => q.GetAuthorWithBooks(authorId))
                .Returns((Author?)null);
            var mappings = new Mock<IBookVMsMappings>();
            mappings.Setup(m => m.Map(It.IsAny<IEnumerable<Book>>(), It.IsAny<Paging>()));
            var lang = new Mock<ILanguageRepository>().Object;
            var service = new BookService(commands.Object, queries.Object, lang, mappings.Object);

            var result = service.GetPartialListForAuthor(authorId, pageSize, pageNo);

            result.Should()
                .NotBeNull().And
                .BeAssignableTo<IPartialListVM>();
            queries.Verify(
                q => q.GetAuthorWithBooks(authorId),
                Times.Once);
            queries.VerifyNoOtherCalls();
            commands.VerifyNoOtherCalls();
            mappings.VerifyNoOtherCalls();
            result.Paging.Should().NotBeNull();
            result.Paging.CurrentPage.Should().Be(1);
            result.Paging.PageSize.Should().Be(pageSize);
            result.Values.Should().NotBeNull().And.BeEmpty();
        }

        [Fact]
        public void GetPartialListForGenre_ExistingLiteratureGenre()
        {
            int pageSize = 2, pageNo = 3;
            var genre = Helpers.GetGenre(1);
            var books = Helpers.GetBooksList(4);
            genre.Books = books;
            var commands = new Mock<IBookCommandsRepository>();
            var queries = new Mock<IBookModuleQueriesRepository>();
            queries.Setup(q => q.GetLiteratureGenreWithBooks(genre.Id))
                .Returns(genre);
            var mappings = new Mock<IBookVMsMappings>();
            mappings.Setup(m => m.Map(books, It.IsAny<Paging>()))
                .Returns(new PartialListVM());
            var lang = new Mock<ILanguageRepository>().Object;
            var service = new BookService(commands.Object, queries.Object, lang, mappings.Object);

            var result = service.GetPartialListForGenre(genre.Id, pageSize, pageNo);

            result.Should()
                .NotBeNull().And
                .BeOfType<PartialListVM>();
            queries.Verify(
                q => q.GetLiteratureGenreWithBooks(genre.Id),
                Times.Once);
            queries.VerifyNoOtherCalls();
            commands.VerifyNoOtherCalls();
            mappings.Verify(
                m => m.Map(books, It.Is<Paging>(p => p.PageSize == pageSize && p.CurrentPage == pageNo)),
                Times.Once());
            mappings.VerifyNoOtherCalls();
        }

        [Fact]
        public void GetPartialListForGenre_NotExistingLiteratureGenre()
        {
            int pageSize = 2, pageNo = 3, genreId = 1;
            var commands = new Mock<IBookCommandsRepository>();
            var queries = new Mock<IBookModuleQueriesRepository>();
            queries.Setup(q => q.GetLiteratureGenreWithBooks(genreId))
                .Returns((LiteratureGenre?)null);
            var mappings = new Mock<IBookVMsMappings>();
            mappings.Setup(m => m.Map(It.IsAny<IEnumerable<Book>>(), It.IsAny<Paging>()));
            var lang = new Mock<ILanguageRepository>().Object;
            var service = new BookService(commands.Object, queries.Object, lang, mappings.Object);

            var result = service.GetPartialListForGenre(genreId, pageSize, pageNo);

            result.Should()
                .NotBeNull().And
                .BeAssignableTo<IPartialListVM>();
            queries.Verify(
                q => q.GetLiteratureGenreWithBooks(genreId),
                Times.Once);
            queries.VerifyNoOtherCalls();
            commands.VerifyNoOtherCalls();
            mappings.VerifyNoOtherCalls();
            result.Paging.Should().NotBeNull();
            result.Paging.CurrentPage.Should().Be(1);
            result.Paging.PageSize.Should().Be(pageSize);
            result.Values.Should().NotBeNull().And.BeEmpty();
        }

        [Fact]
        public void GetPartialListForSeries_ExistingBookSeries()
        {
            int pageSize = 2, pageNo = 3;
            var series = Helpers.GetSeries(1);
            var books = Helpers.GetBooksList(4);
            series.Books = books;
            var commands = new Mock<IBookCommandsRepository>();
            var queries = new Mock<IBookModuleQueriesRepository>();
            queries.Setup(q => q.GetBookSeriesWithBooks(series.Id))
                .Returns(series);
            var mappings = new Mock<IBookVMsMappings>();
            mappings.Setup(m => m.Map(books, It.IsAny<Paging>()))
                .Returns(new PartialListVM());
            var lang = new Mock<ILanguageRepository>().Object;
            var service = new BookService(commands.Object, queries.Object, lang, mappings.Object);

            var result = service.GetPartialListForSeries(series.Id, pageSize, pageNo);

            result.Should()
                .NotBeNull().And
                .BeOfType<PartialListVM>();
            queries.Verify(
                q => q.GetBookSeriesWithBooks(series.Id),
                Times.Once);
            queries.VerifyNoOtherCalls();
            commands.VerifyNoOtherCalls();
            mappings.Verify(
                m => m.Map(books, It.Is<Paging>(p => p.PageSize == pageSize && p.CurrentPage == pageNo)),
                Times.Once());
            mappings.VerifyNoOtherCalls();
        }

        [Fact]
        public void GetPartialListForSeries_NotExistingBookSeries()
        {
            int pageSize = 2, pageNo = 3, seriesId = 1;
            var commands = new Mock<IBookCommandsRepository>();
            var queries = new Mock<IBookModuleQueriesRepository>();
            queries.Setup(q => q.GetBookSeriesWithBooks(seriesId))
                .Returns((BookSeries?)null);
            var mappings = new Mock<IBookVMsMappings>();
            mappings.Setup(m => m.Map(It.IsAny<IEnumerable<Book>>(), It.IsAny<Paging>()));
            var lang = new Mock<ILanguageRepository>().Object;
            var service = new BookService(commands.Object, queries.Object, lang, mappings.Object);

            var result = service.GetPartialListForSeries(seriesId, pageSize, pageNo);

            result.Should()
                .NotBeNull().And
                .BeAssignableTo<IPartialListVM>();
            queries.Verify(
                q => q.GetBookSeriesWithBooks(seriesId),
                Times.Once);
            queries.VerifyNoOtherCalls();
            commands.VerifyNoOtherCalls();
            mappings.VerifyNoOtherCalls();
            result.Paging.Should().NotBeNull();
            result.Paging.CurrentPage.Should().Be(1);
            result.Paging.PageSize.Should().Be(pageSize);
            result.Values.Should().NotBeNull().And.BeEmpty();
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
}