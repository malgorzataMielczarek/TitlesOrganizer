using System.ComponentModel;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Base;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class SeriesForListVM : BaseForListVM<BookSeries>, IForListVM<BookSeries>
    {
        [DisplayName("Series")]
        public override string Description { get; set; } = null!;
    }

    public class ListSeriesForListVM : BaseListVM<BookSeries>, IListVM<BookSeries>
    {
        [DisplayName("Series")]
        public override List<IForListVM<BookSeries>> Values { get; set; } = new List<IForListVM<BookSeries>>();
    }

    public static partial class MappingExtensions
    {
        public static SeriesForListVM Map(this BookSeries series)
        {
            return new SeriesForListVM()
            {
                Id = series.Id,
                Description = series.Title
            };
        }

        public static IForListVM<T> Map<T>(this BookSeries series)
            where T : BookSeries
        {
            return (IForListVM<T>)series.Map();
        }

        public static ListSeriesForListVM MapToList(this IQueryable<BookSeries> series, Paging paging, Filtering filtering)
        {
            return (ListSeriesForListVM)series
                .Sort(filtering.SortBy, s => s.Title)
                .MapToList<BookSeries>(paging, filtering);
        }

        public static IQueryable<IForListVM<BookSeries>> MapToList(this IQueryable<BookSeries> series, ref Paging paging)
        {
            return series
                .OrderBy(s => s.Title)
                .MapToList<BookSeries>(ref paging);
        }
    }
}