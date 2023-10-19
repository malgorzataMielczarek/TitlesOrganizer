using System.ComponentModel.DataAnnotations;
using TitlesOrganizer.Application.ViewModels.BookVMs.QueryVMs.ForListVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs.QueryVMs.DetailsVMs
{
    public class GenreDetailsVM
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [ScaffoldColumn(false)]
        public string Name { get; set; } = null!;

        public List<BookForListVM> Books { get; set; } = new List<BookForListVM>();

        [ScaffoldColumn(false)]
        public Paging BooksPaging { get; set; } = new Paging();

        public List<SeriesForListVM> Series { get; set; } = new List<SeriesForListVM>();

        [ScaffoldColumn(false)]
        public Paging SeriesPaging { get; set; } = new Paging();

        public List<AuthorForListVM> Author { get; set; } = new List<AuthorForListVM>();

        [ScaffoldColumn(false)]
        public Paging AuthorsPaging { get; set; } = new Paging();
    }

    public static partial class MappingExtensions
    {
        public static GenreDetailsVM MapToDetails(this LiteratureGenre genre, ListData<Book> books, ListData<BookSeries> series, ListData<Author> authors)
        {
            var (booksList, booksPaging) = books.MapToList();
            var (seriesList, seriesPaging) = series.MapToList();
            var (authorsList, authorsPaging) = authors.MapToList();
            return new GenreDetailsVM()
            {
                Id = genre.Id,
                Name = genre.Name,
                Books = booksList,
                BooksPaging = booksPaging,
                Series = seriesList,
                SeriesPaging = seriesPaging,
                Author = authorsList,
                AuthorsPaging = authorsPaging
            };
        }

        public static GenreDetailsVM MapToDetails(this LiteratureGenre genreWithAllRelatedObjects, Paging booksPaging, Paging seriesPaging, Paging authorsPaging)
        {
            var genre = new GenreDetailsVM()
            {
                Id = genreWithAllRelatedObjects.Id,
                Name = genreWithAllRelatedObjects.Name
            };

            var books = genreWithAllRelatedObjects.Books?.AsQueryable();
            if (books != null)
            {
                genre.Books = books.MapToList(ref booksPaging).ToList();
                genre.BooksPaging = booksPaging;
                genre.Series = books.Where(b => b.BookSeries != null).Select(b => b.BookSeries!).MapToList(ref seriesPaging).ToList();
                genre.SeriesPaging = seriesPaging;
                genre.Author = books.SelectMany(b => b.Authors).MapToList(ref authorsPaging).ToList();
                genre.AuthorsPaging = authorsPaging;
            }

            return genre;
        }
    }
}