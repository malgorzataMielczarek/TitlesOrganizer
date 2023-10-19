using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TitlesOrganizer.Application.ViewModels.BookVMs.QueryVMs.ForListVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs.QueryVMs.ReferencesVMs.ForSeriesVMs
{
    public class BookForSeriesVM
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [ScaffoldColumn(false)]
        public bool IsForSeries { get; set; }

        [DisplayName("Book")]
        public string Title { get; set; } = null!;
    }

    public class ListBookForSeriesVM
    {
        [ScaffoldColumn(false)]
        public SeriesForListVM Series { get; set; } = new SeriesForListVM();

        [DisplayName("Previously selected books")]
        public List<BookForSeriesVM> SelectedBooks { get; set; } = new List<BookForSeriesVM>();

        [DisplayName("Other books")]
        public List<BookForSeriesVM> NotSelectedBooks { get; set; } = new List<BookForSeriesVM>();

        [ScaffoldColumn(false)]
        public Paging Paging { get; set; } = new Paging();

        [ScaffoldColumn(false)]
        public Filtering Filtering { get; set; } = new Filtering();
    }

    public static partial class MappingExtensions
    {
        public static IQueryable<BookForSeriesVM> MapForSeries(this IQueryable<Book> booksWithSeries, int seriesId)
        {
            return booksWithSeries.Select(b => new BookForSeriesVM
            {
                Id = b.Id,
                Title = b.Title,
                IsForSeries = b.BookSeriesId == seriesId
            });
        }

        public static ListBookForSeriesVM MapForSeriesToList(this IQueryable<Book> booksWithSeries, BookSeries series, Paging paging, Filtering filtering)
        {
            var query = booksWithSeries
                .Sort(filtering.SortBy, b => b.Title)
                .MapForSeries(series.Id);
            var selectedBooks = query.Where(b => b.IsForSeries).ToList();
            var notSelectedBooks = query.Where(b => !b.IsForSeries && b.Title.Contains(filtering.SearchString));
            paging.Count = notSelectedBooks.Count();
            var limitedList = notSelectedBooks
                .Skip(paging.PageSize * (paging.CurrentPage - 1))
                .Take(paging.PageSize)
                .ToList();

            return new ListBookForSeriesVM()
            {
                Series = series.Map(),
                SelectedBooks = selectedBooks,
                NotSelectedBooks = limitedList,
                Paging = paging,
                Filtering = filtering
            };
        }
    }
}