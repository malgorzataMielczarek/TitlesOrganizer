using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TitlesOrganizer.Application.ViewModels.BookVMs.QueryVMs.ForListVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs.QueryVMs.ReferencesVMs.ForAuthorVMs
{
    public class BookForAuthorVM
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [ScaffoldColumn(false)]
        public bool IsForAuthor { get; set; }

        [DisplayName("Book")]
        public string Title { get; set; } = null!;
    }

    public class ListBookForAuthorVM
    {
        [ScaffoldColumn(false)]
        public AuthorForListVM Author { get; set; } = new AuthorForListVM();

        [DisplayName("Previously selected books")]
        public List<BookForAuthorVM> SelectedBooks { get; set; } = new List<BookForAuthorVM>();

        [DisplayName("Other books")]
        public List<BookForAuthorVM> NotSelectedBooks { get; set; } = new List<BookForAuthorVM>();

        [ScaffoldColumn(false)]
        public Paging Paging { get; set; } = new Paging();

        [ScaffoldColumn(false)]
        public Filtering Filtering { get; set; } = new Filtering();
    }

    public static partial class MappingExtensions
    {
        public static IQueryable<BookForAuthorVM> MapForAuthor(this IQueryable<Book> booksWithAuthors, int authorId)
        {
            return booksWithAuthors.Select(b => new BookForAuthorVM
            {
                Id = b.Id,
                Title = b.Title,
                IsForAuthor = b.Authors.Any(a => a.Id == authorId)
            });
        }

        public static ListBookForAuthorVM MapForAuthorToList(this IQueryable<Book> booksWithAuthors, Author author, Paging paging, Filtering filtering)
        {
            var query = booksWithAuthors
                .Sort(filtering.SortBy, b => b.Title)
                .MapForAuthor(author.Id);
            var selectedBooks = query.Where(b => b.IsForAuthor).ToList();
            var notSelectedBooks = query.Where(b => !b.IsForAuthor && b.Title.Contains(filtering.SearchString));
            paging.Count = notSelectedBooks.Count();
            var limitedList = notSelectedBooks
                .Skip(paging.PageSize * (paging.CurrentPage - 1))
                .Take(paging.PageSize)
                .ToList();

            return new ListBookForAuthorVM()
            {
                Author = author.Map(),
                SelectedBooks = selectedBooks,
                NotSelectedBooks = limitedList,
                Paging = paging,
                Filtering = filtering
            };
        }
    }
}