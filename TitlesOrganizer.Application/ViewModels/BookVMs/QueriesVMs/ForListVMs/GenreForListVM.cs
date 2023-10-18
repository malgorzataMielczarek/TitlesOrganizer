using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TitlesOrganizer.Application.ViewModels.Common;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs.QueriesVMs.ForListVMs
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
        public Paging Paging { get; set; } = new Paging();
        public Filtering Filtering { get; set; } = new Filtering();
    }

    public static partial class MappingExtensions
    {
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

        public static ListGenreForListVM MapToList(this ListData<LiteratureGenre> genres)
        {
            return genres.Values.MapToList(genres.Paging, genres.Filtering);
        }
    }
}