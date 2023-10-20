using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs.QueryVMs.ForListVMs
{
    public class AuthorForListVM
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [DisplayName("Author")]
        public string FullName { get; set; } = null!;
    }

    public class ListAuthorForListVM
    {
        public List<AuthorForListVM> Authors { get; set; } = new List<AuthorForListVM>();

        [ScaffoldColumn(false)]
        public Paging Paging { get; set; } = new Paging();

        [ScaffoldColumn(false)]
        public Filtering Filtering { get; set; } = new Filtering();
    }

    public static partial class MappingExtensions
    {
        public static AuthorForListVM Map(this Author author)
        {
            return new AuthorForListVM()
            {
                Id = author.Id,
                FullName = author.Name + " " + author.LastName
            };
        }

        public static IQueryable<AuthorForListVM> Map(this IQueryable<Author> authors)
        {
            return authors.Select(a => new AuthorForListVM()
            {
                Id = a.Id,
                FullName = a.Name + " " + a.LastName
            });
        }

        public static List<AuthorForListVM> Map(this ICollection<Author> authors)
        {
            return authors.Select(a => new AuthorForListVM()
            {
                Id = a.Id,
                FullName = a.Name + " " + a.LastName
            }).ToList();
        }

        public static List<AuthorForListVM> Map(this IEnumerable<Author> authors)
        {
            return authors.Select(a => new AuthorForListVM()
            {
                Id = a.Id,
                FullName = a.Name + " " + a.LastName
            }).ToList();
        }

        public static ListAuthorForListVM MapToList(this IQueryable<Author> authors, Paging paging, Filtering filtering)
        {
            var queryable = authors
                .Sort(filtering.SortBy, a => a.LastName, a => a.Name)
                .Map()
                .Where(a => a.FullName.Contains(filtering.SearchString));
            paging.Count = queryable.Count();
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

        public static IQueryable<AuthorForListVM> MapToList(this IQueryable<Author> authors, ref Paging paging)
        {
            paging.Count = authors.Count();
            var limitedList = authors
                .OrderBy(a => a.LastName).ThenBy(a => a.Name)
                .Skip(paging.PageSize * (paging.CurrentPage - 1))
                .Take(paging.PageSize)
                .Map();

            return limitedList;
        }
    }
}