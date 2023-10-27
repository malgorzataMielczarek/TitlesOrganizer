using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs.ForListVMs
{
    public class BookForListVM
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [DisplayName("Book")]
        public string Title { get; set; } = null!;
    }

    public class ListBookForListVM
    {
        public List<BookForListVM> Books { get; set; } = new List<BookForListVM>();

        [ScaffoldColumn(false)]
        public Paging Paging { get; set; } = new Paging();

        [ScaffoldColumn(false)]
        public Filtering Filtering { get; set; } = new Filtering();
    }

    public static partial class MappingExtensions
    {
        public static BookForListVM Map(this Book book)
        {
            return new BookForListVM()
            {
                Id = book.Id,
                Title = book.Title
            };
        }

        public static IQueryable<BookForListVM> Map(this IQueryable<Book> books)
        {
            return books.Select(b => new BookForListVM()
            {
                Id = b.Id,
                Title = b.Title
            });
        }

        public static List<BookForListVM> Map(this ICollection<Book> books)
        {
            return books.Select(b => new BookForListVM()
            {
                Id = b.Id,
                Title = b.Title
            }).ToList();
        }

        public static List<BookForListVM> Map(this IEnumerable<Book> books)
        {
            return books.Select(b => new BookForListVM()
            {
                Id = b.Id,
                Title = b.Title
            }).ToList();
        }

        public static ListBookForListVM MapToList(this IQueryable<Book> books, Paging paging, Filtering filtering)
        {
            var list = books
                .Where(b => b.Title.Contains(filtering.SearchString))
                .Sort(filtering.SortBy, b => b.Title);
            paging.Count = list.Count();
            var limitedList = list
                .Skip(paging.PageSize * (paging.CurrentPage - 1))
                .Take(paging.PageSize)
                .Map()
                .ToList();

            return new ListBookForListVM()
            {
                Books = limitedList,
                Paging = paging,
                Filtering = filtering
            };
        }

        public static IQueryable<BookForListVM> MapToList(this IQueryable<Book> books, ref Paging paging)
        {
            paging.Count = books.Count();
            var limitedList = books
                .OrderBy(b => b.Title)
                .Skip(paging.PageSize * (paging.CurrentPage - 1))
                .Take(paging.PageSize)
                .Map();

            return limitedList;
        }
    }
}