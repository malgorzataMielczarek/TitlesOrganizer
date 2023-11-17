using System.ComponentModel;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Base;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class SeriesForBookVM : BaseForItemVM<BookSeries, Book>, IForItemVM<BookSeries, Book>
    {
        [DisplayName("Series")]
        public override string Description { get; set; } = string.Empty;
    }

    public class ListSeriesForBookVM : IListForItemVM<BookSeries, Book>
    {
        [DisplayName("Book")]
        public IForListVM<Book> Item { get; set; } = new BookForListVM();

        [DisplayName("Series")]
        public List<IForItemVM<BookSeries, Book>> Values { get; set; } = new List<IForItemVM<BookSeries, Book>>();

        public Paging Paging { get; set; } = new Paging();
        public Filtering Filtering { get; set; } = new Filtering();
    }

    public static partial class MappingExtensions
    {
        public static IForItemVM<BookSeries, Book> MapForItem(this BookSeries series, Book book)
        {
            return new SeriesForBookVM()
            {
                Id = series.Id,
                Description = series.Title,
                IsForItem = series.Books.Any(b => b.Id == book.Id)
            };
        }

        public static IEnumerable<IForItemVM<BookSeries, Book>> MapForItem(this IQueryable<BookSeries> sortedList, Book item)
        {
            return sortedList.Select(it => it.MapForItem(item)).ToList();
        }

        public static ListSeriesForBookVM MapForItemToList(this IQueryable<BookSeries> series, Book book, Paging paging, Filtering filtering)
        {
            return series.Sort(filtering.SortBy, s => s.Title)
                .MapForItem(book)
                .MapForItemToList<BookSeries, Book, ListSeriesForBookVM>(book.Map(), paging, filtering);
        }
    }
}