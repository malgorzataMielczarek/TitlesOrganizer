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
            Author author = new Author() { Id = 1 };
            var commandsRepo = new Mock<IAuthorCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetAuthor(author.Id)).Returns(author);
            commandsRepo.Setup(r => r.Delete(author));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new AuthorService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.Delete(author.Id);

            queriesRepo.Verify(r => r.GetAuthor(author.Id), Times.Once);
            commandsRepo.Verify(r => r.Delete(author), Times.Once);
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void Delete_NotExistingAuthorId()
        {
            Author author = new Author() { Id = 1 };
            var commandsRepo = new Mock<IAuthorCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetAuthor(author.Id)).Returns((Author?)null);
            commandsRepo.Setup(r => r.Delete(author));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new AuthorService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.Delete(author.Id);

            queriesRepo.Verify(r => r.GetAuthor(author.Id), Times.Once);
            commandsRepo.Verify(r => r.Delete(author), Times.Never);
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void Get_ExistingAuthorId()
        {
            int pageSize = 3, pageNo = 2;
            Author author = new Author() { Id = 1 };
            var commandsRepo = new Mock<IAuthorCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetAuthorWithBooks(author.Id)).Returns(author);
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new AuthorServiceForTest(commandsRepo.Object, queriesRepo.Object, mapper);
            var result = service.Get(author.Id, pageSize, pageNo);

            queriesRepo.Verify(r => r.GetAuthorWithBooks(author.Id), Times.Once());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            result.Should().NotBeNull().And.BeOfType<AuthorVM>();
            result.Id.Should().Be(author.Id);
            result.BooksPaging.PageSize.Should().Be(pageSize);
            result.BooksPaging.CurrentPage.Should().Be(pageNo);
        }

        [Fact]
        public void Get_NotExistingAuthorId()
        {
            int pageSize = 3, pageNo = 2;
            int authorId = 1;
            var commandsRepo = new Mock<IAuthorCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetAuthorWithBooks(authorId)).Returns((Author?)null);
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new AuthorServiceForTest(commandsRepo.Object, queriesRepo.Object, mapper);
            var result = service.Get(authorId, pageSize, pageNo);

            queriesRepo.Verify(r => r.GetAuthorWithBooks(authorId), Times.Once());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            result.Should().NotBeNull().And.BeOfType<AuthorVM>();
            result.Id.Should().Be(default);
            result.BooksPaging.PageSize.Should().Be(pageSize);
            result.BooksPaging.CurrentPage.Should().Be(1);
            result.BooksPaging.Count.Should().Be(0);
        }

        [Fact]
        public void GetDetails_ExistingAuthorId()
        {
            // Arrange
            int booksPageSize = 5, booksPageNo = 2, seriesPageSize = 1, seriesPageNo = 3, genrePageSize = 3, genrePageNo = 4;
            Author author = new Author() { Id = 1 };
            var books = Helpers.GetBooksList(10);
            var series = Helpers.GetSeriesList(2);
            var additionalSeries = Helpers.GetSeries(3);
            var genres = Helpers.GetGenresList(3);
            var additionalGenre = Helpers.GetGenre(4);

            books[0].Authors.Add(author);
            books[1].Authors.Add(author);
            books[3].Authors.Add(author);
            books[4].Authors.Add(author);
            books[5].Authors.Add(author);
            books[7].Authors.Add(author);
            books[8].Authors.Add(author);

            books[1].Series = series[0];
            books[1].SeriesId = series[0].Id;
            books[3].Series = series[1];
            books[3].SeriesId = series[1].Id;
            books[5].Series = series[0];
            books[5].SeriesId = series[0].Id;
            books[6].Series = additionalSeries;
            books[6].SeriesId = additionalSeries.Id;
            books[8].Series = series[1];
            books[8].SeriesId = series[1].Id;

            books[3].Genres = genres;
            books[5].Genres = genres;
            books[6].Genres = genres;
            books[6].Genres.Add(additionalGenre);

            var commandsRepo = new Mock<IAuthorCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetAuthor(author.Id)).Returns(author);
            queriesRepo.Setup(r => r.GetAllBooksWithAllRelatedObjects()).Returns(books.AsQueryable());
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new AuthorServiceForTest(commandsRepo.Object, queriesRepo.Object, mapper);

            // Act
            var result = service.GetDetails(author.Id, booksPageSize, booksPageNo, seriesPageSize, seriesPageNo, genrePageSize, genrePageNo);

            // Assert
            queriesRepo.Verify(r => r.GetAuthor(author.Id), Times.Once());
            queriesRepo.Verify(r => r.GetAllBooksWithAllRelatedObjects(), Times.Once());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            result.Should().NotBeNull().And.BeOfType<AuthorDetailsVM>();
            result.Id.Should().Be(author.Id);
            result.Books.Paging.PageSize.Should().Be(booksPageSize);
            result.Books.Paging.CurrentPage.Should().Be(booksPageNo);
            result.Series.Paging.PageSize.Should().Be(seriesPageSize);
            result.Series.Paging.CurrentPage.Should().Be(seriesPageNo);
            result.Genres.Paging.PageSize.Should().Be(genrePageSize);
            result.Genres.Paging.CurrentPage.Should().Be(genrePageNo);
            service.Books.Should().NotBeNull().And.HaveCount(7).And
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
            service.Series.Should().NotBeNull().And.HaveCount(2).And
                .Contain(series[0]).And
                .Contain(series[1]).And
                .NotContain(additionalSeries);
            service.Genres.Should().NotBeNull().And.HaveCount(3).And
                .Contain(genres[0]).And
                .Contain(genres[1]).And
                .Contain(genres[2]).And
                .NotContain(additionalGenre);
        }

        [Fact]
        public void GetDetails_NotExistingAuthorId()
        {
            // Arrange
            int booksPageSize = 5, booksPageNo = 2, seriesPageSize = 1, seriesPageNo = 3, genrePageSize = 3, genrePageNo = 4;
            int authorId = 1;
            var commandsRepo = new Mock<IAuthorCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetAuthor(authorId)).Returns((Author?)null);
            queriesRepo.Setup(r => r.GetAllBooksWithAllRelatedObjects());
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new AuthorServiceForTest(commandsRepo.Object, queriesRepo.Object, mapper);

            // Act
            var result = service.GetDetails(authorId, booksPageSize, booksPageNo, seriesPageSize, seriesPageNo, genrePageSize, genrePageNo);

            // Assert
            queriesRepo.Verify(r => r.GetAuthor(authorId), Times.Once());
            queriesRepo.Verify(r => r.GetAllBooksWithAllRelatedObjects(), Times.Never());
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

        [Fact]
        public void SelectBooks_ExistingAuthorIdAndEmptyList()

        {
            var book1 = new Book() { Id = 1, Title = "Title1" };
            var author = new Author()
            {
                Id = 1,
                Books = new List<Book>() { book1 }
            };
            List<int> booksIds = new List<int>();
            var commandsRepo = new Mock<IAuthorCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetAuthor(author.Id)).Returns(author);
            queriesRepo.Setup(r => r.GetBook(It.IsAny<int>()));
            commandsRepo.Setup(r => r.UpdateAuthorBooksRelation(author));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new AuthorService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectBooks(author.Id, booksIds);

            queriesRepo.Verify(r => r.GetAuthor(author.Id), Times.Once());
            queriesRepo.Verify(r => r.GetBook(It.IsAny<int>()), Times.Never());
            author.Books.Should().NotBeNull().And.BeEmpty();
            commandsRepo.Verify(r => r.UpdateAuthorBooksRelation
            (
                It.Is<Author>(a =>
                    a.Equals(author) &&
                    a.Books != null && a.Books.Count == 0)
            ), Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectBooks_ExistingAuthorIdAndExistingBooksIds()
        {
            var book1 = new Book() { Id = 1, Title = "Title1" };
            var book2 = new Book() { Id = 2, Title = "Title2" };
            var book3 = new Book() { Id = 3, Title = "Title3" };
            var author = new Author()
            {
                Id = 1,
                Books = new List<Book>() { book1 }
            };
            List<int> booksIds = new List<int>() { book2.Id, book3.Id };
            var commandsRepo = new Mock<IAuthorCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetAuthor(author.Id)).Returns(author);
            queriesRepo.Setup(r => r.GetBook(book2.Id)).Returns(book2);
            queriesRepo.Setup(r => r.GetBook(book3.Id)).Returns(book3);
            commandsRepo.Setup(r => r.UpdateAuthorBooksRelation(author));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new AuthorService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectBooks(author.Id, booksIds);

            queriesRepo.Verify(r => r.GetAuthor(author.Id), Times.Once());
            queriesRepo.Verify(r => r.GetBook(book2.Id), Times.Once());
            queriesRepo.Verify(r => r.GetBook(book3.Id), Times.Once());
            author.Books.Should().HaveCount(booksIds.Count).And.Contain(book2).And.Contain(book3).And.NotContain(book1);
            commandsRepo.Verify(r => r.UpdateAuthorBooksRelation
            (
                It.Is<Author>(a =>
                    a.Equals(author) &&
                    a.Books != null && a.Books.Count == booksIds.Count &&
                    a.Books.Contains(book2) && a.Books.Contains(book3) && !a.Books.Contains(book1))
            ), Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectBooks_ExistingAuthorIdAndNotExistingBooksIds()
        {
            var book1 = new Book() { Id = 1, Title = "Title1" };
            var author = new Author()
            {
                Id = 1,
                Books = new List<Book>() { book1 }
            };
            List<int> booksIds = new List<int>() { 2, 3 };
            var commandsRepo = new Mock<IAuthorCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetAuthor(author.Id)).Returns(author);
            queriesRepo.Setup(r => r.GetBook(2)).Returns((Book?)null);
            queriesRepo.Setup(r => r.GetBook(3)).Returns((Book?)null);
            commandsRepo.Setup(r => r.UpdateAuthorBooksRelation(author));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new AuthorService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectBooks(author.Id, booksIds);

            queriesRepo.Verify(r => r.GetAuthor(author.Id), Times.Once());
            queriesRepo.Verify(r => r.GetBook(2), Times.Once());
            queriesRepo.Verify(r => r.GetBook(3), Times.Once());
            author.Books.Should().NotBeNull().And.BeEmpty();
            commandsRepo.Verify(r => r.UpdateAuthorBooksRelation
            (
                It.Is<Author>(a =>
                    a.Equals(author) &&
                    a.Books != null && a.Books.Count == 0)
            ), Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectBooks_NotExistingAuthorIdAndBooksIds()
        {
            int authorId = 1;
            var book1 = new Book() { Id = 1, Title = "Title1" };
            List<int> booksIds = new List<int>() { book1.Id };
            var commandsRepo = new Mock<IAuthorCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetAuthor(authorId)).Returns((Author?)null);
            queriesRepo.Setup(r => r.GetBook(book1.Id)).Returns(book1);
            commandsRepo.Setup(r => r.UpdateAuthorBooksRelation(It.IsAny<Author>()));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new AuthorService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectBooks(authorId, booksIds);

            queriesRepo.Verify(r => r.GetAuthor(authorId), Times.Once());
            queriesRepo.Verify(r => r.GetBook(book1.Id), Times.Never());
            commandsRepo.Verify(r => r.UpdateAuthorBooksRelation(It.IsAny<Author>()), Times.Never());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void Upsert_NewAuthor_NewId()
        {
            AuthorVM author = new AuthorVM() { Name = "Name", LastName = "Last Name" };
            var commandsRepo = new Mock<IAuthorCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            commandsRepo.Setup(r => r.Insert(It.IsAny<Author>())).Returns(1);
            commandsRepo.Setup(r => r.Update(It.IsAny<Author>()));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new AuthorServiceForTest(commandsRepo.Object, queriesRepo.Object, mapper);
            var result = service.Upsert(author);

            commandsRepo.Verify(r => r.Insert(It.Is<Author>(a => a.Id == default)), Times.Once());
            commandsRepo.Verify(r => r.Update(It.IsAny<Author>()), Times.Never());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
            result.Should().Be(1);
        }

        [Fact]
        public void Upsert_ExistingAuthor_AuthorsId()
        {
            AuthorVM author = new AuthorVM() { Id = 1, Name = "Name", LastName = "Last Name" };
            var commandsRepo = new Mock<IAuthorCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            commandsRepo.Setup(r => r.Insert(It.IsAny<Author>())).Returns(1);
            commandsRepo.Setup(r => r.Update(It.IsAny<Author>()));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new AuthorServiceForTest(commandsRepo.Object, queriesRepo.Object, mapper);
            var result = service.Upsert(author);

            commandsRepo.Verify(r => r.Insert(It.IsAny<Author>()), Times.Never());
            commandsRepo.Verify(r => r.Update(It.Is<Author>(a => a.Id == 1)), Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
            result.Should().Be(1);
        }

        public class AuthorServiceForTest : AuthorService
        {
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
                    BooksPaging = new Paging() { CurrentPage = bookPageNo, PageSize = bookPageSize },
                };
            }

            protected override AuthorDetailsVM MapToDetails(Author author, IQueryable<Book> books, int bookPageSize, int bookPageNo, IQueryable<BookSeries> series, int seriesPageSize, int seriesPageNo, IQueryable<LiteratureGenre> genres, int genrePageSize, int genrePageNo)
            {
                Books = books;
                Series = series;
                Genres = genres;

                return new AuthorDetailsVM()
                {
                    Id = author.Id,
                    Books = new PartialList<Book>()
                    {
                        Paging = new Paging()
                        {
                            PageSize = bookPageSize,
                            CurrentPage = bookPageNo,
                            Count = books.Count()
                        }
                    },
                    Series = new PartialList<BookSeries>()
                    {
                        Paging = new Paging()
                        {
                            CurrentPage = seriesPageNo,
                            PageSize = seriesPageSize,
                            Count = series.Count()
                        }
                    },
                    Genres = new PartialList<LiteratureGenre>()
                    {
                        Paging = new Paging()
                        {
                            PageSize = genrePageSize,
                            CurrentPage = genrePageNo,
                            Count = genres.Count()
                        }
                    }
                };
            }
        }
    }
}