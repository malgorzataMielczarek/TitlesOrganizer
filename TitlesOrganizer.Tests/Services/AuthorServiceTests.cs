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
            var mapper = new Mock<IMapper>().Object;
            var service = new AuthorService(commandsRepo.Object, queriesRepo.Object, mapper);

            service.Delete(author.Id);

            queriesRepo.Verify(
                r => r.GetAuthor(author.Id),
                Times.Once);
            commandsRepo.Verify(
                r => r.Delete(author),
                Times.Once);
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
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
            var mapper = new Mock<IMapper>().Object;
            var service = new AuthorService(commandsRepo.Object, queriesRepo.Object, mapper);

            service.Delete(author.Id);

            queriesRepo.Verify(
                r => r.GetAuthor(author.Id),
                Times.Once);
            commandsRepo.Verify(
                r => r.Delete(author),
                Times.Never);
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void Get_ExistingAuthorId()
        {
            int pageSize = 3, pageNo = 2;
            var author = new Author() { Id = 1 };
            var commandsRepo = new Mock<IAuthorCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetAuthorWithBooks(author.Id))
                .Returns(author);
            var mapper = new Mock<IMapper>().Object;
            var service = new AuthorServiceForTest(commandsRepo.Object, queriesRepo.Object, mapper);

            var result = service.Get(author.Id, pageSize, pageNo);

            queriesRepo.Verify(
                r => r.GetAuthorWithBooks(author.Id),
                Times.Once());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeOfType<AuthorVM>();
            result.Id.Should().Be(author.Id);
            result.Books.Paging.PageSize.Should().Be(pageSize);
            result.Books.Paging.CurrentPage.Should().Be(pageNo);
        }

        [Fact]
        public void Get_NotExistingAuthorId()
        {
            int pageSize = 3, pageNo = 2;
            int authorId = 1;
            var commandsRepo = new Mock<IAuthorCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetAuthorWithBooks(authorId))
                .Returns((Author?)null);
            var mapper = new Mock<IMapper>().Object;
            var service = new AuthorServiceForTest(commandsRepo.Object, queriesRepo.Object, mapper);

            var result = service.Get(authorId, pageSize, pageNo);

            queriesRepo.Verify(
                r => r.GetAuthorWithBooks(authorId),
                Times.Once());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeOfType<AuthorVM>();
            result.Id.Should().Be(default);
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

            var commandsRepo = new Mock<IAuthorCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetAuthor(author.Id))
                .Returns(author);
            queriesRepo.Setup(r => r.GetAllBooksWithAllRelatedObjects())
                .Returns(books.AsQueryable());
            var mapper = new Mock<IMapper>().Object;

            var service = new AuthorServiceForTest(commandsRepo.Object, queriesRepo.Object, mapper);

            // Act
            var result = service.GetDetails(author.Id, booksPageSize, booksPageNo, seriesPageSize, seriesPageNo, genrePageSize, genrePageNo);

            // Assert
            queriesRepo.Verify(
                r => r.GetAuthor(author.Id),
                Times.Once());
            queriesRepo.Verify(
                r => r.GetAllBooksWithAllRelatedObjects(),
                Times.Once());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeOfType<AuthorDetailsVM>();
            result.Id.Should().Be(author.Id);
            result.Books.Paging.PageSize.Should().Be(booksPageSize);
            result.Books.Paging.CurrentPage.Should().Be(booksPageNo);
            result.Series.Paging.PageSize.Should().Be(seriesPageSize);
            result.Series.Paging.CurrentPage.Should().Be(seriesPageNo);
            result.Genres.Paging.PageSize.Should().Be(genrePageSize);
            result.Genres.Paging.CurrentPage.Should().Be(genrePageNo);
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
            service.Series.Should()
                .NotBeNull().And
                .HaveCount(2).And
                .Contain(s1).And
                .Contain(s2).And
                .NotContain(s3);
            service.Genres.Should()
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
            int authorId = 1;
            var commandsRepo = new Mock<IAuthorCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetAuthor(authorId))
                .Returns((Author?)null);
            queriesRepo.Setup(r => r.GetAllBooksWithAllRelatedObjects());
            var mapper = new Mock<IMapper>().Object;

            var service = new AuthorServiceForTest(commandsRepo.Object, queriesRepo.Object, mapper);

            // Act
            var result = service.GetDetails(authorId, booksPageSize, booksPageNo, seriesPageSize, seriesPageNo, genrePageSize, genrePageNo);

            // Assert
            queriesRepo.Verify(
                r => r.GetAuthor(authorId),
                Times.Once());
            queriesRepo.Verify(
                r => r.GetAllBooksWithAllRelatedObjects(),
                Times.Never());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            result.Should().NotBeNull().And.BeOfType<AuthorDetailsVM>();
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
            service.Books.Should().BeNull();
            service.Series.Should().BeNull();
            service.Genres.Should().BeNull();
        }

        [Theory]
        [InlineData(5, 5, 1, SortByEnum.Ascending, "Name")]
        [InlineData(6, 3, 2, SortByEnum.Descending, null)]
        public void GetList(int count, int pageSize, int pageNo, SortByEnum sort, string? search)
        {
            var authors = Helpers.GetAuthorsList(count).AsQueryable();
            var commandsRepo = new Mock<IAuthorCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetAllAuthors())
                .Returns(authors);
            var mapper = new Mock<IMapper>().Object;
            var service = new AuthorServiceForTest(commandsRepo.Object, queriesRepo.Object, mapper);

            var result = service.GetList(sort, pageSize, pageNo, search);

            queriesRepo.Verify(
                r => r.GetAllAuthors(),
                Times.Once());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            service.Authors.Should()
                .NotBeNull().And
                .HaveCount(count).And
                .Equal(authors);
            result.Should()
                .NotBeNull().And
                .BeOfType<ListAuthorForListVM>();
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
            var a1 = Helpers.GetAuthor(1);
            var a2 = Helpers.GetAuthor(2);
            var a3 = Helpers.GetAuthor(3);
            var a4 = Helpers.GetAuthor(4);
            a1.Books = new[] { b1, b2 };
            a2.Books = new[] { b1 };
            a3.Books = new[] { b2 };
            a4.Books = new List<Book>();
            var authors = new[] { a1, a2, a3, a4 }.AsQueryable();
            var book = Helpers.GetBook(b1.Id);
            var commands = new Mock<IAuthorCommandsRepository>();
            var queries = new Mock<IBookModuleQueriesRepository>();
            queries.Setup(q => q.GetBook(b1.Id))
                .Returns(book);
            queries.Setup(q => q.GetAllAuthorsWithBooks())
                .Returns(authors);
            var mapper = new Mock<IMapper>().Object;
            var service = new AuthorServiceForTest(commands.Object, queries.Object, mapper);

            var result = service.GetListForBook(b1.Id, sort, pageSize, pageNo, search);

            result.Should()
                .NotBeNull().And
                .BeOfType<ListAuthorForBookVM>();
            queries.Verify(
                q => q.GetBook(b1.Id),
                Times.Once);
            queries.Verify(
                q => q.GetAllAuthorsWithBooks(),
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
            service.Authors.Should()
                .NotBeNull().And
                .Equal(authors);
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
            int bookId = 3;
            var a1 = Helpers.GetAuthor(1);
            var a2 = Helpers.GetAuthor(2);
            var a3 = Helpers.GetAuthor(3);
            var a4 = Helpers.GetAuthor(4);
            a1.Books = new[] { b1, b2 };
            a2.Books = new[] { b1 };
            a3.Books = new[] { b2 };
            a4.Books = new List<Book>();
            var authors = new[] { a1, a2, a3, a4 }.AsQueryable();
            var commands = new Mock<IAuthorCommandsRepository>();
            var queries = new Mock<IBookModuleQueriesRepository>();
            queries.Setup(q => q.GetBook(bookId))
                .Returns((Book?)null);
            queries.Setup(q => q.GetAllAuthorsWithBooks())
                .Returns(authors);
            var mapper = new Mock<IMapper>().Object;
            var service = new AuthorServiceForTest(commands.Object, queries.Object, mapper);

            var result = service.GetListForBook(bookId, sort, pageSize, pageNo, search);

            result.Should()
                .NotBeNull().And
                .BeOfType<ListAuthorForBookVM>();
            queries.Verify(
                q => q.GetBook(bookId),
                Times.Once);
            queries.Verify(
                q => q.GetAllAuthorsWithBooks(),
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
            service.Authors.Should()
                .NotBeNull().And
                .Equal(authors);
            service.Books.Should()
                .NotBeNullOrEmpty().And
                .NotContainNulls().And
                .ContainSingle(b => b.Id == default);
        }

        [Fact]
        public void GetPartialListForGenre_ExistingGenre()
        {
            var genre = Helpers.GetGenre();
            var b1 = Helpers.GetBook(1);
            var b2 = Helpers.GetBook(2);
            var b3 = Helpers.GetBook(3);
            genre.Books = new[]
            {
                Helpers.GetBook(b1.Id),
                Helpers.GetBook(b2.Id)
            };
            var a1 = Helpers.GetAuthor(1);
            var a2 = Helpers.GetAuthor(2);
            var a3 = Helpers.GetAuthor(3);
            var a4 = Helpers.GetAuthor(4);
            var a5 = Helpers.GetAuthor(5);
            a1.Books = new[] { b1, b2 };
            a2.Books = new[] { b1 };
            a3.Books = new[] { b1, b2, b3 };
            a4.Books = new[] { b3 };
            a5.Books = new List<Book>();
            var authors = new[] { a1, a2, a3, a4, a5 }.AsQueryable();
            var comm = new Mock<IAuthorCommandsRepository>();
            var query = new Mock<IBookModuleQueriesRepository>();
            query.Setup(q => q.GetLiteratureGenreWithBooks(genre.Id))
                .Returns(genre);
            query.Setup(q => q.GetAllAuthorsWithBooks())
                .Returns(authors);
            var mapper = new Mock<IMapper>().Object;
            var service = new AuthorServiceForTest(comm.Object, query.Object, mapper);
            int pageSize = 2, pageNo = 3;

            var result = service.GetPartialListForGenre(genre.Id, pageSize, pageNo);

            result.Should()
                .NotBeNull().And
                .BeOfType<PartialList<Author>>();
            result.Paging.Should().NotBeNull();
            result.Paging.CurrentPage.Should().Be(pageNo);
            result.Paging.PageSize.Should().Be(pageSize);
            query.Verify(
                q => q.GetLiteratureGenreWithBooks(genre.Id),
                Times.Once);
            query.Verify(
                q => q.GetAllAuthorsWithBooks(),
                Times.Once);
            query.VerifyNoOtherCalls();
            comm.VerifyNoOtherCalls();
            service.Authors.Should()
                .NotBeNullOrEmpty().And
                .Equal(a1, a2, a3).And
                .NotContain(new[] { a4, a5 });
        }

        [Fact]
        public void GetPartialListForGenre_NotExistingGenre()
        {
            int genreId = 1;
            var comm = new Mock<IAuthorCommandsRepository>();
            var query = new Mock<IBookModuleQueriesRepository>();
            query.Setup(q => q.GetLiteratureGenreWithBooks(genreId))
                .Returns((LiteratureGenre?)null);
            query.Setup(q => q.GetAllAuthorsWithBooks());
            var mapper = new Mock<IMapper>().Object;
            var service = new AuthorServiceForTest(comm.Object, query.Object, mapper);
            int pageSize = 2, pageNo = 3;

            var result = service.GetPartialListForGenre(genreId, pageSize, pageNo);

            result.Should()
                .NotBeNull().And
                .BeOfType<PartialList<Author>>();
            result.Paging.Should().NotBeNull();
            result.Paging.CurrentPage.Should().Be(1);
            result.Paging.PageSize.Should().Be(pageSize);
            result.Paging.Count.Should().Be(0);
            result.Values.Should().NotBeNull().And.BeEmpty();
            query.Verify(
                q => q.GetLiteratureGenreWithBooks(genreId),
                Times.Once);
            query.Verify(
                q => q.GetAllAuthorsWithBooks(),
                Times.Never);
            query.VerifyNoOtherCalls();
            comm.VerifyNoOtherCalls();
            service.Authors.Should().BeNull();
        }

        [Fact]
        public void SelectBooks_ExistingAuthorIdAndEmptyList()

        {
            var book1 = Helpers.GetBook(1);
            var author = new Author()
            {
                Id = 1,
                Books = new[] { book1 }
            };
            var booksIds = new List<int>();
            var commandsRepo = new Mock<IAuthorCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetAuthor(author.Id))
                .Returns(author);
            queriesRepo.Setup(r => r.GetBook(It.IsAny<int>()));
            commandsRepo.Setup(r => r.UpdateAuthorBooksRelation(author));
            var mapper = new Mock<IMapper>().Object;
            var service = new AuthorServiceForTest(commandsRepo.Object, queriesRepo.Object, mapper);

            service.SelectBooks(author.Id, booksIds);

            queriesRepo.Verify(
                r => r.GetAuthor(author.Id),
                Times.Once());
            queriesRepo.Verify(
                r => r.GetBook(It.IsAny<int>()),
                Times.Never());
            author.Books.Should().NotBeNull().And.BeEmpty();
            commandsRepo.Verify(
                r => r.UpdateAuthorBooksRelation(It.Is<Author>(
                    a => a.Equals(author) &&
                    a.Books != null &&
                    a.Books.Count == 0)),
                Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
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
                Books = new[] { book1 }
            };
            var booksIds = new List<int>() { book2.Id, book3.Id };
            var commandsRepo = new Mock<IAuthorCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetAuthor(author.Id))
                .Returns(author);
            queriesRepo.Setup(r => r.GetBook(book2.Id))
                .Returns(book2);
            queriesRepo.Setup(r => r.GetBook(book3.Id))
                .Returns(book3);
            commandsRepo.Setup(r => r.UpdateAuthorBooksRelation(author));
            var mapper = new Mock<IMapper>().Object;
            var service = new AuthorServiceForTest(commandsRepo.Object, queriesRepo.Object, mapper);

            service.SelectBooks(author.Id, booksIds);

            queriesRepo.Verify(r => r.GetAuthor(author.Id), Times.Once());
            queriesRepo.Verify(r => r.GetBook(book2.Id), Times.Once());
            queriesRepo.Verify(r => r.GetBook(book3.Id), Times.Once());
            author.Books.Should().HaveCount(booksIds.Count).And.Contain(book2).And.Contain(book3).And.NotContain(book1);
            commandsRepo.Verify(
                r => r.UpdateAuthorBooksRelation(It.Is<Author>(
                    a => a.Equals(author) &&
                    a.Books != null &&
                    a.Books.Count == booksIds.Count &&
                    a.Books.Contains(book2) &&
                    a.Books.Contains(book3) &&
                    !a.Books.Contains(book1))),
                Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectBooks_ExistingAuthorIdAndNotExistingBooksIds()
        {
            var book1 = Helpers.GetBook(1);
            var author = new Author()
            {
                Id = 1,
                Books = new[] { book1 }
            };
            var booksIds = new List<int>() { 2, 3 };
            var commandsRepo = new Mock<IAuthorCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetAuthor(author.Id))
                .Returns(author);
            queriesRepo.Setup(r => r.GetBook(2))
                .Returns((Book?)null);
            queriesRepo.Setup(r => r.GetBook(3))
                .Returns((Book?)null);
            commandsRepo.Setup(r => r.UpdateAuthorBooksRelation(author));
            var mapper = new Mock<IMapper>().Object;
            var service = new AuthorServiceForTest(commandsRepo.Object, queriesRepo.Object, mapper);

            service.SelectBooks(author.Id, booksIds);

            queriesRepo.Verify(
                r => r.GetAuthor(author.Id),
                Times.Once());
            queriesRepo.Verify(r => r.GetBook(2), Times.Once());
            queriesRepo.Verify(r => r.GetBook(3), Times.Once());
            author.Books.Should().NotBeNull().And.BeEmpty();
            commandsRepo.Verify(
                r => r.UpdateAuthorBooksRelation(It.Is<Author>(
                    a => a.Equals(author) &&
                    a.Books != null &&
                    a.Books.Count == 0)),
                Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectBooks_NotExistingAuthorIdAndBooksIds()
        {
            int authorId = 1;
            var book1 = Helpers.GetBook(1);
            var booksIds = new List<int>() { book1.Id };
            var commandsRepo = new Mock<IAuthorCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetAuthor(authorId))
                .Returns((Author?)null);
            queriesRepo.Setup(r => r.GetBook(book1.Id))
                .Returns(book1);
            commandsRepo.Setup(r => r.UpdateAuthorBooksRelation(It.IsAny<Author>()));
            var mapper = new Mock<IMapper>().Object;
            var service = new AuthorServiceForTest(commandsRepo.Object, queriesRepo.Object, mapper);

            service.SelectBooks(authorId, booksIds);

            queriesRepo.Verify(
                r => r.GetAuthor(authorId),
                Times.Once());
            queriesRepo.Verify(
                r => r.GetBook(book1.Id),
                Times.Never());
            commandsRepo.Verify(
                r => r.UpdateAuthorBooksRelation(It.IsAny<Author>()),
                Times.Never());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void Upsert_NewAuthor_NewId()
        {
            var author = new AuthorVM()
            {
                Name = "Name",
                LastName = "Last Name"
            };
            var commandsRepo = new Mock<IAuthorCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            commandsRepo.Setup(r => r.Insert(It.IsAny<Author>()))
                .Returns(1);
            commandsRepo.Setup(r => r.Update(It.IsAny<Author>()));
            var mapper = new Mock<IMapper>().Object;
            var service = new AuthorServiceForTest(commandsRepo.Object, queriesRepo.Object, mapper);

            var result = service.Upsert(author);

            commandsRepo.Verify(
                r => r.Insert(It.Is<Author>(a => a.Id == default)),
                Times.Once());
            commandsRepo.Verify(
                r => r.Update(It.IsAny<Author>()),
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
            var commandsRepo = new Mock<IAuthorCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            commandsRepo.Setup(r => r.Insert(It.IsAny<Author>()))
                .Returns(1);
            commandsRepo.Setup(r => r.Update(It.IsAny<Author>()));
            var mapper = new Mock<IMapper>().Object;
            var service = new AuthorServiceForTest(commandsRepo.Object, queriesRepo.Object, mapper);

            var result = service.Upsert(author);

            commandsRepo.Verify(
                r => r.Insert(It.IsAny<Author>()),
                Times.Never());
            commandsRepo.Verify(
                r => r.Update(It.Is<Author>(a => a.Id == 1)),
                Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
            result.Should().Be(1);
        }

        public class AuthorServiceForTest : AuthorService
        {
            public IQueryable<Author>? Authors { get; private set; }
            public IQueryable<Book>? Books { get; private set; }
            public IQueryable<BookSeries>? Series { get; private set; }
            public IQueryable<LiteratureGenre>? Genres { get; private set; }

            public AuthorServiceForTest(IAuthorCommandsRepository authorCommandsRepository, IBookModuleQueriesRepository bookModuleQueriesRepository, IMapper mapper) : base(authorCommandsRepository, bookModuleQueriesRepository, mapper)
            {
            }

            protected override Author Map(AuthorVM author)
            {
                return new Author()
                {
                    Id = author.Id
                };
            }

            protected override AuthorVM Map(Author authorWithBooks, int bookPageSize, int bookPageNo)
            {
                return new AuthorVM()
                {
                    Id = authorWithBooks.Id,
                    Books = new PartialList<Book>()
                    {
                        Paging = new Paging() { CurrentPage = bookPageNo, PageSize = bookPageSize }
                    }
                };
            }

            protected override AuthorDetailsVM MapToDetails(Author author)
            {
                return new AuthorDetailsVM() { Id = author.Id };
            }

            protected override AuthorDetailsVM MapAuthorDetailsBooks(AuthorDetailsVM authorDetails, IQueryable<Book> books, int pageSize, int pageNo)
            {
                Books = books;
                authorDetails.Books = new PartialList<Book>()
                {
                    Paging = new Paging()
                    {
                        PageSize = pageSize,
                        CurrentPage = pageNo,
                        Count = books.Count()
                    }
                };

                return authorDetails;
            }

            protected override AuthorDetailsVM MapAuthorDetailsGenres(AuthorDetailsVM authorDetails, IQueryable<LiteratureGenre> genres, int pageSize, int pageNo)
            {
                Genres = genres;
                authorDetails.Genres = new PartialList<LiteratureGenre>()
                {
                    Paging = new Paging()
                    {
                        PageSize = pageSize,
                        CurrentPage = pageNo,
                        Count = genres.Count()
                    }
                };

                return authorDetails;
            }

            protected override AuthorDetailsVM MapAuthorDetailsSeries(AuthorDetailsVM authorDetails, IQueryable<BookSeries> series, int pageSize, int pageNo)
            {
                Series = series;
                authorDetails.Series = new PartialList<BookSeries>()
                {
                    Paging = new Paging()
                    {
                        PageSize = pageSize,
                        CurrentPage = pageNo,
                        Count = series.Count()
                    }
                };

                return authorDetails;
            }

            protected override ListAuthorForListVM MapToList(IQueryable<Author> authorList, SortByEnum sortBy, int pageSize, int pageNo, string searchString)
            {
                Authors = authorList;
                return new ListAuthorForListVM()
                {
                    Paging = new Paging()
                    {
                        PageSize = pageSize,
                        CurrentPage = pageNo,
                        Count = authorList.Count()
                    },
                    Filtering = new Filtering()
                    {
                        SortBy = sortBy,
                        SearchString = searchString
                    }
                };
            }

            protected override ListAuthorForBookVM MapForBook(IQueryable<Author> authorsWithBooks, Book book, SortByEnum sortBy, int pageSize, int pageNo, string searchString)
            {
                Authors = authorsWithBooks;
                Books = new List<Book> { book }.AsQueryable();
                return new ListAuthorForBookVM()
                {
                    Paging = new Paging
                    {
                        CurrentPage = pageNo,
                        PageSize = pageSize
                    },
                    Filtering = new Filtering
                    {
                        SearchString = searchString,
                        SortBy = sortBy
                    }
                };
            }

            protected override PartialList<Author> MapToPartialList(IQueryable<Author> authors, int pageSize, int pageNo)
            {
                Authors = authors;
                return new PartialList<Author>
                {
                    Paging = new Paging()
                    {
                        CurrentPage = pageNo,
                        PageSize = pageSize
                    }
                };
            }
        }
    }
}