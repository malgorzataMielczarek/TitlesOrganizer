// Ignore Spelling: Upsert

using AutoMapper;
using FluentAssertions;
using Moq;
using TitlesOrganizer.Application.Services;
using TitlesOrganizer.Domain.Interfaces;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Tests.Services
{
    public class LiteratureGenreServiceTests
    {
        [Fact]
        public void Delete_ExistingLiteratureGenreId()
        {
            LiteratureGenre genre = new LiteratureGenre() { Id = 1, Name = "Name" };
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookQueriesRepository>();
            queriesRepo.Setup(r => r.GetLiteratureGenre(genre.Id)).Returns(genre);
            commandsRepo.Setup(r => r.Delete(genre));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new LiteratureGenreService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.Delete(genre.Id);

            queriesRepo.Verify(r => r.GetLiteratureGenre(genre.Id), Times.Once);
            commandsRepo.Verify(r => r.Delete(genre), Times.Once);
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void Delete_NotExistingLiteratureGenreId()
        {
            LiteratureGenre genre = new LiteratureGenre() { Id = 1, Name = "Name" };
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookQueriesRepository>();
            queriesRepo.Setup(r => r.GetLiteratureGenre(genre.Id)).Returns((LiteratureGenre?)null);
            commandsRepo.Setup(r => r.Delete(genre));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new LiteratureGenreService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.Delete(genre.Id);

            queriesRepo.Verify(r => r.GetLiteratureGenre(genre.Id), Times.Once);
            commandsRepo.Verify(r => r.Delete(genre), Times.Never);
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectForBook_ExistingBookIdAndExistingLiteratureGenresIds()
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
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookQueriesRepository>();
            queriesRepo.Setup(r => r.GetBook(book.Id)).Returns(book);
            queriesRepo.Setup(r => r.GetLiteratureGenre(genre2.Id)).Returns(genre2);
            queriesRepo.Setup(r => r.GetLiteratureGenre(genre3.Id)).Returns(genre3);
            commandsRepo.Setup(r => r.UpdateBookGenresRelation(book));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new LiteratureGenreService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectForBook(book.Id, genresIds);

            queriesRepo.Verify(r => r.GetBook(book.Id), Times.Once());
            queriesRepo.Verify(r => r.GetLiteratureGenre(genre2.Id), Times.Once());
            queriesRepo.Verify(r => r.GetLiteratureGenre(genre3.Id), Times.Once());
            book.Genres.Should().HaveCount(genresIds.Count).And.Contain(genre2).And.Contain(genre3).And.NotContain(genre1);
            commandsRepo.Verify(r => r.UpdateBookGenresRelation
            (
                It.Is<Book>(b =>
                    b.Equals(book) &&
                    b.Genres != null && b.Genres.Count == genresIds.Count &&
                    b.Genres.Contains(genre2) && b.Genres.Contains(genre3) && !b.Genres.Contains(genre1))
            ), Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectForBook_ExistingBookIdAndNotExistingLiteratureGenresIds()
        {
            var genre1 = new LiteratureGenre() { Id = 1, Name = "Name1" };
            var book = new Book()
            {
                Id = 1,
                Title = "Title",
                Genres = new List<LiteratureGenre>() { genre1 }
            };
            List<int> genresIds = new List<int>() { 2, 3 };
            var commendsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookQueriesRepository>();
            queriesRepo.Setup(r => r.GetBook(book.Id)).Returns(book);
            queriesRepo.Setup(r => r.GetLiteratureGenre(2)).Returns((LiteratureGenre?)null);
            queriesRepo.Setup(r => r.GetLiteratureGenre(3)).Returns((LiteratureGenre?)null);
            commendsRepo.Setup(r => r.UpdateBookGenresRelation(book));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new LiteratureGenreService(commendsRepo.Object, queriesRepo.Object, mapper);
            service.SelectForBook(book.Id, genresIds);

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
        public void SelectForBook_ExistingBookIdAndEmptyList()
        {
            var genre1 = new LiteratureGenre() { Id = 1, Name = "Name1" };
            var book = new Book()
            {
                Id = 1,
                Title = "Title",
                Genres = new List<LiteratureGenre>() { genre1 }
            };
            List<int> genresIds = new List<int>();
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookQueriesRepository>();
            queriesRepo.Setup(r => r.GetBook(book.Id)).Returns(book);
            queriesRepo.Setup(r => r.GetLiteratureGenre(It.IsAny<int>()));
            commandsRepo.Setup(r => r.UpdateBookGenresRelation(book));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new LiteratureGenreService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectForBook(book.Id, genresIds);

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
        public void SelectForBook_NotExistingBookIdAndLiteratureGenresIds()
        {
            int bookId = 1;
            var genre1 = new LiteratureGenre() { Id = 1, Name = "Name1" };
            List<int> genresIds = new List<int>() { genre1.Id };
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookQueriesRepository>();
            queriesRepo.Setup(r => r.GetBook(bookId)).Returns((Book?)null);
            queriesRepo.Setup(r => r.GetLiteratureGenre(genre1.Id)).Returns(genre1);
            commandsRepo.Setup(r => r.UpdateBookGenresRelation(It.IsAny<Book>()));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new LiteratureGenreService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectForBook(bookId, genresIds);

            queriesRepo.Verify(r => r.GetBook(bookId), Times.Once());
            queriesRepo.Verify(r => r.GetLiteratureGenre(genre1.Id), Times.Never());
            commandsRepo.Verify(r => r.UpdateBookGenresRelation(It.IsAny<Book>()), Times.Never());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }
    }
}