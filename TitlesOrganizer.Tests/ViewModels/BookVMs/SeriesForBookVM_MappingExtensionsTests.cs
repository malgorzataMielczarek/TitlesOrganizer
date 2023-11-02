// Ignore Spelling: Queryable

using FluentAssertions;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Tests.ViewModels.BookVMs
{
    public class SeriesForBookVM_MappingExtensionsTests
    {
        [Fact]
        public void MapForBook_IQueryableBookSeriesWithBooksAndBookId_IQueryableSeriesForBookVM()
        {
            var series = GetSeriesWithBooks();
            int bookId = 1;
            var result = series.MapForItem(bookId);

            result.Should().NotBeNull().And.BeAssignableTo<IQueryable<SeriesForBookVM>>().And.AllBeOfType<SeriesForBookVM>().And.HaveCount(series.Count());
            result.ElementAt(0).Id.Should().Be(1);
            result.ElementAt(1).Id.Should().Be(2);
            result.ElementAt(2).Id.Should().Be(3);
            result.ElementAt(3).Id.Should().Be(4);
            result.ElementAt(0).Description.Should().Be("Title");
            result.ElementAt(1).Description.Should().Be("Example");
            result.ElementAt(2).Description.Should().Be("Test");
            result.ElementAt(3).Description.Should().Be("Another Title");
            result.ElementAt(0).IsForItem.Should().BeTrue();
            result.ElementAt(1).IsForItem.Should().BeFalse();
            result.ElementAt(2).IsForItem.Should().BeFalse();
            result.ElementAt(3).IsForItem.Should().BeFalse();
        }

        [Theory]
        [InlineData(11, 3, 1, 3)]
        [InlineData(11, 3, 2, 1)]
        [InlineData(1, 3, 1, 3)]
        public void MapToList_IQueryableBookSeriesWithBooksAndBookAndPagingAndFiltering_ListSeriesForBookVMWithCorrectAmountOfElementsAndCorrectPaging(int bookId, int pageSize, int pageNo, int pageCount)
        {
            var series = GetSeriesWithBooks();
            int count = series.Count();
            var book = Helpers.GetBook(bookId);
            var paging = new Paging() { CurrentPage = pageNo, PageSize = pageSize };
            var filtering = new Filtering();

            var result = series.MapForItemToList(book, paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListSeriesForBookVM>();
            result.Item.Should().NotBeNull().And.BeOfType<BookForListVM>();
            result.Values.Should().NotBeNull().And.BeOfType<List<SeriesForBookVM>>().And.HaveCount(pageCount);
            result.Paging.Should().Be(paging);
            result.Paging.CurrentPage.Should().Be(pageNo);
            result.Paging.PageSize.Should().Be(pageSize);
            result.Paging.Count.Should().Be(count);
            result.Filtering.Should().Be(filtering);
            result.Filtering.SearchString.Should().Be(filtering.SearchString);
            result.Filtering.SortBy.Should().Be(filtering.SortBy);
        }

        [Fact]
        public void MapToList_IQueryableBookSeriesWithBooksAndBookAndPagingAndFiltering_ListSeriesForBookVMWithOrderedElements()
        {
            var series = GetSeriesWithBooks();
            var book = Helpers.GetBook(1);
            var paging = new Paging() { CurrentPage = 1, PageSize = series.Count() };
            var filtering = new Filtering();

            var result = series.MapForItemToList(book, paging, filtering);

            result.Values.ElementAt(0).Id.Should().Be(1);
            result.Values.ElementAt(1).Id.Should().Be(4);
            result.Values.ElementAt(2).Id.Should().Be(2);
            result.Values.ElementAt(3).Id.Should().Be(3);
            result.Values.ElementAt(0).Description.Should().Be("Title");
            result.Values.ElementAt(1).Description.Should().Be("Another Title");
            result.Values.ElementAt(2).Description.Should().Be("Example");
            result.Values.ElementAt(3).Description.Should().Be("Test");
        }

        [Fact]
        public void MapToList_IQueryableBookSeriesWithBooksAndBookAndPagingAndFiltering_ListSeriesForBookVMWithFilteredElements()
        {
            var series = GetSeriesWithBooks();
            var book = Helpers.GetBook(3);
            var paging = new Paging() { CurrentPage = 1, PageSize = series.Count() };
            var filtering = new Filtering() { SearchString = "Title" };

            var result = series.MapForItemToList(book, paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListSeriesForBookVM>();
            result.Values.Should().NotBeNull().And.HaveCount(3);
            result.Values.ElementAt(0).Id.Should().Be(2);
            result.Values.ElementAt(1).Id.Should().Be(4);
            result.Values.ElementAt(2).Id.Should().Be(1);
            result.Values.ElementAt(0).Description.Should().Be("Example");
            result.Values.ElementAt(1).Description.Should().Be("Another Title");
            result.Values.ElementAt(2).Description.Should().Be("Title");
            result.Paging.Should().NotBeNull();
            result.Paging.CurrentPage.Should().Be(paging.CurrentPage);
            result.Paging.PageSize.Should().Be(paging.PageSize);
            result.Paging.Count.Should().Be(3);
        }

        [Fact]
        public void MapToList_IQueryableBookSeriesWithBooksAndBookAndPagingAndFiltering_ListSeriesForBookVMWithElementsOrderedDescending()
        {
            var series = GetSeriesWithBooks();
            var book = Helpers.GetBook(3);
            var paging = new Paging() { CurrentPage = 1, PageSize = series.Count() };
            var filtering = new Filtering() { SortBy = SortByEnum.Descending };

            var result = series.MapForItemToList(book, paging, filtering);

            result.Values.ElementAt(0).Id.Should().Be(2);
            result.Values.ElementAt(1).Id.Should().Be(1);
            result.Values.ElementAt(2).Id.Should().Be(3);
            result.Values.ElementAt(3).Id.Should().Be(4);
            result.Values.ElementAt(0).Description.Should().Be("Example");
            result.Values.ElementAt(1).Description.Should().Be("Title");
            result.Values.ElementAt(2).Description.Should().Be("Test");
            result.Values.ElementAt(3).Description.Should().Be("Another Title");
        }

        private IQueryable<BookSeries> GetSeriesWithBooks()
        {
            var books = Helpers.GetBooksList(10);

            return new List<BookSeries>()
            {
                new BookSeries(){ Id = 1, Title = "Title", Books = new List<Book>() { books[0], books[1] } },
                new BookSeries(){ Id = 2, Title = "Example", Books = new List<Book>() { books[2], books[3], books[4] } },
                new BookSeries(){ Id = 3, Title = "Test", Books = new List<Book>() { books[5], books[6], books[7], books[8] } },
                new BookSeries(){ Id = 4, Title = "Another Title", Books = new List<Book>() { books[9] } }
            }.AsQueryable();
        }
    }
}