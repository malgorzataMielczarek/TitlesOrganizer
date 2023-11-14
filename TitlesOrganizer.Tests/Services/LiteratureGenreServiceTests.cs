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
    public class LiteratureGenreServiceTests
    {
        [Fact]
        public void Delete_ExistingLiteratureGenreId()
        {
            var genre = new LiteratureGenre()
            {
                Id = 1,
                Name = "Name"
            };
            var commandsRepo = new Mock<ILiteratureGenreCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetLiteratureGenre(genre.Id))
                .Returns(genre);
            commandsRepo.Setup(r => r.Delete(genre));
            var mapper = new Mock<IMapper>().Object;
            var service = new LiteratureGenreServiceForTest(commandsRepo.Object, queriesRepo.Object, mapper);

            service.Delete(genre.Id);

            queriesRepo.Verify(
                r => r.GetLiteratureGenre(genre.Id),
                Times.Once);
            commandsRepo.Verify(
                r => r.Delete(genre),
                Times.Once);
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void Delete_NotExistingLiteratureGenreId()
        {
            var genre = new LiteratureGenre()
            {
                Id = 1,
                Name = "Name"
            };
            var commandsRepo = new Mock<ILiteratureGenreCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetLiteratureGenre(genre.Id))
                .Returns((LiteratureGenre?)null);
            commandsRepo.Setup(r => r.Delete(genre));
            var mapper = new Mock<IMapper>().Object;
            var service = new LiteratureGenreServiceForTest(commandsRepo.Object, queriesRepo.Object, mapper);

            service.Delete(genre.Id);

            queriesRepo.Verify(
                r => r.GetLiteratureGenre(genre.Id),
                Times.Once);
            commandsRepo.Verify(
                r => r.Delete(genre),
                Times.Never);
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void Get_ExistingGenreId()
        {
            int pageSize = 3, pageNo = 2;
            var books = Helpers.GetBooksList(3);
            var genre = new LiteratureGenre()
            {
                Id = 1,
                Name = "Name1",
                Books = books
            };
            var commandsRepo = new Mock<ILiteratureGenreCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetLiteratureGenreWithBooks(genre.Id))
                .Returns(genre);
            var mapper = new Mock<IMapper>().Object;
            var service = new LiteratureGenreServiceForTest(commandsRepo.Object, queriesRepo.Object, mapper);

            var result = service.Get(genre.Id, pageSize, pageNo);

            queriesRepo.Verify(
                r => r.GetLiteratureGenreWithBooks(genre.Id),
                Times.Once());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeOfType<GenreVM>();
            result.Id.Should().Be(genre.Id);
            result.Name.Should().Be(genre.Name);
            result.Books.Paging.PageSize.Should().Be(pageSize);
            result.Books.Paging.CurrentPage.Should().Be(pageNo);
            result.Books.Paging.Count.Should().Be(3);
            service.Books.Should().Equal(books);
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
            var mapper = new Mock<IMapper>().Object;
            var service = new LiteratureGenreServiceForTest(commandsRepo.Object, queriesRepo.Object, mapper);

            var result = service.Get(genreId, pageSize, pageNo);

            queriesRepo.Verify(
                r => r.GetLiteratureGenreWithBooks(genreId),
                Times.Once());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeOfType<GenreVM>();
            result.Id.Should().Be(default);
            result.Name.Should().Be(string.Empty);
            result.Books.Paging.PageSize.Should().Be(pageSize);
            result.Books.Paging.CurrentPage.Should().Be(1);
            result.Books.Paging.Count.Should().Be(0);
            service.Books.Should().BeNull();
        }

        [Fact]
        public void GetDetails_ExistingGenreId()
        {
            // Arrange
            int authorsPageSize = 3, authorsPageNo = 4, booksPageSize = 5, booksPageNo = 2, seriesPageSize = 1, seriesPageNo = 3;
            var genre = Helpers.GetGenre(1);
            var books = Helpers.GetBooksList(10);
            var s1 = Helpers.GetSeries(1);
            var s2 = Helpers.GetSeries(2);
            var s3 = Helpers.GetSeries(3);
            var a1 = Helpers.GetAuthor(1);
            var a2 = Helpers.GetAuthor(2);
            var a3 = Helpers.GetAuthor(3);
            var a4 = Helpers.GetAuthor(4);

            books[0].Genres.Add(Helpers.GetGenre(genre.Id));
            books[1].Genres.Add(Helpers.GetGenre(genre.Id));
            books[3].Genres.Add(Helpers.GetGenre(genre.Id));
            books[4].Genres.Add(Helpers.GetGenre(genre.Id));
            books[5].Genres.Add(Helpers.GetGenre(genre.Id));
            books[7].Genres.Add(Helpers.GetGenre(genre.Id));
            books[8].Genres.Add(Helpers.GetGenre(genre.Id));

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

            books[3].Authors = new[] { a1, a2, a3 };
            books[5].Authors = new[] { a1, a3 };
            books[6].Authors = new[] { a2, a4 };

            var commandsRepo = new Mock<ILiteratureGenreCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetLiteratureGenre(genre.Id))
                .Returns(genre);
            queriesRepo.Setup(r => r.GetAllBooksWithAuthorsGenresAndSeries())
                .Returns(books.AsQueryable());
            var mapper = new Mock<IMapper>().Object;

            var service = new LiteratureGenreServiceForTest(commandsRepo.Object, queriesRepo.Object, mapper);

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
            result.Should()
                .NotBeNull().And
                .BeOfType<GenreDetailsVM>();
            result.Id.Should().Be(genre.Id);
            result.Title.Should().Be(genre.Name);
            result.Authors.Paging.PageSize.Should().Be(authorsPageSize);
            result.Authors.Paging.CurrentPage.Should().Be(authorsPageNo);
            result.Authors.Paging.Count.Should().Be(3);
            result.Books.Paging.PageSize.Should().Be(booksPageSize);
            result.Books.Paging.CurrentPage.Should().Be(booksPageNo);
            result.Books.Paging.Count.Should().Be(7);
            result.Series.Paging.PageSize.Should().Be(seriesPageSize);
            result.Series.Paging.CurrentPage.Should().Be(seriesPageNo);
            result.Series.Paging.Count.Should().Be(2);
            service.Authors.Should()
                .NotBeNull().And
                .HaveCount(3).And
                .Contain(a1).And
                .Contain(a2).And
                .Contain(a3).And
                .NotContain(a4);
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
            var mapper = new Mock<IMapper>().Object;
            var service = new LiteratureGenreServiceForTest(commandsRepo.Object, queriesRepo.Object, mapper);

            var result = service.GetDetails(genreId, booksPageSize, booksPageNo, authorsPageSize, authorsPageNo, seriesPageSize, seriesPageNo);

            queriesRepo.Verify(
                r => r.GetLiteratureGenre(genreId),
                Times.Once());
            queriesRepo.Verify(
                r => r.GetAllBooksWithAuthorsGenresAndSeries(),
                Times.Never());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeOfType<GenreDetailsVM>();
            result.Id.Should().Be(default);
            result.Title.Should().Be(string.Empty);
            result.Authors.Paging.CurrentPage.Should().Be(1);
            result.Authors.Paging.Count.Should().Be(0);
            result.Books.Paging.PageSize.Should().Be(booksPageSize);
            result.Books.Paging.CurrentPage.Should().Be(1);
            result.Books.Paging.Count.Should().Be(0);
            result.Series.Paging.PageSize.Should().Be(seriesPageSize);
            result.Series.Paging.CurrentPage.Should().Be(1);
            result.Series.Paging.Count.Should().Be(0);
            result.Authors.Paging.PageSize.Should().Be(authorsPageSize);
            service.Authors.Should().BeNull();
            service.Books.Should().BeNull();
            service.Series.Should().BeNull();
        }

        [Theory]
        [InlineData(5, 5, 1, SortByEnum.Ascending, "Name")]
        [InlineData(6, 3, 2, SortByEnum.Descending, null)]
        public void GetList(int count, int pageSize, int pageNo, SortByEnum sort, string? search)
        {
            var genres = Helpers.GetGenresList(count).AsQueryable();
            var commandsRepo = new Mock<ILiteratureGenreCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetAllLiteratureGenres())
                .Returns(genres);
            var mapper = new Mock<IMapper>().Object;
            var service = new LiteratureGenreServiceForTest(commandsRepo.Object, queriesRepo.Object, mapper);

            var result = service.GetList(sort, pageSize, pageNo, search);

            queriesRepo.Verify(
                r => r.GetAllLiteratureGenres(),
                Times.Once());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            service.Genres.Should()
                .NotBeNull().And
                .HaveCount(count).And
                .Equal(genres);
            result.Should()
                .NotBeNull().And
                .BeOfType<ListGenreForListVM>();
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
            var g1 = Helpers.GetGenre(1);
            var g2 = Helpers.GetGenre(2);
            var g3 = Helpers.GetGenre(3);
            var g4 = Helpers.GetGenre(4);
            g1.Books = new[] { b1, b2 };
            g2.Books = new[] { b1 };
            g3.Books = new[] { b2 };
            g4.Books = new List<Book>();
            var genres = new[] { g1, g2, g3, g4 }.AsQueryable();
            var book = Helpers.GetBook(b1.Id);
            var commands = new Mock<ILiteratureGenreCommandsRepository>();
            var queries = new Mock<IBookModuleQueriesRepository>();
            queries.Setup(q => q.GetBook(b1.Id))
                .Returns(book);
            queries.Setup(q => q.GetAllLiteratureGenresWithBooks())
                .Returns(genres);
            var mapper = new Mock<IMapper>().Object;
            var service = new LiteratureGenreServiceForTest(commands.Object, queries.Object, mapper);

            var result = service.GetListForBook(b1.Id, sort, pageSize, pageNo, search);

            result.Should()
                .NotBeNull().And
                .BeOfType<ListGenreForBookVM>();
            queries.Verify(
                q => q.GetBook(b1.Id),
                Times.Once);
            queries.Verify(
                q => q.GetAllLiteratureGenresWithBooks(),
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
            service.Genres.Should()
                .NotBeNull().And
                .Equal(genres);
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
            var g1 = Helpers.GetGenre(1);
            var g2 = Helpers.GetGenre(2);
            var g3 = Helpers.GetGenre(3);
            var g4 = Helpers.GetGenre(4);
            g1.Books = new[] { b1, b2 };
            g2.Books = new[] { b1 };
            g3.Books = new[] { b2 };
            g4.Books = new List<Book>();
            var genres = new[] { g1, g2, g3, g4 }.AsQueryable();
            var commands = new Mock<ILiteratureGenreCommandsRepository>();
            var queries = new Mock<IBookModuleQueriesRepository>();
            queries.Setup(q => q.GetBook(bookId))
                .Returns((Book?)null);
            queries.Setup(q => q.GetAllLiteratureGenresWithBooks())
                .Returns(genres);
            var mapper = new Mock<IMapper>().Object;
            var service = new LiteratureGenreServiceForTest(commands.Object, queries.Object, mapper);

            var result = service.GetListForBook(bookId, sort, pageSize, pageNo, search);

            result.Should()
                .NotBeNull().And
                .BeOfType<ListGenreForBookVM>();
            queries.Verify(
                q => q.GetBook(bookId),
                Times.Once);
            queries.Verify(
                q => q.GetAllLiteratureGenresWithBooks(),
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
            service.Genres.Should()
                .NotBeNull().And
                .Equal(genres);
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
            var b4 = Helpers.GetBook(4);
            author.Books = new[]
            {
                Helpers.GetBook(b1.Id),
                Helpers.GetBook(b2.Id)
            };
            b1.Authors = new[] { Helpers.GetAuthor(author.Id) };
            b2.Authors = new[] { Helpers.GetAuthor(author.Id) };
            var g1 = Helpers.GetGenre(1);
            var g2 = Helpers.GetGenre(2);
            var g3 = Helpers.GetGenre(3);
            var g4 = Helpers.GetGenre(4);
            var g5 = Helpers.GetGenre(5);
            b1.Genres = new[] { g1, g2, g3 };
            b2.Genres = new[] { g1, g3 };
            b3.Genres = new[] { g3, g4 };
            b4.Genres = new[] { g5 };
            var books = new[] { b1, b2, b3, b4 }.AsQueryable();
            var comm = new Mock<ILiteratureGenreCommandsRepository>();
            var query = new Mock<IBookModuleQueriesRepository>();
            query.Setup(q => q.GetAuthor(author.Id))
                .Returns(author);
            query.Setup(q => q.GetAllBooksWithAuthorsGenresAndSeries())
                .Returns(books);
            var mapper = new Mock<IMapper>().Object;
            var service = new LiteratureGenreServiceForTest(comm.Object, query.Object, mapper);
            int pageSize = 2, pageNo = 3;

            var result = service.GetPartialListForAuthor(author.Id, pageSize, pageNo);

            result.Should()
                .NotBeNull().And
                .BeOfType<PartialList<LiteratureGenre>>();
            result.Paging.Should().NotBeNull();
            result.Paging.CurrentPage.Should().Be(pageNo);
            result.Paging.PageSize.Should().Be(pageSize);
            query.Verify(
                q => q.GetAuthor(author.Id),
                Times.Once);
            query.Verify(
                q => q.GetAllBooksWithAuthorsGenresAndSeries(),
                Times.Once);
            query.VerifyNoOtherCalls();
            comm.VerifyNoOtherCalls();
            service.Genres.Should()
                .NotBeNullOrEmpty().And
                .Equal(g1, g2, g3).And
                .NotContain(new[] { g4, g5 });
        }

        [Fact]
        public void GetPartialListForAuthor_NotExistingAuthor()
        {
            var authorId = 1;
            var comm = new Mock<ILiteratureGenreCommandsRepository>();
            var query = new Mock<IBookModuleQueriesRepository>();
            query.Setup(q => q.GetAuthor(authorId))
                .Returns((Author?)null);
            query.Setup(q => q.GetAllBooksWithAuthorsGenresAndSeries());
            var mapper = new Mock<IMapper>().Object;
            var service = new LiteratureGenreServiceForTest(comm.Object, query.Object, mapper);
            int pageSize = 2, pageNo = 3;

            var result = service.GetPartialListForAuthor(authorId, pageSize, pageNo);

            result.Should()
                .NotBeNull().And
                .BeOfType<PartialList<LiteratureGenre>>();
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
            service.Genres.Should().BeNull();
        }

        [Fact]
        public void SelectBooks_ExistingLiteratureGenreIdAndExistingBooksIds()
        {
            var b1 = Helpers.GetBook(1);
            var b2 = Helpers.GetBook(2);
            var b3 = Helpers.GetBook(3);
            var genre = new LiteratureGenre()
            {
                Id = 1,
                Name = "Name",
                Books = new[] { b1 }
            };
            var booksIds = new List<int>() { b2.Id, b3.Id };
            var commandsRepo = new Mock<ILiteratureGenreCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetLiteratureGenre(genre.Id))
                .Returns(genre);
            queriesRepo.Setup(r => r.GetBook(b2.Id))
                .Returns(b2);
            queriesRepo.Setup(r => r.GetBook(b3.Id))
                .Returns(b3);
            commandsRepo.Setup(r => r.UpdateLiteratureGenreBooksRelation(genre));
            var mapper = new Mock<IMapper>().Object;
            var service = new LiteratureGenreServiceForTest(commandsRepo.Object, queriesRepo.Object, mapper);

            service.SelectBooks(genre.Id, booksIds);

            queriesRepo.Verify(
                r => r.GetLiteratureGenre(genre.Id),
                Times.Once());
            queriesRepo.Verify(
                r => r.GetBook(b2.Id),
                Times.Once());
            queriesRepo.Verify(
                r => r.GetBook(b3.Id),
                Times.Once());
            genre.Books.Should()
                .HaveCount(booksIds.Count).And
                .Contain(b2).And
                .Contain(b3).And
                .NotContain(b1);
            commandsRepo.Verify(
                r => r.UpdateLiteratureGenreBooksRelation(
                    It.Is<LiteratureGenre>(
                        g => g.Equals(genre) &&
                        g.Books != null &&
                        g.Books.Count == booksIds.Count &&
                        g.Books.Contains(b2) &&
                        g.Books.Contains(b3) &&
                        !g.Books.Contains(b1))),
                Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectBooks_ExistingLiteratureGenreIdAndNotExistingBooksIds()
        {
            var book1 = Helpers.GetBook(1);
            var genre = new LiteratureGenre()
            {
                Id = 1,
                Name = "Name",
                Books = new[] { book1 }
            };
            var booksIds = new List<int>() { 2, 3 };
            var commandsRepo = new Mock<ILiteratureGenreCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetLiteratureGenre(genre.Id))
                .Returns(genre);
            queriesRepo.Setup(r => r.GetBook(2))
                .Returns((Book?)null);
            queriesRepo.Setup(r => r.GetBook(3))
                .Returns((Book?)null);
            commandsRepo.Setup(r => r.UpdateLiteratureGenreBooksRelation(genre));
            var mapper = new Mock<IMapper>().Object;
            var service = new LiteratureGenreServiceForTest(commandsRepo.Object, queriesRepo.Object, mapper);

            service.SelectBooks(genre.Id, booksIds);

            queriesRepo.Verify(
                r => r.GetLiteratureGenre(genre.Id),
                Times.Once());
            queriesRepo.Verify(r => r.GetBook(2), Times.Once());
            queriesRepo.Verify(r => r.GetBook(3), Times.Once());
            genre.Books.Should().NotBeNull().And.BeEmpty();
            commandsRepo.Verify(
                r => r.UpdateLiteratureGenreBooksRelation(
                    It.Is<LiteratureGenre>(
                        g => g.Equals(genre) &&
                        g.Books != null &&
                        g.Books.Count == 0)),
                Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectBooks_ExistingLiteratureGenreIdAndEmptyList()
        {
            var book1 = Helpers.GetBook(1);
            var genre = new LiteratureGenre()
            {
                Id = 1,
                Name = "Name",
                Books = new[] { book1 }
            };
            var booksIds = new List<int>();
            var commandsRepo = new Mock<ILiteratureGenreCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetLiteratureGenre(genre.Id))
                .Returns(genre);
            queriesRepo.Setup(r => r.GetBook(It.IsAny<int>()));
            commandsRepo.Setup(r => r.UpdateLiteratureGenreBooksRelation(genre));
            var mapper = new Mock<IMapper>().Object;
            var service = new LiteratureGenreServiceForTest(commandsRepo.Object, queriesRepo.Object, mapper);

            service.SelectBooks(genre.Id, booksIds);

            queriesRepo.Verify(
                r => r.GetLiteratureGenre(genre.Id),
                Times.Once());
            queriesRepo.Verify(
                r => r.GetBook(It.IsAny<int>()),
                Times.Never());
            genre.Books.Should().NotBeNull().And.BeEmpty();
            commandsRepo.Verify(
                r => r.UpdateLiteratureGenreBooksRelation(
                    It.Is<LiteratureGenre>(
                        g => g.Equals(genre) &&
                        g.Books != null && g.Books.Count == 0)),
                Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectBooks_NotExistingLiteratureGenreIdAndBooksIds()
        {
            int genreId = 1;
            var book1 = Helpers.GetBook(1);
            var booksIds = new List<int>() { book1.Id };
            var commandsRepo = new Mock<ILiteratureGenreCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetLiteratureGenre(genreId))
                .Returns((LiteratureGenre?)null);
            queriesRepo.Setup(r => r.GetBook(book1.Id))
                .Returns(book1);
            commandsRepo.Setup(r => r.UpdateLiteratureGenreBooksRelation(It.IsAny<LiteratureGenre>()));
            var mapper = new Mock<IMapper>().Object;
            var service = new LiteratureGenreServiceForTest(commandsRepo.Object, queriesRepo.Object, mapper);

            service.SelectBooks(genreId, booksIds);

            queriesRepo.Verify(
                r => r.GetLiteratureGenre(genreId),
                Times.Once());
            queriesRepo.Verify(
                r => r.GetBook(book1.Id),
                Times.Never());
            commandsRepo.Verify(
                r => r.UpdateLiteratureGenreBooksRelation(
                    It.IsAny<LiteratureGenre>()),
                Times.Never());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void Upsert_NewLiteratureGenre_NewId()
        {
            var genre = new GenreVM()
            {
                Name = "Name"
            };
            var commandsRepo = new Mock<ILiteratureGenreCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            commandsRepo.Setup(r => r.Insert(It.IsAny<LiteratureGenre>()))
                .Returns(1);
            commandsRepo.Setup(r => r.Update(It.IsAny<LiteratureGenre>()));
            var mapper = new Mock<IMapper>().Object;
            var service = new LiteratureGenreServiceForTest(commandsRepo.Object, queriesRepo.Object, mapper);

            var result = service.Upsert(genre);

            commandsRepo.Verify(
                r => r.Insert(It.Is<LiteratureGenre>(
                    g => g.Id == default)),
                Times.Once());
            commandsRepo.Verify(
                r => r.Update(It.IsAny<LiteratureGenre>()),
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
            var commandsRepo = new Mock<ILiteratureGenreCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            commandsRepo.Setup(r => r.Insert(It.IsAny<LiteratureGenre>()))
                .Returns(1);
            commandsRepo.Setup(r => r.Update(It.IsAny<LiteratureGenre>()));
            var mapper = new Mock<IMapper>().Object;
            var service = new LiteratureGenreServiceForTest(commandsRepo.Object, queriesRepo.Object, mapper);

            var result = service.Upsert(genre);

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

    internal class LiteratureGenreServiceForTest : LiteratureGenreService
    {
        public IQueryable<Author>? Authors { get; private set; }
        public IQueryable<Book>? Books { get; private set; }
        public IQueryable<LiteratureGenre>? Genres { get; private set; }
        public IQueryable<BookSeries>? Series { get; private set; }

        public LiteratureGenreServiceForTest(ILiteratureGenreCommandsRepository literatureGenreCommandsRepository, IBookModuleQueriesRepository bookModuleQueriesRepository, IMapper mapper) : base(literatureGenreCommandsRepository, bookModuleQueriesRepository, mapper)
        {
        }

        protected override LiteratureGenre Map(GenreVM genre)
        {
            return new LiteratureGenre()
            {
                Id = genre.Id,
                Name = genre.Name
            };
        }

        protected override GenreVM Map(LiteratureGenre genreWithBooks, int booksPageSize, int booksPageNo)
        {
            Books = genreWithBooks.Books?.AsQueryable();
            return new GenreVM()
            {
                Id = genreWithBooks.Id,
                Name = genreWithBooks.Name,
                Books = new PartialList<Book>()
                {
                    Paging = new Paging()
                    {
                        Count = Books?.Count() ?? 0,
                        PageSize = booksPageSize,
                        CurrentPage = booksPageNo
                    }
                }
            };
        }

        protected override GenreDetailsVM MapToDetails(LiteratureGenre genre)
        {
            return new GenreDetailsVM()
            {
                Id = genre.Id,
                Title = genre.Name
            };
        }

        protected override GenreDetailsVM MapGenreDetailsAuthors(GenreDetailsVM genreDetails, IQueryable<Author> authors, int pageSize, int pageNo)
        {
            Authors = authors;
            genreDetails.Authors.Paging = new Paging()
            {
                Count = authors.Count(),
                PageSize = pageSize,
                CurrentPage = pageNo
            };

            return genreDetails;
        }

        protected override GenreDetailsVM MapGenreDetailsBooks(GenreDetailsVM genreDetails, IQueryable<Book> books, int pageSize, int pageNo)
        {
            Books = books;
            genreDetails.Books.Paging = new Paging()
            {
                Count = books.Count(),
                PageSize = pageSize,
                CurrentPage = pageNo
            };

            return genreDetails;
        }

        protected override GenreDetailsVM MapGenreDetailsSeries(GenreDetailsVM genreDetails, IQueryable<BookSeries> series, int pageSize, int pageNo)
        {
            Series = series;
            genreDetails.Series.Paging = new Paging()
            {
                Count = series.Count(),
                PageSize = pageSize,
                CurrentPage = pageNo
            };

            return genreDetails;
        }

        protected override ListGenreForListVM MapToList(IQueryable<LiteratureGenre> genres, SortByEnum sortBy, int pageSize, int pageNo, string searchString)
        {
            Genres = genres;

            return new ListGenreForListVM()
            {
                Paging = new Paging
                {
                    Count = genres.Count(),
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

        protected override ListGenreForBookVM MapForBook(IQueryable<LiteratureGenre> genresWithBooks, Book book, SortByEnum sortBy, int pageSize, int pageNo, string searchString)
        {
            Genres = genresWithBooks;
            Books = new[] { book }.AsQueryable();

            return new ListGenreForBookVM()
            {
                Paging = new Paging
                {
                    Count = genresWithBooks.Count(),
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

        protected override PartialList<LiteratureGenre> MapToPartialList(IQueryable<LiteratureGenre> genres, int pageSize, int pageNo)
        {
            Genres = genres;

            return new PartialList<LiteratureGenre>
            {
                Paging = new Paging
                {
                    Count = genres.Count(),
                    CurrentPage = pageNo,
                    PageSize = pageSize
                }
            };
        }
    }
}