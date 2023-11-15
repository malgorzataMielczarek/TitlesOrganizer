using System.ComponentModel;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Base;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class GenreForListVM : BaseForListVM<LiteratureGenre>, IForListVM<LiteratureGenre>
    {
        [DisplayName("Genre")]
        public override string Description { get; set; } = null!;
    }

    public class ListGenreForListVM : BaseListVM<LiteratureGenre>, IListVM<LiteratureGenre>
    {
        [DisplayName("Genres")]
        public override List<IForListVM<LiteratureGenre>> Values { get; set; }

        public ListGenreForListVM()
        {
            Values = new List<IForListVM<LiteratureGenre>>();
        }

        public ListGenreForListVM(List<IForListVM<LiteratureGenre>> values, Paging paging, Filtering filtering)
        {
            Values = values;
            Paging = paging;
            Filtering = filtering;
        }
    }

    public static partial class MappingExtensions
    {
        public static GenreForListVM Map(this LiteratureGenre genre)
        {
            return new GenreForListVM()
            {
                Id = genre.Id,
                Description = genre.Name
            };
        }

        public static IQueryable<IForListVM<LiteratureGenre>> Map(this IQueryable<LiteratureGenre> items)
        {
            return items.Select(it => it.Map());
        }

        public static List<IForListVM<LiteratureGenre>> Map(this IEnumerable<LiteratureGenre> items)
        {
            return items.Select(it => (IForListVM<LiteratureGenre>)it.Map()).ToList();
        }

        public static ListGenreForListVM MapToList(this IQueryable<LiteratureGenre> genres, Paging paging, Filtering filtering)
        {
            var values = genres
                .Sort(filtering.SortBy, g => g.Name)
                .Where(g => g.Name.Contains(filtering.SearchString))
                .SkipAndTake(ref paging)
                .Map();

            return new ListGenreForListVM(values, paging, filtering);
        }

        public static List<IForListVM<LiteratureGenre>> MapToList(this IEnumerable<LiteratureGenre> genres, ref Paging paging)
        {
            return genres
                .OrderBy(g => g.Name)
                .SkipAndTake(ref paging)
                .Map();
        }

        public static IPartialList<LiteratureGenre> MapToPartialList(this IEnumerable<LiteratureGenre> genres, Paging paging)
        {
            return new PartialList<LiteratureGenre>()
            {
                Values = genres.MapToList(ref paging),
                Paging = paging
            };
        }
    }
}