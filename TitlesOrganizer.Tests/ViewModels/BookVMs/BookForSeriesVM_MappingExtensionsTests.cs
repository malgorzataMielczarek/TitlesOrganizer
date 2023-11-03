// Ignore Spelling: Queryable

using FluentAssertions;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Tests.ViewModels.BookVMs
{
    public class BookForSeriesVM_MappingExtensionsTests
    {
        [Fact]
        public void MapForItem_BookWithGivenBookSeries_IForItemVMBookBookSeries()
        {
            var series = Helpers.GetSeries(1);
            var book = new Book()
            {
                Id = 1,
                Title = "Title",
                Series = series,
                SeriesId = series.Id
            };

            var result = book.MapForItem(series);

            result.Should().NotBeNull().And.BeAssignableTo<IForItemVM<Book, BookSeries>>();
            result.Id.Should().Be(1);
            result.Description.Should().Be("Title");
            result.IsForItem.Should().BeTrue();
        }

        [Fact]
        public void MapForItem_BookWithoutGivenBookSeries_IForItemVMBookBookSeries()
        {
            var series = Helpers.GetSeries(1);
            var anotherSeries = Helpers.GetSeries(2);
            var book = new Book()
            {
                Id = 1,
                Title = "Title",
                Series = series,
                SeriesId = series.Id
            };

            var result = book.MapForItem(anotherSeries);

            result.Should().NotBeNull().And.BeAssignableTo<IForItemVM<Book, BookSeries>>();
            result.Id.Should().Be(1);
            result.Description.Should().Be("Title");
            result.IsForItem.Should().BeFalse();
        }

        [Fact]
        public void MapForItemToList_IQueryableBookAndBookSeriesAndPagingAndFiltering_ListBookForSeriesVMWithOrderedValues()
        {
            var series = Helpers.GetSeries(1);
            var books = new List<Book>()
            {
                new Book(){ Id = 1, Title = "Title", SeriesId = series.Id },
                new Book(){ Id = 2, Title = "Example" },
                new Book(){ Id = 3, Title = "Test", SeriesId = 2 },
                new Book(){ Id = 4, Title = "Another Title", SeriesId = 2 }
            }.AsQueryable();
            var paging = new Paging() { CurrentPage = 1, PageSize = books.Count() };
            var filtering = new Filtering();

            var result = books.MapForItemToList(series, paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListBookForSeriesVM>();
            result.SelectedValues.Should()
                .NotBeNullOrEmpty().And
                .BeAssignableTo<List<IForItemVM<Book, BookSeries>>>().And
                .HaveCount(1).And
                .ContainSingle(b => b.Id == 1 && b.Description == "Title");
            result.Values.Should()
                .NotBeNullOrEmpty().And
                .BeAssignableTo<List<IForItemVM<Book, BookSeries>>>().And
                .HaveCount(3).And
                .BeInAscendingOrder(b => b.Description);
        }

        [Fact]
        public void MapForItemToList_IQueryableBookAndBookSeriesAndPagingAndFilteringSortByDescending_ListBookForSeriesVMWithValuesInDescOrder()
        {
            var series = Helpers.GetSeries(1);
            var books = new List<Book>()
            {
                new Book(){ Id = 1, Title = "Title", SeriesId = series.Id },
                new Book(){ Id = 2, Title = "Example" },
                new Book(){ Id = 3, Title = "Test", SeriesId = 2 },
                new Book(){ Id = 4, Title = "Another Title", SeriesId = 2 }
            }.AsQueryable();
            var paging = new Paging() { CurrentPage = 1, PageSize = books.Count() };
            var filtering = new Filtering() { SortBy = SortByEnum.Descending };

            var result = books.MapForItemToList(series, paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListBookForSeriesVM>();
            result.SelectedValues.Should()
                .NotBeNullOrEmpty().And
                .BeAssignableTo<List<IForItemVM<Book, BookSeries>>>().And
                .HaveCount(1).And
                .ContainSingle(b => b.Id == 1 && b.Description == "Title");
            result.Values.Should()
                .NotBeNullOrEmpty().And
                .BeAssignableTo<List<IForItemVM<Book, BookSeries>>>().And
                .HaveCount(3).And
                .BeInDescendingOrder(b => b.Description);
        }
    }
}