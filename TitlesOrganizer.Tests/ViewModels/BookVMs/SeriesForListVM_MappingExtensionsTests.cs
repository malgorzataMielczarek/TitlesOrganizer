// Ignore Spelling: Queryable

using FluentAssertions;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;
using TitlesOrganizer.Tests.Helpers;

namespace TitlesOrganizer.Tests.ViewModels.BookVMs
{
    public class SeriesForListVM_MappingExtensionsTests
    {
        [Fact]
        public void Map_BookSeries_SeriesForListVM()
        {
            var series = BookModuleHelpers.GetSeries();

            var result = series.Map();

            result.Should().NotBeNull().And.BeOfType<SeriesForListVM>();
            result.Id.Should().Be(series.Id);
            result.Description.Should().NotBeNullOrWhiteSpace().And.Be(series.Title);
        }

        [Fact]
        public void Map_IEnumerableBookSeries_ListIForListVMBookSeries()
        {
            var series = BookModuleHelpers.GetSeriesList(4).AsEnumerable();

            var result = series.Map();

            result.Should()
                .NotBeNullOrEmpty().And
                .BeAssignableTo<List<IForListVM<BookSeries>>>().And
                .HaveCount(4);
        }

        [Fact]
        public void MapToList_IQueryableBookSeriesAndPaging_IQueryableIForListVMBookSeriesWithOrderedElements()
        {
            var series = new List<BookSeries>()
            {
                new BookSeries(){Id = 1, Title = "Title"},
                new BookSeries(){Id = 2, Title = "Example"},
                new BookSeries(){Id = 3, Title = "Test"},
                new BookSeries(){Id = 4, Title = "Another Title"},
                new BookSeries(){Id = 5, Title = "One more Title"}
            }.AsQueryable();
            var paging = new Paging() { CurrentPage = 1, PageSize = series.Count() };

            var result = series.MapToList(ref paging);

            result.Should()
                .NotBeNullOrEmpty().And
                .BeAssignableTo<IQueryable<IForListVM<BookSeries>>>().And
                .HaveCount(series.Count()).And
                .BeInAscendingOrder(s => s.Description);
        }

        [Fact]
        public void MapToList_IEnumerableBookSeriesAndPaging_ListIForListVMBookSeriesWithOrderedElements()
        {
            var series = new List<BookSeries>()
            {
                new BookSeries(){Id = 1, Title = "Title"},
                new BookSeries(){Id = 2, Title = "Example"},
                new BookSeries(){Id = 3, Title = "Test"},
                new BookSeries(){Id = 4, Title = "Another Title"},
                new BookSeries(){Id = 5, Title = "One more Title"}
            }.AsEnumerable();
            var paging = new Paging() { CurrentPage = 1, PageSize = series.Count() };

            var result = series.MapToList(ref paging);

            result.Should()
                .NotBeNullOrEmpty().And
                .BeAssignableTo<List<IForListVM<BookSeries>>>().And
                .HaveCount(series.Count()).And
                .BeInAscendingOrder(s => s.Description);
        }

        [Fact]
        public void MapToList_IQueryableBookSeriesAndPagingAndFiltering_ListSeriesForListVMWithOrderedValues()
        {
            var series = new List<BookSeries>()
            {
                new BookSeries(){Id = 1, Title = "Title"},
                new BookSeries(){Id = 2, Title = "Example"},
                new BookSeries(){Id = 3, Title = "Test"},
                new BookSeries(){Id = 4, Title = "Another Title"},
                new BookSeries(){Id = 5, Title = "One more Title"}
            }.AsQueryable();
            var paging = new Paging() { CurrentPage = 1, PageSize = series.Count() };
            var filtering = new Filtering();

            var result = series.MapToList(paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListSeriesForListVM>();
            result.Values.Should()
                .NotBeNullOrEmpty().And
                .BeAssignableTo<List<IForListVM<BookSeries>>>().And
                .HaveCount(series.Count()).And
                .BeInAscendingOrder(s => s.Description);
        }

        [Fact]
        public void MapToList_IQueryableBookSeriesAndPagingAndFilteringSortByDescending_ListSeriesForListVMWithValuesInDescOrder()
        {
            var series = new List<BookSeries>()
            {
                new BookSeries(){Id = 1, Title = "Title"},
                new BookSeries(){Id = 2, Title = "Example"},
                new BookSeries(){Id = 3, Title = "Test"},
                new BookSeries(){Id = 4, Title = "Another Title"},
                new BookSeries(){Id = 5, Title = "One more Title"}
            }.AsQueryable();
            var paging = new Paging() { CurrentPage = 1, PageSize = series.Count() };
            var filtering = new Filtering() { SortBy = SortByEnum.Descending };

            var result = series.MapToList(paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListSeriesForListVM>();
            result.Values.Should()
                .NotBeNullOrEmpty().And
                .BeAssignableTo<List<IForListVM<BookSeries>>>().And
                .HaveCount(series.Count()).And
                .BeInDescendingOrder(s => s.Description);
        }
    }
}