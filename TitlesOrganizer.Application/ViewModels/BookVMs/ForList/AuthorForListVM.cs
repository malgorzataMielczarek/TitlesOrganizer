using TitlesOrganizer.Application.ViewModels.Common;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs.ForList
{
    public class AuthorForListVM
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
    }

    public class ListAuthorForListVM
    {
        public List<AuthorForListVM> Authors { get; set; } = new List<AuthorForListVM>();
        public Paging Paging { get; set; } = new Paging();
        public Filtering Filtering { get; set; } = new Filtering();
    }

    public static partial class MappingExtensions
    {
        public static IQueryable<AuthorForListVM> Map(this IQueryable<Author> authors)
        {
            return authors.Select(a => new AuthorForListVM()
            {
                Id = a.Id,
                FullName = a.Name + " " + a.LastName
            });
        }

        public static ListAuthorForListVM MapToList(this IQueryable<Author> authors, Paging paging, Filtering filtering)
        {
            var queryable = authors
                .Sort(filtering.SortBy, a => a.LastName, a => a.Name)
                .Map()
                .Where(a => a.FullName.Contains(filtering.SearchString));
            int count = queryable.Count();
            var limitedList = queryable
                .Skip(paging.PageSize * (paging.CurrentPage - 1))
                .Take(paging.PageSize)
                .ToList();

            return new ListAuthorForListVM()
            {
                Authors = limitedList,
                Paging = paging,
                Filtering = filtering
            };
        }
    }
}