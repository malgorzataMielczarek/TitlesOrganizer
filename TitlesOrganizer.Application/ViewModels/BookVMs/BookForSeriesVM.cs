using System.ComponentModel;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Base;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class BookForSeriesVM : BaseForItemVM<Book, BookSeries>, IForItemVM<Book, BookSeries>
    {
        [DisplayName("Book")]
        public override string Description { get; set; } = null!;
    }

    public class ListBookForSeriesVM : IDoubleListForItemVM<Book, BookSeries>
    {
        [DisplayName("Series")]
        public IForListVM<BookSeries> Item { get; set; } = new SeriesForListVM();

        [DisplayName("Previously selected books")]
        public List<IForItemVM<Book, BookSeries>> SelectedValues { get; set; } = new List<IForItemVM<Book, BookSeries>>();

        [DisplayName("Other books")]
        public List<IForItemVM<Book, BookSeries>> Values { get; set; } = new List<IForItemVM<Book, BookSeries>>();

        public Paging Paging { get; set; } = new Paging();
        public Filtering Filtering { get; set; } = new Filtering();
    }

    public static partial class MappingExtensions
    {
        public static IForItemVM<Book, BookSeries> MapForItem(this Book books, BookSeries series)
        {
            return new BookForSeriesVM
            {
                Id = books.Id,
                Description = books.Title,
                IsForItem = books.SeriesId == series.Id
            };
        }

        public static IQueryable<IForItemVM<Book, BookSeries>> MapForItem(this IQueryable<Book> sortedList, BookSeries item)
        {
            return sortedList.Select(it => it.MapForItem(item));
        }

        public static ListBookForSeriesVM MapForItemToList(this IQueryable<Book> books, BookSeries series, Paging paging, Filtering filtering)
        {
            return books
                .Sort(filtering.SortBy, b => b.Title)
                .MapForItem(series)
                .MapForItemToDoubleList<Book, BookSeries, ListBookForSeriesVM>(series.Map(), paging, filtering);
        }
    }
}