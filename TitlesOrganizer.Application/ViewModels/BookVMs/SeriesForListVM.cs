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
        public override List<IForListVM<BookSeries>> Values { get; set; }

        public ListSeriesForListVM()
        {
            Values = new List<IForListVM<BookSeries>>();
        }

        public ListSeriesForListVM(List<IForListVM<BookSeries>> values, Paging paging, Filtering filtering)
        {
            Values = values;
            Paging = paging;
            Filtering = filtering;
        }
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

        public static List<IForListVM<BookSeries>> Map(this IEnumerable<BookSeries> items)
        {
            return items.Select(it => (IForListVM<BookSeries>)it.Map()).ToList();
        }

        public static ListSeriesForListVM MapToList(this IQueryable<BookSeries> series, Paging paging, Filtering filtering)
        {
            var values = series
                .Sort(filtering.SortBy, s => s.Title)
                .Where(s => s.Title.Contains(filtering.SearchString))
                .SkipAndTake(ref paging)
                .Map();

            return new ListSeriesForListVM(values, paging, filtering);
        }

        public static List<IForListVM<BookSeries>> MapToList(this IEnumerable<BookSeries> series, ref Paging paging)
        {
            return series
                .OrderBy(s => s.Title)
                .SkipAndTake(ref paging)
                .Map();
        }

        public static IPartialList<BookSeries> MapToPartialList(this IEnumerable<BookSeries> series, Paging paging)
        {
            return new PartialList<BookSeries>()
            {
                Values = series.MapToList(ref paging).ToList(),
                Paging = paging
            };
        }
    }
}