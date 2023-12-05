// Ignore Spelling: Upsert

using AutoMapper;
using FluentAssertions;
using Moq;
using TitlesOrganizer.Application.Services;
using TitlesOrganizer.Application.ViewModels.Concrete.BookVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Interfaces;
using TitlesOrganizer.Domain.Models;
using TitlesOrganizer.Tests.ViewModels.BookVMs;

namespace TitlesOrganizer.Tests.Services
{
    public class BookSeriesServiceTests
    {
        [Fact]
        public void Delete_ExistingBookSeriesId()
        {
            var series = new BookSeries()
            {
                Id = 1,
                Title = "Title"
            };
            var commandsRepo = new Mock<IBookSeriesCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBookSeries(series.Id))
                .Returns(series);
            commandsRepo.Setup(r => r.Delete(series));
            IMapper mapper = new Mock<IMapper>().Object;
            var service = new BookSeriesServiceForTest(commandsRepo.Object, queriesRepo.Object, mapper);

            service.Delete(series.Id);

            queriesRepo.Verify(
                r => r.GetBookSeries(series.Id),
                Times.Once);
            commandsRepo.Verify(
                r => r.Delete(series),
                Times.Once);
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void Delete_NotExistingBookSeriesId()
        {
            var series = new BookSeries()
            {
                Id = 1,
                Title = "Title"
            };
            var commandsRepo = new Mock<IBookSeriesCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBookSeries(series.Id))
                .Returns((BookSeries?)null);
            commandsRepo.Setup(r => r.Delete(series));
            IMapper mapper = new Mock<IMapper>().Object;
            var service = new BookSeriesServiceForTest(commandsRepo.Object, queriesRepo.Object, mapper);

            service.Delete(series.Id);

            queriesRepo.Verify(
                r => r.GetBookSeries(series.Id),
                Times.Once);
            commandsRepo.Verify(
                r => r.Delete(series),
                Times.Never);
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void Get_ExistingBookSeriesId()
        {
            int pageSize = 3, pageNo = 2, booksCount = 3;
            var books = Helpers.GetBooksList(booksCount);
            var series = new BookSeries()
            {
                Id = 1,
                Title = "Title",
                Books = books
            };
            var commandsRepo = new Mock<IBookSeriesCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBookSeriesWithBooks(series.Id))
                .Returns(series);
            var mapper = new Mock<IMapper>().Object;
            var service = new BookSeriesServiceForTest(commandsRepo.Object, queriesRepo.Object, mapper);

            var result = service.Get(series.Id, pageSize, pageNo);

            queriesRepo.Verify(
                r => r.GetBookSeriesWithBooks(series.Id),
                Times.Once());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeOfType<SeriesVM>();
            result.Id.Should().Be(series.Id);
            result.Title.Should().Be(series.Title);
            result.Books.Paging.PageSize.Should().Be(pageSize);
            result.Books.Paging.CurrentPage.Should().Be(pageNo);
            result.Books.Paging.Count.Should().Be(booksCount);
            service.Books.Should()
                .NotBeNullOrEmpty().And
                .Equal(books);
        }

        [Fact]
        public void Get_NotExistingBookSeriesId()
        {
            int pageSize = 3, pageNo = 2;
            var seriesId = 1;
            var commandsRepo = new Mock<IBookSeriesCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBookSeriesWithBooks(seriesId))
                .Returns((BookSeries?)null);
            var mapper = new Mock<IMapper>().Object;
            var service = new BookSeriesServiceForTest(commandsRepo.Object, queriesRepo.Object, mapper);

            var result = service.Get(seriesId, pageSize, pageNo);

            queriesRepo.Verify(
                r => r.GetBookSeriesWithBooks(seriesId),
                Times.Once());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeOfType<SeriesVM>();
            result.Id.Should().Be(default);
            result.Title.Should().Be(string.Empty);
            result.Books.Paging.PageSize.Should().Be(pageSize);
            result.Books.Paging.CurrentPage.Should().Be(1);
            result.Books.Paging.Count.Should().Be(0);
            service.Books.Should().BeNull();
        }

        [Fact]
        public void GetDetails_ExistingBookSeriesId()
        {
            // Arrange
            int booksPageSize = 5, booksPageNo = 2;
            var series = Helpers.GetSeries(1);
            var books = Helpers.GetBooksList(10);
            var a1 = Helpers.GetAuthor(1);
            var a2 = Helpers.GetAuthor(2);
            var a3 = Helpers.GetAuthor(3);
            var g1 = Helpers.GetGenre(1);
            var g2 = Helpers.GetGenre(2);
            var g3 = Helpers.GetGenre(3);
            var g4 = Helpers.GetGenre(4);

            books[0].Series = Helpers.GetSeries(series.Id);
            books[0].SeriesId = series.Id;
            books[1].Series = Helpers.GetSeries(series.Id);
            books[1].SeriesId = series.Id;
            books[3].Series = Helpers.GetSeries(series.Id);
            books[3].SeriesId = series.Id;
            books[4].Series = Helpers.GetSeries(series.Id);
            books[4].SeriesId = series.Id;
            books[5].Series = Helpers.GetSeries(series.Id);
            books[5].SeriesId = series.Id;
            books[7].Series = Helpers.GetSeries(series.Id);
            books[7].SeriesId = series.Id;
            books[8].Series = Helpers.GetSeries(series.Id);
            books[8].SeriesId = series.Id;

            books[1].Authors = new[] { a1, a2 };
            books[3].Authors = new[] { a2 };
            books[5].Authors = new[] { a1 };
            books[6].Authors = new[] { a1, a3 };
            books[8].Authors = new[] { a2, a1 };

            books[3].Genres = new[] { g1, g2, g3 };
            books[5].Genres = new[] { g1, g3 };
            books[6].Genres = new[] { g2, g4 };

            var commandsRepo = new Mock<IBookSeriesCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBookSeries(series.Id))
                .Returns(series);
            queriesRepo.Setup(r => r.GetAllBooksWithAuthorsGenresAndSeries())
                .Returns(books.AsQueryable());
            var mapper = new Mock<IMapper>().Object;

            var service = new BookSeriesServiceForTest(commandsRepo.Object, queriesRepo.Object, mapper);

            // Act
            var result = service.GetDetails(series.Id, booksPageSize, booksPageNo);

            // Assert
            queriesRepo.Verify(
                r => r.GetBookSeries(series.Id),
                Times.Once());
            queriesRepo.Verify(
                r => r.GetAllBooksWithAuthorsGenresAndSeries(),
                Times.Once());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            result.Should().NotBeNull().And.BeOfType<SeriesDetailsVM>();
            result.Id.Should().Be(series.Id);
            result.Title.Should().Be(series.Title);
            result.Books.Paging.PageSize.Should().Be(booksPageSize);
            result.Books.Paging.CurrentPage.Should().Be(booksPageNo);
            result.Books.Paging.Count.Should().Be(7);
            service.Books.Should()
                .NotBeNull().And
                .HaveCount(7).And
                .Contain(books[0]).And
                .Contain(books[1]).And
                .NotContain(books[2]).And
                .Contain(books[3]).And
                .Contain(books[4]).And
                .Contain(books[5]).And
                .NotContain(books[6]).And
                .Contain(books[7]).And
                .Contain(books[8]).And
                .NotContain(books[9]);
            service.Authors.Should()
                .NotBeNull().And
                .HaveCount(2).And
                .Contain(a1).And
                .Contain(a2).And
                .NotContain(a3);
            service.Genres.Should()
                .NotBeNull().And
                .HaveCount(3).And
                .Contain(g1).And
                .Contain(g2).And
                .Contain(g3).And
                .NotContain(g4);
        }

        [Fact]
        public void GetDetails_NotExistingBookSeriesId()
        {
            int booksPageSize = 5, booksPageNo = 2;
            var seriesId = 1;
            var commandsRepo = new Mock<IBookSeriesCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBookSeries(seriesId))
                .Returns((BookSeries?)null);
            queriesRepo.Setup(r => r.GetAllBooksWithAuthorsGenresAndSeries());
            var mapper = new Mock<IMapper>().Object;
            var service = new BookSeriesServiceForTest(commandsRepo.Object, queriesRepo.Object, mapper);

            var result = service.GetDetails(seriesId, booksPageSize, booksPageNo);

            queriesRepo.Verify(
                r => r.GetBookSeries(seriesId),
                Times.Once());
            queriesRepo.Verify(
                r => r.GetAllBooksWithAuthorsGenresAndSeries(),
                Times.Never());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            result.Should().NotBeNull().And.BeOfType<SeriesDetailsVM>();
            result.Id.Should().Be(default);
            result.Title.Should().Be(string.Empty);
            result.Books.Paging.PageSize.Should().Be(booksPageSize);
            result.Books.Paging.CurrentPage.Should().Be(1);
            result.Books.Paging.Count.Should().Be(0);
            service.Books.Should().BeNull();
            service.Authors.Should().BeNull();
            service.Genres.Should().BeNull();
        }

        [Theory]
        [InlineData(5, 5, 1, SortByEnum.Ascending, "Name")]
        [InlineData(6, 3, 2, SortByEnum.Descending, null)]
        public void GetList(int count, int pageSize, int pageNo, SortByEnum sort, string? search)
        {
            var series = Helpers.GetSeriesList(count).AsQueryable();
            var commandsRepo = new Mock<IBookSeriesCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetAllBookSeries())
                .Returns(series);
            var mapper = new Mock<IMapper>().Object;
            var service = new BookSeriesServiceForTest(commandsRepo.Object, queriesRepo.Object, mapper);

            var result = service.GetList(sort, pageSize, pageNo, search);

            queriesRepo.Verify(
                r => r.GetAllBookSeries(),
                Times.Once());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            service.Series.Should()
                .NotBeNull().And
                .HaveCount(count).And
                .Equal(series);
            result.Should()
                .NotBeNull().And
                .BeOfType<ListSeriesForListVM>();
            result.Paging.Should().NotBeNull();
            result.Paging.PageSize.Should().Be(pageSize);
            result.Paging.CurrentPage.Should().Be(pageNo);
            result.Paging.Count.Should().Be(count);
            result.Filtering.Should().NotBeNull();
            result.Filtering.SearchString.Should()
                .NotBeNull().And
                .Be(search ?? string.Empty);
            result.Filtering.SortBy.Should().Be(sort);
        }

        [Theory]
        [InlineData(5, 1, SortByEnum.Ascending, "Name")]
        [InlineData(3, 2, SortByEnum.Descending, null)]
        public void GetListForBook_ExistingBook(int pageSize, int pageNo, SortByEnum sort, string? search)
        {
            var b1 = Helpers.GetBook(1);
            var b2 = Helpers.GetBook(2);
            var b3 = Helpers.GetBook(2);
            var b4 = Helpers.GetBook(2);
            var s1 = Helpers.GetSeries(1);
            var s2 = Helpers.GetSeries(2);
            var s3 = Helpers.GetSeries(3);
            s1.Books = new[] { b1, b2, b3 };
            s2.Books = new[] { b4 };
            s3.Books = new List<Book>();
            var series = new[] { s1, s2, s3 }.AsQueryable();
            var book = Helpers.GetBook(b1.Id);
            var commands = new Mock<IBookSeriesCommandsRepository>();
            var queries = new Mock<IBookModuleQueriesRepository>();
            queries.Setup(q => q.GetBook(b1.Id))
                .Returns(book);
            queries.Setup(q => q.GetAllBookSeriesWithBooks())
                .Returns(series);
            var mapper = new Mock<IMapper>().Object;
            var service = new BookSeriesServiceForTest(commands.Object, queries.Object, mapper);

            var result = service.GetListForBook(b1.Id, sort, pageSize, pageNo, search);

            result.Should()
                .NotBeNull().And
                .BeOfType<ListSeriesForBookVM>();
            queries.Verify(
                q => q.GetBook(b1.Id),
                Times.Once);
            queries.Verify(
                q => q.GetAllBookSeriesWithBooks(),
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
            service.Series.Should()
                .NotBeNull().And
                .Equal(series);
            service.Books.Should()
                .NotBeNullOrEmpty().And
                .ContainSingle(b => b.Equals(book));
        }

        [Theory]
        [InlineData(5, 1, SortByEnum.Ascending, "Name")]
        [InlineData(3, 2, SortByEnum.Descending, null)]
        public void GetListForBook_NotExistingBook(int pageSize, int pageNo, SortByEnum sort, string? search)
        {
            var b1 = Helpers.GetBook(1);
            var b2 = Helpers.GetBook(2);
            var b3 = Helpers.GetBook(2);
            var b4 = Helpers.GetBook(2);
            int bookId = 5;
            var s1 = Helpers.GetSeries(1);
            var s2 = Helpers.GetSeries(2);
            var s3 = Helpers.GetSeries(3);
            s1.Books = new[] { b1, b2, b3 };
            s2.Books = new[] { b4 };
            s3.Books = new List<Book>();
            var series = new[] { s1, s2, s3 }.AsQueryable();
            var commands = new Mock<IBookSeriesCommandsRepository>();
            var queries = new Mock<IBookModuleQueriesRepository>();
            queries.Setup(q => q.GetBook(bookId))
                .Returns((Book?)null);
            queries.Setup(q => q.GetAllBookSeriesWithBooks())
                .Returns(series);
            var mapper = new Mock<IMapper>().Object;
            var service = new BookSeriesServiceForTest(commands.Object, queries.Object, mapper);

            var result = service.GetListForBook(bookId, sort, pageSize, pageNo, search);

            result.Should()
                .NotBeNull().And
                .BeOfType<ListSeriesForBookVM>();
            queries.Verify(
                q => q.GetBook(bookId),
                Times.Once);
            queries.Verify(
                q => q.GetAllBookSeriesWithBooks(),
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
            service.Series.Should()
                .NotBeNull().And
                .Equal(series);
            service.Books.Should()
                .NotBeNullOrEmpty().And
                .NotContainNulls().And
                .ContainSingle(b => b.Id == default);
        }

        [Fact]
        public void GetPartialListForAuthor_ExistingAuthor()
        {
            var author = Helpers.GetAuthor();
            var b1 = Helpers.GetBook(1);
            var b2 = Helpers.GetBook(2);
            var b3 = Helpers.GetBook(3);
            b1.Authors = new[] { author };
            b2.Authors = new[] { author };
            var s1 = Helpers.GetSeries(1);
            var s2 = Helpers.GetSeries(2);
            var s3 = Helpers.GetSeries(3);
            b1.Series = s1;
            b1.SeriesId = s1.Id;
            b2.Series = s2;
            b2.SeriesId = s2.Id;
            b3.Series = s3;
            b3.SeriesId = s3.Id;
            var books = new[] { b1, b2, b3 }.AsQueryable();
            var comm = new Mock<IBookSeriesCommandsRepository>();
            var query = new Mock<IBookModuleQueriesRepository>();
            query.Setup(q => q.GetAuthor(author.Id))
                .Returns(Helpers.GetAuthor(author.Id));
            query.Setup(q => q.GetAllBooksWithAuthorsGenresAndSeries())
                .Returns(books);
            var mapper = new Mock<IMapper>().Object;
            var service = new BookSeriesServiceForTest(comm.Object, query.Object, mapper);
            int pageSize = 2, pageNo = 3;

            var result = service.GetPartialListForAuthor(author.Id, pageSize, pageNo);

            result.Should()
                .NotBeNull().And
                .BeOfType<PartialList<BookSeries>>();
            result.Paging.Should().NotBeNull();
            result.Paging.CurrentPage.Should().Be(pageNo);
            result.Paging.PageSize.Should().Be(pageSize);
            result.Paging.Count.Should().Be(2);
            query.Verify(
                q => q.GetAuthor(author.Id),
                Times.Once);
            query.Verify(
                q => q.GetAllBooksWithAuthorsGenresAndSeries(),
                Times.Once);
            query.VerifyNoOtherCalls();
            comm.VerifyNoOtherCalls();
            service.Series.Should()
                .NotBeNullOrEmpty().And
                .HaveCount(2).And
                .Equal(s1, s2).And
                .NotContain(s3);
        }

        [Fact]
        public void GetPartialListForAuthor_NotExistingAuthor()
        {
            var authorId = 1;
            var comm = new Mock<IBookSeriesCommandsRepository>();
            var query = new Mock<IBookModuleQueriesRepository>();
            query.Setup(q => q.GetAuthor(authorId))
                .Returns((Author?)null);
            query.Setup(q => q.GetAllBooksWithAuthorsGenresAndSeries());
            var mapper = new Mock<IMapper>().Object;
            var service = new BookSeriesServiceForTest(comm.Object, query.Object, mapper);
            int pageSize = 2, pageNo = 3;

            var result = service.GetPartialListForAuthor(authorId, pageSize, pageNo);

            result.Should()
                .NotBeNull().And
                .BeOfType<PartialList<BookSeries>>();
            result.Paging.Should().NotBeNull();
            result.Paging.CurrentPage.Should().Be(1);
            result.Paging.PageSize.Should().Be(pageSize);
            result.Paging.Count.Should().Be(0);
            query.Verify(
                q => q.GetAuthor(authorId),
                Times.Once);
            query.Verify(
                q => q.GetAllBooksWithAuthorsGenresAndSeries(),
                Times.Never);
            query.VerifyNoOtherCalls();
            comm.VerifyNoOtherCalls();
            service.Series.Should().BeNull();
        }

        [Fact]
        public void GetPartialListForGenre_ExistingGenre()
        {
            var genre = Helpers.GetGenre();
            var b1 = Helpers.GetBook(1);
            var b2 = Helpers.GetBook(2);
            var b3 = Helpers.GetBook(3);
            b1.Genres = new[] { genre };
            b2.Genres = new[] { genre };
            var s1 = Helpers.GetSeries(1);
            var s2 = Helpers.GetSeries(2);
            var s3 = Helpers.GetSeries(3);
            b1.Series = s1;
            b1.SeriesId = s1.Id;
            b2.Series = s2;
            b2.SeriesId = s2.Id;
            b3.Series = s3;
            b3.SeriesId = s3.Id;
            var books = new[] { b1, b2, b3 }.AsQueryable();
            var comm = new Mock<IBookSeriesCommandsRepository>();
            var query = new Mock<IBookModuleQueriesRepository>();
            query.Setup(q => q.GetLiteratureGenre(genre.Id))
                .Returns(Helpers.GetGenre(genre.Id));
            query.Setup(q => q.GetAllBooksWithAuthorsGenresAndSeries())
                .Returns(books);
            var mapper = new Mock<IMapper>().Object;
            var service = new BookSeriesServiceForTest(comm.Object, query.Object, mapper);
            int pageSize = 2, pageNo = 3;

            var result = service.GetPartialListForGenre(genre.Id, pageSize, pageNo);

            result.Should()
                .NotBeNull().And
                .BeOfType<PartialList<BookSeries>>();
            result.Paging.Should().NotBeNull();
            result.Paging.CurrentPage.Should().Be(pageNo);
            result.Paging.PageSize.Should().Be(pageSize);
            result.Paging.Count.Should().Be(2);
            query.Verify(
                q => q.GetLiteratureGenre(genre.Id),
                Times.Once);
            query.Verify(
                q => q.GetAllBooksWithAuthorsGenresAndSeries(),
                Times.Once);
            query.VerifyNoOtherCalls();
            comm.VerifyNoOtherCalls();
            service.Series.Should()
                .NotBeNullOrEmpty().And
                .HaveCount(2).And
                .Equal(s1, s2).And
                .NotContain(s3);
        }

        [Fact]
        public void GetPartialListForGenre_NotExistingGenre()
        {
            var genreId = 1;
            var comm = new Mock<IBookSeriesCommandsRepository>();
            var query = new Mock<IBookModuleQueriesRepository>();
            query.Setup(q => q.GetLiteratureGenre(genreId))
                .Returns((LiteratureGenre?)null);
            query.Setup(q => q.GetAllBooksWithAuthorsGenresAndSeries());
            var mapper = new Mock<IMapper>().Object;
            var service = new BookSeriesServiceForTest(comm.Object, query.Object, mapper);
            int pageSize = 2, pageNo = 3;

            var result = service.GetPartialListForGenre(genreId, pageSize, pageNo);

            result.Should()
                .NotBeNull().And
                .BeOfType<PartialList<BookSeries>>();
            result.Paging.Should().NotBeNull();
            result.Paging.CurrentPage.Should().Be(1);
            result.Paging.PageSize.Should().Be(pageSize);
            result.Paging.Count.Should().Be(0);
            query.Verify(
                q => q.GetLiteratureGenre(genreId),
                Times.Once);
            query.Verify(
                q => q.GetAllBooksWithAuthorsGenresAndSeries(),
                Times.Never);
            query.VerifyNoOtherCalls();
            comm.VerifyNoOtherCalls();
            service.Series.Should().BeNull();
        }

        [Fact]
        public void SelectBooks_ExistingBookSeriesIdAndExistingBooksIds()
        {
            var book1 = new Book() { Id = 1, Title = "Title1" };
            var book2 = new Book() { Id = 2, Title = "Title2" };
            var book3 = new Book() { Id = 3, Title = "Title3" };
            var series = new BookSeries()
            {
                Id = 1,
                Title = "Title",
                Books = new[] { book1 }
            };
            var booksIds = new[] { book2.Id, book3.Id };
            var commandsRepo = new Mock<IBookSeriesCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBookSeries(series.Id))
                .Returns(series);
            queriesRepo.Setup(r => r.GetBook(book2.Id))
                .Returns(book2);
            queriesRepo.Setup(r => r.GetBook(book3.Id))
                .Returns(book3);
            commandsRepo.Setup(r => r.UpdateBookSeriesBooksRelation(series));
            var mapper = new Mock<IMapper>().Object;
            var service = new BookSeriesServiceForTest(commandsRepo.Object, queriesRepo.Object, mapper);

            service.SelectBooks(series.Id, booksIds);

            queriesRepo.Verify(
                r => r.GetBookSeries(series.Id),
                Times.Once());
            queriesRepo.Verify(
                r => r.GetBook(book2.Id),
                Times.Once());
            queriesRepo.Verify(
                r => r.GetBook(book3.Id),
                Times.Once());
            series.Books.Should()
                .HaveCount(2).And
                .Contain(book2).And
                .Contain(book3).And
                .NotContain(book1);
            commandsRepo.Verify(
                r => r.UpdateBookSeriesBooksRelation(
                    It.Is<BookSeries>(s =>
                        s.Equals(series) &&
                        s.Books != null &&
                        s.Books.Count == 2 &&
                        s.Books.Contains(book2) &&
                        s.Books.Contains(book3) &&
                        !s.Books.Contains(book1))),
                Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectBooks_ExistingBookSeriesIdAndNotExistingBooksIds()
        {
            var book1 = new Book() { Id = 1, Title = "Title1" };
            var series = new BookSeries()
            {
                Id = 1,
                Title = "Title",
                Books = new[] { book1 }
            };
            var booksIds = new[] { 2, 3 };
            var commandsRepo = new Mock<IBookSeriesCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBookSeries(series.Id))
                .Returns(series);
            queriesRepo.Setup(r => r.GetBook(2))
                .Returns((Book?)null);
            queriesRepo.Setup(r => r.GetBook(3))
                .Returns((Book?)null);
            commandsRepo.Setup(r => r.UpdateBookSeriesBooksRelation(series));
            var mapper = new Mock<IMapper>().Object;
            var service = new BookSeriesServiceForTest(commandsRepo.Object, queriesRepo.Object, mapper);

            service.SelectBooks(series.Id, booksIds);

            queriesRepo.Verify(
                r => r.GetBookSeries(series.Id),
                Times.Once());
            queriesRepo.Verify(
                r => r.GetBook(2),
                Times.Once());
            queriesRepo.Verify(
                r => r.GetBook(3),
                Times.Once());
            series.Books.Should().NotBeNull().And.BeEmpty();
            commandsRepo.Verify(
                r => r.UpdateBookSeriesBooksRelation(
                    It.Is<BookSeries>(
                        s => s.Equals(series) &&
                        s.Books != null &&
                        s.Books.Count == 0)),
                Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectBooks_ExistingBookSeriesIdAndEmptyList()
        {
            var book1 = new Book() { Id = 1, Title = "Title1" };
            var series = new BookSeries()
            {
                Id = 1,
                Title = "Title",
                Books = new[] { book1 }
            };
            var booksIds = Array.Empty<int>();
            var commandsRepo = new Mock<IBookSeriesCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBookSeries(series.Id))
                .Returns(series);
            queriesRepo.Setup(r => r.GetBook(It.IsAny<int>()));
            commandsRepo.Setup(r => r.UpdateBookSeriesBooksRelation(series));
            var mapper = new Mock<IMapper>().Object;
            var service = new BookSeriesServiceForTest(commandsRepo.Object, queriesRepo.Object, mapper);

            service.SelectBooks(series.Id, booksIds);

            queriesRepo.Verify(
                r => r.GetBookSeries(series.Id),
                Times.Once());
            queriesRepo.Verify(
                r => r.GetBook(It.IsAny<int>()),
                Times.Never());
            series.Books.Should().NotBeNull().And.BeEmpty();
            commandsRepo.Verify(
                r => r.UpdateBookSeriesBooksRelation(
                    It.Is<BookSeries>(
                        s => s.Equals(series) &&
                        s.Books != null &&
                        s.Books.Count == 0)),
                Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectBooks_NotExistingBookSeriesIdAndBooksIds()
        {
            int seriesId = 1;
            var book1 = new Book() { Id = 1, Title = "Title1" };
            var booksIds = new[] { book1.Id };
            var commandsRepo = new Mock<IBookSeriesCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBookSeries(seriesId))
                .Returns((BookSeries?)null);
            queriesRepo.Setup(r => r.GetBook(book1.Id))
                .Returns(book1);
            commandsRepo.Setup(r => r.UpdateBookSeriesBooksRelation(It.IsAny<BookSeries>()));
            var mapper = new Mock<IMapper>().Object;
            var service = new BookSeriesServiceForTest(commandsRepo.Object, queriesRepo.Object, mapper);

            service.SelectBooks(seriesId, booksIds);

            queriesRepo.Verify(
                r => r.GetBookSeries(seriesId),
                Times.Once());
            queriesRepo.Verify(
                r => r.GetBook(book1.Id),
                Times.Never());
            commandsRepo.Verify(
                r => r.UpdateBookSeriesBooksRelation(It.IsAny<BookSeries>()),
                Times.Never());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void Upsert_NewSeries_NewId()
        {
            var series = new SeriesVM() { Title = "Title" };
            var commandsRepo = new Mock<IBookSeriesCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            commandsRepo.Setup(r => r.Insert(It.IsAny<BookSeries>()))
                .Returns(1);
            commandsRepo.Setup(r => r.Update(It.IsAny<BookSeries>()));
            var mapper = new Mock<IMapper>().Object;
            var service = new BookSeriesServiceForTest(commandsRepo.Object, queriesRepo.Object, mapper);

            var result = service.Upsert(series);

            commandsRepo.Verify(
                r => r.Insert(It.Is<BookSeries>(s => s.Id == default)),
                Times.Once());
            commandsRepo.Verify(
                r => r.Update(It.IsAny<BookSeries>()),
                Times.Never());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
            result.Should().Be(1);
        }

        [Fact]
        public void Upsert_ExistingSeries_SeriesId()
        {
            var series = new SeriesVM()
            {
                Id = 1,
                Title = "Title"
            };
            var commandsRepo = new Mock<IBookSeriesCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            commandsRepo.Setup(r => r.Insert(It.IsAny<BookSeries>()))
                .Returns(1);
            commandsRepo.Setup(r => r.Update(It.IsAny<BookSeries>()));
            var mapper = new Mock<IMapper>().Object;
            var service = new BookSeriesServiceForTest(commandsRepo.Object, queriesRepo.Object, mapper);

            var result = service.Upsert(series);

            commandsRepo.Verify(
                r => r.Insert(It.IsAny<BookSeries>()),
                Times.Never());
            commandsRepo.Verify(
                r => r.Update(It.Is<BookSeries>(s => s.Id == 1)),
                Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
            result.Should().Be(1);
        }
    }

    internal class BookSeriesServiceForTest : BookSeriesService
    {
        public IEnumerable<Author>? Authors { get; private set; }
        public IEnumerable<Book>? Books { get; private set; }
        public IEnumerable<LiteratureGenre>? Genres { get; private set; }
        public IEnumerable<BookSeries>? Series { get; private set; }

        public BookSeriesServiceForTest(IBookSeriesCommandsRepository bookSeriesCommandsRepository, IBookModuleQueriesRepository bookModuleQueriesRepository, IMapper mapper) : base(bookSeriesCommandsRepository, bookModuleQueriesRepository, mapper)
        {
        }

        protected override BookSeries Map(SeriesVM series)
        {
            return new BookSeries()
            {
                Id = series.Id,
                Title = series.Title
            };
        }

        protected override SeriesVM Map(BookSeries seriesWithBooks, int bookPageSize, int bookPageNo)
        {
            Books = seriesWithBooks.Books.AsQueryable();

            return new SeriesVM()
            {
                Id = seriesWithBooks.Id,
                Title = seriesWithBooks.Title,
                Books = new PartialList<Book>()
                {
                    Paging = new Paging()
                    {
                        CurrentPage = bookPageNo,
                        PageSize = bookPageSize,
                        Count = Books.Count()
                    }
                }
            };
        }

        protected override SeriesDetailsVM MapToDetails(BookSeries series, IEnumerable<Book> books, int bookPageSize, int bookPageNo, IEnumerable<Author> authors, IEnumerable<LiteratureGenre> genres)
        {
            Books = books;
            Authors = authors;
            Genres = genres;

            return new SeriesDetailsVM()
            {
                Id = series.Id,
                Title = series.Title,
                Books = new PartialList<Book>()
                {
                    Paging = new Paging()
                    {
                        CurrentPage = bookPageNo,
                        PageSize = bookPageSize,
                        Count = books.Count()
                    }
                }
            };
        }

        protected override ListSeriesForListVM MapToList(IQueryable<BookSeries> series, SortByEnum sortBy, int pageSize, int pageNo, string searchString)
        {
            Series = series;

            return new ListSeriesForListVM()
            {
                Paging = new Paging()
                {
                    CurrentPage = pageNo,
                    PageSize = pageSize,
                    Count = series.Count()
                },
                Filtering = new Filtering()
                {
                    SearchString = searchString,
                    SortBy = sortBy
                }
            };
        }

        protected override ListSeriesForBookVM MapForBook(IQueryable<BookSeries> series, Book book, SortByEnum sortBy, int pageSize, int pageNo, string searchString)
        {
            Series = series;
            Books = new[] { book }.AsQueryable();

            return new ListSeriesForBookVM()
            {
                Paging = new Paging()
                {
                    CurrentPage = pageNo,
                    PageSize = pageSize,
                    Count = series.Count()
                },
                Filtering = new Filtering()
                {
                    SearchString = searchString,
                    SortBy = sortBy
                }
            };
        }

        protected override PartialList<BookSeries> MapToPartialList(IEnumerable<BookSeries> series, int pageSize, int pageNo)
        {
            Series = series;

            return new PartialList<BookSeries>()
            {
                Paging = new Paging()
                {
                    CurrentPage = pageNo,
                    PageSize = pageSize,
                    Count = series.Count()
                }
            };
        }
    }
}