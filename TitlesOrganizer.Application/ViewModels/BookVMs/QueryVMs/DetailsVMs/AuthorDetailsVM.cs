using System.ComponentModel.DataAnnotations;
using TitlesOrganizer.Application.ViewModels.BookVMs.QueryVMs.ForListVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs.QueryVMs.DetailsVMs
{
    public class AuthorDetailsVM
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [ScaffoldColumn(false)]
        public string FullName { get; set; } = null!;

        public List<BookForListVM> Books { get; set; } = new List<BookForListVM>();

        [ScaffoldColumn(false)]
        public Paging BooksPaging { get; set; } = new Paging();

        public List<SeriesForListVM> Series { get; set; } = new List<SeriesForListVM>();

        [ScaffoldColumn(false)]
        public Paging SeriesPaging { get; set; } = new Paging();

        public List<GenreForListVM> Genres { get; set; } = new List<GenreForListVM>();

        [ScaffoldColumn(false)]
        public Paging GenresPaging { get; set; } = new Paging();
    }

    public static partial class MappingExtensions
    {
        public static AuthorDetailsVM MapToDetails(this Author author, ListData<Book> books, ListData<BookSeries> series, ListData<LiteratureGenre> genres)
        {
            var (booksList, booksPaging) = books.MapToList();
            var (seriesList, seriesPaging) = series.MapToList();
            var (genresList, genresPaging) = genres.MapToList();
            return new AuthorDetailsVM()
            {
                Id = author.Id,
                FullName = author.Name + " " + author.LastName,
                Books = booksList,
                BooksPaging = booksPaging,
                Series = seriesList,
                SeriesPaging = seriesPaging,
                Genres = genresList,
                GenresPaging = genresPaging
            };
        }

        public static AuthorDetailsVM MapToDetails(this Author authorWithAllRelatedObjects, Paging booksPaging, Paging seriesPaging, Paging genresPaging)
        {
            return new AuthorDetailsVM()
            {
                Id = authorWithAllRelatedObjects.Id,
                FullName = authorWithAllRelatedObjects.Name + " " + authorWithAllRelatedObjects.LastName,
                Books = authorWithAllRelatedObjects.Books.AsQueryable().MapToList(ref booksPaging).ToList(),
                BooksPaging = booksPaging,
                Series = authorWithAllRelatedObjects.Books.AsQueryable().Where(b => b.BookSeries != null).Select(b => b.BookSeries!).MapToList(ref seriesPaging).ToList(),
                SeriesPaging = seriesPaging,
                Genres = authorWithAllRelatedObjects.Books.SelectMany(b => b.Genres).AsQueryable().MapToList(ref genresPaging).ToList(),
                GenresPaging = genresPaging,
            };
        }
    }
}