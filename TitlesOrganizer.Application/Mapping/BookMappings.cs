using AutoMapper;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Application.ViewModels.BookVMs.QueriesVMs.ForListVMs;
using TitlesOrganizer.Application.ViewModels.Common;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.Mapping
{
    public static class BookMappingsExtensions
    {
        public static IQueryable<AuthorForBookVM> Map(this IQueryable<Author> authors, int bookId)
        {
            return authors.Select(a => new AuthorForBookVM()
            {
                Id = a.Id,
                FullName = a.Name + " " + a.LastName,
                IsForBook = a.Books.Any(b => b.Id == bookId),
                OtherBooks = string.Join(", ", a.Books.Where(b => b.Id != bookId).OrderBy(b => b.Title).Select(b => b.Title))
            });
        }

        public static IQueryable<GenreForBookVM> Map(this IQueryable<LiteratureGenre> genres, int bookId)
        {
            return genres.Select(g => new GenreForBookVM()
            {
                Id = g.Id,
                Name = g.Name,
                IsForBook = g.Books != null && g.Books.Any(b => b.Id == bookId)
            });
        }

        public static IQueryable<SeriesForBookVM> Map(this IQueryable<BookSeries> series, int bookId)
        {
            return series.Select(s => new SeriesForBookVM()
            {
                Id = s.Id,
                Title = s.Title ?? string.Empty,
                IsForBook = s.Books.Any(b => b.Id == bookId),
                OtherBooks = string.Join(", ", s.Books.Where(b => b.Id != bookId).OrderBy(b => b.Title).Select(b => b.NumberInSeries))
            });
        }

        public static ListAuthorForBookVM MapToList(this IQueryable<Author> authors, int bookId, string bookTitle, SortByEnum sortBy, int pageSize, int pageNo, string searchString)
        {
            searchString ??= string.Empty;

            var list = authors
                .Sort(sortBy, a => a.LastName, a => a.Name);
            int count = list.Count();
            var limitedList = list
                .Map(bookId)
                .Where(a => a.FullName.Contains(searchString))
                .Skip(pageSize * (pageNo - 1))
                .Take(pageSize);
            var selectedAuthors = list.Where(a => a.Books.Any(b => b.Id == bookId)).Map().ToList();

            return new ListAuthorForBookVM(limitedList, count, sortBy, pageSize, pageNo, searchString)
            {
                BookId = bookId,
                BookTitle = bookTitle,
                SelectedAuthors = selectedAuthors
            };
        }

        public static ListGenreForBookVM MapToList(this IQueryable<LiteratureGenre> genres, int bookId, SortByEnum sortBy, int pageSize, int pageNo, string searchString)
        {
            searchString ??= string.Empty;

            var list = genres
                .Map(bookId)
                .Where(g => g.Name.Contains(searchString) || g.IsForBook)
                .Sort(SortByEnum.Descending, g => g.IsForBook,
                (SortBy: sortBy, Selector: g => g.Name));
            int count = list.Count();
            var limitedList = list
                .Skip(pageSize * (pageNo - 1))
                .Take(pageSize);

            return new ListGenreForBookVM(limitedList, count, sortBy, pageSize, pageNo, searchString)
            {
                BookId = bookId
            };
        }

        public static ListSeriesForBookVM MapToList(this IQueryable<BookSeries> series, int bookId, SortByEnum sortBy, int pageSize, int pageNo, string searchString)
        {
            var list = series
                .Map(bookId)
                .Where(s => s.IsForBook
                || string.IsNullOrEmpty(searchString)
                || (s.Title != null && s.Title.Contains(searchString))
                || s.OtherBooks.Contains(searchString))
                .Sort(SortByEnum.Descending, s => s.IsForBook,
                (SortBy: sortBy, Selector: s => s.Title),
                (SortBy: sortBy, Selector: s => s.OtherBooks));
            int count = list.Count();
            var limitedList = list
                .Skip(pageSize * (pageNo - 1))
                .Take(pageSize);

            return new ListSeriesForBookVM(limitedList, count, sortBy, pageSize, pageNo, searchString)
            {
                BookId = bookId
            };
        }
    }

    public class BookMappings : Profile
    {
        public BookMappings()
        {
            CreateProjection<Book, BookForListVM>();
        }
    }
}