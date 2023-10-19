using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TitlesOrganizer.Application.ViewModels.BookVMs.QueryVMs.ForListVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs.QueryVMs.ReferencesVMs.ForBookVMs
{
    public class AuthorForBookVM
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [ScaffoldColumn(false)]
        public bool IsForBook { get; set; }

        [DisplayName("Author")]
        public string FullName { get; set; } = null!;
    }

    public class ListAuthorForBookVM
    {
        [ScaffoldColumn(false)]
        public BookForListVM Book { get; set; } = new BookForListVM();

        [DisplayName("Previously selected authors")]
        public List<AuthorForBookVM> SelectedAuthors { get; set; } = new List<AuthorForBookVM>();

        [DisplayName("Other authors")]
        public List<AuthorForBookVM> NotSelectedAuthors { get; set; } = new List<AuthorForBookVM>();

        [ScaffoldColumn(false)]
        public Paging Paging { get; set; } = new Paging();

        [ScaffoldColumn(false)]
        public Filtering Filtering { get; set; } = new Filtering();
    }

    public static partial class MappingExtensions
    {
        public static IQueryable<AuthorForBookVM> MapForBook(this IQueryable<Author> authorsWithBooks, int bookId)
        {
            return authorsWithBooks.Select(a => new AuthorForBookVM()
            {
                Id = a.Id,
                FullName = a.Name + " " + a.LastName,
                IsForBook = a.Books.Any(b => b.Id == bookId)
            });
        }

        public static ListAuthorForBookVM MapForBookToList(this IQueryable<Author> authorsWithBooks, Book book, Paging paging, Filtering filtering)
        {
            var query = authorsWithBooks
                .Sort(filtering.SortBy, a => a.LastName, a => a.Name)
                .MapForBook(book.Id);
            var selectedAuthors = query.Where(a => a.IsForBook).ToList();
            var notSelectedAuthors = query.Where(a => !a.IsForBook && a.FullName.Contains(filtering.SearchString));
            paging.Count = notSelectedAuthors.Count();
            var limitedList = notSelectedAuthors
                .Skip(paging.PageSize * (paging.CurrentPage - 1))
                .Take(paging.PageSize)
                .ToList();

            return new ListAuthorForBookVM()
            {
                Book = book.Map(),
                SelectedAuthors = selectedAuthors,
                NotSelectedAuthors = limitedList,
                Paging = paging,
                Filtering = filtering
            };
        }
    }
}