using System.ComponentModel;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Base;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class SeriesDetailsVM : BaseDetailsVM<BookSeries>, IDetailsVM<BookSeries>
    {
        [DisplayName("Original title")]
        public string OriginalTitle { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public PartialList<Book> Books { get; set; } = new PartialList<Book>();
        public List<IForListVM<Author>> Authors { get; set; } = new List<IForListVM<Author>>();
        public List<IForListVM<LiteratureGenre>> Genres { get; set; } = new List<IForListVM<LiteratureGenre>>();
    }

    public static partial class MappingExtensions
    {
        public static SeriesDetailsVM MapToDetails(this BookSeries series, IQueryable<Book> books, Paging booksPaging, IQueryable<Author> authors, IQueryable<LiteratureGenre> genres)
        {
            return new SeriesDetailsVM()
            {
                Id = series.Id,
                Title = series.Title,
                OriginalTitle = series.OriginalTitle ?? string.Empty,
                Description = series.Description ?? string.Empty,
                Books = new PartialList<Book>()
                {
                    Values = books.MapToList(ref booksPaging).ToList(),
                    Paging = booksPaging
                },
                Authors = authors.Map().ToList(),
                Genres = genres.Map().ToList()
            };
        }
    }
}