// Ignore Spelling: Queryable

using FluentAssertions;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;

namespace TitlesOrganizer.Tests.ViewModels.BookVMs
{
    public class BookForSeriesVM_MappingExtensionsTests
    {
        [Fact]
        public void MapForAuthor_IQueryableBookAndSeriesId_IQueryableBookForSeriesVM()
        {
            var books = GetBooksWithSeriesId();
            int seriesId = 1;

            var result = books.MapForItem(seriesId);

            result.Should().NotBeNull().And.BeAssignableTo<IQueryable<BookForSeriesVM>>().And.AllBeOfType<BookForSeriesVM>().And.HaveCount(books.Count());
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
            result.ElementAt(2).IsForItem.Should().BeTrue();
            result.ElementAt(3).IsForItem.Should().BeTrue();
        }

        [Theory]
        [InlineData(3, 0, 4, 3, 1, 3)]
        [InlineData(3, 0, 4, 3, 2, 1)]
        [InlineData(1, 3, 1, 3, 1, 1)]
        public void MapToList_IQueryableBookAndBookSeriesAndPagingAndFiltering_ListBookForSeriesVMWithCorrectAmountOfElementsAndCorrectPaging(int seriesId, int selectedCount, int notSelectedCount, int pageSize, int pageNo, int pageCount)
        {
            var books = GetBooksWithSeriesId();
            var series = Helpers.GetSeries(seriesId);
            var paging = new Paging() { CurrentPage = pageNo, PageSize = pageSize };
            var filtering = new Filtering();

            var result = books.MapForItemToList(series, paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListBookForSeriesVM>();
            result.Item.Should().NotBeNull().And.BeOfType<SeriesForListVM>();
            result.SelectedValues.Should().NotBeNull().And.BeOfType<List<BookForSeriesVM>>().And.HaveCount(selectedCount);
            result.Values.Should().NotBeNull().And.BeOfType<List<BookForSeriesVM>>().And.HaveCount(pageCount);
            result.Paging.Should().Be(paging);
            result.Paging.CurrentPage.Should().Be(pageNo);
            result.Paging.PageSize.Should().Be(pageSize);
            result.Paging.Count.Should().Be(notSelectedCount);
            result.Filtering.Should().Be(filtering);
            result.Filtering.SearchString.Should().Be(filtering.SearchString);
            result.Filtering.SortBy.Should().Be(filtering.SortBy);
        }

        [Fact]
        public void MapToList_IQueryableBookAndBookSeriesAndPagingAndFiltering_ListBookForSeriesVMWithOrderedSelectedBooks()
        {
            var books = GetBooksWithSeriesId();
            var series = Helpers.GetSeries(1);
            var paging = new Paging() { CurrentPage = 1, PageSize = books.Count() };
            var filtering = new Filtering();

            var result = books.MapForItemToList(series, paging, filtering);

            result.SelectedValues.ElementAt(0).Id.Should().Be(4);
            result.SelectedValues.ElementAt(1).Id.Should().Be(3);
            result.SelectedValues.ElementAt(2).Id.Should().Be(1);
            result.SelectedValues.ElementAt(0).Description.Should().Be("Another Title");
            result.SelectedValues.ElementAt(1).Description.Should().Be("Test");
            result.SelectedValues.ElementAt(2).Description.Should().Be("Title");
        }

        [Fact]
        public void MapToList_IQueryableBookAndBookSeriesAndPagingAndFiltering_ListBookForSeriesVMWithOrderedNotSelectedBooks()
        {
            var books = GetBooksWithSeriesId();
            var series = Helpers.GetSeries(3);
            var paging = new Paging() { CurrentPage = 1, PageSize = books.Count() };
            var filtering = new Filtering();

            var result = books.MapForItemToList(series, paging, filtering);

            result.Values.ElementAt(0).Id.Should().Be(4);
            result.Values.ElementAt(1).Id.Should().Be(2);
            result.Values.ElementAt(2).Id.Should().Be(3);
            result.Values.ElementAt(3).Id.Should().Be(1);
            result.Values.ElementAt(0).Description.Should().Be("Another Title");
            result.Values.ElementAt(1).Description.Should().Be("Example");
            result.Values.ElementAt(2).Description.Should().Be("Test");
            result.Values.ElementAt(3).Description.Should().Be("Title");
        }

        [Fact]
        public void MapToList_IQueryableBookAndBookSeriesAndPagingAndFiltering_ListBookForSeriesVMWithFilteredNotSelectedBooks()
        {
            var books = GetBooksWithSeriesId();
            var series = Helpers.GetSeries(3);
            var paging = new Paging() { CurrentPage = 1, PageSize = books.Count() };
            var filtering = new Filtering() { SearchString = "Title" };

            var result = books.MapForItemToList(series, paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListBookForSeriesVM>();
            result.Values.Should().NotBeNull().And.HaveCount(2);
            result.Values.ElementAt(0).Id.Should().Be(4);
            result.Values.ElementAt(1).Id.Should().Be(1);
            result.Values.ElementAt(0).Description.Should().Be("Another Title");
            result.Values.ElementAt(1).Description.Should().Be("Title");
            result.Paging.Should().NotBeNull();
            result.Paging.CurrentPage.Should().Be(paging.CurrentPage);
            result.Paging.PageSize.Should().Be(paging.PageSize);
            result.Paging.Count.Should().Be(2);
        }

        [Fact]
        public void MapToList_IQueryableBookAndBookSeriesAndPagingAndFiltering_ListBookForSeriesVMWithNotSelectedBooksOrderedDescending()
        {
            var books = GetBooksWithSeriesId();
            var series = Helpers.GetSeries(3);
            var paging = new Paging() { CurrentPage = 1, PageSize = books.Count() };
            var filtering = new Filtering() { SortBy = SortByEnum.Descending };

            var result = books.MapForItemToList(series, paging, filtering);

            result.Values.ElementAt(0).Id.Should().Be(1);
            result.Values.ElementAt(1).Id.Should().Be(3);
            result.Values.ElementAt(2).Id.Should().Be(2);
            result.Values.ElementAt(3).Id.Should().Be(4);
            result.Values.ElementAt(0).Description.Should().Be("Title");
            result.Values.ElementAt(1).Description.Should().Be("Test");
            result.Values.ElementAt(2).Description.Should().Be("Example");
            result.Values.ElementAt(3).Description.Should().Be("Another Title");
        }

        private IQueryable<Domain.Models.Book> GetBooksWithSeriesId()
        {
            return new List<Domain.Models.Book>()
            {
                new Domain.Models.Book(){ Id = 1, Title = "Title", SeriesId = 1 },
                new Domain.Models.Book(){ Id = 2, Title = "Example" },
                new Domain.Models.Book(){ Id = 3, Title = "Test", SeriesId = 1 },
                new Domain.Models.Book(){ Id = 4, Title = "Another Title", SeriesId = 1 }
            }.AsQueryable();
        }
    }
}