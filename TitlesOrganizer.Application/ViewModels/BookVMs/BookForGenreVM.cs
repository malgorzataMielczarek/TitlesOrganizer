using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class BookForGenreVM
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [ScaffoldColumn(false)]
        public bool IsForGenre { get; set; }

        [DisplayName("Book")]
        public string Title { get; set; } = null!;
    }

    public class ListBookForGenreVM
    {
        [ScaffoldColumn(false)]
        public GenreForListVM Genre { get; set; } = new GenreForListVM();

        [DisplayName("Previously selected books")]
        public List<BookForGenreVM> SelectedBooks { get; set; } = new List<BookForGenreVM>();

        [DisplayName("Other books")]
        public List<BookForGenreVM> NotSelectedBooks { get; set; } = new List<BookForGenreVM>();

        [ScaffoldColumn(false)]
        public Paging Paging { get; set; } = new Paging();

        [ScaffoldColumn(false)]
        public Filtering Filtering { get; set; } = new Filtering();
    }

    public static partial class MappingExtensions
    {
        public static IQueryable<BookForGenreVM> MapForGenre(this IQueryable<Book> booksWithGenres, int genreId)
        {
            return booksWithGenres.Select(b => new BookForGenreVM
            {
                Id = b.Id,
                Title = b.Title,
                IsForGenre = b.Genres.Any(g => g.Id == genreId)
            });
        }

        public static ListBookForGenreVM MapForGenreToList(this IQueryable<Book> booksWithGenres, LiteratureGenre genre, Paging paging, Filtering filtering)
        {
            var query = booksWithGenres
                .Sort(filtering.SortBy, b => b.Title)
                .MapForGenre(genre.Id);
            var selectedBooks = query.Where(b => b.IsForGenre).ToList();
            var notSelectedBooks = query.Where(b => !b.IsForGenre && b.Title.Contains(filtering.SearchString));
            paging.Count = notSelectedBooks.Count();
            var limitedList = notSelectedBooks
                .Skip(paging.PageSize * (paging.CurrentPage - 1))
                .Take(paging.PageSize)
                .ToList();

            return new ListBookForGenreVM()
            {
                Genre = genre.Map(),
                SelectedBooks = selectedBooks,
                NotSelectedBooks = limitedList,
                Paging = paging,
                Filtering = filtering
            };
        }
    }
}