using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TitlesOrganizer.Application.ViewModels.BookVMs.ForListVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs.ReferencesVMs.ForBookVMs
{
    public class GenreForBookVM
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [ScaffoldColumn(false)]
        public bool IsForBook { get; set; }

        [DisplayName("Genre")]
        public string Name { get; set; } = null!;
    }

    public class ListGenreForBookVM
    {
        [ScaffoldColumn(false)]
        public BookForListVM Book { get; set; } = new BookForListVM();

        [DisplayName("Previously selected genres")]
        public List<GenreForBookVM> SelectedGenres { get; set; } = new List<GenreForBookVM>();

        [DisplayName("Other genres")]
        public List<GenreForBookVM> NotSelectedGenres { get; set; } = new List<GenreForBookVM>();

        [ScaffoldColumn(false)]
        public Paging Paging { get; set; } = new Paging();

        [ScaffoldColumn(false)]
        public Filtering Filtering { get; set; } = new Filtering();
    }

    public static partial class MappingExtensions
    {
        public static IQueryable<GenreForBookVM> MapForBook(this IQueryable<LiteratureGenre> genresWithBooks, int bookId)
        {
            return genresWithBooks.Select(g => new GenreForBookVM
            {
                Id = g.Id,
                Name = g.Name,
                IsForBook = g.Books != null && g.Books.Any(b => b.Id == bookId)
            });
        }

        public static ListGenreForBookVM MapForBookToList(this IQueryable<LiteratureGenre> genresWithBooks, Book book, Paging paging, Filtering filtering)
        {
            var query = genresWithBooks
                .Sort(filtering.SortBy, g => g.Name)
                .MapForBook(book.Id);
            var selectedGenres = query.Where(g => g.IsForBook).ToList();
            var notSelectedGenres = query.Where(g => !g.IsForBook && g.Name.Contains(filtering.SearchString));
            paging.Count = notSelectedGenres.Count();
            var limitedList = notSelectedGenres
                .Skip(paging.PageSize * (paging.CurrentPage - 1))
                .Take(paging.PageSize)
                .ToList();

            return new ListGenreForBookVM()
            {
                Book = book.Map(),
                SelectedGenres = selectedGenres,
                NotSelectedGenres = limitedList,
                Paging = paging,
                Filtering = filtering
            };
        }
    }
}