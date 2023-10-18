using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TitlesOrganizer.Application.ViewModels.BookVMs.QueriesVMs.ForListVMs;
using TitlesOrganizer.Application.ViewModels.Common;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs.QueriesVMs.DetailsVMs
{
    public class SeriesDetailsVM
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [ScaffoldColumn(false)]
        public string Title { get; set; } = string.Empty;

        [DisplayName("Original title")]
        public string OriginalTitle { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public ListBookForListVM Books { get; set; } = new ListBookForListVM();

        public List<AuthorForListVM> Authors { get; set; } = new List<AuthorForListVM>();

        public List<GenreForListVM> Genres { get; set; } = new List<GenreForListVM>();
    }

    public static partial class MappingExtensions
    {
        public static SeriesDetailsVM MapToDetails(this BookSeries series, ListData<Book> books, IQueryable<Author> authors, IQueryable<LiteratureGenre> genres)
        {
            return new SeriesDetailsVM()
            {
                Id = series.Id,
                Title = series.Title,
                OriginalTitle = series.OriginalTitle ?? string.Empty,
                Description = series.Description ?? string.Empty,
                Books = books.MapToList(),
                Authors = authors.Map().ToList(),
                Genres = genres.Map().ToList()
            };
        }

        public static SeriesDetailsVM MapToDetails(this BookSeries seriesWithAllRelatedObjects, Paging booksPaging, Filtering booksFiltering)
        {
            return new SeriesDetailsVM()
            {
                Id = seriesWithAllRelatedObjects.Id,
                Title = seriesWithAllRelatedObjects.Title,
                OriginalTitle = seriesWithAllRelatedObjects.OriginalTitle ?? string.Empty,
                Description = seriesWithAllRelatedObjects.Description ?? string.Empty,
                Books = seriesWithAllRelatedObjects.Books.AsQueryable().MapToList(booksPaging, booksFiltering),
                Authors = seriesWithAllRelatedObjects.Books.SelectMany(b => b.Authors).Map(),
                Genres = seriesWithAllRelatedObjects.Books.SelectMany(b => b.Genres).Map()
            };
        }
    }
}