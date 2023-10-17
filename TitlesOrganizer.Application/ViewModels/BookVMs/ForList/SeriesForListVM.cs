using TitlesOrganizer.Application.ViewModels.Common;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs.ForList
{
    public class SeriesForListVM
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
    }

    public class ListSeriesForListVM
    {
        public List<SeriesForListVM> Series { get; set; } = new List<SeriesForListVM>();
        public Paging Paging { get; set; } = new Paging();
        public Filtering Filtering { get; set; } = new Filtering();
    }

    public static partial class MappingExtensions
    {
        public static IQueryable<SeriesForListVM> Map(this IQueryable<BookSeries> series)
        {
            return series.Select(s => new SeriesForListVM()
            {
                Id = s.Id,
                Title = s.Title ?? string.Empty
            });
        }

        public static ListSeriesForListVM MapToList(this IQueryable<BookSeries> series, Paging paging, Filtering filtering)
        {
            var queryable = series
                .Where(s => s.Title.Contains(filtering.SearchString))
                .Sort(filtering.SortBy, s => s.Title)
                .Map();
            int count = queryable.Count();
            var limitedList = queryable
                .Skip(paging.PageSize * (paging.CurrentPage - 1))
                .Take(paging.PageSize)
                .ToList();

            return new ListSeriesForListVM()
            {
                Series = limitedList,
                Paging = paging,
                Filtering = filtering
            };
        }
    }
}