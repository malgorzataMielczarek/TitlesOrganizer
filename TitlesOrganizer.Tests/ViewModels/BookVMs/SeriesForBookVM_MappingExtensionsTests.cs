// Ignore Spelling: Queryable

using FluentAssertions;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;
using TitlesOrganizer.Tests.Helpers;

namespace TitlesOrganizer.Tests.ViewModels.BookVMs
{
    public class SeriesForBookVM_MappingExtensionsTests
    {
        [Fact]
        public void MapForItem_BookSeriesWithGivenBook_IForItemVMBookSeriesBook()
        {
            var book = BookModuleHelpers.GetBook(1);
            var series = new BookSeries()
            {
                Id = 1,
                Title = "Test",
                Books = { book }
            };

            var result = series.MapForItem(book);

            result.Should().NotBeNull().And.BeAssignableTo<IForItemVM<BookSeries, Book>>();
            result.Id.Should().Be(1);
            result.Description.Should().Be("Test");
            result.IsForItem.Should().BeTrue();
        }

        [Fact]
        public void MapForItem_BookSeriesWithoutGivenBook_IForItemVMBookSeriesBook()
        {
            var book = BookModuleHelpers.GetBook(1);
            var anotherBook = BookModuleHelpers.GetBook(2);
            var series = new BookSeries()
            {
                Id = 1,
                Title = "Test",
                Books = { book }
            };

            var result = series.MapForItem(anotherBook);

            result.Should().NotBeNull().And.BeAssignableTo<IForItemVM<BookSeries, Book>>();
            result.Id.Should().Be(1);
            result.Description.Should().Be("Test");
            result.IsForItem.Should().BeFalse();
        }

        [Fact]
        public void MapForItemToList_IQueryableBookSeriesAndBookAndPagingAndFiltering_ListSeriesForBookVMWithOrderedValues()
        {
            var book1 = BookModuleHelpers.GetBook(1);
            var book2 = BookModuleHelpers.GetBook(2);
            var book3 = BookModuleHelpers.GetBook(3);
            var series = new List<BookSeries>()
            {
                new BookSeries(){ Id = 1, Title = "Title", Books = new List<Book>() { book1 } },
                new BookSeries(){ Id = 2, Title = "Example", Books = new List<Book>() { book2 } },
                new BookSeries(){ Id = 3, Title = "Test", Books = new List<Book>() { book3 } },
                new BookSeries(){ Id = 4, Title = "Another Title", Books = new List<Book>() }
            }.AsQueryable();
            var paging = new Paging() { CurrentPage = 1, PageSize = series.Count() };
            var filtering = new Filtering();

            var result = series.MapForItemToList(book3, paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListSeriesForBookVM>();
            result.Values.Should()
                .NotBeNullOrEmpty().And
                .BeAssignableTo<List<IForItemVM<BookSeries, Book>>>().And
                .HaveCount(series.Count()).And
                .SatisfyRespectively(
                    first =>
                    {
                        first.Id.Should().Be(3);
                        first.Description.Should().Be("Test");
                        first.IsForItem.Should().BeTrue();
                    },
                    second =>
                    {
                        second.Id.Should().Be(4);
                        second.Description.Should().Be("Another Title");
                        second.IsForItem.Should().BeFalse();
                    },
                    third =>
                    {
                        third.Id.Should().Be(2);
                        third.Description.Should().Be("Example");
                        third.IsForItem.Should().BeFalse();
                    },
                    fourth =>
                    {
                        fourth.Id.Should().Be(1);
                        fourth.Description.Should().Be("Title");
                        fourth.IsForItem.Should().BeFalse();
                    }
                );
        }

        [Fact]
        public void MapForItemToList_IQueryableBookSeriesAndBookAndPagingAndFilteringSortByDescending_ListSeriesForBookVMWithValuesInDescOrder()
        {
            var book1 = BookModuleHelpers.GetBook(1);
            var book2 = BookModuleHelpers.GetBook(2);
            var book3 = BookModuleHelpers.GetBook(3);
            var series = new List<BookSeries>()
            {
                new BookSeries(){ Id = 1, Title = "Title", Books = new List<Book>() { book1 } },
                new BookSeries(){ Id = 2, Title = "Example", Books = new List<Book>() { book2 } },
                new BookSeries(){ Id = 3, Title = "Test", Books = new List<Book>() { book3 } },
                new BookSeries(){ Id = 4, Title = "Another Title", Books = new List<Book>() }
            }.AsQueryable();
            var paging = new Paging() { CurrentPage = 1, PageSize = series.Count() };
            var filtering = new Filtering() { SortBy = SortByEnum.Descending };

            var result = series.MapForItemToList(book3, paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListSeriesForBookVM>();
            result.Values.Should()
                .NotBeNullOrEmpty().And
                .BeAssignableTo<List<IForItemVM<BookSeries, Book>>>().And
                .HaveCount(series.Count()).And
                .SatisfyRespectively(
                    first =>
                    {
                        first.Id.Should().Be(3);
                        first.Description.Should().Be("Test");
                        first.IsForItem.Should().BeTrue();
                    },
                    second =>
                    {
                        second.Id.Should().Be(1);
                        second.Description.Should().Be("Title");
                        second.IsForItem.Should().BeFalse();
                    },
                    third =>
                    {
                        third.Id.Should().Be(2);
                        third.Description.Should().Be("Example");
                        third.IsForItem.Should().BeFalse();
                    },
                    fourth =>
                    {
                        fourth.Id.Should().Be(4);
                        fourth.Description.Should().Be("Another Title");
                        fourth.IsForItem.Should().BeFalse();
                    }
                );
        }
    }
}