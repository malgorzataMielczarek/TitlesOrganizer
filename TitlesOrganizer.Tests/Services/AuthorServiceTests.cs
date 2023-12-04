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
using TitlesOrganizer.Tests.ViewModels.BookVMs;

namespace TitlesOrganizer.Tests.Services
{
    public class AuthorServiceTests
    {
        [Fact]
        public void Delete_ExistingAuthorId()
        {
            var author = new Author() { Id = 1 };
            var commandsRepo = new Mock<IAuthorCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetAuthor(author.Id))
                .Returns(author);
            commandsRepo.Setup(r => r.Delete(author));
            var mapping = new Mock<IBookVMsMappings>();
            var service = new AuthorService(commandsRepo.Object, queriesRepo.Object, mapping.Object);

            service.Delete(author.Id);

            queriesRepo.Verify(
                r => r.GetAuthor(author.Id),
                Times.Once);
            commandsRepo.Verify(
                r => r.Delete(author),
                Times.Once);
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
            mapping.VerifyNoOtherCalls();
        }

        [Fact]
        public void Delete_NotExistingAuthorId()
        {
            var author = new Author() { Id = 1 };
            var commandsRepo = new Mock<IAuthorCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetAuthor(author.Id))
                .Returns((Author?)null);
            commandsRepo.Setup(r => r.Delete(author));
            var mapping = new Mock<IBookVMsMappings>();
            var service = new AuthorService(commandsRepo.Object, queriesRepo.Object, mapping.Object);

            service.Delete(author.Id);

            queriesRepo.Verify(
                r => r.GetAuthor(author.Id),
                Times.Once);
            commandsRepo.Verify(
                r => r.Delete(author),
                Times.Never);
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
            mapping.VerifyNoOtherCalls();
        }

        [Fact]
        public void Get_ExistingAuthorId()
        {
            int pageSize = 3, pageNo = 2;
            var id = 1;
            var books = Helpers.GetBooksList(3);
            var author = new Author()
            {
                Id = id,
                Books = books
            };
            var authorVM = new AuthorVM() { Id = id };
            var commandsRepo = new Mock<IAuthorCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetAuthorWithBooks(author.Id))
                .Returns(author);
            var mapping = new Mock<IBookVMsMappings>();
            mapping.Setup(m => m.Map<Author, AuthorVM>(author)).Returns(authorVM);
            mapping.Setup(m => m.Map(books, It.IsAny<Paging>())).Returns(new PartialListVM());
            var service = new AuthorService(commandsRepo.Object, queriesRepo.Object, mapping.Object);

            var result = service.Get(author.Id, pageSize, pageNo);

            queriesRepo.Verify(
                r => r.GetAuthorWithBooks(author.Id),
                Times.Once());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            mapping.Verify(
                m => m.Map<Author, AuthorVM>(author),
                Times.Once());
            mapping.Verify(
                m => m.Map(books, It.Is<Paging>(p => p.CurrentPage == pageNo && p.PageSize == pageSize)),
                Times.Once());
            mapping.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeOfType<AuthorVM>();
            result.Id.Should().Be(author.Id);
        }

        [Fact]
        public void Get_NotExistingAuthorId()
        {
            int pageSize = 3, pageNo = 2;
            var id = 1;
            var commandsRepo = new Mock<IAuthorCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetAuthorWithBooks(id))
                .Returns((Author?)null);
            var mapping = new Mock<IBookVMsMappings>();
            mapping.Setup(m => m.Map<Author, AuthorVM>(It.IsAny<Author>()));
            mapping.Setup(m => m.Map(It.IsAny<IEnumerable<Book>>(), It.IsAny<Paging>()));
            var service = new AuthorService(commandsRepo.Object, queriesRepo.Object, mapping.Object);

            var result = service.Get(id, pageSize, pageNo);

            queriesRepo.Verify(
                r => r.GetAuthorWithBooks(id),
                Times.Once());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            mapping.Verify(
                m => m.Map<Author, AuthorVM>(It.IsAny<Author>()),
                Times.Never());
            mapping.Verify(
                m => m.Map(It.IsAny<IEnumerable<Book>>(), It.IsAny<Paging>()),
                Times.Never());
            mapping.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeOfType<AuthorVM>();
            result.Id.Should().Be(default);
            result.Books.Should()
                .NotBeNull().And
                .BeAssignableTo<IPartialListVM>();
            result.Books.Paging.Should()
                .NotBeNull().And
                .BeOfType<Paging>();
            result.Books.Paging.PageSize.Should().Be(pageSize);
            result.Books.Paging.CurrentPage.Should().Be(1);
            result.Books.Paging.Count.Should().Be(0);
        }

        [Fact]
        public void GetDetails_ExistingAuthorId()
        {
            // Arrange
            int booksPageSize = 5, booksPageNo = 2, seriesPageSize = 1, seriesPageNo = 3, genrePageSize = 3, genrePageNo = 4;
            var author = Helpers.GetAuthor(1);
            var authorDetails = new AuthorDetailsVM() { Id = author.Id };
            var books = Helpers.GetBooksList(10);
            var s1 = Helpers.GetSeries(1);
            var s2 = Helpers.GetSeries(2);
            var s3 = Helpers.GetSeries(3);
            var g1 = Helpers.GetGenre(1);
            var g2 = Helpers.GetGenre(2);
            var g3 = Helpers.GetGenre(3);
            var g4 = Helpers.GetGenre(4);

            books[0].Authors.Add(Helpers.GetAuthor(author.Id));
            books[1].Authors.Add(Helpers.GetAuthor(author.Id));
            books[3].Authors.Add(Helpers.GetAuthor(author.Id));
            books[4].Authors.Add(Helpers.GetAuthor(author.Id));
            books[5].Authors.Add(Helpers.GetAuthor(author.Id));
            books[7].Authors.Add(Helpers.GetAuthor(author.Id));
            books[8].Authors.Add(Helpers.GetAuthor(author.Id));

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

            books[3].Genres = new[] { g1, g2, g3 };
            books[5].Genres = new[] { g1, g3 };
            books[6].Genres = new[] { g2, g4 };

            IEnumerable<Book> booksParam = null;
            IEnumerable<BookSeries> seriesParam = null;
            IEnumerable<LiteratureGenre> genresParam = null;

            var commandsRepo = new Mock<IAuthorCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetAuthor(author.Id))
                .Returns(author);
            queriesRepo.Setup(r => r.GetAllBooksWithAuthorsGenresAndSeries())
                .Returns(books.AsQueryable());

            var mappings = new Mock<IBookVMsMappings>();
            mappings.Setup(m => m.Map<Author, AuthorDetailsVM>(author))
                .Returns(authorDetails);
            mappings.Setup(m => m.Map(It.IsAny<IEnumerable<Book>>(), It.IsAny<Paging>()))
                .Callback<IEnumerable<Book>, Paging>((b, p) => booksParam = b)
                .Returns(new PartialListVM());
            mappings.Setup(m => m.Map(It.IsAny<IEnumerable<BookSeries>>(), It.IsAny<Paging>()))
                .Callback<IEnumerable<BookSeries>, Paging>((s, p) => seriesParam = s)
                .Returns(new PartialListVM());
            mappings.Setup(m => m.Map(It.IsAny<IEnumerable<LiteratureGenre>>(), It.IsAny<Paging>()))
                .Callback<IEnumerable<LiteratureGenre>, Paging>((g, p) => genresParam = g)
                .Returns(new PartialListVM());

            var service = new AuthorService(commandsRepo.Object, queriesRepo.Object, mappings.Object);

            // Act
            var result = service.GetDetails(author.Id, booksPageSize, booksPageNo, seriesPageSize, seriesPageNo, genrePageSize, genrePageNo);

            // Assert
            queriesRepo.Verify(
                r => r.GetAuthor(author.Id),
                Times.Once());
            queriesRepo.Verify(
                r => r.GetAllBooksWithAuthorsGenresAndSeries(),
                Times.Once());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            mappings.Verify(
                m => m.Map(It.IsAny<IEnumerable<Book>>(),
                    It.Is<Paging>(p => p.CurrentPage == booksPageNo && p.PageSize == booksPageSize)),
                Times.Once());
            mappings.Verify(
                m => m.Map(It.IsAny<IEnumerable<BookSeries>>(),
                    It.Is<Paging>(p => p.CurrentPage == seriesPageNo && p.PageSize == seriesPageSize)),
                Times.Once());
            mappings.Verify(
                m => m.Map(It.IsAny<IEnumerable<LiteratureGenre>>(),
                    It.Is<Paging>(p => p.CurrentPage == genrePageNo && p.PageSize == genrePageSize)),
                Times.Once());
            mappings.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeOfType<AuthorDetailsVM>();
            result.Id.Should().Be(author.Id);
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
            seriesParam.Should()
                .NotBeNull().And
                .HaveCount(2).And
                .Contain(s1).And
                .Contain(s2).And
                .NotContain(s3);
            genresParam.Should()
                .NotBeNull().And
                .HaveCount(3).And
                .Contain(g1).And
                .Contain(g2).And
                .Contain(g3).And
                .NotContain(g4);
        }

        [Fact]
        public void GetDetails_NotExistingAuthorId()
        {
            // Arrange
            int booksPageSize = 5, booksPageNo = 2, seriesPageSize = 1, seriesPageNo = 3, genrePageSize = 3, genrePageNo = 4;
            var authorId = 1;
            var commandsRepo = new Mock<IAuthorCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetAuthor(authorId))
                .Returns((Author?)null);
            queriesRepo.Setup(r => r.GetAllBooksWithAuthorsGenresAndSeries());

            var mappings = new Mock<IBookVMsMappings>();
            mappings.Setup(m => m.Map<Author, AuthorDetailsVM>(It.IsAny<Author>()));
            mappings.Setup(m => m.Map(It.IsAny<IEnumerable<Book>>(), It.IsAny<Paging>()));
            mappings.Setup(m => m.Map(It.IsAny<IEnumerable<BookSeries>>(), It.IsAny<Paging>()));
            mappings.Setup(m => m.Map(It.IsAny<IEnumerable<LiteratureGenre>>(), It.IsAny<Paging>()));

            var service = new AuthorService(commandsRepo.Object, queriesRepo.Object, mappings.Object);

            // Act
            var result = service.GetDetails(authorId, booksPageSize, booksPageNo, seriesPageSize, seriesPageNo, genrePageSize, genrePageNo);

            // Assert
            queriesRepo.Verify(
                r => r.GetAuthor(authorId),
                Times.Once());
            queriesRepo.Verify(
                r => r.GetAllBooksWithAuthorsGenresAndSeries(),
                Times.Never());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            mappings.Verify(
                m => m.Map(It.IsAny<IEnumerable<Book>>(), It.IsAny<Paging>()),
                Times.Never());
            mappings.Verify(
                m => m.Map(It.IsAny<IEnumerable<BookSeries>>(), It.IsAny<Paging>()),
                Times.Never());
            mappings.Verify(
                m => m.Map(It.IsAny<IEnumerable<LiteratureGenre>>(), It.IsAny<Paging>()),
                Times.Never());
            mappings.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeOfType<AuthorDetailsVM>();
            result.Id.Should().Be(default);
            result.Books.Paging.PageSize.Should().Be(booksPageSize);
            result.Books.Paging.CurrentPage.Should().Be(1);
            result.Books.Paging.Count.Should().Be(0);
            result.Series.Paging.PageSize.Should().Be(seriesPageSize);
            result.Series.Paging.CurrentPage.Should().Be(1);
            result.Series.Paging.Count.Should().Be(0);
            result.Genres.Paging.PageSize.Should().Be(genrePageSize);
            result.Genres.Paging.CurrentPage.Should().Be(1);
            result.Genres.Paging.Count.Should().Be(0);
        }

        [Theory]
        [InlineData("Search")]
        [InlineData(null)]
        public void GetList(string? search)
        {
            int count = 3, pageSize = 2, pageNo = 2;
            SortByEnum sort = SortByEnum.Descending;
            var authors = Helpers.GetAuthorsList(count).AsQueryable();
            var commandsRepo = new Mock<IAuthorCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetAllAuthors())
                .Returns(authors);
            Paging paging = null;
            Filtering filtering = null;
            var mappings = new Mock<IBookVMsMappings>();
            mappings.Setup(m => m.Map(authors, It.IsAny<Paging>(), It.IsAny<Filtering>()))
                .Callback<IQueryable<Author>, Paging, Filtering>((a, p, f) =>
                {
                    paging = p;
                    filtering = f;
                })
                .Returns(new ListVM());
            var service = new AuthorService(commandsRepo.Object, queriesRepo.Object, mappings.Object);

            var result = service.GetList(sort, pageSize, pageNo, search);

            queriesRepo.Verify(
                r => r.GetAllAuthors(),
                Times.Once());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            mappings.Verify(
                m => m.Map(authors, It.IsAny<Paging>(), It.IsAny<Filtering>()),
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
            var book = Helpers.GetBook(1);
            var authors = Helpers.GetAuthorsList(3).AsQueryable();
            var commands = new Mock<IAuthorCommandsRepository>();
            var queries = new Mock<IBookModuleQueriesRepository>();
            queries.Setup(q => q.GetBook(book.Id))
                .Returns(book);
            queries.Setup(q => q.GetAllAuthorsWithBooks())
                .Returns(authors);
            var mappings = new Mock<IBookVMsMappings>();
            Paging paging = null;
            Filtering filtering = null;
            mappings.Setup(m => m.MapToDoubleListForItem(authors, book, It.IsAny<Paging>(), It.IsAny<Filtering>()))
                .Callback<IQueryable<Author>, Book, Paging, Filtering>((a, b, p, f) =>
                {
                    paging = p;
                    filtering = f;
                })
                .Returns(new DoubleListForItemVM());
            var service = new AuthorService(commands.Object, queries.Object, mappings.Object);

            var result = service.GetListForBook(book.Id, sort, pageSize, pageNo, search);

            result.Should()
                .NotBeNull().And
                .BeOfType<DoubleListForItemVM>();
            queries.Verify(
                q => q.GetBook(book.Id),
                Times.Once);
            queries.Verify(
                q => q.GetAllAuthorsWithBooks(),
                Times.Once);
            queries.VerifyNoOtherCalls();
            commands.VerifyNoOtherCalls();
            mappings.Verify(
                m => m.MapToDoubleListForItem(authors, book, It.IsAny<Paging>(), It.IsAny<Filtering>()),
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
            var authors = Helpers.GetAuthorsList(3).AsQueryable();
            var commands = new Mock<IAuthorCommandsRepository>();
            var queries = new Mock<IBookModuleQueriesRepository>();
            queries.Setup(q => q.GetBook(bookId))
                .Returns((Book?)null);
            queries.Setup(q => q.GetAllAuthorsWithBooks())
                .Returns(authors);
            var mappings = new Mock<IBookVMsMappings>();
            Paging paging = null;
            Filtering filtering = null;
            mappings.Setup(m => m.MapToDoubleListForItem(authors, It.IsAny<Book>(), It.IsAny<Paging>(), It.IsAny<Filtering>()))
                .Callback<IQueryable<Author>, Book, Paging, Filtering>((a, b, p, f) =>
                {
                    paging = p;
                    filtering = f;
                })
                .Returns(new DoubleListForItemVM());
            var service = new AuthorService(commands.Object, queries.Object, mappings.Object);

            var result = service.GetListForBook(bookId, sort, pageSize, pageNo, search);

            result.Should()
                .NotBeNull().And
                .BeOfType<DoubleListForItemVM>();
            queries.Verify(
                q => q.GetBook(bookId),
                Times.Once);
            queries.Verify(
                q => q.GetAllAuthorsWithBooks(),
                Times.Once);
            queries.VerifyNoOtherCalls();
            commands.VerifyNoOtherCalls();
            mappings.Verify(
                m => m.MapToDoubleListForItem(authors, It.Is<Book>(b => b.Id == default), It.IsAny<Paging>(), It.IsAny<Filtering>()),
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
        public void GetPartialListForGenre_ExistingGenre()
        {
            var genre = Helpers.GetGenre();
            var b1 = Helpers.GetBook(1);
            var b2 = Helpers.GetBook(2);
            var b3 = Helpers.GetBook(3);
            var a1 = Helpers.GetAuthor(1);
            var a2 = Helpers.GetAuthor(2);
            var a3 = Helpers.GetAuthor(3);
            var a4 = Helpers.GetAuthor(4);
            b1.Authors = [a1, a2, a3];
            b2.Authors = [a1, a3];
            b3.Authors = [a3, a4];
            b1.Genres = [Helpers.GetGenre(genre.Id)];
            b2.Genres = [Helpers.GetGenre(genre.Id)];
            var books = new[] { b1, b2, b3 }.AsQueryable();
            var comm = new Mock<IAuthorCommandsRepository>();
            var query = new Mock<IBookModuleQueriesRepository>();
            query.Setup(q => q.GetLiteratureGenre(genre.Id))
                .Returns(genre);
            query.Setup(q => q.GetAllBooksWithAuthorsGenresAndSeries())
                .Returns(books);
            var mappings = new Mock<IBookVMsMappings>();
            IEnumerable<Author> authors = null;
            Paging paging = null;
            mappings.Setup(m => m.Map(It.IsAny<IEnumerable<Author>>(), It.IsAny<Paging>()))
                .Callback<IEnumerable<Author>, Paging>((a, p) =>
                {
                    authors = a;
                    paging = p;
                })
                .Returns(new PartialListVM());
            var service = new AuthorService(comm.Object, query.Object, mappings.Object);
            int pageSize = 2, pageNo = 3;

            var result = service.GetPartialListForGenre(genre.Id, pageSize, pageNo);

            result.Should()
                .NotBeNull().And
                .BeOfType<PartialListVM>();
            query.Verify(
                q => q.GetLiteratureGenre(genre.Id),
                Times.Once);
            query.Verify(
                q => q.GetAllBooksWithAuthorsGenresAndSeries(),
                Times.Once);
            query.VerifyNoOtherCalls();
            comm.VerifyNoOtherCalls();
            mappings.Verify(
                m => m.Map(It.IsAny<IEnumerable<Author>>(), It.IsAny<Paging>()),
                Times.Once());
            mappings.VerifyNoOtherCalls();
            authors.Should()
                .NotBeNullOrEmpty().And
                .HaveCount(3).And
                .Equal(a1, a2, a3).And
                .NotContain(new[] { a4 });
            paging.Should().NotBeNull();
            paging!.CurrentPage.Should().Be(pageNo);
            paging.PageSize.Should().Be(pageSize);
        }

        [Fact]
        public void GetPartialListForGenre_NotExistingGenre()
        {
            int genreId = 1;
            var comm = new Mock<IAuthorCommandsRepository>();
            var query = new Mock<IBookModuleQueriesRepository>();
            query.Setup(q => q.GetLiteratureGenre(genreId))
                .Returns((LiteratureGenre?)null);
            query.Setup(q => q.GetAllBooksWithAuthorsGenresAndSeries());
            var mappings = new Mock<IBookVMsMappings>();
            mappings.Setup(m => m.Map(It.IsAny<IEnumerable<Author>>(), It.IsAny<Paging>()));
            var service = new AuthorService(comm.Object, query.Object, mappings.Object);
            int pageSize = 2, pageNo = 3;

            var result = service.GetPartialListForGenre(genreId, pageSize, pageNo);

            result.Should()
                .NotBeNull().And
                .BeAssignableTo<IPartialListVM>();
            result.Paging.Should().NotBeNull();
            result.Paging.CurrentPage.Should().Be(1);
            result.Paging.PageSize.Should().Be(pageSize);
            result.Paging.Count.Should().Be(0);
            result.Values.Should().NotBeNull().And.BeEmpty();
            query.Verify(
                q => q.GetLiteratureGenre(genreId),
                Times.Once);
            query.Verify(
                q => q.GetAllBooksWithAuthorsGenresAndSeries(),
                Times.Never);
            query.VerifyNoOtherCalls();
            comm.VerifyNoOtherCalls();
            mappings.Verify(
                m => m.Map(It.IsAny<IEnumerable<Author>>(), It.IsAny<Paging>()),
                Times.Never());
            mappings.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectBooks_ExistingAuthorIdAndEmptyArray()

        {
            var book1 = Helpers.GetBook(1);
            var author = new Author()
            {
                Id = 1,
                Books = new List<Book>() { book1 }
            };
            var booksIds = Array.Empty<int>();
            var commandsRepo = new Mock<IAuthorCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetAuthorWithBooks(author.Id))
                .Returns(author);
            queriesRepo.Setup(r => r.GetBook(It.IsAny<int>()));
            Author authorParam = null;
            commandsRepo.Setup(r => r.UpdateAuthorBooksRelation(author))
                .Callback<Author>(a => authorParam = a);
            var mappings = new Mock<IBookVMsMappings>();
            var service = new AuthorService(commandsRepo.Object, queriesRepo.Object, mappings.Object);

            service.SelectBooks(author.Id, booksIds);

            queriesRepo.Verify(
                r => r.GetAuthorWithBooks(author.Id),
                Times.Once());
            queriesRepo.Verify(
                r => r.GetBook(It.IsAny<int>()),
                Times.Never());
            commandsRepo.Verify(
                r => r.UpdateAuthorBooksRelation(author),
                Times.Once());
            authorParam.Should().NotBeNull();
            authorParam!.Books.Should().NotBeNull().And.BeEmpty();
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
            mappings.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectBooks_ExistingAuthorIdAndExistingBooksIds()
        {
            var book1 = Helpers.GetBook(1);
            var book2 = Helpers.GetBook(2);
            var book3 = Helpers.GetBook(3);
            var author = new Author()
            {
                Id = 1,
                Books = new List<Book>() { book1 }
            };
            var booksIds = new[] { book2.Id, book3.Id };
            var commandsRepo = new Mock<IAuthorCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetAuthorWithBooks(author.Id))
                .Returns(author);
            queriesRepo.Setup(r => r.GetBook(book2.Id))
                .Returns(book2);
            queriesRepo.Setup(r => r.GetBook(book3.Id))
                .Returns(book3);
            Author authorParam = null;
            commandsRepo.Setup(r => r.UpdateAuthorBooksRelation(author))
                .Callback<Author>(a => authorParam = a);
            var mappings = new Mock<IBookVMsMappings>();
            var service = new AuthorService(commandsRepo.Object, queriesRepo.Object, mappings.Object);

            service.SelectBooks(author.Id, booksIds);

            queriesRepo.Verify(r => r.GetAuthorWithBooks(author.Id), Times.Once());
            queriesRepo.Verify(r => r.GetBook(book2.Id), Times.Once());
            queriesRepo.Verify(r => r.GetBook(book3.Id), Times.Once());
            commandsRepo.Verify(
                r => r.UpdateAuthorBooksRelation(author),
                Times.Once());
            authorParam.Should().NotBeNull();
            authorParam!.Books.Should()
                .NotBeNull().And
                .HaveCount(2).And
                .Contain(book2).And
                .Contain(book3).And
                .NotContain(book1);
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
            mappings.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectBooks_ExistingAuthorIdAndNotExistingBooksIds()
        {
            var book1 = Helpers.GetBook(1);
            var author = new Author()
            {
                Id = 1,
                Books = new List<Book>() { book1 }
            };
            var booksIds = new[] { 2, 3 };
            var commandsRepo = new Mock<IAuthorCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetAuthorWithBooks(author.Id))
                .Returns(author);
            queriesRepo.Setup(r => r.GetBook(2))
                .Returns((Book?)null);
            queriesRepo.Setup(r => r.GetBook(3))
                .Returns((Book?)null);
            Author authorParam = null;
            commandsRepo.Setup(r => r.UpdateAuthorBooksRelation(author))
                .Callback<Author>(a => authorParam = a);
            var mappings = new Mock<IBookVMsMappings>();
            var service = new AuthorService(commandsRepo.Object, queriesRepo.Object, mappings.Object);

            service.SelectBooks(author.Id, booksIds);

            queriesRepo.Verify(
                r => r.GetAuthorWithBooks(author.Id),
                Times.Once());
            queriesRepo.Verify(r => r.GetBook(2), Times.Once());
            queriesRepo.Verify(r => r.GetBook(3), Times.Once());
            commandsRepo.Verify(
                r => r.UpdateAuthorBooksRelation(It.Is<Author>(
                    a => a.Equals(author) &&
                    a.Books != null &&
                    a.Books.Count == 0)),
                Times.Once());
            authorParam.Should().NotBeNull();
            authorParam!.Books.Should().NotBeNull().And.BeEmpty();
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
            mappings.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectBooks_NotExistingAuthorIdAndBooksIds()
        {
            int authorId = 1;
            var book1 = Helpers.GetBook(1);
            var booksIds = new[] { book1.Id };
            var commandsRepo = new Mock<IAuthorCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetAuthorWithBooks(authorId))
                .Returns((Author?)null);
            queriesRepo.Setup(r => r.GetBook(book1.Id))
                .Returns(book1);
            commandsRepo.Setup(r => r.UpdateAuthorBooksRelation(It.IsAny<Author>()));
            var mappings = new Mock<IBookVMsMappings>();
            var service = new AuthorService(commandsRepo.Object, queriesRepo.Object, mappings.Object);

            service.SelectBooks(authorId, booksIds);

            queriesRepo.Verify(
                r => r.GetAuthorWithBooks(authorId),
                Times.Once());
            queriesRepo.Verify(
                r => r.GetBook(book1.Id),
                Times.Never());
            commandsRepo.Verify(
                r => r.UpdateAuthorBooksRelation(It.IsAny<Author>()),
                Times.Never());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
            mappings.VerifyNoOtherCalls();
        }

        [Fact]
        public void Upsert_NewAuthor_NewId()
        {
            var author = new AuthorVM()
            {
                Name = "Name",
                LastName = "Last Name"
            };
            var entity = new Author()
            {
                Id = author.Id,
                Name = author.Name,
                LastName = author.LastName
            };
            var commandsRepo = new Mock<IAuthorCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            commandsRepo.Setup(r => r.Insert(entity))
                .Returns(1);
            commandsRepo.Setup(r => r.Update(entity));
            var mappings = new Mock<IBookVMsMappings>();
            mappings.Setup(m => m.Map<AuthorVM, Author>(author))
                .Returns(entity);
            var service = new AuthorService(commandsRepo.Object, queriesRepo.Object, mappings.Object);

            var result = service.Upsert(author);

            mappings.Verify(
                m => m.Map<AuthorVM, Author>(author),
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

        [Fact]
        public void Upsert_ExistingAuthor_AuthorsId()
        {
            var author = new AuthorVM()
            {
                Id = 1,
                Name = "Name",
                LastName = "Last Name"
            };
            var entity = new Author()
            {
                Id = author.Id,
                Name = author.Name,
                LastName = author.LastName
            };
            var commandsRepo = new Mock<IAuthorCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            commandsRepo.Setup(r => r.Insert(entity))
                .Returns(1);
            commandsRepo.Setup(r => r.Update(entity));
            var mappings = new Mock<IBookVMsMappings>();
            mappings.Setup(m => m.Map<AuthorVM, Author>(author))
                .Returns(entity);
            var service = new AuthorService(commandsRepo.Object, queriesRepo.Object, mappings.Object);

            var result = service.Upsert(author);

            mappings.Verify(
                m => m.Map<AuthorVM, Author>(author),
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
    }
}