using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs.QueryVMs.ForListVMs
{
    public class GenreForListVM
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [DisplayName("Genre")]
        public string Name { get; set; } = null!;
    }

    public class ListGenreForListVM
    {
        public List<GenreForListVM> Genres { get; set; } = new List<GenreForListVM>();

        [ScaffoldColumn(false)]
        public Paging Paging { get; set; } = new Paging();

        [ScaffoldColumn(false)]
        public Filtering Filtering { get; set; } = new Filtering();
    }

    public static partial class MappingExtensions
    {
        public static GenreForListVM Map(this LiteratureGenre genre)
        {
            return new GenreForListVM()
            {
                Id = genre.Id,
                Name = genre.Name
            };
        }

        public static IQueryable<GenreForListVM> Map(this IQueryable<LiteratureGenre> genres)
        {
            return genres.Select(g => new GenreForListVM()
            {
                Id = g.Id,
                Name = g.Name
            });
        }

        public static List<GenreForListVM> Map(this ICollection<LiteratureGenre> genres)
        {
            return genres.Select(g => new GenreForListVM()
            {
                Id = g.Id,
                Name = g.Name
            }).ToList();
        }

        public static List<GenreForListVM> Map(this IEnumerable<LiteratureGenre> genres)
        {
            return genres.Select(g => new GenreForListVM()
            {
                Id = g.Id,
                Name = g.Name
            }).ToList();
        }

        public static ListGenreForListVM MapToList(this IQueryable<LiteratureGenre> genres, Paging paging, Filtering filtering)
        {
            var queryable = genres
                .Sort(filtering.SortBy, g => g.Name)
                .Where(g => g.Name.Contains(filtering.SearchString))
                .Map();
            paging.Count = queryable.Count();
            var limitedList = queryable
                .Skip(paging.PageSize * (paging.CurrentPage - 1))
                .Take(paging.PageSize)
                .ToList();

            return new ListGenreForListVM()
            {
                Genres = limitedList,
                Paging = paging,
                Filtering = filtering
            };
        }

        public static (List<GenreForListVM>, Paging) MapToList(this ListData<LiteratureGenre> genres)
        {
            genres.Paging.Count = genres.Values.Count();
            var limitedList = genres.Values
                .OrderBy(g => g.Name)
                .Skip(genres.Paging.PageSize * (genres.Paging.CurrentPage - 1))
                .Take(genres.Paging.PageSize)
                .Map()
                .ToList();

            return (limitedList, genres.Paging);
        }

        public static IQueryable<GenreForListVM> MapToList(this IQueryable<LiteratureGenre> genres, ref Paging paging)
        {
            paging.Count = genres.Count();
            var limitedList = genres
                .OrderBy(g => g.Name)
                .Skip(paging.PageSize * (paging.CurrentPage - 1))
                .Take(paging.PageSize)
                .Map();

            return limitedList;
        }
    }
}