using TitlesOrganizer.Application.ViewModels.BookVMs.ForList;
using TitlesOrganizer.Application.ViewModels.Common;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs.Details
{
    public class AuthorDetailsVM
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public List<BookForListVM> Books { get; set; } = new List<BookForListVM>();
        public List<SeriesForListVM> Series { get; set; } = new List<SeriesForListVM>();
        public List<GenreForListVM> Genres { get; set; } = new List<GenreForListVM>();
    }

    public class ListAuthorDetailsVM
    {
        public List<AuthorDetailsVM> Authors { get; set; } = new List<AuthorDetailsVM>();
        public Paging Paging { get; set; } = new Paging();
        public Filtering Filtering { get; set; } = new Filtering();
    }

    public static partial class MappingExtensions
    {
        public static AuthorDetailsVM MapToDetails(this Author author, IQueryable<Book> books, IQueryable<BookSeries> series, IQueryable<LiteratureGenre> genres)
        {
            return new AuthorDetailsVM()
            {
                Id = author.Id,
                FullName = author.Name + " " + author.LastName,
                Books = books.Map().ToList(),
                Series = series.Map().ToList(),
                Genres = genres.Map().ToList()
            };
        }

        public static ListAuthorDetailsVM FilterMapDetails(this IQueryable<Author> authors, Paging paging, Filtering filtering)
        {
            return authors.MapToList(paging, filtering).MapToListDetails();
        }

        private static List<AuthorDetailsVM> MapToDetails(this List<AuthorForListVM> authorsForList)
        {
            return authorsForList.Select(a => new AuthorDetailsVM() { Id = a.Id, FullName = a.FullName }).ToList();
        }

        private static ListAuthorDetailsVM MapToListDetails(this ListAuthorForListVM authorsList)
        {
            return new ListAuthorDetailsVM()
            {
                Authors = authorsList.Authors.MapToDetails(),
                Paging = authorsList.Paging,
                Filtering = authorsList.Filtering
            };
        }
    }
}