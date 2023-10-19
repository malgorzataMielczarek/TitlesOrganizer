using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TitlesOrganizer.Application.ViewModels.BookVMs.QueryVMs.ForListVMs;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs.QueryVMs.DetailsVMs
{
    public class BookDetailsVM
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [ScaffoldColumn(false)]
        public string Title { get; set; } = null!;

        [ScaffoldColumn(false)]
        public string InSeries { get; set; } = null!;

        [ScaffoldColumn(false)]
        public string SeriesTitle { get; set; } = null!;

        [ScaffoldColumn(false)]
        public int? SeriesId { get; set; }

        public List<AuthorForListVM> Authors { get; set; } = new List<AuthorForListVM>();

        public string Description { get; set; } = null!;

        [DisplayName("Original title")]
        public string OriginalTitle { get; set; } = null!;

        [DisplayName("Original language")]
        public string OriginalLanguage { get; set; } = null!;

        public string Year { get; set; } = null!;

        public string Edition { get; set; } = null!;

        public List<GenreForListVM> Genres { get; set; } = new List<GenreForListVM>();
    }

    public static partial class MappingExtensions
    {
        public static BookDetailsVM MapToDetails(this Book book)
        {
            return new BookDetailsVM()
            {
                Id = book.Id,
                Title = book.Title,
                OriginalTitle = book.OriginalTitle ?? string.Empty,
                OriginalLanguage = book.OriginalLanguage?.Name ?? string.Empty,
                Authors = book.Authors.Map(),
                Genres = book.Genres.Map(),
                Description = book.Description ?? string.Empty,
                Year = book.Year?.ToString() ?? string.Empty,
                Edition = book.Edition ?? string.Empty,
                SeriesId = book.BookSeriesId,
                SeriesTitle = book.BookSeries?.Title ?? string.Empty,
                InSeries = InSeries(book.NumberInSeries, book.BookSeries)
            };
        }

        private static string InSeries(int? numberInSeries, BookSeries? bookSeries)
        {
            if (bookSeries == null)
            {
                return string.Empty;
            }

            StringBuilder result = new StringBuilder();
            if (numberInSeries != null)
            {
                result.Append(numberInSeries);
                int count;
                if ((count = bookSeries.Books.Count) > 0)
                {
                    result.Append(" of ");
                    result.Append(count);
                }

                if (bookSeries.Title != null)
                {
                    result.Append(" in ");
                }
            }
            else if (bookSeries.Title != null)
            {
                result.Append("Part of ");
            }

            return result.ToString();
        }
    }
}