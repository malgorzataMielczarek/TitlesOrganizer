using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs.ForListVMs
{
    public class SeriesForListVM
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [DisplayName("Series")]
        public string Title { get; set; } = null!;
    }

    public class ListSeriesForListVM
    {
        public List<SeriesForListVM> Series { get; set; } = new List<SeriesForListVM>();

        [ScaffoldColumn(false)]
        public Paging Paging { get; set; } = new Paging();

        [ScaffoldColumn(false)]
        public Filtering Filtering { get; set; } = new Filtering();
    }

    public static partial class MappingExtensions
    {
        public static SeriesForListVM Map(this BookSeries series)
        {
            return new SeriesForListVM()
            {
                Id = series.Id,
                Title = series.Title
            };
        }

        public static IQueryable<SeriesForListVM> Map(this IQueryable<BookSeries> series)
        {
            return series.Select(s => new SeriesForListVM()
            {
                Id = s.Id,
                Title = s.Title
            });
        }

        public static List<SeriesForListVM> Map(this ICollection<BookSeries> series)
        {
            return series.Select(s => new SeriesForListVM()
            {
                Id = s.Id,
                Title = s.Title
            }).ToList();
        }

        public static List<SeriesForListVM> Map(this IEnumerable<BookSeries> series)
        {
            return series.Select(s => new SeriesForListVM()
            {
                Id = s.Id,
                Title = s.Title
            }).ToList();
        }

        public static ListSeriesForListVM MapToList(this IQueryable<BookSeries> series, Paging paging, Filtering filtering)
        {
            var queryable = series
                .Where(s => s.Title.Contains(filtering.SearchString))
                .Sort(filtering.SortBy, s => s.Title)
                .Map();
            paging.Count = queryable.Count();
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

        public static IQueryable<SeriesForListVM> MapToList(this IQueryable<BookSeries> series, ref Paging paging)
        {
            paging.Count = series.Count();
            var limitedList = series
                .OrderBy(s => s.Title)
                .Skip(paging.PageSize * (paging.CurrentPage - 1))
                .Take(paging.PageSize)
                .Map();

            return limitedList;
        }
    }
}