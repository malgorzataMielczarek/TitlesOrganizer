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

            var result = books.MapForSeries(seriesId);

            result.Should().NotBeNull().And.BeAssignableTo<IQueryable<BookForSeriesVM>>().And.AllBeOfType<BookForSeriesVM>().And.HaveCount(books.Count());
            result.ElementAt(0).Id.Should().Be(1);
            result.ElementAt(1).Id.Should().Be(2);
            result.ElementAt(2).Id.Should().Be(3);
            result.ElementAt(3).Id.Should().Be(4);
            result.ElementAt(0).Title.Should().Be("Title");
            result.ElementAt(1).Title.Should().Be("Example");
            result.ElementAt(2).Title.Should().Be("Test");
            result.ElementAt(3).Title.Should().Be("Another Title");
            result.ElementAt(0).IsForSeries.Should().BeTrue();
            result.ElementAt(1).IsForSeries.Should().BeFalse();
            result.ElementAt(2).IsForSeries.Should().BeTrue();
            result.ElementAt(3).IsForSeries.Should().BeTrue();
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

            var result = books.MapForSeriesToList(series, paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListBookForSeriesVM>();
            result.Series.Should().NotBeNull().And.BeOfType<SeriesForListVM>();
            result.SelectedBooks.Should().NotBeNull().And.BeOfType<List<BookForSeriesVM>>().And.HaveCount(selectedCount);
            result.NotSelectedBooks.Should().NotBeNull().And.BeOfType<List<BookForSeriesVM>>().And.HaveCount(pageCount);
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

            var result = books.MapForSeriesToList(series, paging, filtering);

            result.SelectedBooks.ElementAt(0).Id.Should().Be(4);
            result.SelectedBooks.ElementAt(1).Id.Should().Be(3);
            result.SelectedBooks.ElementAt(2).Id.Should().Be(1);
            result.SelectedBooks.ElementAt(0).Title.Should().Be("Another Title");
            result.SelectedBooks.ElementAt(1).Title.Should().Be("Test");
            result.SelectedBooks.ElementAt(2).Title.Should().Be("Title");
        }

        [Fact]
        public void MapToList_IQueryableBookAndBookSeriesAndPagingAndFiltering_ListBookForSeriesVMWithOrderedNotSelectedBooks()
        {
            var books = GetBooksWithSeriesId();
            var series = Helpers.GetSeries(3);
            var paging = new Paging() { CurrentPage = 1, PageSize = books.Count() };
            var filtering = new Filtering();

            var result = books.MapForSeriesToList(series, paging, filtering);

            result.NotSelectedBooks.ElementAt(0).Id.Should().Be(4);
            result.NotSelectedBooks.ElementAt(1).Id.Should().Be(2);
            result.NotSelectedBooks.ElementAt(2).Id.Should().Be(3);
            result.NotSelectedBooks.ElementAt(3).Id.Should().Be(1);
            result.NotSelectedBooks.ElementAt(0).Title.Should().Be("Another Title");
            result.NotSelectedBooks.ElementAt(1).Title.Should().Be("Example");
            result.NotSelectedBooks.ElementAt(2).Title.Should().Be("Test");
            result.NotSelectedBooks.ElementAt(3).Title.Should().Be("Title");
        }

        [Fact]
        public void MapToList_IQueryableBookAndBookSeriesAndPagingAndFiltering_ListBookForSeriesVMWithFilteredNotSelectedBooks()
        {
            var books = GetBooksWithSeriesId();
            var series = Helpers.GetSeries(3);
            var paging = new Paging() { CurrentPage = 1, PageSize = books.Count() };
            var filtering = new Filtering() { SearchString = "Title" };

            var result = books.MapForSeriesToList(series, paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListBookForSeriesVM>();
            result.NotSelectedBooks.Should().NotBeNull().And.HaveCount(2);
            result.NotSelectedBooks.ElementAt(0).Id.Should().Be(4);
            result.NotSelectedBooks.ElementAt(1).Id.Should().Be(1);
            result.NotSelectedBooks.ElementAt(0).Title.Should().Be("Another Title");
            result.NotSelectedBooks.ElementAt(1).Title.Should().Be("Title");
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

            var result = books.MapForSeriesToList(series, paging, filtering);

            result.NotSelectedBooks.ElementAt(0).Id.Should().Be(1);
            result.NotSelectedBooks.ElementAt(1).Id.Should().Be(3);
            result.NotSelectedBooks.ElementAt(2).Id.Should().Be(2);
            result.NotSelectedBooks.ElementAt(3).Id.Should().Be(4);
            result.NotSelectedBooks.ElementAt(0).Title.Should().Be("Title");
            result.NotSelectedBooks.ElementAt(1).Title.Should().Be("Test");
            result.NotSelectedBooks.ElementAt(2).Title.Should().Be("Example");
            result.NotSelectedBooks.ElementAt(3).Title.Should().Be("Another Title");
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