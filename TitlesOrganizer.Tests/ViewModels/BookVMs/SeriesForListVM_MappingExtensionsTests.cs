// Ignore Spelling: Queryable

using FluentAssertions;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Tests.ViewModels.BookVMs
{
    public class SeriesForListVM_MappingExtensionsTests
    {
        [Fact]
        public void Map_BookSeries_SeriesForListVM()
        {
            var series = Helpers.GetSeries();

            var result = series.Map();

            result.Should().NotBeNull().And.BeOfType<SeriesForListVM>();
            result.Id.Should().Be(series.Id);
            result.Title.Should().NotBeNullOrWhiteSpace().And.Be(series.Title);
        }

        [Fact]
        public void Map_IQueryableBookSeries_IQueryableSeriesForListVM()
        {
            int count = 2;
            var series = Helpers.GetSeriesList(count).AsQueryable();

            var result = series.Map();

            result.Should().NotBeNull().And.BeAssignableTo<IQueryable<SeriesForListVM>>().And.AllBeOfType<SeriesForListVM>().And.HaveCount(count);
            result.FirstOrDefault()?.Id.Should().Be(series.FirstOrDefault()?.Id);
            result.FirstOrDefault()?.Title.Should().Be(series.FirstOrDefault()?.Title);
            result.LastOrDefault()?.Id.Should().Be(series.LastOrDefault()?.Id);
            result.LastOrDefault()?.Title.Should().Be(series.LastOrDefault()?.Title);
        }

        [Fact]
        public void Map_ICollectionBookSeries_ListSeriesForListVM()
        {
            int count = 2;
            var series = Helpers.GetSeriesList(count) as ICollection<BookSeries>;

            var result = series.Map();

            result.Should().NotBeNull().And.BeOfType<List<SeriesForListVM>>().And.AllBeOfType<SeriesForListVM>().And.HaveCount(count);
            result.FirstOrDefault()?.Id.Should().Be(series.FirstOrDefault()?.Id);
            result.FirstOrDefault()?.Title.Should().Be(series.FirstOrDefault()?.Title);
            result.LastOrDefault()?.Id.Should().Be(series.LastOrDefault()?.Id);
            result.LastOrDefault()?.Title.Should().Be(series.LastOrDefault()?.Title);
        }

        [Fact]
        public void Map_IEnumerableBookSeries_ListSeriesForListVM()
        {
            int count = 2;
            var books = Helpers.GetSeriesList(count).AsEnumerable();

            var result = books.Map();

            result.Should().NotBeNull().And.BeOfType<List<SeriesForListVM>>().And.AllBeOfType<SeriesForListVM>().And.HaveCount(count);
            result.FirstOrDefault()?.Id.Should().Be(books.FirstOrDefault()?.Id);
            result.FirstOrDefault()?.Title.Should().Be(books.FirstOrDefault()?.Title);
            result.LastOrDefault()?.Id.Should().Be(books.LastOrDefault()?.Id);
            result.LastOrDefault()?.Title.Should().Be(books.LastOrDefault()?.Title);
        }

        [Theory]
        [InlineData(3, 1, 5, 3)]
        [InlineData(3, 2, 5, 2)]
        public void MapToList_IQueryableBookSeriesAndPaging_IQueryableSeriesForListVMWithCorrectAmountOfElementsAndCorrectPaging(int pageSize, int pageNo, int count, int pageCount)
        {
            var series = Helpers.GetSeriesList(count).AsQueryable();
            var paging = new Paging() { CurrentPage = pageNo, PageSize = pageSize };

            var result = series.MapToList(ref paging);

            result.Should().NotBeNull().And.BeAssignableTo<IQueryable<SeriesForListVM>>().And.AllBeOfType<SeriesForListVM>().And.HaveCount(pageCount);
            paging.CurrentPage.Should().Be(pageNo);
            paging.PageSize.Should().Be(pageSize);
            paging.Count.Should().Be(count);
        }

        [Fact]
        public void MapToList_IQueryableBookSeriesAndPaging_IQueryableSeriesForListVMWithOrderedElements()
        {
            IQueryable<BookSeries> series = new List<BookSeries>()
            {
                new BookSeries(){Id = 1, Title = "Title"},
                new BookSeries(){Id = 2, Title = "Example"},
                new BookSeries(){Id = 3, Title = "Test"},
                new BookSeries(){Id = 4, Title = "Another Title"},
                new BookSeries(){Id = 5, Title = "One more Title"}
            }.AsQueryable(); // After ordering: 4, 2, 5, 3, 1
            var paging = new Paging() { CurrentPage = 1, PageSize = series.Count() };

            var result = series.MapToList(ref paging);

            result.ElementAt(0).Id.Should().Be(4);
            result.ElementAt(1).Id.Should().Be(2);
            result.ElementAt(2).Id.Should().Be(5);
            result.ElementAt(3).Id.Should().Be(3);
            result.ElementAt(4).Id.Should().Be(1);
            result.ElementAt(0).Title.Should().Be("Another Title");
            result.ElementAt(1).Title.Should().Be("Example");
            result.ElementAt(2).Title.Should().Be("One more Title");
            result.ElementAt(3).Title.Should().Be("Test");
            result.ElementAt(4).Title.Should().Be("Title");
        }

        [Theory]
        [InlineData(3, 1, 5, 3)]
        [InlineData(3, 2, 5, 2)]
        public void MapToList_IQueryableBookSeriesAndPagingAndFiltering_ListSeriesForListVMWithCorrectAmountOfElementsAndCorrectPaging(int pageSize, int pageNo, int count, int pageCount)
        {
            var series = Helpers.GetSeriesList(count).AsQueryable();
            var paging = new Paging() { CurrentPage = pageNo, PageSize = pageSize };
            var filtering = new Filtering() { SortBy = SortByEnum.Descending, SearchString = "Title" };

            var result = series.MapToList(paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListSeriesForListVM>();
            result.Series.Should().HaveCount(pageCount);
            result.Paging.Should().Be(paging);
            result.Paging.CurrentPage.Should().Be(pageNo);
            result.Paging.PageSize.Should().Be(pageSize);
            result.Paging.Count.Should().Be(count);
            result.Filtering.Should().Be(filtering);
            result.Filtering.SearchString.Should().Be(filtering.SearchString);
            result.Filtering.SortBy.Should().Be(filtering.SortBy);
        }

        [Fact]
        public void MapToList_IQueryableBookSeriesAndPagingAndFiltering_ListSeriesForListVMWithOrderedElements()
        {
            IQueryable<BookSeries> series = new List<BookSeries>()
            {
                new BookSeries(){Id = 1, Title = "Title"},
                new BookSeries(){Id = 2, Title = "Example"},
                new BookSeries(){Id = 3, Title = "Test"},
                new BookSeries(){Id = 4, Title = "Another Title"},
                new BookSeries(){Id = 5, Title = "One more Title"}
            }.AsQueryable(); // After ordering: 4, 2, 5, 3, 1
            var paging = new Paging() { CurrentPage = 1, PageSize = series.Count() };

            var result = series.MapToList(paging, new Filtering());

            result.Series.ElementAt(0).Id.Should().Be(4);
            result.Series.ElementAt(1).Id.Should().Be(2);
            result.Series.ElementAt(2).Id.Should().Be(5);
            result.Series.ElementAt(3).Id.Should().Be(3);
            result.Series.ElementAt(4).Id.Should().Be(1);
            result.Series.ElementAt(0).Title.Should().Be("Another Title");
            result.Series.ElementAt(1).Title.Should().Be("Example");
            result.Series.ElementAt(2).Title.Should().Be("One more Title");
            result.Series.ElementAt(3).Title.Should().Be("Test");
            result.Series.ElementAt(4).Title.Should().Be("Title");
        }

        [Fact]
        public void MapToList_IQueryableBookSeriesAndPagingAndFiltering_ListSeriesForListVMWithFilteredElements()
        {
            IQueryable<BookSeries> series = new List<BookSeries>()
            {
                new BookSeries(){Id = 1, Title = "Title"},
                new BookSeries(){Id = 2, Title = "Example"},
                new BookSeries(){Id = 3, Title = "Test"},
                new BookSeries(){Id = 4, Title = "Another Title"},
                new BookSeries(){Id = 5, Title = "One more Title"}
            }.AsQueryable();
            var paging = new Paging() { CurrentPage = 1, PageSize = series.Count() };
            var filtering = new Filtering() { SortBy = SortByEnum.Ascending, SearchString = "Title" };

            var result = series.MapToList(paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListSeriesForListVM>();
            result.Series.Should().NotBeNull().And.HaveCount(3);
            result.Series.ElementAt(0).Id.Should().Be(4);
            result.Series.ElementAt(1).Id.Should().Be(5);
            result.Series.ElementAt(2).Id.Should().Be(1);
            result.Series.ElementAt(0).Title.Should().Be("Another Title");
            result.Series.ElementAt(1).Title.Should().Be("One more Title");
            result.Series.ElementAt(2).Title.Should().Be("Title");
            result.Paging.Should().NotBeNull();
            result.Paging.CurrentPage.Should().Be(paging.CurrentPage);
            result.Paging.PageSize.Should().Be(paging.PageSize);
            result.Paging.Count.Should().Be(3);
        }

        [Fact]
        public void MapToList_IQueryableBookSeriesAndPagingAndFiltering_ListSeriesForListVMWithElementsOrderedDescending()
        {
            IQueryable<BookSeries> series = new List<BookSeries>()
            {
                new BookSeries(){Id = 1, Title = "Title"},
                new BookSeries(){Id = 2, Title = "Example"},
                new BookSeries(){Id = 3, Title = "Test"},
                new BookSeries(){Id = 4, Title = "Another Title"},
                new BookSeries(){Id = 5, Title = "One more Title"}
            }.AsQueryable(); // After ordering: 4, 2, 5, 3, 1 - desc: 1, 3, 5, 2, 4
            var paging = new Paging() { CurrentPage = 1, PageSize = series.Count() };
            var filtering = new Filtering() { SortBy = SortByEnum.Descending };

            var result = series.MapToList(paging, filtering);

            result.Series.ElementAt(0).Id.Should().Be(1);
            result.Series.ElementAt(1).Id.Should().Be(3);
            result.Series.ElementAt(2).Id.Should().Be(5);
            result.Series.ElementAt(3).Id.Should().Be(2);
            result.Series.ElementAt(4).Id.Should().Be(4);
            result.Series.ElementAt(0).Title.Should().Be("Title");
            result.Series.ElementAt(1).Title.Should().Be("Test");
            result.Series.ElementAt(2).Title.Should().Be("One more Title");
            result.Series.ElementAt(3).Title.Should().Be("Example");
            result.Series.ElementAt(4).Title.Should().Be("Another Title");
        }
    }
}