// Ignore Spelling: Upsert

using AutoMapper;
using FluentAssertions;
using Moq;
using TitlesOrganizer.Application.Services;
using TitlesOrganizer.Application.ViewModels.BookVMs.UpdateVMs;
using TitlesOrganizer.Domain.Interfaces;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Tests.Services
{
    public class BookCommandsServiceTests
    {
        [Fact]
        public void DeleteAuthor_ExistingAuthorId()
        {
            Author author = new Author() { Id = 1 };
            var repositoryMock = new Mock<IBookCommandsRepository>();
            repositoryMock.Setup(r => r.GetAuthor(author.Id)).Returns(author);
            repositoryMock.Setup(r => r.Delete(author));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(repositoryMock.Object, mapper);
            service.DeleteAuthor(author.Id);

            repositoryMock.Verify(r => r.GetAuthor(author.Id), Times.Once);
            repositoryMock.Verify(r => r.Delete(author), Times.Once);
            repositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void DeleteAuthor_NotExistingAuthorId()
        {
            Author author = new Author() { Id = 1 };
            var repositoryMock = new Mock<IBookCommandsRepository>();
            repositoryMock.Setup(r => r.GetAuthor(author.Id)).Returns((Author?)null);
            repositoryMock.Setup(r => r.Delete(author));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(repositoryMock.Object, mapper);
            service.DeleteAuthor(author.Id);

            repositoryMock.Verify(r => r.GetAuthor(author.Id), Times.Once);
            repositoryMock.Verify(r => r.Delete(author), Times.Never);
            repositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void DeleteBook_ExistingBookId()
        {
            Book book = new Book() { Id = 1, Title = "Title" };
            var repositoryMock = new Mock<IBookCommandsRepository>();
            repositoryMock.Setup(r => r.GetBook(book.Id)).Returns(book);
            repositoryMock.Setup(r => r.Delete(book));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(repositoryMock.Object, mapper);
            service.DeleteBook(book.Id);

            repositoryMock.Verify(r => r.GetBook(book.Id), Times.Once);
            repositoryMock.Verify(r => r.Delete(book), Times.Once);
            repositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void DeleteBook_NotExistingBookId()
        {
            Book book = new Book() { Id = 1, Title = "Title" };
            var repositoryMock = new Mock<IBookCommandsRepository>();
            repositoryMock.Setup(r => r.GetBook(book.Id)).Returns((Book?)null);
            repositoryMock.Setup(r => r.Delete(book));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(repositoryMock.Object, mapper);
            service.DeleteBook(book.Id);

            repositoryMock.Verify(r => r.GetBook(book.Id), Times.Once);
            repositoryMock.Verify(r => r.Delete(book), Times.Never);
            repositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void DeleteGenre_ExistingLiteratureGenreId()
        {
            LiteratureGenre genre = new LiteratureGenre() { Id = 1, Name = "Name" };
            var repositoryMock = new Mock<IBookCommandsRepository>();
            repositoryMock.Setup(r => r.GetLiteratureGenre(genre.Id)).Returns(genre);
            repositoryMock.Setup(r => r.Delete(genre));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(repositoryMock.Object, mapper);
            service.DeleteGenre(genre.Id);

            repositoryMock.Verify(r => r.GetLiteratureGenre(genre.Id), Times.Once);
            repositoryMock.Verify(r => r.Delete(genre), Times.Once);
            repositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void DeleteGenre_NotExistingLiteratureGenreId()
        {
            LiteratureGenre genre = new LiteratureGenre() { Id = 1, Name = "Name" };
            var repositoryMock = new Mock<IBookCommandsRepository>();
            repositoryMock.Setup(r => r.GetLiteratureGenre(genre.Id)).Returns((LiteratureGenre?)null);
            repositoryMock.Setup(r => r.Delete(genre));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(repositoryMock.Object, mapper);
            service.DeleteGenre(genre.Id);

            repositoryMock.Verify(r => r.GetLiteratureGenre(genre.Id), Times.Once);
            repositoryMock.Verify(r => r.Delete(genre), Times.Never);
            repositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void DeleteSeries_ExistingBookSeriesId()
        {
            BookSeries series = new BookSeries() { Id = 1, Title = "Title" };
            var repositoryMock = new Mock<IBookCommandsRepository>();
            repositoryMock.Setup(r => r.GetBookSeries(series.Id)).Returns(series);
            repositoryMock.Setup(r => r.Delete(series));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(repositoryMock.Object, mapper);
            service.DeleteSeries(series.Id);

            repositoryMock.Verify(r => r.GetBookSeries(series.Id), Times.Once);
            repositoryMock.Verify(r => r.Delete(series), Times.Once);
            repositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void DeleteSeries_NotExistingBookSeriesId()
        {
            BookSeries series = new BookSeries() { Id = 1, Title = "Title" };
            var repositoryMock = new Mock<IBookCommandsRepository>();
            repositoryMock.Setup(r => r.GetBookSeries(series.Id)).Returns((BookSeries?)null);
            repositoryMock.Setup(r => r.Delete(series));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(repositoryMock.Object, mapper);
            service.DeleteSeries(series.Id);

            repositoryMock.Verify(r => r.GetBookSeries(series.Id), Times.Once);
            repositoryMock.Verify(r => r.Delete(series), Times.Never);
            repositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectAuthorsForBook_ExistingBookIdAndExistingAuthorsIds()
        {
            Author author1 = new Author() { Id = 1 };
            Author author2 = new Author() { Id = 2 };
            Author author3 = new Author() { Id = 3 };
            Book book = new Book()
            {
                Id = 1,
                Title = "Title",
                Authors = new List<Author>() { author1 }
            };
            List<int> authorsIds = new List<int> { author2.Id, author3.Id };
            var repositoryMock = new Mock<IBookCommandsRepository>();
            repositoryMock.Setup(r => r.GetBook(book.Id)).Returns(book);
            repositoryMock.Setup(r => r.GetAuthor(author2.Id)).Returns(author2);
            repositoryMock.Setup(r => r.GetAuthor(author3.Id)).Returns(author3);
            repositoryMock.Setup(r => r.UpdateBookAuthorsRelation(book));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(repositoryMock.Object, mapper);
            service.SelectAuthorsForBook(book.Id, authorsIds);

            repositoryMock.Verify(r => r.GetBook(book.Id), Times.Once());
            repositoryMock.Verify(r => r.GetAuthor(author2.Id), Times.Once());
            repositoryMock.Verify(r => r.GetAuthor(author3.Id), Times.Once());
            book.Authors.Should().HaveCount(authorsIds.Count);
            book.Authors.Should().Contain(author2).And.Contain(author3).And.NotContain(author1);
            repositoryMock.Verify(r => r.UpdateBookAuthorsRelation
            (
                It.Is<Book>(b =>
                    b.Equals(book) &&
                    b.Authors != null && b.Authors.Count == authorsIds.Count &&
                    b.Authors.Contains(author2) && b.Authors.Contains(author3) && !b.Authors.Contains(author1))
            ), Times.Once());
            repositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectAuthorsForBook_ExistingBookIdAndNotExistingAuthorsIds()
        {
            Book book = new Book()
            {
                Id = 1,
                Title = "Title",
                Authors = new List<Author>() { new Author() { Id = 1 } }
            };
            List<int> authorsIds = new List<int> { 2, 3 };
            var repositoryMock = new Mock<IBookCommandsRepository>();
            repositoryMock.Setup(r => r.GetBook(book.Id)).Returns(book);
            repositoryMock.Setup(r => r.GetAuthor(2)).Returns((Author?)null);
            repositoryMock.Setup(r => r.GetAuthor(3)).Returns((Author?)null);
            repositoryMock.Setup(r => r.UpdateBookAuthorsRelation(book));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(repositoryMock.Object, mapper);
            service.SelectAuthorsForBook(book.Id, authorsIds);

            repositoryMock.Verify(r => r.GetBook(book.Id), Times.Once());
            repositoryMock.Verify(r => r.GetAuthor(2), Times.Once());
            repositoryMock.Verify(r => r.GetAuthor(3), Times.Once());
            book.Authors.Should().NotBeNull().And.BeEmpty();
            repositoryMock.Verify(r => r.UpdateBookAuthorsRelation
            (
                It.Is<Book>(b =>
                    b.Equals(book) &&
                    b.Authors != null && b.Authors.Count == 0)
            ), Times.Once());
            repositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectAuthorsForBook_ExistingBookIdAndEmptyList()
        {
            Book book = new Book()
            {
                Id = 1,
                Title = "Title",
                Authors = new List<Author>() { new Author() { Id = 1 } }
            };
            List<int> authorsIds = new List<int>();
            var repositoryMock = new Mock<IBookCommandsRepository>();
            repositoryMock.Setup(r => r.GetBook(book.Id)).Returns(book);
            repositoryMock.Setup(r => r.GetAuthor(It.IsAny<int>()));
            repositoryMock.Setup(r => r.UpdateBookAuthorsRelation(book));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(repositoryMock.Object, mapper);
            service.SelectAuthorsForBook(book.Id, authorsIds);

            repositoryMock.Verify(r => r.GetBook(book.Id), Times.Once());
            repositoryMock.Verify(r => r.GetAuthor(It.IsAny<int>()), Times.Never());
            book.Authors.Should().NotBeNull().And.BeEmpty();
            repositoryMock.Verify(r => r.UpdateBookAuthorsRelation
            (
                It.Is<Book>(b =>
                    b.Equals(book) &&
                    b.Authors != null && b.Authors.Count == 0)
            ), Times.Once());
            repositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectAuthorsForBook_NotExistingBookIdAndAuthorsIds()
        {
            int bookId = 1;
            Author author = new Author { Id = 1 };
            List<int> authorsIds = new List<int> { author.Id };
            var repositoryMock = new Mock<IBookCommandsRepository>();
            repositoryMock.Setup(r => r.GetBook(bookId)).Returns((Book?)null);
            repositoryMock.Setup(r => r.GetAuthor(author.Id)).Returns(author);
            repositoryMock.Setup(r => r.UpdateBookAuthorsRelation(It.IsAny<Book>()));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(repositoryMock.Object, mapper);
            service.SelectAuthorsForBook(bookId, authorsIds);

            repositoryMock.Verify(r => r.GetBook(bookId), Times.Once());
            repositoryMock.Verify(r => r.GetAuthor(It.IsAny<int>()), Times.Never());
            repositoryMock.Verify(r => r.UpdateBookAuthorsRelation(It.IsAny<Book>()), Times.Never());
            repositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectBooksForAuthor_ExistingAuthorIdAndExistingBooksIds()
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
            var repositoryMock = new Mock<IBookCommandsRepository>();
            repositoryMock.Setup(r => r.GetAuthor(author.Id)).Returns(author);
            repositoryMock.Setup(r => r.GetBook(book2.Id)).Returns(book2);
            repositoryMock.Setup(r => r.GetBook(book3.Id)).Returns(book3);
            repositoryMock.Setup(r => r.UpdateAuthorBooksRelation(author));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(repositoryMock.Object, mapper);
            service.SelectBooksForAuthor(author.Id, booksIds);

            repositoryMock.Verify(r => r.GetAuthor(author.Id), Times.Once());
            repositoryMock.Verify(r => r.GetBook(book2.Id), Times.Once());
            repositoryMock.Verify(r => r.GetBook(book3.Id), Times.Once());
            author.Books.Should().HaveCount(booksIds.Count).And.Contain(book2).And.Contain(book3).And.NotContain(book1);
            repositoryMock.Verify(r => r.UpdateAuthorBooksRelation
            (
                It.Is<Author>(a =>
                    a.Equals(author) &&
                    a.Books != null && a.Books.Count == booksIds.Count &&
                    a.Books.Contains(book2) && a.Books.Contains(book3) && !a.Books.Contains(book1))
            ), Times.Once());
            repositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectBooksForAuthor_ExistingAuthorIdAndNotExistingBooksIds()
        {
            var book1 = new Book() { Id = 1, Title = "Title1" };
            var author = new Author()
            {
                Id = 1,
                Books = new List<Book>() { book1 }
            };
            List<int> booksIds = new List<int>() { 2, 3 };
            var repositoryMock = new Mock<IBookCommandsRepository>();
            repositoryMock.Setup(r => r.GetAuthor(author.Id)).Returns(author);
            repositoryMock.Setup(r => r.GetBook(2)).Returns((Book?)null);
            repositoryMock.Setup(r => r.GetBook(3)).Returns((Book?)null);
            repositoryMock.Setup(r => r.UpdateAuthorBooksRelation(author));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(repositoryMock.Object, mapper);
            service.SelectBooksForAuthor(author.Id, booksIds);

            repositoryMock.Verify(r => r.GetAuthor(author.Id), Times.Once());
            repositoryMock.Verify(r => r.GetBook(2), Times.Once());
            repositoryMock.Verify(r => r.GetBook(3), Times.Once());
            author.Books.Should().NotBeNull().And.BeEmpty();
            repositoryMock.Verify(r => r.UpdateAuthorBooksRelation
            (
                It.Is<Author>(a =>
                    a.Equals(author) &&
                    a.Books != null && a.Books.Count == 0)
            ), Times.Once());
            repositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectBooksForAuthor_ExistingAuthorIdAndEmptyList()
        {
            var book1 = new Book() { Id = 1, Title = "Title1" };
            var author = new Author()
            {
                Id = 1,
                Books = new List<Book>() { book1 }
            };
            List<int> booksIds = new List<int>();
            var repositoryMock = new Mock<IBookCommandsRepository>();
            repositoryMock.Setup(r => r.GetAuthor(author.Id)).Returns(author);
            repositoryMock.Setup(r => r.GetBook(It.IsAny<int>()));
            repositoryMock.Setup(r => r.UpdateAuthorBooksRelation(author));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(repositoryMock.Object, mapper);
            service.SelectBooksForAuthor(author.Id, booksIds);

            repositoryMock.Verify(r => r.GetAuthor(author.Id), Times.Once());
            repositoryMock.Verify(r => r.GetBook(It.IsAny<int>()), Times.Never());
            author.Books.Should().NotBeNull().And.BeEmpty();
            repositoryMock.Verify(r => r.UpdateAuthorBooksRelation
            (
                It.Is<Author>(a =>
                    a.Equals(author) &&
                    a.Books != null && a.Books.Count == 0)
            ), Times.Once());
            repositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectBooksForAuthor_NotExistingAuthorIdAndBooksIds()
        {
            int authorId = 1;
            var book1 = new Book() { Id = 1, Title = "Title1" };
            List<int> booksIds = new List<int>() { book1.Id };
            var repositoryMock = new Mock<IBookCommandsRepository>();
            repositoryMock.Setup(r => r.GetAuthor(authorId)).Returns((Author?)null);
            repositoryMock.Setup(r => r.GetBook(book1.Id)).Returns(book1);
            repositoryMock.Setup(r => r.UpdateAuthorBooksRelation(It.IsAny<Author>()));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(repositoryMock.Object, mapper);
            service.SelectBooksForAuthor(authorId, booksIds);

            repositoryMock.Verify(r => r.GetAuthor(authorId), Times.Once());
            repositoryMock.Verify(r => r.GetBook(book1.Id), Times.Never());
            repositoryMock.Verify(r => r.UpdateAuthorBooksRelation(It.IsAny<Author>()), Times.Never());
            repositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectBooksForGenre_ExistingLiteratureGenreIdAndExistingBooksIds()
        {
            var book1 = new Book() { Id = 1, Title = "Title1" };
            var book2 = new Book() { Id = 2, Title = "Title2" };
            var book3 = new Book() { Id = 3, Title = "Title3" };
            var genre = new LiteratureGenre()
            {
                Id = 1,
                Name = "Name",
                Books = new List<Book>() { book1 }
            };
            List<int> booksIds = new List<int>() { book2.Id, book3.Id };
            var repositoryMock = new Mock<IBookCommandsRepository>();
            repositoryMock.Setup(r => r.GetLiteratureGenre(genre.Id)).Returns(genre);
            repositoryMock.Setup(r => r.GetBook(book2.Id)).Returns(book2);
            repositoryMock.Setup(r => r.GetBook(book3.Id)).Returns(book3);
            repositoryMock.Setup(r => r.UpdateLiteratureGenreBooksRelation(genre));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(repositoryMock.Object, mapper);
            service.SelectBooksForGenre(genre.Id, booksIds);

            repositoryMock.Verify(r => r.GetLiteratureGenre(genre.Id), Times.Once());
            repositoryMock.Verify(r => r.GetBook(book2.Id), Times.Once());
            repositoryMock.Verify(r => r.GetBook(book3.Id), Times.Once());
            genre.Books.Should().HaveCount(booksIds.Count).And.Contain(book2).And.Contain(book3).And.NotContain(book1);
            repositoryMock.Verify(r => r.UpdateLiteratureGenreBooksRelation
            (
                It.Is<LiteratureGenre>(g =>
                    g.Equals(genre) &&
                    g.Books != null && g.Books.Count == booksIds.Count &&
                    g.Books.Contains(book2) && g.Books.Contains(book3) && !g.Books.Contains(book1))
            ), Times.Once());
            repositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectBooksForGenre_ExistingLiteratureGenreIdAndNotExistingBooksIds()
        {
            var book1 = new Book() { Id = 1, Title = "Title1" };
            var genre = new LiteratureGenre()
            {
                Id = 1,
                Name = "Name",
                Books = new List<Book>() { book1 }
            };
            List<int> booksIds = new List<int>() { 2, 3 };
            var repositoryMock = new Mock<IBookCommandsRepository>();
            repositoryMock.Setup(r => r.GetLiteratureGenre(genre.Id)).Returns(genre);
            repositoryMock.Setup(r => r.GetBook(2)).Returns((Book?)null);
            repositoryMock.Setup(r => r.GetBook(3)).Returns((Book?)null);
            repositoryMock.Setup(r => r.UpdateLiteratureGenreBooksRelation(genre));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(repositoryMock.Object, mapper);
            service.SelectBooksForGenre(genre.Id, booksIds);

            repositoryMock.Verify(r => r.GetLiteratureGenre(genre.Id), Times.Once());
            repositoryMock.Verify(r => r.GetBook(2), Times.Once());
            repositoryMock.Verify(r => r.GetBook(3), Times.Once());
            genre.Books.Should().NotBeNull().And.BeEmpty();
            repositoryMock.Verify(r => r.UpdateLiteratureGenreBooksRelation
            (
                It.Is<LiteratureGenre>(g =>
                    g.Equals(genre) &&
                    g.Books != null && g.Books.Count == 0)
            ), Times.Once());
            repositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectBooksForGenre_ExistingLiteratureGenreIdAndEmptyList()
        {
            var book1 = new Book() { Id = 1, Title = "Title1" };
            var genre = new LiteratureGenre()
            {
                Id = 1,
                Name = "Name",
                Books = new List<Book>() { book1 }
            };
            List<int> booksIds = new List<int>();
            var repositoryMock = new Mock<IBookCommandsRepository>();
            repositoryMock.Setup(r => r.GetLiteratureGenre(genre.Id)).Returns(genre);
            repositoryMock.Setup(r => r.GetBook(It.IsAny<int>()));
            repositoryMock.Setup(r => r.UpdateLiteratureGenreBooksRelation(genre));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(repositoryMock.Object, mapper);
            service.SelectBooksForGenre(genre.Id, booksIds);

            repositoryMock.Verify(r => r.GetLiteratureGenre(genre.Id), Times.Once());
            repositoryMock.Verify(r => r.GetBook(It.IsAny<int>()), Times.Never());
            genre.Books.Should().NotBeNull().And.BeEmpty();
            repositoryMock.Verify(r => r.UpdateLiteratureGenreBooksRelation
            (
                It.Is<LiteratureGenre>(g =>
                    g.Equals(genre) &&
                    g.Books != null && g.Books.Count == 0)
            ), Times.Once());
            repositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectBooksForGenre_NotExistingLiteratureGenreIdAndBooksIds()
        {
            int genreId = 1;
            var book1 = new Book() { Id = 1, Title = "Title1" };
            List<int> booksIds = new List<int>() { book1.Id };
            var repositoryMock = new Mock<IBookCommandsRepository>();
            repositoryMock.Setup(r => r.GetLiteratureGenre(genreId)).Returns((LiteratureGenre?)null);
            repositoryMock.Setup(r => r.GetBook(book1.Id)).Returns(book1);
            repositoryMock.Setup(r => r.UpdateLiteratureGenreBooksRelation(It.IsAny<LiteratureGenre>()));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(repositoryMock.Object, mapper);
            service.SelectBooksForGenre(genreId, booksIds);

            repositoryMock.Verify(r => r.GetLiteratureGenre(genreId), Times.Once());
            repositoryMock.Verify(r => r.GetBook(book1.Id), Times.Never());
            repositoryMock.Verify(r => r.UpdateLiteratureGenreBooksRelation(It.IsAny<LiteratureGenre>()), Times.Never());
            repositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectBooksForSeries_ExistingBookSeriesIdAndExistingBooksIds()
        {
            var book1 = new Book() { Id = 1, Title = "Title1" };
            var book2 = new Book() { Id = 2, Title = "Title2" };
            var book3 = new Book() { Id = 3, Title = "Title3" };
            var series = new BookSeries()
            {
                Id = 1,
                Title = "Title",
                Books = new List<Book>() { book1 }
            };
            List<int> booksIds = new List<int>() { book2.Id, book3.Id };
            var repositoryMock = new Mock<IBookCommandsRepository>();
            repositoryMock.Setup(r => r.GetBookSeries(series.Id)).Returns(series);
            repositoryMock.Setup(r => r.GetBook(book2.Id)).Returns(book2);
            repositoryMock.Setup(r => r.GetBook(book3.Id)).Returns(book3);
            repositoryMock.Setup(r => r.UpdateBookSeriesBooksRelation(series));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(repositoryMock.Object, mapper);
            service.SelectBooksForSeries(series.Id, booksIds);

            repositoryMock.Verify(r => r.GetBookSeries(series.Id), Times.Once());
            repositoryMock.Verify(r => r.GetBook(book2.Id), Times.Once());
            repositoryMock.Verify(r => r.GetBook(book3.Id), Times.Once());
            series.Books.Should().HaveCount(booksIds.Count).And.Contain(book2).And.Contain(book3).And.NotContain(book1);
            repositoryMock.Verify(r => r.UpdateBookSeriesBooksRelation
            (
                It.Is<BookSeries>(s =>
                    s.Equals(series) &&
                    s.Books != null && s.Books.Count == booksIds.Count &&
                    s.Books.Contains(book2) && s.Books.Contains(book3) && !s.Books.Contains(book1))
            ), Times.Once());
            repositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectBooksForSeries_ExistingBookSeriesIdAndNotExistingBooksIds()
        {
            var book1 = new Book() { Id = 1, Title = "Title1" };
            var series = new BookSeries()
            {
                Id = 1,
                Title = "Title",
                Books = new List<Book>() { book1 }
            };
            List<int> booksIds = new List<int>() { 2, 3 };
            var repositoryMock = new Mock<IBookCommandsRepository>();
            repositoryMock.Setup(r => r.GetBookSeries(series.Id)).Returns(series);
            repositoryMock.Setup(r => r.GetBook(2)).Returns((Book?)null);
            repositoryMock.Setup(r => r.GetBook(3)).Returns((Book?)null);
            repositoryMock.Setup(r => r.UpdateBookSeriesBooksRelation(series));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(repositoryMock.Object, mapper);
            service.SelectBooksForSeries(series.Id, booksIds);

            repositoryMock.Verify(r => r.GetBookSeries(series.Id), Times.Once());
            repositoryMock.Verify(r => r.GetBook(2), Times.Once());
            repositoryMock.Verify(r => r.GetBook(3), Times.Once());
            series.Books.Should().NotBeNull().And.BeEmpty();
            repositoryMock.Verify(r => r.UpdateBookSeriesBooksRelation
            (
                It.Is<BookSeries>(s =>
                    s.Equals(series) &&
                    s.Books != null && s.Books.Count == 0)
            ), Times.Once());
            repositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectBooksForSeries_ExistingBookSeriesIdAndEmptyList()
        {
            var book1 = new Book() { Id = 1, Title = "Title1" };
            var series = new BookSeries()
            {
                Id = 1,
                Title = "Title",
                Books = new List<Book>() { book1 }
            };
            List<int> booksIds = new List<int>();
            var repositoryMock = new Mock<IBookCommandsRepository>();
            repositoryMock.Setup(r => r.GetBookSeries(series.Id)).Returns(series);
            repositoryMock.Setup(r => r.GetBook(It.IsAny<int>()));
            repositoryMock.Setup(r => r.UpdateBookSeriesBooksRelation(series));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(repositoryMock.Object, mapper);
            service.SelectBooksForSeries(series.Id, booksIds);

            repositoryMock.Verify(r => r.GetBookSeries(series.Id), Times.Once());
            repositoryMock.Verify(r => r.GetBook(It.IsAny<int>()), Times.Never());
            series.Books.Should().NotBeNull().And.BeEmpty();
            repositoryMock.Verify(r => r.UpdateBookSeriesBooksRelation
            (
                It.Is<BookSeries>(s =>
                    s.Equals(series) &&
                    s.Books != null && s.Books.Count == 0)
            ), Times.Once());
            repositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectBooksForSeries_NotExistingBookSeriesIdAndBooksIds()
        {
            int seriesId = 1;
            var book1 = new Book() { Id = 1, Title = "Title1" };
            List<int> booksIds = new List<int>() { book1.Id };
            var repositoryMock = new Mock<IBookCommandsRepository>();
            repositoryMock.Setup(r => r.GetBookSeries(seriesId)).Returns((BookSeries?)null);
            repositoryMock.Setup(r => r.GetBook(book1.Id)).Returns(book1);
            repositoryMock.Setup(r => r.UpdateBookSeriesBooksRelation(It.IsAny<BookSeries>()));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(repositoryMock.Object, mapper);
            service.SelectBooksForSeries(seriesId, booksIds);

            repositoryMock.Verify(r => r.GetBookSeries(seriesId), Times.Once());
            repositoryMock.Verify(r => r.GetBook(book1.Id), Times.Never());
            repositoryMock.Verify(r => r.UpdateBookSeriesBooksRelation(It.IsAny<BookSeries>()), Times.Never());
            repositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectGenresForBook_ExistingBookIdAndExistingLiteratureGenresIds()
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
            List<int> genresIds = new List<int>() { genre2.Id, genre3.Id };
            var repositoryMock = new Mock<IBookCommandsRepository>();
            repositoryMock.Setup(r => r.GetBook(book.Id)).Returns(book);
            repositoryMock.Setup(r => r.GetLiteratureGenre(genre2.Id)).Returns(genre2);
            repositoryMock.Setup(r => r.GetLiteratureGenre(genre3.Id)).Returns(genre3);
            repositoryMock.Setup(r => r.UpdateBookGenresRelation(book));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(repositoryMock.Object, mapper);
            service.SelectGenresForBook(book.Id, genresIds);

            repositoryMock.Verify(r => r.GetBook(book.Id), Times.Once());
            repositoryMock.Verify(r => r.GetLiteratureGenre(genre2.Id), Times.Once());
            repositoryMock.Verify(r => r.GetLiteratureGenre(genre3.Id), Times.Once());
            book.Genres.Should().HaveCount(genresIds.Count).And.Contain(genre2).And.Contain(genre3).And.NotContain(genre1);
            repositoryMock.Verify(r => r.UpdateBookGenresRelation
            (
                It.Is<Book>(b =>
                    b.Equals(book) &&
                    b.Genres != null && b.Genres.Count == genresIds.Count &&
                    b.Genres.Contains(genre2) && b.Genres.Contains(genre3) && !b.Genres.Contains(genre1))
            ), Times.Once());
            repositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectGenresForBook_ExistingBookIdAndNotExistingLiteratureGenresIds()
        {
            var genre1 = new LiteratureGenre() { Id = 1, Name = "Name1" };
            var book = new Book()
            {
                Id = 1,
                Title = "Title",
                Genres = new List<LiteratureGenre>() { genre1 }
            };
            List<int> genresIds = new List<int>() { 2, 3 };
            var repositoryMock = new Mock<IBookCommandsRepository>();
            repositoryMock.Setup(r => r.GetBook(book.Id)).Returns(book);
            repositoryMock.Setup(r => r.GetLiteratureGenre(2)).Returns((LiteratureGenre?)null);
            repositoryMock.Setup(r => r.GetLiteratureGenre(3)).Returns((LiteratureGenre?)null);
            repositoryMock.Setup(r => r.UpdateBookGenresRelation(book));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(repositoryMock.Object, mapper);
            service.SelectGenresForBook(book.Id, genresIds);

            repositoryMock.Verify(r => r.GetBook(book.Id), Times.Once());
            repositoryMock.Verify(r => r.GetLiteratureGenre(2), Times.Once());
            repositoryMock.Verify(r => r.GetLiteratureGenre(3), Times.Once());
            book.Genres.Should().NotBeNull().And.BeEmpty();
            repositoryMock.Verify(r => r.UpdateBookGenresRelation
            (
                It.Is<Book>(b =>
                    b.Equals(book) &&
                    b.Genres != null && b.Genres.Count == 0)
            ), Times.Once());
            repositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectGenresForBook_ExistingBookIdAndEmptyList()
        {
            var genre1 = new LiteratureGenre() { Id = 1, Name = "Name1" };
            var book = new Book()
            {
                Id = 1,
                Title = "Title",
                Genres = new List<LiteratureGenre>() { genre1 }
            };
            List<int> genresIds = new List<int>();
            var repositoryMock = new Mock<IBookCommandsRepository>();
            repositoryMock.Setup(r => r.GetBook(book.Id)).Returns(book);
            repositoryMock.Setup(r => r.GetLiteratureGenre(It.IsAny<int>()));
            repositoryMock.Setup(r => r.UpdateBookGenresRelation(book));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(repositoryMock.Object, mapper);
            service.SelectGenresForBook(book.Id, genresIds);

            repositoryMock.Verify(r => r.GetBook(book.Id), Times.Once());
            repositoryMock.Verify(r => r.GetLiteratureGenre(It.IsAny<int>()), Times.Never());
            book.Genres.Should().NotBeNull().And.BeEmpty();
            repositoryMock.Verify(r => r.UpdateBookGenresRelation
            (
                It.Is<Book>(b =>
                    b.Equals(book) &&
                    b.Genres != null && b.Genres.Count == 0)
            ), Times.Once());
            repositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectGenresForBook_NotExistingBookIdAndLiteratureGenresIds()
        {
            int bookId = 1;
            var genre1 = new LiteratureGenre() { Id = 1, Name = "Name1" };
            List<int> genresIds = new List<int>() { genre1.Id };
            var repositoryMock = new Mock<IBookCommandsRepository>();
            repositoryMock.Setup(r => r.GetBook(bookId)).Returns((Book?)null);
            repositoryMock.Setup(r => r.GetLiteratureGenre(genre1.Id)).Returns(genre1);
            repositoryMock.Setup(r => r.UpdateBookGenresRelation(It.IsAny<Book>()));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(repositoryMock.Object, mapper);
            service.SelectGenresForBook(bookId, genresIds);

            repositoryMock.Verify(r => r.GetBook(bookId), Times.Once());
            repositoryMock.Verify(r => r.GetLiteratureGenre(genre1.Id), Times.Never());
            repositoryMock.Verify(r => r.UpdateBookGenresRelation(It.IsAny<Book>()), Times.Never());
            repositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectSeriesForBook_ExistingBookIdAndExistingBookSeriesId()
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
            var repositoryMock = new Mock<IBookCommandsRepository>();
            repositoryMock.Setup(r => r.GetBook(book.Id)).Returns(book);
            repositoryMock.Setup(r => r.GetBookSeries(series2.Id)).Returns(series2);
            repositoryMock.Setup(r => r.UpdateBookSeriesRelation(book));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(repositoryMock.Object, mapper);
            service.SelectSeriesForBook(book.Id, series2.Id);

            repositoryMock.Verify(r => r.GetBook(book.Id), Times.Once());
            repositoryMock.Verify(r => r.GetBookSeries(series2.Id), Times.Once());
            book.Series.Should().Be(series2);
            book.SeriesId.Should().Be(series2.Id);
            repositoryMock.Verify(r => r.UpdateBookSeriesRelation
            (
                It.Is<Book>(b =>
                    b.Equals(book) &&
                    b.Series == series2 &&
                    b.SeriesId == series2.Id)
            ), Times.Once());
            repositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectSeriesForBook_ExistingBookIdAndNotExistingBookSeriesId()
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
            var repositoryMock = new Mock<IBookCommandsRepository>();
            repositoryMock.Setup(r => r.GetBook(book.Id)).Returns(book);
            repositoryMock.Setup(r => r.GetBookSeries(newSeriesId)).Returns((BookSeries?)null);
            repositoryMock.Setup(r => r.UpdateBookSeriesRelation(book));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(repositoryMock.Object, mapper);
            service.SelectSeriesForBook(book.Id, newSeriesId);

            repositoryMock.Verify(r => r.GetBook(book.Id), Times.Once());
            repositoryMock.Verify(r => r.GetBookSeries(newSeriesId), Times.Once());
            book.Series.Should().BeNull();
            book.SeriesId.Should().BeNull();
            repositoryMock.Verify(r => r.UpdateBookSeriesRelation
            (
                It.Is<Book>(b =>
                    b.Equals(book) &&
                    b.Series == null &&
                    b.SeriesId == null)
            ), Times.Once());
            repositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectSeriesForBook_ExistingBookIdAndNull()
        {
            var series1 = new BookSeries() { Id = 1, Title = "Title1" };
            var book = new Book()
            {
                Id = 1,
                Title = "Title",
                Series = series1,
                SeriesId = series1.Id
            };
            var repositoryMock = new Mock<IBookCommandsRepository>();
            repositoryMock.Setup(r => r.GetBook(book.Id)).Returns(book);
            repositoryMock.Setup(r => r.GetBookSeries(It.IsAny<int>()));
            repositoryMock.Setup(r => r.UpdateBookSeriesRelation(book));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(repositoryMock.Object, mapper);
            service.SelectSeriesForBook(book.Id, null);

            repositoryMock.Verify(r => r.GetBook(book.Id), Times.Once());
            repositoryMock.Verify(r => r.GetBookSeries(It.IsAny<int>()), Times.Never());
            book.Series.Should().BeNull();
            book.SeriesId.Should().BeNull();
            repositoryMock.Verify(r => r.UpdateBookSeriesRelation
            (
                It.Is<Book>(b =>
                    b.Equals(book) &&
                    b.Series == null &&
                    b.SeriesId == null)
            ), Times.Once());
            repositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectSeriesForBook_NotExistingBookIdAndBookSeriesId()
        {
            int bookId = 1;
            var series1 = new BookSeries() { Id = 1, Title = "Title1" };
            var repositoryMock = new Mock<IBookCommandsRepository>();
            repositoryMock.Setup(r => r.GetBook(bookId)).Returns((Book?)null);
            repositoryMock.Setup(r => r.GetBookSeries(series1.Id)).Returns(series1);
            repositoryMock.Setup(r => r.UpdateBookSeriesRelation(It.IsAny<Book>()));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(repositoryMock.Object, mapper);
            service.SelectSeriesForBook(bookId, series1.Id);

            repositoryMock.Verify(r => r.GetBook(bookId), Times.Once());
            repositoryMock.Verify(r => r.GetBookSeries(series1.Id), Times.Never());
            repositoryMock.Verify(r => r.UpdateBookSeriesRelation(It.IsAny<Book>()), Times.Never());
            repositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void UpsertAuthor_NewAuthor_NewId()
        {
            AuthorVM author = new AuthorVM() { Name = "Name", LastName = "Last Name" };
            //var mapMock = Mock.Of<MappingExtensions>()
        }
    }
}