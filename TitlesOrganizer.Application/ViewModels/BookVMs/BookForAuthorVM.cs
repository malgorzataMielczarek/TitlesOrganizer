using System.ComponentModel;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class BookForAuthorVM : IForItemVM
    {
        public bool IsForItem { get; set; }
        public int Id { get; set; }

        [DisplayName("Book")]
        public string Description { get; set; } = null!;
    }

    public class ListBookForAuthorVM : IDoubleListForItemVM<BookForAuthorVM, AuthorForListVM>
    {
        [DisplayName("Author")]
        public AuthorForListVM Item { get; set; } = new AuthorForListVM();

        [DisplayName("Previously selected books")]
        public List<BookForAuthorVM> SelectedValues { get; set; } = new List<BookForAuthorVM>();

        [DisplayName("Other books")]
        public List<BookForAuthorVM> Values { get; set; } = new List<BookForAuthorVM>();

        public Paging Paging { get; set; } = new Paging();
        public Filtering Filtering { get; set; } = new Filtering();
    }

    public static partial class MappingExtensions
    {
        public static IQueryable<BookForAuthorVM> MapForAuthor(this IQueryable<Book> booksWithAuthors, int authorId)
        {
            return booksWithAuthors.Select(b => new BookForAuthorVM
            {
                Id = b.Id,
                Description = b.Title,
                IsForItem = b.Authors.Any(a => a.Id == authorId)
            });
        }

        public static ListBookForAuthorVM MapForAuthorToList(this IQueryable<Book> booksWithAuthors, Author author, Paging paging, Filtering filtering)
        {
            var query = booksWithAuthors
                .Sort(filtering.SortBy, b => b.Title)
                .MapForAuthor(author.Id);
            var selectedBooks = query.Where(b => b.IsForItem).ToList();
            var notSelectedBooks = query.Where(b => !b.IsForItem && b.Description.Contains(filtering.SearchString));
            paging.Count = notSelectedBooks.Count();
            var limitedList = notSelectedBooks
                .Skip(paging.PageSize * (paging.CurrentPage - 1))
                .Take(paging.PageSize)
                .ToList();

            return new ListBookForAuthorVM()
            {
                Item = author.Map(),
                SelectedValues = selectedBooks,
                Values = limitedList,
                Paging = paging,
                Filtering = filtering
            };
        }
    }
}