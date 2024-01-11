// Ignore Spelling: Upsert

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
using TitlesOrganizer.Tests.Helpers;

namespace TitlesOrganizer.Tests.Services
{
    public class BookSeriesServiceTests
    {
        [Fact]
        public void Delete_ExistingBookSeriesWithBooks()
        {
            var id = 1;
            var seriesWithBooks = BookModuleHelpers.GetSeries(id);
            seriesWithBooks.Books = BookModuleHelpers.GetBooksList(3);
            var commandsRepo = new Mock<IBookSeriesCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBookSeriesWithBooks(id))
                .Returns(seriesWithBooks);
            BookSeries seriesParam = null;
            commandsRepo.Setup(r => r.UpdateBookSeriesBooksRelation(It.IsAny<BookSeries>()))
                .Callback<BookSeries>(s => seriesParam = s);
            commandsRepo.Setup(r => r.Delete(seriesWithBooks));
            var mappings = new Mock<IBookVMsMappings>();
            var service = new BookSeriesService(commandsRepo.Object, queriesRepo.Object, mappings.Object);

            service.Delete(id);

            queriesRepo.Verify(
                r => r.GetBookSeriesWithBooks(id),
                Times.Once);
            commandsRepo.Verify(
                r => r.UpdateBookSeriesBooksRelation(seriesWithBooks),
                Times.Once);
            seriesParam.Should()
                .NotBeNull().And
                .Be(seriesWithBooks);
            seriesParam!.Books.Should()
                .NotBeNull().And
                .BeEmpty();
            commandsRepo.Verify(
                r => r.Delete(seriesWithBooks),
                Times.Once);
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
            mappings.VerifyNoOtherCalls();
        }

        [Fact]
        public void Delete_ExistingBookSeriesWithoutBooks()
        {
            var id = 1;
            var seriesWithBooks = BookModuleHelpers.GetSeries(id);
            seriesWithBooks.Books = [];
            var commandsRepo = new Mock<IBookSeriesCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBookSeriesWithBooks(id))
                .Returns(seriesWithBooks);
            commandsRepo.Setup(r => r.UpdateBookSeriesBooksRelation(It.IsAny<BookSeries>()));
            commandsRepo.Setup(r => r.Delete(seriesWithBooks));
            var mappings = new Mock<IBookVMsMappings>();
            var service = new BookSeriesService(commandsRepo.Object, queriesRepo.Object, mappings.Object);

            service.Delete(id);

            queriesRepo.Verify(
                r => r.GetBookSeriesWithBooks(id),
                Times.Once);
            commandsRepo.Verify(
                r => r.UpdateBookSeriesBooksRelation(seriesWithBooks),
                Times.Never);
            commandsRepo.Verify(
                r => r.Delete(seriesWithBooks),
                Times.Once);
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
            mappings.VerifyNoOtherCalls();
        }

        [Fact]
        public void Delete_NotExistingBookSeriesId()
        {
            var seriesId = 1;
            var commandsRepo = new Mock<IBookSeriesCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBookSeriesWithBooks(seriesId))
                .Returns((BookSeries?)null);
            commandsRepo.Setup(r => r.UpdateBookSeriesBooksRelation(It.IsAny<BookSeries>()));
            commandsRepo.Setup(r => r.Delete(It.IsAny<BookSeries>()));
            var mappings = new Mock<IBookVMsMappings>();
            var service = new BookSeriesService(commandsRepo.Object, queriesRepo.Object, mappings.Object);

            service.Delete(seriesId);

            queriesRepo.Verify(
                r => r.GetBookSeriesWithBooks(seriesId),
                Times.Once);
            commandsRepo.Verify(
                r => r.UpdateBookSeriesBooksRelation(It.IsAny<BookSeries>()),
                Times.Never);
            commandsRepo.Verify(
                r => r.Delete(It.IsAny<BookSeries>()),
                Times.Never);
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
            mappings.VerifyNoOtherCalls();
        }

        [Fact]
        public void Get_ExistingBookSeriesId()
        {
            int pageSize = 3, pageNo = 2, booksCount = 3;
            var books = BookModuleHelpers.GetBooksList(booksCount);
            var series = new BookSeries()
            {
                Id = 1,
                Title = "Title",
                Books = books
            };
            var seriesVM = new SeriesVM()
            {
                Id = series.Id,
                Title = series.Title
            };
            Paging pagingParam = null;
            var commandsRepo = new Mock<IBookSeriesCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBookSeriesWithBooks(series.Id))
                .Returns(series);
            var mappings = new Mock<IBookVMsMappings>();
            mappings.Setup(m => m.Map<BookSeries, SeriesVM>(series))
                .Returns(seriesVM);
            mappings.Setup(m => m.Map(books, It.IsAny<Paging>()))
                .Callback<IEnumerable<Book>, Paging>((b, p) => pagingParam = p)
                .Returns(new PartialListVM());
            var service = new BookSeriesService(commandsRepo.Object, queriesRepo.Object, mappings.Object);

            var result = service.Get(series.Id, pageSize, pageNo);

            queriesRepo.Verify(
                r => r.GetBookSeriesWithBooks(series.Id),
                Times.Once());
            mappings.Verify(
                m => m.Map<BookSeries, SeriesVM>(series),
                Times.Once());
            mappings.Verify(
                m => m.Map(books, It.IsAny<Paging>()),
                Times.Once());
            pagingParam.Should().NotBeNull();
            pagingParam!.PageSize.Should().Be(pageSize);
            pagingParam.CurrentPage.Should().Be(pageNo);
            pagingParam.Count.Should().Be(0);
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            mappings.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeOfType<SeriesVM>();
            result.Id.Should().Be(series.Id);
            result.Title.Should().Be(series.Title);
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
            var mappings = new Mock<IBookVMsMappings>();
            mappings.Setup(m => m.Map<BookSeries, SeriesVM>(It.IsAny<BookSeries>()));
            mappings.Setup(m => m.Map(It.IsAny<IEnumerable<Book>>(), It.IsAny<Paging>()));
            var service = new BookSeriesService(commandsRepo.Object, queriesRepo.Object, mappings.Object);

            var result = service.Get(seriesId, pageSize, pageNo);

            queriesRepo.Verify(
                r => r.GetBookSeriesWithBooks(seriesId),
                Times.Once());
            mappings.Verify(
                m => m.Map<BookSeries, SeriesVM>(It.IsAny<BookSeries>()),
                Times.Never());
            mappings.Verify(
                m => m.Map(It.IsAny<IEnumerable<Book>>(), It.IsAny<Paging>()),
                Times.Never());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            mappings.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeOfType<SeriesVM>();
            result.Id.Should().Be(default);
            result.Title.Should().Be(string.Empty);
            result.Books.Paging.PageSize.Should().Be(pageSize);
            result.Books.Paging.CurrentPage.Should().Be(1);
            result.Books.Paging.Count.Should().Be(0);
        }

        [Fact]
        public void GetDetails_ExistingBookSeriesId()
        {
            // Arrange
            int booksPageSize = 5, booksPageNo = 2;
            var series = BookModuleHelpers.GetSeries(1);
            var seriesDetails = new SeriesDetailsVM() { Id = series.Id, Title = series.Title };
            var books = BookModuleHelpers.GetBooksList(10);
            var a1 = BookModuleHelpers.GetAuthor(1);
            var a2 = BookModuleHelpers.GetAuthor(2);
            var a3 = BookModuleHelpers.GetAuthor(3);
            var g1 = BookModuleHelpers.GetGenre(1);
            var g2 = BookModuleHelpers.GetGenre(2);
            var g3 = BookModuleHelpers.GetGenre(3);
            var g4 = BookModuleHelpers.GetGenre(4);

            books[0].Series = BookModuleHelpers.GetSeries(series.Id);
            books[0].SeriesId = series.Id;
            books[1].Series = BookModuleHelpers.GetSeries(series.Id);
            books[1].SeriesId = series.Id;
            books[3].Series = BookModuleHelpers.GetSeries(series.Id);
            books[3].SeriesId = series.Id;
            books[4].Series = BookModuleHelpers.GetSeries(series.Id);
            books[4].SeriesId = series.Id;
            books[5].Series = BookModuleHelpers.GetSeries(series.Id);
            books[5].SeriesId = series.Id;
            books[7].Series = BookModuleHelpers.GetSeries(series.Id);
            books[7].SeriesId = series.Id;
            books[8].Series = BookModuleHelpers.GetSeries(series.Id);
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

            var mappings = new Mock<IBookVMsMappings>();
            mappings.Setup(m => m.Map<BookSeries, SeriesDetailsVM>(series))
                .Returns(seriesDetails);
            Paging pagingParam = null;
            IEnumerable<Book> booksParam = null;
            mappings.Setup(m => m.Map(It.IsAny<IEnumerable<Book>>(), It.IsAny<Paging>()))
                .Callback<IEnumerable<Book>, Paging>((b, p) =>
                {
                    booksParam = b;
                    pagingParam = p;
                })
                .Returns(new PartialListVM());
            IEnumerable<Author> authorsParam = null;
            mappings.Setup(m => m.Map(It.IsAny<IEnumerable<Author>>()))
                .Callback<IEnumerable<Author>>(a => authorsParam = a)
                .Returns([]);
            IEnumerable<LiteratureGenre> genresParam = null;
            mappings.Setup(m => m.Map(It.IsAny<IEnumerable<LiteratureGenre>>()))
                .Callback<IEnumerable<LiteratureGenre>>(g => genresParam = g)
                .Returns([]);

            var service = new BookSeriesService(commandsRepo.Object, queriesRepo.Object, mappings.Object);

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
            mappings.Verify(
                m => m.Map<BookSeries, SeriesDetailsVM>(series),
                Times.Once());
            mappings.Verify(
                m => m.Map(It.IsAny<IEnumerable<Book>>(), It.IsAny<Paging>()),
                Times.Once());
            booksParam.Should()
                .NotBeNullOrEmpty().And
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
            pagingParam.Should().NotBeNull();
            pagingParam!.PageSize.Should().Be(booksPageSize);
            pagingParam.CurrentPage.Should().Be(booksPageNo);
            mappings.Verify(
                m => m.Map(It.IsAny<IEnumerable<Author>>()),
                Times.Once());
            authorsParam.Should()
                .NotBeNull().And
                .HaveCount(2).And
                .Contain(a1).And
                .Contain(a2).And
                .NotContain(a3);
            mappings.Verify(
                m => m.Map(It.IsAny<IEnumerable<LiteratureGenre>>()),
                Times.Once());
            genresParam.Should()
                .NotBeNull().And
                .HaveCount(3).And
                .Contain(g1).And
                .Contain(g2).And
                .Contain(g3).And
                .NotContain(g4);
            mappings.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeOfType<SeriesDetailsVM>().And
                .Be(seriesDetails);
            result.Id.Should().Be(series.Id);
            result.Title.Should().Be(series.Title);
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
            var mappings = new Mock<IBookVMsMappings>();
            mappings.Setup(m => m.Map<BookSeries, SeriesDetailsVM>(It.IsAny<BookSeries>()));
            mappings.Setup(m => m.Map(It.IsAny<IQueryable<Book>>(), It.IsAny<Paging>()));
            mappings.Setup(m => m.Map(It.IsAny<IEnumerable<Author>>()));
            mappings.Setup(m => m.Map(It.IsAny<IEnumerable<LiteratureGenre>>()));
            var service = new BookSeriesService(commandsRepo.Object, queriesRepo.Object, mappings.Object);

            var result = service.GetDetails(seriesId, booksPageSize, booksPageNo);

            queriesRepo.Verify(
                r => r.GetBookSeries(seriesId),
                Times.Once());
            queriesRepo.Verify(
                r => r.GetAllBooksWithAuthorsGenresAndSeries(),
                Times.Never());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            mappings.Verify(
                m => m.Map<BookSeries, SeriesDetailsVM>(It.IsAny<BookSeries>()),
                Times.Never());
            mappings.Verify(
                m => m.Map(It.IsAny<IQueryable<Book>>(), It.IsAny<Paging>()),
                Times.Never());
            mappings.Verify(
                m => m.Map(It.IsAny<IEnumerable<Author>>()),
                Times.Never());
            mappings.Verify(
                m => m.Map(It.IsAny<IEnumerable<LiteratureGenre>>()),
                Times.Never());
            mappings.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeOfType<SeriesDetailsVM>();
            result.Id.Should().Be(default);
            result.Title.Should().Be(string.Empty);
            result.Books.Paging.PageSize.Should().Be(booksPageSize);
            result.Books.Paging.CurrentPage.Should().Be(1);
            result.Books.Paging.Count.Should().Be(0);
        }

        [Theory]
        [InlineData("Name")]
        [InlineData(null)]
        public void GetList(string? search)
        {
            int count = 5, pageSize = 3, pageNo = 2;
            SortByEnum sort = SortByEnum.Descending;
            var series = BookModuleHelpers.GetSeriesList(count).AsQueryable();
            var commandsRepo = new Mock<IBookSeriesCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetAllBookSeries())
                .Returns(series);
            Paging paging = null;
            Filtering filtering = null;
            var mappings = new Mock<IBookVMsMappings>();
            mappings.Setup(m => m.Map(series, It.IsAny<Paging>(), It.IsAny<Filtering>()))
                .Callback<IQueryable<BookSeries>, Paging, Filtering>((s, p, f) =>
                {
                    paging = p;
                    filtering = f;
                })
                .Returns(new ListVM());
            var service = new BookSeriesService(commandsRepo.Object, queriesRepo.Object, mappings.Object);

            var result = service.GetList(sort, pageSize, pageNo, search);

            queriesRepo.Verify(
                r => r.GetAllBookSeries(),
                Times.Once());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            mappings.Verify(
                m => m.Map(series, It.IsAny<Paging>(), It.IsAny<Filtering>()),
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
        public void GetListForBook_ExistingBook(string? search)
        {
            int pageSize = 4, pageNo = 2;
            SortByEnum sort = SortByEnum.Descending;
            var book = BookModuleHelpers.GetBook(1);
            var series = BookModuleHelpers.GetSeriesList(3).AsQueryable();
            var commands = new Mock<IBookSeriesCommandsRepository>();
            var queries = new Mock<IBookModuleQueriesRepository>();
            queries.Setup(q => q.GetBook(book.Id))
                .Returns(book);
            queries.Setup(q => q.GetAllBookSeriesWithBooks())
                .Returns(series);
            var mappings = new Mock<IBookVMsMappings>();
            Paging paging = null;
            Filtering filtering = null;
            mappings.Setup(m => m.MapToListForItem(series, book, It.IsAny<Paging>(), It.IsAny<Filtering>()))
                .Callback<IQueryable<BookSeries>, Book, Paging, Filtering>((s, b, p, f) =>
                {
                    paging = p;
                    filtering = f;
                })
                .Returns(new ListForItemVM());
            var service = new BookSeriesService(commands.Object, queries.Object, mappings.Object);

            var result = service.GetListForBook(book.Id, sort, pageSize, pageNo, search);

            result.Should()
                .NotBeNull().And
                .BeOfType<ListForItemVM>();
            queries.Verify(
                q => q.GetBook(book.Id),
                Times.Once());
            queries.Verify(
                q => q.GetAllBookSeriesWithBooks(),
                Times.Once());
            queries.VerifyNoOtherCalls();
            commands.VerifyNoOtherCalls();
            mappings.Verify(
                m => m.MapToListForItem(series, book, It.IsAny<Paging>(), It.IsAny<Filtering>()),
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
        public void GetListForBook_NotExistingBook(string? search)
        {
            int pageSize = 4, pageNo = 2;
            SortByEnum sort = SortByEnum.Descending;
            var bookId = 1;
            var series = BookModuleHelpers.GetSeriesList(3).AsQueryable();
            var commands = new Mock<IBookSeriesCommandsRepository>();
            var queries = new Mock<IBookModuleQueriesRepository>();
            queries.Setup(q => q.GetBook(bookId))
                .Returns((Book?)null);
            queries.Setup(q => q.GetAllBookSeriesWithBooks())
                .Returns(series);
            var mappings = new Mock<IBookVMsMappings>();
            Paging paging = null;
            Filtering filtering = null;
            mappings.Setup(m => m.MapToListForItem(series, It.IsAny<Book>(), It.IsAny<Paging>(), It.IsAny<Filtering>()))
                .Callback<IQueryable<BookSeries>, Book, Paging, Filtering>((s, b, p, f) =>
                {
                    paging = p;
                    filtering = f;
                })
                .Returns(new ListForItemVM());
            var service = new BookSeriesService(commands.Object, queries.Object, mappings.Object);

            var result = service.GetListForBook(bookId, sort, pageSize, pageNo, search);

            result.Should()
                .NotBeNull().And
                .BeOfType<ListForItemVM>();
            queries.Verify(
                q => q.GetBook(bookId),
                Times.Once());
            queries.Verify(
                q => q.GetAllBookSeriesWithBooks(),
                Times.Once());
            queries.VerifyNoOtherCalls();
            commands.VerifyNoOtherCalls();
            mappings.Verify(
                m => m.MapToListForItem(series, It.Is<Book>(b => b.Id == default), It.IsAny<Paging>(), It.IsAny<Filtering>()),
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
            var author = BookModuleHelpers.GetAuthor();
            var b1 = BookModuleHelpers.GetBook(1);
            var b2 = BookModuleHelpers.GetBook(2);
            var b3 = BookModuleHelpers.GetBook(3);
            b1.Authors = [author];
            b2.Authors = [author];
            var s1 = BookModuleHelpers.GetSeries(1);
            var s2 = BookModuleHelpers.GetSeries(2);
            var s3 = BookModuleHelpers.GetSeries(3);
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
                .Returns(BookModuleHelpers.GetAuthor(author.Id));
            query.Setup(q => q.GetAllBooksWithAuthorsGenresAndSeries())
                .Returns(books);
            var mappings = new Mock<IBookVMsMappings>();
            IEnumerable<BookSeries> seriesParam = null;
            Paging pagingParam = null;
            mappings.Setup(m => m.Map(It.IsAny<IEnumerable<BookSeries>>(), It.IsAny<Paging>()))
                .Callback<IEnumerable<BookSeries>, Paging>((s, p) =>
                {
                    seriesParam = s;
                    pagingParam = p;
                })
                .Returns(new PartialListVM());
            var service = new BookSeriesService(comm.Object, query.Object, mappings.Object);

            var result = service.GetPartialListForAuthor(author.Id, pageSize, pageNo);

            query.Verify(
                q => q.GetAuthor(author.Id),
                Times.Once());
            query.Verify(
                q => q.GetAllBooksWithAuthorsGenresAndSeries(),
                Times.Once());
            query.VerifyNoOtherCalls();
            comm.VerifyNoOtherCalls();
            mappings.Verify(
                m => m.Map(It.IsAny<IEnumerable<BookSeries>>(), It.IsAny<Paging>()),
                Times.Once());
            mappings.VerifyNoOtherCalls();
            seriesParam.Should()
                .NotBeNullOrEmpty().And
                .HaveCount(2).And
                .Equal(s1, s2).And
                .NotContain(s3);
            pagingParam.Should().NotBeNull();
            pagingParam!.CurrentPage.Should().Be(pageNo);
            pagingParam.PageSize.Should().Be(pageSize);
            pagingParam.Count.Should().Be(default);
            result.Should()
                .NotBeNull().And
                .BeOfType<PartialListVM>();
        }

        [Fact]
        public void GetPartialListForAuthor_NotExistingAuthor()
        {
            int pageSize = 2, pageNo = 3;
            var authorId = 1;
            var comm = new Mock<IBookSeriesCommandsRepository>();
            var query = new Mock<IBookModuleQueriesRepository>();
            query.Setup(q => q.GetAuthor(authorId))
                .Returns((Author?)null);
            query.Setup(q => q.GetAllBooksWithAuthorsGenresAndSeries());
            var mappings = new Mock<IBookVMsMappings>();
            mappings.Setup(m => m.Map(It.IsAny<IEnumerable<BookSeries>>(), It.IsAny<Paging>()));
            var service = new BookSeriesService(comm.Object, query.Object, mappings.Object);

            var result = service.GetPartialListForAuthor(authorId, pageSize, pageNo);

            query.Verify(
                q => q.GetAuthor(authorId),
                Times.Once);
            query.Verify(
                q => q.GetAllBooksWithAuthorsGenresAndSeries(),
                Times.Never());
            query.VerifyNoOtherCalls();
            comm.VerifyNoOtherCalls();
            mappings.Verify(
                m => m.Map(It.IsAny<IEnumerable<BookSeries>>(), It.IsAny<Paging>()),
                Times.Never());
            mappings.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeAssignableTo<IPartialListVM>();
            result.Paging.Should().NotBeNull();
            result.Paging.CurrentPage.Should().Be(1);
            result.Paging.PageSize.Should().Be(pageSize);
            result.Paging.Count.Should().Be(0);
        }

        [Fact]
        public void GetPartialListForGenre_ExistingGenre()
        {
            int pageSize = 2, pageNo = 3;
            var genre = BookModuleHelpers.GetGenre();
            var b1 = BookModuleHelpers.GetBook(1);
            var b2 = BookModuleHelpers.GetBook(2);
            var b3 = BookModuleHelpers.GetBook(3);
            b1.Genres = new[] { genre };
            b2.Genres = new[] { genre };
            var s1 = BookModuleHelpers.GetSeries(1);
            var s2 = BookModuleHelpers.GetSeries(2);
            var s3 = BookModuleHelpers.GetSeries(3);
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
                .Returns(BookModuleHelpers.GetGenre(genre.Id));
            query.Setup(q => q.GetAllBooksWithAuthorsGenresAndSeries())
                .Returns(books);
            var mappings = new Mock<IBookVMsMappings>();
            IEnumerable<BookSeries> seriesParam = null;
            Paging pagingParam = null;
            mappings.Setup(m => m.Map(It.IsAny<IEnumerable<BookSeries>>(), It.IsAny<Paging>()))
                .Callback<IEnumerable<BookSeries>, Paging>((s, p) =>
                {
                    seriesParam = s;
                    pagingParam = p;
                })
                .Returns(new PartialListVM());
            var service = new BookSeriesService(comm.Object, query.Object, mappings.Object);

            var result = service.GetPartialListForGenre(genre.Id, pageSize, pageNo);

            query.Verify(
                q => q.GetLiteratureGenre(genre.Id),
                Times.Once);
            query.Verify(
                q => q.GetAllBooksWithAuthorsGenresAndSeries(),
                Times.Once);
            query.VerifyNoOtherCalls();
            comm.VerifyNoOtherCalls();
            mappings.Verify(
                m => m.Map(It.IsAny<IEnumerable<BookSeries>>(), It.IsAny<Paging>()),
                Times.Once());
            seriesParam.Should()
                .NotBeNullOrEmpty().And
                .HaveCount(2).And
                .Equal(s1, s2).And
                .NotContain(s3);
            pagingParam.Should().NotBeNull();
            pagingParam!.CurrentPage.Should().Be(pageNo);
            pagingParam.PageSize.Should().Be(pageSize);
            pagingParam.Count.Should().Be(default);
            result.Should()
                .NotBeNull().And
                .BeOfType<PartialListVM>();
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
            var mappings = new Mock<IBookVMsMappings>();
            mappings.Setup(m => m.Map(It.IsAny<IEnumerable<BookSeries>>(), It.IsAny<Paging>()));
            var service = new BookSeriesService(comm.Object, query.Object, mappings.Object);
            int pageSize = 2, pageNo = 3;

            var result = service.GetPartialListForGenre(genreId, pageSize, pageNo);

            query.Verify(
                q => q.GetLiteratureGenre(genreId),
                Times.Once);
            query.Verify(
                q => q.GetAllBooksWithAuthorsGenresAndSeries(),
                Times.Never);
            query.VerifyNoOtherCalls();
            comm.VerifyNoOtherCalls();
            mappings.Verify(
                m => m.Map(It.IsAny<IEnumerable<BookSeries>>(), It.IsAny<Paging>()),
                Times.Never());
            mappings.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeAssignableTo<IPartialListVM>();
            result.Paging.Should().NotBeNull();
            result.Paging.CurrentPage.Should().Be(1);
            result.Paging.PageSize.Should().Be(pageSize);
            result.Paging.Count.Should().Be(0);
        }

        [Fact]
        public void SelectBooks_ExistingBookSeriesIdAndEmptyArray()
        {
            var book = BookModuleHelpers.GetBook(1);
            var series = new BookSeries()
            {
                Id = 1,
                Title = "Title",
                Books = [book]
            };
            var booksIds = Array.Empty<int>();
            var commandsRepo = new Mock<IBookSeriesCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBookSeriesWithBooks(series.Id))
                .Returns(series);
            queriesRepo.Setup(r => r.GetBook(It.IsAny<int>()));
            BookSeries seriesParam = null;
            commandsRepo.Setup(r => r.UpdateBookSeriesBooksRelation(It.IsAny<BookSeries>()))
                .Callback<BookSeries>(s => seriesParam = s);
            var mappings = new Mock<IBookVMsMappings>();
            var service = new BookSeriesService(commandsRepo.Object, queriesRepo.Object, mappings.Object);

            service.SelectBooks(series.Id, booksIds);

            queriesRepo.Verify(
                r => r.GetBookSeriesWithBooks(series.Id),
                Times.Once());
            queriesRepo.Verify(
                r => r.GetBook(It.IsAny<int>()),
                Times.Never());
            commandsRepo.Verify(
                r => r.UpdateBookSeriesBooksRelation(series),
                Times.Once());
            seriesParam.Should()
                .NotBeNull().And
                .Be(series);
            seriesParam!.Books.Should()
                .NotBeNull().And
                .BeEmpty();
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
            mappings.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectBooks_ExistingBookSeriesIdAndExistingBooksIds()
        {
            var b1 = BookModuleHelpers.GetBook(1);
            var b2 = BookModuleHelpers.GetBook(2);
            var b3 = BookModuleHelpers.GetBook(3);
            var series = new BookSeries()
            {
                Id = 1,
                Title = "Title",
                Books = [b1]
            };
            var booksIds = new[] { b2.Id, b3.Id };
            var commandsRepo = new Mock<IBookSeriesCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBookSeriesWithBooks(series.Id))
                .Returns(series);
            queriesRepo.Setup(r => r.GetBook(b2.Id))
                .Returns(b2);
            queriesRepo.Setup(r => r.GetBook(b3.Id))
                .Returns(b3);
            BookSeries seriesParam = null;
            commandsRepo.Setup(r => r.UpdateBookSeriesBooksRelation(It.IsAny<BookSeries>()))
                .Callback<BookSeries>(s => seriesParam = s);
            var mappings = new Mock<IBookVMsMappings>();
            var service = new BookSeriesService(commandsRepo.Object, queriesRepo.Object, mappings.Object);

            service.SelectBooks(series.Id, booksIds);

            queriesRepo.Verify(
                r => r.GetBookSeriesWithBooks(series.Id),
                Times.Once());
            queriesRepo.Verify(
                r => r.GetBook(b2.Id),
                Times.Once());
            queriesRepo.Verify(
                r => r.GetBook(b3.Id),
                Times.Once());
            commandsRepo.Verify(
                r => r.UpdateBookSeriesBooksRelation(series),
                Times.Once());
            seriesParam.Should()
                .NotBeNull().And
                .Be(series);
            seriesParam!.Books.Should()
                .NotBeNull().And
                .HaveCount(2).And
                .Contain(b2).And
                .Contain(b3).And
                .NotContain(b1);
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
            mappings.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectBooks_ExistingBookSeriesIdAndNotExistingBooksIds()
        {
            var book = BookModuleHelpers.GetBook(1);
            var series = new BookSeries()
            {
                Id = 1,
                Title = "Title",
                Books = [book]
            };
            var booksIds = new[] { 2, 3 };
            var commandsRepo = new Mock<IBookSeriesCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBookSeriesWithBooks(series.Id))
                .Returns(series);
            queriesRepo.Setup(r => r.GetBook(2))
                .Returns((Book?)null);
            queriesRepo.Setup(r => r.GetBook(3))
                .Returns((Book?)null);
            BookSeries seriesParam = null;
            commandsRepo.Setup(r => r.UpdateBookSeriesBooksRelation(It.IsAny<BookSeries>()))
                .Callback<BookSeries>(s => seriesParam = s);
            var mappings = new Mock<IBookVMsMappings>();
            var service = new BookSeriesService(commandsRepo.Object, queriesRepo.Object, mappings.Object);

            service.SelectBooks(series.Id, booksIds);

            queriesRepo.Verify(
                r => r.GetBookSeriesWithBooks(series.Id),
                Times.Once());
            queriesRepo.Verify(
                r => r.GetBook(2),
                Times.Once());
            queriesRepo.Verify(
                r => r.GetBook(3),
                Times.Once());
            commandsRepo.Verify(
                r => r.UpdateBookSeriesBooksRelation(series),
                Times.Once());
            seriesParam.Should()
                .NotBeNull().And
                .Be(series);
            seriesParam!.Books.Should()
                .NotBeNull().And
                .BeEmpty();
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
            mappings.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectBooks_NotExistingBookSeriesIdAndBooksIds()
        {
            int seriesId = 1;
            var book = BookModuleHelpers.GetBook(1);
            var booksIds = new[] { book.Id };
            var commandsRepo = new Mock<IBookSeriesCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBookSeriesWithBooks(seriesId))
                .Returns((BookSeries?)null);
            queriesRepo.Setup(r => r.GetBook(book.Id))
                .Returns(book);
            commandsRepo.Setup(r => r.UpdateBookSeriesBooksRelation(It.IsAny<BookSeries>()));
            var mappings = new Mock<IBookVMsMappings>();
            var service = new BookSeriesService(commandsRepo.Object, queriesRepo.Object, mappings.Object);

            service.SelectBooks(seriesId, booksIds);

            queriesRepo.Verify(
                r => r.GetBookSeriesWithBooks(seriesId),
                Times.Once());
            queriesRepo.Verify(
                r => r.GetBook(book.Id),
                Times.Never());
            commandsRepo.Verify(
                r => r.UpdateBookSeriesBooksRelation(It.IsAny<BookSeries>()),
                Times.Never());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
            mappings.VerifyNoOtherCalls();
        }

        [Fact]
        public void Upsert_ExistingSeries_SeriesId()
        {
            var series = new SeriesVM() { Id = 1, Title = "Title" };
            var entity = new BookSeries() { Id = series.Id, Title = series.Title };
            var commandsRepo = new Mock<IBookSeriesCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            commandsRepo.Setup(r => r.Insert(entity))
                .Returns(1);
            commandsRepo.Setup(r => r.Update(entity));
            var mappings = new Mock<IBookVMsMappings>();
            mappings.Setup(m => m.Map<SeriesVM, BookSeries>(series))
                .Returns(entity);
            var service = new BookSeriesService(commandsRepo.Object, queriesRepo.Object, mappings.Object);

            var result = service.Upsert(series);

            mappings.Verify(
                m => m.Map<SeriesVM, BookSeries>(series),
                Times.Once());
            mappings.VerifyNoOtherCalls();
            entity.Id.Should().NotBe(default);
            commandsRepo.Verify(
                r => r.Insert(entity),
                Times.Never());
            commandsRepo.Verify(
                r => r.Update(entity),
                Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
            result.Should().Be(1);
        }

        [Fact]
        public void Upsert_NewSeries_NewId()
        {
            var series = new SeriesVM() { Title = "Title" };
            var entity = new BookSeries() { Id = series.Id, Title = series.Title };
            var commandsRepo = new Mock<IBookSeriesCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            commandsRepo.Setup(r => r.Insert(entity))
                .Returns(1);
            commandsRepo.Setup(r => r.Update(entity));
            var mappings = new Mock<IBookVMsMappings>();
            mappings.Setup(m => m.Map<SeriesVM, BookSeries>(series))
                .Returns(entity);
            var service = new BookSeriesService(commandsRepo.Object, queriesRepo.Object, mappings.Object);

            var result = service.Upsert(series);

            mappings.Verify(
                m => m.Map<SeriesVM, BookSeries>(series),
                Times.Once());
            mappings.VerifyNoOtherCalls();
            entity.Id.Should().Be(default);
            commandsRepo.Verify(
                r => r.Insert(entity),
                Times.Once());
            commandsRepo.Verify(
                r => r.Update(entity),
                Times.Never());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
            result.Should().Be(1);
        }
    }
}