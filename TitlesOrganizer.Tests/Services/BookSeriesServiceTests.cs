// Ignore Spelling: Upsert

using AutoMapper;
using FluentAssertions;
using Moq;
using TitlesOrganizer.Application.Services;
using TitlesOrganizer.Domain.Interfaces;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Tests.Services
{
    public class BookSeriesServiceTests
    {
        [Fact]
        public void Delete_ExistingBookSeriesId()
        {
            BookSeries series = new BookSeries() { Id = 1, Title = "Title" };
            var commandsRepo = new Mock<IBookSeriesCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBookSeries(series.Id)).Returns(series);
            commandsRepo.Setup(r => r.Delete(series));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookSeriesService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.Delete(series.Id);

            queriesRepo.Verify(r => r.GetBookSeries(series.Id), Times.Once);
            commandsRepo.Verify(r => r.Delete(series), Times.Once);
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void Delete_NotExistingBookSeriesId()
        {
            BookSeries series = new BookSeries() { Id = 1, Title = "Title" };
            var commandsRepo = new Mock<IBookSeriesCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBookSeries(series.Id)).Returns((BookSeries?)null);
            commandsRepo.Setup(r => r.Delete(series));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookSeriesService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.Delete(series.Id);

            queriesRepo.Verify(r => r.GetBookSeries(series.Id), Times.Once);
            commandsRepo.Verify(r => r.Delete(series), Times.Never);
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
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
                Books = new List<Book>() { book1 }
            };
            List<int> booksIds = new List<int>() { book2.Id, book3.Id };
            var commandsRepo = new Mock<IBookSeriesCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBookSeries(series.Id)).Returns(series);
            queriesRepo.Setup(r => r.GetBook(book2.Id)).Returns(book2);
            queriesRepo.Setup(r => r.GetBook(book3.Id)).Returns(book3);
            commandsRepo.Setup(r => r.UpdateBookSeriesBooksRelation(series));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookSeriesService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectBooks(series.Id, booksIds);

            queriesRepo.Verify(r => r.GetBookSeries(series.Id), Times.Once());
            queriesRepo.Verify(r => r.GetBook(book2.Id), Times.Once());
            queriesRepo.Verify(r => r.GetBook(book3.Id), Times.Once());
            series.Books.Should().HaveCount(booksIds.Count).And.Contain(book2).And.Contain(book3).And.NotContain(book1);
            commandsRepo.Verify(r => r.UpdateBookSeriesBooksRelation
            (
                It.Is<BookSeries>(s =>
                    s.Equals(series) &&
                    s.Books != null && s.Books.Count == booksIds.Count &&
                    s.Books.Contains(book2) && s.Books.Contains(book3) && !s.Books.Contains(book1))
            ), Times.Once());
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
                Books = new List<Book>() { book1 }
            };
            List<int> booksIds = new List<int>() { 2, 3 };
            var commandsRepo = new Mock<IBookSeriesCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBookSeries(series.Id)).Returns(series);
            queriesRepo.Setup(r => r.GetBook(2)).Returns((Book?)null);
            queriesRepo.Setup(r => r.GetBook(3)).Returns((Book?)null);
            commandsRepo.Setup(r => r.UpdateBookSeriesBooksRelation(series));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookSeriesService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectBooks(series.Id, booksIds);

            queriesRepo.Verify(r => r.GetBookSeries(series.Id), Times.Once());
            queriesRepo.Verify(r => r.GetBook(2), Times.Once());
            queriesRepo.Verify(r => r.GetBook(3), Times.Once());
            series.Books.Should().NotBeNull().And.BeEmpty();
            commandsRepo.Verify(r => r.UpdateBookSeriesBooksRelation
            (
                It.Is<BookSeries>(s =>
                    s.Equals(series) &&
                    s.Books != null && s.Books.Count == 0)
            ), Times.Once());
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
                Books = new List<Book>() { book1 }
            };
            List<int> booksIds = new List<int>();
            var commandsRepo = new Mock<IBookSeriesCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBookSeries(series.Id)).Returns(series);
            queriesRepo.Setup(r => r.GetBook(It.IsAny<int>()));
            commandsRepo.Setup(r => r.UpdateBookSeriesBooksRelation(series));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookSeriesService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectBooks(series.Id, booksIds);

            queriesRepo.Verify(r => r.GetBookSeries(series.Id), Times.Once());
            queriesRepo.Verify(r => r.GetBook(It.IsAny<int>()), Times.Never());
            series.Books.Should().NotBeNull().And.BeEmpty();
            commandsRepo.Verify(r => r.UpdateBookSeriesBooksRelation
            (
                It.Is<BookSeries>(s =>
                    s.Equals(series) &&
                    s.Books != null && s.Books.Count == 0)
            ), Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectBooks_NotExistingBookSeriesIdAndBooksIds()
        {
            int seriesId = 1;
            var book1 = new Book() { Id = 1, Title = "Title1" };
            List<int> booksIds = new List<int>() { book1.Id };
            var commandsRepo = new Mock<IBookSeriesCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBookSeries(seriesId)).Returns((BookSeries?)null);
            queriesRepo.Setup(r => r.GetBook(book1.Id)).Returns(book1);
            commandsRepo.Setup(r => r.UpdateBookSeriesBooksRelation(It.IsAny<BookSeries>()));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookSeriesService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectBooks(seriesId, booksIds);

            queriesRepo.Verify(r => r.GetBookSeries(seriesId), Times.Once());
            queriesRepo.Verify(r => r.GetBook(book1.Id), Times.Never());
            commandsRepo.Verify(r => r.UpdateBookSeriesBooksRelation(It.IsAny<BookSeries>()), Times.Never());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }
    }
}