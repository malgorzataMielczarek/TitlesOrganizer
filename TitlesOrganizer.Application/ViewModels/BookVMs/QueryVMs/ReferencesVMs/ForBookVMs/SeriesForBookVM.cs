using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TitlesOrganizer.Application.ViewModels.BookVMs.QueryVMs.ForListVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs.QueryVMs.ReferencesVMs.ForBookVMs
{
    public class SeriesForBookVM
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [ScaffoldColumn(false)]
        public bool IsForBook { get; set; }

        [DisplayName("Series")]
        public string Title { get; set; } = string.Empty;
    }

    public class ListSeriesForBookVM
    {
        [ScaffoldColumn(false)]
        public BookForListVM Book { get; set; } = new BookForListVM();

        public List<SeriesForBookVM> Series { get; set; } = new List<SeriesForBookVM>();

        [ScaffoldColumn(false)]
        public Paging Paging { get; set; } = new Paging();

        [ScaffoldColumn(false)]
        public Filtering Filtering { get; set; } = new Filtering();
    }

    public static partial class MappingExtensions
    {
        public static IQueryable<SeriesForBookVM> MapForBook(this IQueryable<BookSeries> series, int bookId)
        {
            return series.Select(s => new SeriesForBookVM()
            {
                Id = s.Id,
                Title = s.Title,
                IsForBook = s.Books.Any(b => b.Id == bookId)
            });
        }

        public static ListSeriesForBookVM MapForBookToList(this IQueryable<BookSeries> series, Book book, Paging paging, Filtering filtering)
        {
            var query = series
                .MapForBook(book.Id)
                .Where(s => s.IsForBook || s.Title.Contains(filtering.SearchString))
                .Sort(SortByEnum.Descending, s => s.IsForBook, (SortBy: filtering.SortBy, Selector: s => s.Title));
            paging.Count = query.Count();
            var limitedList = query
                .Skip(paging.PageSize * (paging.CurrentPage - 1))
                .Take(paging.PageSize)
                .ToList();

            return new ListSeriesForBookVM()
            {
                Book = book.Map(),
                Series = limitedList,
                Paging = paging,
                Filtering = filtering
            };
        }
    }
}