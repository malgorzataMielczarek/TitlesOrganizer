using TitlesOrganizer.Application.ViewModels.Common;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs.ForList
{
    public class GenreForListVM
    {
        public int Id { get; set; }
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
            return genres.Select(a => new GenreForListVM()
            {
                Id = a.Id,
                Name = a.Name
            });
        }

        public static ListGenreForListVM MapToList(this IQueryable<LiteratureGenre> genres, Paging paging, Filtering filtering)
        {
            var queryable = genres
                .Sort(filtering.SortBy, a => a.Name)
                .Map()
                .Where(a => a.Name.Contains(filtering.SearchString));
            int count = queryable.Count();
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
    }
}