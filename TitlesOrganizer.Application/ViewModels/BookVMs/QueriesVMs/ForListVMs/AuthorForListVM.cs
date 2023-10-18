using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TitlesOrganizer.Application.ViewModels.Common;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs.QueriesVMs.ForListVMs
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

        public static ListAuthorForListVM MapToList(this ListData<Author> authors)
        {
            return authors.Values.MapToList(authors.Paging, authors.Filtering);
        }
    }
}