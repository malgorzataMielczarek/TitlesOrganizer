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
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookQueriesRepository>();
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
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookQueriesRepository>();
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
        public void SelectForBook_ExistingBookIdAndExistingBookSeriesId()
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
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookQueriesRepository>();
            queriesRepo.Setup(r => r.GetBook(book.Id)).Returns(book);
            queriesRepo.Setup(r => r.GetBookSeries(series2.Id)).Returns(series2);
            commandsRepo.Setup(r => r.UpdateBookSeriesRelation(book));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookSeriesService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectForBook(book.Id, series2.Id);

            queriesRepo.Verify(r => r.GetBook(book.Id), Times.Once());
            queriesRepo.Verify(r => r.GetBookSeries(series2.Id), Times.Once());
            book.Series.Should().Be(series2);
            book.SeriesId.Should().Be(series2.Id);
            commandsRepo.Verify(r => r.UpdateBookSeriesRelation
            (
                It.Is<Book>(b =>
                    b.Equals(book) &&
                    b.Series == series2 &&
                    b.SeriesId == series2.Id)
            ), Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectForBook_ExistingBookIdAndNotExistingBookSeriesId()
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
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookQueriesRepository>();
            queriesRepo.Setup(r => r.GetBook(book.Id)).Returns(book);
            queriesRepo.Setup(r => r.GetBookSeries(newSeriesId)).Returns((BookSeries?)null);
            commandsRepo.Setup(r => r.UpdateBookSeriesRelation(book));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookSeriesService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectForBook(book.Id, newSeriesId);

            queriesRepo.Verify(r => r.GetBook(book.Id), Times.Once());
            queriesRepo.Verify(r => r.GetBookSeries(newSeriesId), Times.Once());
            book.Series.Should().BeNull();
            book.SeriesId.Should().BeNull();
            commandsRepo.Verify(r => r.UpdateBookSeriesRelation
            (
                It.Is<Book>(b =>
                    b.Equals(book) &&
                    b.Series == null &&
                    b.SeriesId == null)
            ), Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectForBook_ExistingBookIdAndNull()
        {
            var series1 = new BookSeries() { Id = 1, Title = "Title1" };
            var book = new Book()
            {
                Id = 1,
                Title = "Title",
                Series = series1,
                SeriesId = series1.Id
            };
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookQueriesRepository>();
            queriesRepo.Setup(r => r.GetBook(book.Id)).Returns(book);
            queriesRepo.Setup(r => r.GetBookSeries(It.IsAny<int>()));
            commandsRepo.Setup(r => r.UpdateBookSeriesRelation(book));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookSeriesService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectForBook(book.Id, null);

            queriesRepo.Verify(r => r.GetBook(book.Id), Times.Once());
            queriesRepo.Verify(r => r.GetBookSeries(It.IsAny<int>()), Times.Never());
            book.Series.Should().BeNull();
            book.SeriesId.Should().BeNull();
            commandsRepo.Verify(r => r.UpdateBookSeriesRelation
            (
                It.Is<Book>(b =>
                    b.Equals(book) &&
                    b.Series == null &&
                    b.SeriesId == null)
            ), Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectForBook_NotExistingBookIdAndBookSeriesId()
        {
            int bookId = 1;
            var series1 = new BookSeries() { Id = 1, Title = "Title1" };
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookQueriesRepository>();
            queriesRepo.Setup(r => r.GetBook(bookId)).Returns((Book?)null);
            queriesRepo.Setup(r => r.GetBookSeries(series1.Id)).Returns(series1);
            commandsRepo.Setup(r => r.UpdateBookSeriesRelation(It.IsAny<Book>()));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookSeriesService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectForBook(bookId, series1.Id);

            queriesRepo.Verify(r => r.GetBook(bookId), Times.Once());
            queriesRepo.Verify(r => r.GetBookSeries(series1.Id), Times.Never());
            commandsRepo.Verify(r => r.UpdateBookSeriesRelation(It.IsAny<Book>()), Times.Never());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }
    }
}