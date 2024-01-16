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
    public class LiteratureGenreServiceTests
    {
        [Fact]
        public void Delete_ExistingLiteratureGenreId()
        {
            var genre = BookModuleHelpers.GetGenre(1);
            var commandsRepo = new Mock<ILiteratureGenreCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetLiteratureGenre(genre.Id))
                .Returns(genre);
            commandsRepo.Setup(r => r.Delete(genre));
            var mappings = new Mock<IBookVMsMappings>();
            var service = new LiteratureGenreService(commandsRepo.Object, queriesRepo.Object, mappings.Object);

            service.Delete(genre.Id);

            queriesRepo.Verify(
                r => r.GetLiteratureGenre(genre.Id),
                Times.Once);
            commandsRepo.Verify(
                r => r.Delete(genre),
                Times.Once);
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
            mappings.VerifyNoOtherCalls();
        }

        [Fact]
        public void Delete_NotExistingLiteratureGenreId()
        {
            var genreId = 1;
            var commandsRepo = new Mock<ILiteratureGenreCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetLiteratureGenre(genreId))
                .Returns((LiteratureGenre?)null);
            commandsRepo.Setup(r => r.Delete(It.IsAny<LiteratureGenre>()));
            var mappings = new Mock<IBookVMsMappings>();
            var service = new LiteratureGenreService(commandsRepo.Object, queriesRepo.Object, mappings.Object);

            service.Delete(genreId);

            queriesRepo.Verify(
                r => r.GetLiteratureGenre(genreId),
                Times.Once);
            commandsRepo.Verify(
                r => r.Delete(It.IsAny<LiteratureGenre>()),
                Times.Never);
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
            mappings.VerifyNoOtherCalls();
        }

        [Fact]
        public void Get_ExistingGenreWithBooks()
        {
            int pageSize = 3, pageNo = 2;
            var books = BookModuleHelpers.GetBooksList(3);
            var genre = new LiteratureGenre()
            {
                Id = 1,
                Name = "Name1",
                Books = books
            };
            var genreVM = new GenreVM()
            {
                Id = genre.Id,
                Name = genre.Name
            };
            var commandsRepo = new Mock<ILiteratureGenreCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetLiteratureGenreWithBooks(genre.Id))
                .Returns(genre);
            var mappings = new Mock<IBookVMsMappings>();
            mappings.Setup(m => m.Map<LiteratureGenre, GenreVM>(genre))
                .Returns(genreVM);
            Paging pagingParam = null;
            mappings.Setup(m => m.Map(books, It.IsAny<Paging>()))
                .Callback<IEnumerable<Book>, Paging>((b, p) => pagingParam = p)
                .Returns(new PartialListVM());
            var service = new LiteratureGenreService(commandsRepo.Object, queriesRepo.Object, mappings.Object);

            var result = service.Get(genre.Id, pageSize, pageNo);

            queriesRepo.Verify(
                r => r.GetLiteratureGenreWithBooks(genre.Id),
                Times.Once());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            mappings.Verify(
                m => m.Map<LiteratureGenre, GenreVM>(genre),
                Times.Once());
            mappings.Verify(
                m => m.Map(books, It.IsAny<Paging>()),
                Times.Once());
            pagingParam.Should().NotBeNull();
            pagingParam!.PageSize.Should().Be(pageSize);
            pagingParam.CurrentPage.Should().Be(pageNo);
            pagingParam.Count.Should().Be(default);
            mappings.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeOfType<GenreVM>();
            result.Id.Should().Be(genre.Id);
            result.Name.Should().Be(genre.Name);
        }

        [Fact]
        public void Get_ExistingGenreWithoutBooks()
        {
            int pageSize = 3, pageNo = 2;
            var genre = new LiteratureGenre()
            {
                Id = 1,
                Name = "Name1"
            };
            var genreVM = new GenreVM()
            {
                Id = genre.Id,
                Name = genre.Name
            };
            var commandsRepo = new Mock<ILiteratureGenreCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetLiteratureGenreWithBooks(genre.Id))
                .Returns(genre);
            var mappings = new Mock<IBookVMsMappings>();
            mappings.Setup(m => m.Map<LiteratureGenre, GenreVM>(genre))
                .Returns(genreVM);
            mappings.Setup(m => m.Map(It.IsAny<IEnumerable<Book>>(), It.IsAny<Paging>()));
            var service = new LiteratureGenreService(commandsRepo.Object, queriesRepo.Object, mappings.Object);

            var result = service.Get(genre.Id, pageSize, pageNo);

            queriesRepo.Verify(
                r => r.GetLiteratureGenreWithBooks(genre.Id),
                Times.Once());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            mappings.Verify(
                m => m.Map<LiteratureGenre, GenreVM>(genre),
                Times.Once());
            mappings.Verify(
                m => m.Map(It.IsAny<IEnumerable<Book>>(), It.IsAny<Paging>()),
                Times.Never());
            mappings.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeOfType<GenreVM>();
            result.Id.Should().Be(genre.Id);
            result.Name.Should().Be(genre.Name);
            result.Books.Should()
                .NotBeNull().And
                .BeAssignableTo<IPartialListVM>();
            result.Books.Values.Should()
                .NotBeNull().And
                .BeEmpty();
            result.Books.Paging.Should().NotBeNull();
            result.Books.Paging.PageSize.Should().Be(pageSize);
            result.Books.Paging.CurrentPage.Should().Be(1);
            result.Books.Paging.Count.Should().Be(0);
        }

        [Fact]
        public void Get_NotExistingGenreId()
        {
            int pageSize = 3, pageNo = 2;
            var genreId = 1;
            var commandsRepo = new Mock<ILiteratureGenreCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetLiteratureGenreWithBooks(genreId))
                .Returns((LiteratureGenre?)null);
            var mappings = new Mock<IBookVMsMappings>();
            mappings.Setup(m => m.Map<LiteratureGenre, GenreVM>(It.IsAny<LiteratureGenre>()));
            mappings.Setup(m => m.Map(It.IsAny<IEnumerable<Book>>(), It.IsAny<Paging>()));
            var service = new LiteratureGenreService(commandsRepo.Object, queriesRepo.Object, mappings.Object);

            var result = service.Get(genreId, pageSize, pageNo);

            queriesRepo.Verify(
                r => r.GetLiteratureGenreWithBooks(genreId),
                Times.Once());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            mappings.Verify(
                m => m.Map<LiteratureGenre, GenreVM>(It.IsAny<LiteratureGenre>()),
                Times.Never());
            mappings.Verify(
                m => m.Map(It.IsAny<IEnumerable<Book>>(), It.IsAny<Paging>()),
                Times.Never());
            mappings.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeOfType<GenreVM>();
            result.Id.Should().Be(default);
            result.Name.Should().Be(string.Empty);
            result.Books.Paging.PageSize.Should().Be(pageSize);
            result.Books.Paging.CurrentPage.Should().Be(1);
            result.Books.Paging.Count.Should().Be(0);
        }

        [Fact]
        public void GetDetails_ExistingGenreId()
        {
            // Arrange
            int authorsPageSize = 3, authorsPageNo = 4, booksPageSize = 5, booksPageNo = 2, seriesPageSize = 1, seriesPageNo = 3;
            var genre = BookModuleHelpers.GetGenre(1);
            var genreDetails = new GenreDetailsVM()
            {
                Id = genre.Id,
                Title = genre.Name
            };
            var books = BookModuleHelpers.GetBooksList(10);
            var s1 = BookModuleHelpers.GetSeries(1);
            var s2 = BookModuleHelpers.GetSeries(2);
            var s3 = BookModuleHelpers.GetSeries(3);
            var a1 = BookModuleHelpers.GetAuthor(1);
            var a2 = BookModuleHelpers.GetAuthor(2);
            var a3 = BookModuleHelpers.GetAuthor(3);
            var a4 = BookModuleHelpers.GetAuthor(4);

            books[0].Genres.Add(BookModuleHelpers.GetGenre(genre.Id));
            books[1].Genres.Add(BookModuleHelpers.GetGenre(genre.Id));
            books[3].Genres.Add(BookModuleHelpers.GetGenre(genre.Id));
            books[4].Genres.Add(BookModuleHelpers.GetGenre(genre.Id));
            books[5].Genres.Add(BookModuleHelpers.GetGenre(genre.Id));
            books[7].Genres.Add(BookModuleHelpers.GetGenre(genre.Id));
            books[8].Genres.Add(BookModuleHelpers.GetGenre(genre.Id));

            books[1].Series = s1;
            books[1].SeriesId = s1.Id;
            books[3].Series = s2;
            books[3].SeriesId = s2.Id;
            books[5].Series = s1;
            books[5].SeriesId = s1.Id;
            books[6].Series = s3;
            books[6].SeriesId = s3.Id;
            books[8].Series = s2;
            books[8].SeriesId = s2.Id;

            books[3].Creators = new[] { a1, a2, a3 };
            books[5].Creators = new[] { a1, a3 };
            books[6].Creators = new[] { a2, a4 };

            var commandsRepo = new Mock<ILiteratureGenreCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetLiteratureGenre(genre.Id))
                .Returns(genre);
            queriesRepo.Setup(r => r.GetAllBooksWithAuthorsGenresAndSeries())
                .Returns(books.AsQueryable());
            var mappings = new Mock<IBookVMsMappings>();
            mappings.Setup(m => m.Map<LiteratureGenre, GenreDetailsVM>(genre))
                .Returns(genreDetails);
            Paging booksPagingParam = null;
            IEnumerable<Book> booksParam = null;
            mappings.Setup(m => m.Map(It.IsAny<IEnumerable<Book>>(), It.IsAny<Paging>()))
                .Callback<IEnumerable<Book>, Paging>((b, p) =>
                {
                    booksParam = b;
                    booksPagingParam = p;
                })
                .Returns(new PartialListVM());
            Paging authorsPagingParam = null;
            IEnumerable<Creator> authorsParam = null;
            mappings.Setup(m => m.Map(It.IsAny<IEnumerable<Creator>>(), It.IsAny<Paging>()))
                .Callback<IEnumerable<Creator>, Paging>((a, p) =>
                {
                    authorsParam = a;
                    authorsPagingParam = p;
                })
                .Returns(new PartialListVM());
            Paging seriesPagingParam = null;
            IEnumerable<BookSeries> seriesParam = null;
            mappings.Setup(m => m.Map(It.IsAny<IEnumerable<BookSeries>>(), It.IsAny<Paging>()))
                .Callback<IEnumerable<BookSeries>, Paging>((s, p) =>
                {
                    seriesParam = s;
                    seriesPagingParam = p;
                })
                .Returns(new PartialListVM());

            var service = new LiteratureGenreService(commandsRepo.Object, queriesRepo.Object, mappings.Object);

            // Act
            var result = service.GetDetails(genre.Id, booksPageSize, booksPageNo, authorsPageSize, authorsPageNo, seriesPageSize, seriesPageNo);

            // Assert
            queriesRepo.Verify(
                r => r.GetLiteratureGenre(genre.Id),
                Times.Once());
            queriesRepo.Verify(
                r => r.GetAllBooksWithAuthorsGenresAndSeries(),
                Times.Once());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            mappings.Verify(
                m => m.Map<LiteratureGenre, GenreDetailsVM>(genre),
                Times.Once());
            mappings.Verify(
                m => m.Map(It.IsAny<IEnumerable<Book>>(), It.IsAny<Paging>()),
                Times.Once());
            booksParam.Should()
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
            booksPagingParam.Should().NotBeNull();
            booksPagingParam!.PageSize.Should().Be(booksPageSize);
            booksPagingParam.CurrentPage.Should().Be(booksPageNo);
            booksPagingParam.Count.Should().Be(default);
            mappings.Verify(
                m => m.Map(It.IsAny<IEnumerable<Creator>>(), It.IsAny<Paging>()),
                Times.Once());
            authorsParam.Should()
                .NotBeNull().And
                .HaveCount(3).And
                .Contain(a1).And
                .Contain(a2).And
                .Contain(a3).And
                .NotContain(a4);
            authorsPagingParam.Should().NotBeNull();
            authorsPagingParam!.PageSize.Should().Be(authorsPageSize);
            authorsPagingParam.CurrentPage.Should().Be(authorsPageNo);
            authorsPagingParam.Count.Should().Be(default);
            mappings.Verify(
                m => m.Map(It.IsAny<IEnumerable<BookSeries>>(), It.IsAny<Paging>()),
                Times.Once());
            seriesParam.Should()
                .NotBeNull().And
                .HaveCount(2).And
                .Contain(s1).And
                .Contain(s2).And
                .NotContain(s3);
            seriesPagingParam.Should().NotBeNull();
            seriesPagingParam!.PageSize.Should().Be(seriesPageSize);
            seriesPagingParam.CurrentPage.Should().Be(seriesPageNo);
            seriesPagingParam.Count.Should().Be(default);
            mappings.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeOfType<GenreDetailsVM>();
            result.Id.Should().Be(genre.Id);
            result.Title.Should().Be(genre.Name);
        }

        [Fact]
        public void GetDetails_NotExistingGenreId()
        {
            int authorsPageSize = 3, authorsPageNo = 4, booksPageSize = 5, booksPageNo = 2, seriesPageSize = 1, seriesPageNo = 3;
            var genreId = 1;
            var commandsRepo = new Mock<ILiteratureGenreCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetLiteratureGenre(genreId))
                .Returns((LiteratureGenre?)null);
            queriesRepo.Setup(r => r.GetAllBooksWithAuthorsGenresAndSeries());
            var mappings = new Mock<IBookVMsMappings>();
            mappings.Setup(m => m.Map<LiteratureGenre, GenreDetailsVM>(It.IsAny<LiteratureGenre>()));
            mappings.Setup(m => m.Map(It.IsAny<IEnumerable<Book>>(), It.IsAny<Paging>()));
            mappings.Setup(m => m.Map(It.IsAny<IEnumerable<Creator>>(), It.IsAny<Paging>()));
            mappings.Setup(m => m.Map(It.IsAny<IEnumerable<BookSeries>>(), It.IsAny<Paging>()));
            var service = new LiteratureGenreService(commandsRepo.Object, queriesRepo.Object, mappings.Object);

            var result = service.GetDetails(genreId, booksPageSize, booksPageNo, authorsPageSize, authorsPageNo, seriesPageSize, seriesPageNo);

            queriesRepo.Verify(
                r => r.GetLiteratureGenre(genreId),
                Times.Once());
            queriesRepo.Verify(
                r => r.GetAllBooksWithAuthorsGenresAndSeries(),
                Times.Never());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            mappings.Verify(
                m => m.Map<LiteratureGenre, GenreDetailsVM>(It.IsAny<LiteratureGenre>()),
                Times.Never());
            mappings.Verify(
                m => m.Map(It.IsAny<IEnumerable<Book>>(), It.IsAny<Paging>()),
                Times.Never());
            mappings.Verify(
                m => m.Map(It.IsAny<IEnumerable<Creator>>(), It.IsAny<Paging>()),
                Times.Never());
            mappings.Verify(
                m => m.Map(It.IsAny<IEnumerable<BookSeries>>(), It.IsAny<Paging>()),
                Times.Never());
            mappings.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeOfType<GenreDetailsVM>();
            result.Id.Should().Be(default);
            result.Title.Should().Be(string.Empty);
            result.Authors.Should()
                .NotBeNull().And
                .BeAssignableTo<IPartialListVM>();
            result.Authors.Values.Should()
                .NotBeNull().And
                .BeEmpty();
            result.Authors.Paging.Should().NotBeNull();
            result.Authors.Paging.PageSize.Should().Be(authorsPageSize);
            result.Authors.Paging.CurrentPage.Should().Be(1);
            result.Authors.Paging.Count.Should().Be(0);
            result.Books.Should()
                .NotBeNull().And
                .BeAssignableTo<IPartialListVM>();
            result.Books.Values.Should()
                .NotBeNull().And
                .BeEmpty();
            result.Books.Paging.Should().NotBeNull();
            result.Books.Paging.PageSize.Should().Be(booksPageSize);
            result.Books.Paging.CurrentPage.Should().Be(1);
            result.Books.Paging.Count.Should().Be(0);
            result.Series.Should()
                .NotBeNull().And
                .BeAssignableTo<IPartialListVM>();
            result.Series.Values.Should()
                .NotBeNull().And
                .BeEmpty();
            result.Series.Paging.Should().NotBeNull();
            result.Series.Paging.PageSize.Should().Be(seriesPageSize);
            result.Series.Paging.CurrentPage.Should().Be(1);
            result.Series.Paging.Count.Should().Be(0);
        }

        [Theory]
        [InlineData("Name")]
        [InlineData(null)]
        public void GetList(string? search)
        {
            int count = 5, pageSize = 3, pageNo = 2;
            SortByEnum sort = SortByEnum.Descending;
            var genres = BookModuleHelpers.GetGenresList(count).AsQueryable();
            var commandsRepo = new Mock<ILiteratureGenreCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetAllLiteratureGenres())
                .Returns(genres);
            var mappings = new Mock<IBookVMsMappings>();
            Paging pagingParam = null;
            Filtering filteringParam = null;
            mappings.Setup(m => m.Map(genres, It.IsAny<Paging>(), It.IsAny<Filtering>()))
                .Callback<IQueryable<LiteratureGenre>, Paging, Filtering>((g, p, f) =>
                {
                    pagingParam = p;
                    filteringParam = f;
                })
                .Returns(new ListVM());
            var service = new LiteratureGenreService(commandsRepo.Object, queriesRepo.Object, mappings.Object);

            var result = service.GetList(sort, pageSize, pageNo, search);

            queriesRepo.Verify(
                r => r.GetAllLiteratureGenres(),
                Times.Once());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            mappings.Verify(
                m => m.Map(genres, It.IsAny<Paging>(), It.IsAny<Filtering>()),
                Times.Once());
            pagingParam.Should().NotBeNull();
            pagingParam!.PageSize.Should().Be(pageSize);
            pagingParam.CurrentPage.Should().Be(pageNo);
            pagingParam.Count.Should().Be(default);
            filteringParam.Should().NotBeNull();
            filteringParam!.SearchString.Should()
                .NotBeNull().And
                .Be(search ?? string.Empty);
            filteringParam.SortBy.Should().Be(sort);
            mappings.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeOfType<ListVM>();
        }

        [Theory]
        [InlineData("Name")]
        [InlineData(null)]
        public void GetListForBook_ExistingBook(string? search)
        {
            int pageSize = 3, pageNo = 2;
            SortByEnum sort = SortByEnum.Descending;
            var b1 = BookModuleHelpers.GetBook(1);
            var b2 = BookModuleHelpers.GetBook(2);
            var g1 = BookModuleHelpers.GetGenre(1);
            var g2 = BookModuleHelpers.GetGenre(2);
            var g3 = BookModuleHelpers.GetGenre(3);
            var g4 = BookModuleHelpers.GetGenre(4);
            g1.Books = [b1, b2];
            g2.Books = [b1];
            g3.Books = [b2];
            g4.Books = [];
            var genres = new[] { g1, g2, g3, g4 }.AsQueryable();
            var book = BookModuleHelpers.GetBook(b1.Id);
            var commands = new Mock<ILiteratureGenreCommandsRepository>();
            var queries = new Mock<IBookModuleQueriesRepository>();
            queries.Setup(q => q.GetBook(b1.Id))
                .Returns(book);
            queries.Setup(q => q.GetAllLiteratureGenresWithBooks())
                .Returns(genres);
            var mappings = new Mock<IBookVMsMappings>();
            Paging pagingParam = null;
            Filtering filteringParam = null;
            mappings.Setup(m => m.MapToDoubleListForItem(genres, book, It.IsAny<Paging>(), It.IsAny<Filtering>()))
                .Callback<IQueryable<LiteratureGenre>, Book, Paging, Filtering>((g, b, p, f) =>
                {
                    pagingParam = p;
                    filteringParam = f;
                })
                .Returns(new DoubleListForItemVM());
            var service = new LiteratureGenreService(commands.Object, queries.Object, mappings.Object);

            var result = service.GetListForBook(b1.Id, sort, pageSize, pageNo, search);

            queries.Verify(
                q => q.GetBook(b1.Id),
                Times.Once);
            queries.Verify(
                q => q.GetAllLiteratureGenresWithBooks(),
                Times.Once);
            queries.VerifyNoOtherCalls();
            commands.VerifyNoOtherCalls();
            mappings.Verify(
                m => m.MapToDoubleListForItem(genres, book, It.IsAny<Paging>(), It.IsAny<Filtering>()),
                Times.Once());
            pagingParam.Should().NotBeNull();
            pagingParam!.CurrentPage.Should().Be(pageNo);
            pagingParam.PageSize.Should().Be(pageSize);
            filteringParam.Should().NotBeNull();
            filteringParam!.SearchString.Should()
                .NotBeNull().And
                .Be(search ?? string.Empty);
            filteringParam.SortBy.Should().Be(sort);
            result.Should()
                .NotBeNull().And
                .BeOfType<DoubleListForItemVM>();
        }

        [Theory]
        [InlineData("Name")]
        [InlineData(null)]
        public void GetListForBook_NotExistingBook(string? search)
        {
            int pageSize = 3, pageNo = 2;
            SortByEnum sort = SortByEnum.Descending;
            var b1 = BookModuleHelpers.GetBook(1);
            var b2 = BookModuleHelpers.GetBook(2);
            int bookId = 3;
            var g1 = BookModuleHelpers.GetGenre(1);
            var g2 = BookModuleHelpers.GetGenre(2);
            var g3 = BookModuleHelpers.GetGenre(3);
            var g4 = BookModuleHelpers.GetGenre(4);
            g1.Books = [b1, b2];
            g2.Books = [b1];
            g3.Books = [b2];
            g4.Books = [];
            var genres = new[] { g1, g2, g3, g4 }.AsQueryable();
            var commands = new Mock<ILiteratureGenreCommandsRepository>();
            var queries = new Mock<IBookModuleQueriesRepository>();
            queries.Setup(q => q.GetBook(bookId))
                .Returns((Book?)null);
            queries.Setup(q => q.GetAllLiteratureGenresWithBooks())
                .Returns(genres);
            var mappings = new Mock<IBookVMsMappings>();
            Paging pagingParam = null;
            Filtering filteringParam = null;
            mappings.Setup(m => m.MapToDoubleListForItem(genres, It.IsAny<Book>(), It.IsAny<Paging>(), It.IsAny<Filtering>()))
                .Callback<IQueryable<LiteratureGenre>, Book, Paging, Filtering>((g, b, p, f) =>
                {
                    pagingParam = p;
                    filteringParam = f;
                })
                .Returns(new DoubleListForItemVM());
            var service = new LiteratureGenreService(commands.Object, queries.Object, mappings.Object);

            var result = service.GetListForBook(bookId, sort, pageSize, pageNo, search);

            queries.Verify(
                q => q.GetBook(bookId),
                Times.Once);
            queries.Verify(
                q => q.GetAllLiteratureGenresWithBooks(),
                Times.Once);
            queries.VerifyNoOtherCalls();
            commands.VerifyNoOtherCalls();
            mappings.Verify(
                m => m.MapToDoubleListForItem(genres, It.Is<Book>(b => b.Id == default), It.IsAny<Paging>(), It.IsAny<Filtering>()),
                Times.Once());
            pagingParam.Should().NotBeNull();
            pagingParam!.CurrentPage.Should().Be(pageNo);
            pagingParam.PageSize.Should().Be(pageSize);
            filteringParam.Should().NotBeNull();
            filteringParam!.SearchString.Should()
                .NotBeNull().And
                .Be(search ?? string.Empty);
            filteringParam.SortBy.Should().Be(sort);
            result.Should()
                .NotBeNull().And
                .BeOfType<DoubleListForItemVM>();
        }

        [Fact]
        public void GetPartialListForAuthor_ExistingAuthor()
        {
            int pageSize = 2, pageNo = 3;
            var author = BookModuleHelpers.GetAuthor();
            var b1 = BookModuleHelpers.GetBook(1);
            var b2 = BookModuleHelpers.GetBook(2);
            var b3 = BookModuleHelpers.GetBook(3);
            var b4 = BookModuleHelpers.GetBook(4);
            author.Books = [BookModuleHelpers.GetBook(b1.Id), BookModuleHelpers.GetBook(b2.Id)];
            b1.Creators = [BookModuleHelpers.GetAuthor(author.Id)];
            b2.Creators = [BookModuleHelpers.GetAuthor(author.Id)];
            var g1 = BookModuleHelpers.GetGenre(1);
            var g2 = BookModuleHelpers.GetGenre(2);
            var g3 = BookModuleHelpers.GetGenre(3);
            var g4 = BookModuleHelpers.GetGenre(4);
            var g5 = BookModuleHelpers.GetGenre(5);
            b1.Genres = [g1, g2, g3];
            b2.Genres = [g1, g3];
            b3.Genres = [g3, g4];
            b4.Genres = [g5];
            var books = new[] { b1, b2, b3, b4 }.AsQueryable();
            var comm = new Mock<ILiteratureGenreCommandsRepository>();
            var query = new Mock<IBookModuleQueriesRepository>();
            query.Setup(q => q.GetAuthor(author.Id))
                .Returns(author);
            query.Setup(q => q.GetAllBooksWithAuthorsGenresAndSeries())
                .Returns(books);
            var mappings = new Mock<IBookVMsMappings>();
            Paging pagingParam = null;
            IEnumerable<LiteratureGenre> genresParam = null;
            mappings.Setup(m => m.Map(It.IsAny<IEnumerable<LiteratureGenre>>(), It.IsAny<Paging>()))
                .Callback<IEnumerable<LiteratureGenre>, Paging>((g, p) =>
                {
                    genresParam = g;
                    pagingParam = p;
                })
                .Returns(new PartialListVM());
            var service = new LiteratureGenreService(comm.Object, query.Object, mappings.Object);

            var result = service.GetPartialListForAuthor(author.Id, pageSize, pageNo);

            query.Verify(
                q => q.GetAuthor(author.Id),
                Times.Once);
            query.Verify(
                q => q.GetAllBooksWithAuthorsGenresAndSeries(),
                Times.Once);
            query.VerifyNoOtherCalls();
            comm.VerifyNoOtherCalls();
            mappings.Verify(
                m => m.Map(It.IsAny<IEnumerable<LiteratureGenre>>(), It.IsAny<Paging>()),
                Times.Once());
            pagingParam.Should().NotBeNull();
            pagingParam!.CurrentPage.Should().Be(pageNo);
            pagingParam.PageSize.Should().Be(pageSize);
            genresParam.Should()
                .NotBeNullOrEmpty().And
                .Equal(g1, g2, g3).And
                .NotContain(new[] { g4, g5 });
            mappings.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeOfType<PartialListVM>();
        }

        [Fact]
        public void GetPartialListForAuthor_NotExistingAuthor()
        {
            int pageSize = 2, pageNo = 3;
            var authorId = 1;
            var comm = new Mock<ILiteratureGenreCommandsRepository>();
            var query = new Mock<IBookModuleQueriesRepository>();
            query.Setup(q => q.GetAuthor(authorId))
                .Returns((Creator?)null);
            query.Setup(q => q.GetAllBooksWithAuthorsGenresAndSeries());
            var mappings = new Mock<IBookVMsMappings>();
            mappings.Setup(m => m.Map(It.IsAny<IEnumerable<LiteratureGenre>>(), It.IsAny<Paging>()));
            var service = new LiteratureGenreService(comm.Object, query.Object, mappings.Object);

            var result = service.GetPartialListForAuthor(authorId, pageSize, pageNo);

            query.Verify(
                q => q.GetAuthor(authorId),
                Times.Once);
            query.Verify(
                q => q.GetAllBooksWithAuthorsGenresAndSeries(),
                Times.Never);
            query.VerifyNoOtherCalls();
            comm.VerifyNoOtherCalls();
            mappings.Verify(
                m => m.Map(It.IsAny<IEnumerable<LiteratureGenre>>(), It.IsAny<Paging>()),
                Times.Never());
            mappings.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeAssignableTo<IPartialListVM>();
            result.Values.Should()
                .NotBeNull().And
                .BeEmpty();
            result.Paging.Should().NotBeNull();
            result.Paging.CurrentPage.Should().Be(1);
            result.Paging.PageSize.Should().Be(pageSize);
            result.Paging.Count.Should().Be(0);
        }

        [Fact]
        public void SelectBooks_ExistingLiteratureGenreIdAndExistingBooksIds()
        {
            var b1 = BookModuleHelpers.GetBook(1);
            var b2 = BookModuleHelpers.GetBook(2);
            var b3 = BookModuleHelpers.GetBook(3);
            var genre = new LiteratureGenre()
            {
                Id = 1,
                Name = "Name",
                Books = [b1]
            };
            var booksIds = new[] { b2.Id, b3.Id };
            var commandsRepo = new Mock<ILiteratureGenreCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetLiteratureGenreWithBooks(genre.Id))
                .Returns(genre);
            queriesRepo.Setup(r => r.GetBook(b2.Id))
                .Returns(b2);
            queriesRepo.Setup(r => r.GetBook(b3.Id))
                .Returns(b3);
            LiteratureGenre genreParam = null;
            commandsRepo.Setup(r => r.UpdateLiteratureGenreBooksRelation(It.IsAny<LiteratureGenre>()))
                .Callback<LiteratureGenre>(g => genreParam = g);
            var mappings = new Mock<IBookVMsMappings>();
            var service = new LiteratureGenreService(commandsRepo.Object, queriesRepo.Object, mappings.Object);

            service.SelectBooks(genre.Id, booksIds);

            queriesRepo.Verify(
                r => r.GetLiteratureGenreWithBooks(genre.Id),
                Times.Once());
            queriesRepo.Verify(
                r => r.GetBook(b2.Id),
                Times.Once());
            queriesRepo.Verify(
                r => r.GetBook(b3.Id),
                Times.Once());
            commandsRepo.Verify(
                r => r.UpdateLiteratureGenreBooksRelation(genre),
                Times.Once());
            genreParam.Should()
                .NotBeNull().And
                .Be(genre);
            genreParam!.Books.Should()
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
        public void SelectBooks_ExistingLiteratureGenreIdAndNotExistingBooksIds()
        {
            var book = BookModuleHelpers.GetBook(1);
            var genre = new LiteratureGenre()
            {
                Id = 1,
                Name = "Name",
                Books = [book]
            };
            var booksIds = new[] { 2, 3 };
            var commandsRepo = new Mock<ILiteratureGenreCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetLiteratureGenreWithBooks(genre.Id))
                .Returns(genre);
            queriesRepo.Setup(r => r.GetBook(2))
                .Returns((Book?)null);
            queriesRepo.Setup(r => r.GetBook(3))
                .Returns((Book?)null);
            LiteratureGenre genreParam = null;
            commandsRepo.Setup(r => r.UpdateLiteratureGenreBooksRelation(It.IsAny<LiteratureGenre>()))
                .Callback<LiteratureGenre>(g => genreParam = g);
            var mappings = new Mock<IBookVMsMappings>();
            var service = new LiteratureGenreService(commandsRepo.Object, queriesRepo.Object, mappings.Object);

            service.SelectBooks(genre.Id, booksIds);

            queriesRepo.Verify(
                r => r.GetLiteratureGenreWithBooks(genre.Id),
                Times.Once());
            queriesRepo.Verify(
                r => r.GetBook(2),
                Times.Once());
            queriesRepo.Verify(
                r => r.GetBook(3),
                Times.Once());
            commandsRepo.Verify(
                r => r.UpdateLiteratureGenreBooksRelation(genre),
                Times.Once());
            genreParam.Should()
                .NotBeNull().And
                .Be(genre);
            genreParam!.Books.Should()
                .NotBeNull().And
                .BeEmpty();
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
            mappings.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectBooks_ExistingLiteratureGenreIdAndEmptyArray()
        {
            var book = BookModuleHelpers.GetBook(1);
            var genre = new LiteratureGenre()
            {
                Id = 1,
                Name = "Name",
                Books = [book]
            };
            var booksIds = Array.Empty<int>();
            var commandsRepo = new Mock<ILiteratureGenreCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetLiteratureGenreWithBooks(genre.Id))
                .Returns(genre);
            queriesRepo.Setup(r => r.GetBook(It.IsAny<int>()));
            LiteratureGenre genreParam = null;
            commandsRepo.Setup(r => r.UpdateLiteratureGenreBooksRelation(It.IsAny<LiteratureGenre>()))
                .Callback<LiteratureGenre>(g => genreParam = g);
            var mappings = new Mock<IBookVMsMappings>();
            var service = new LiteratureGenreService(commandsRepo.Object, queriesRepo.Object, mappings.Object);

            service.SelectBooks(genre.Id, booksIds);

            queriesRepo.Verify(
                r => r.GetLiteratureGenreWithBooks(genre.Id),
                Times.Once());
            queriesRepo.Verify(
                r => r.GetBook(It.IsAny<int>()),
                Times.Never());
            commandsRepo.Verify(
                r => r.UpdateLiteratureGenreBooksRelation(genre),
                Times.Once());
            genreParam.Should()
                .NotBeNull().And
                .Be(genre);
            genreParam!.Books.Should()
                .NotBeNull().And
                .BeEmpty();
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
            mappings.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectBooks_NotExistingLiteratureGenreIdAndBooksIds()
        {
            int genreId = 1;
            var book1 = BookModuleHelpers.GetBook(1);
            var booksIds = new[] { book1.Id };
            var commandsRepo = new Mock<ILiteratureGenreCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetLiteratureGenreWithBooks(genreId))
                .Returns((LiteratureGenre?)null);
            queriesRepo.Setup(r => r.GetBook(book1.Id))
                .Returns(book1);
            commandsRepo.Setup(r => r.UpdateLiteratureGenreBooksRelation(It.IsAny<LiteratureGenre>()));
            var mappings = new Mock<IBookVMsMappings>();
            var service = new LiteratureGenreService(commandsRepo.Object, queriesRepo.Object, mappings.Object);

            service.SelectBooks(genreId, booksIds);

            queriesRepo.Verify(
                r => r.GetLiteratureGenreWithBooks(genreId),
                Times.Once());
            queriesRepo.Verify(
                r => r.GetBook(book1.Id),
                Times.Never());
            commandsRepo.Verify(
                r => r.UpdateLiteratureGenreBooksRelation(It.IsAny<LiteratureGenre>()),
                Times.Never());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
            mappings.VerifyNoOtherCalls();
        }

        [Fact]
        public void Upsert_NewLiteratureGenre_NewId()
        {
            var genre = new GenreVM()
            {
                Name = "Name"
            };
            var entity = new LiteratureGenre()
            {
                Id = genre.Id,
                Name = genre.Name
            };
            var commandsRepo = new Mock<ILiteratureGenreCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            commandsRepo.Setup(r => r.Insert(entity))
                .Returns(1);
            commandsRepo.Setup(r => r.Update(entity));
            var mappings = new Mock<IBookVMsMappings>();
            mappings.Setup(m => m.Map<GenreVM, LiteratureGenre>(genre))
                .Returns(entity);
            var service = new LiteratureGenreService(commandsRepo.Object, queriesRepo.Object, mappings.Object);

            var result = service.Upsert(genre);

            mappings.Verify(
                m => m.Map<GenreVM, LiteratureGenre>(genre),
                Times.Once());
            mappings.VerifyNoOtherCalls();
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

        [Fact]
        public void Upsert_ExistingLiteratureGenre_GenreId()
        {
            var genre = new GenreVM()
            {
                Id = 1,
                Name = "Name"
            };
            var entity = new LiteratureGenre()
            {
                Id = genre.Id,
                Name = genre.Name
            };
            var commandsRepo = new Mock<ILiteratureGenreCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            commandsRepo.Setup(r => r.Insert(entity))
                .Returns(1);
            commandsRepo.Setup(r => r.Update(entity));
            var mappings = new Mock<IBookVMsMappings>();
            mappings.Setup(m => m.Map<GenreVM, LiteratureGenre>(genre))
                .Returns(entity);
            var service = new LiteratureGenreService(commandsRepo.Object, queriesRepo.Object, mappings.Object);

            var result = service.Upsert(genre);

            mappings.Verify(
                m => m.Map<GenreVM, LiteratureGenre>(genre),
                Times.Once());
            mappings.VerifyNoOtherCalls();
            commandsRepo.Verify(
                r => r.Insert(It.IsAny<LiteratureGenre>()),
                Times.Never());
            commandsRepo.Verify(
                r => r.Update(It.Is<LiteratureGenre>(
                    g => g.Id == 1)),
                Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
            result.Should().Be(1);
        }
    }
}