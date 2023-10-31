using TitlesOrganizer.Application.ViewModels.Base;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.Interfaces
{
    public interface IAuthorService
    {
        void Delete(int id);

        AuthorVM Get(int id, int booksPageSize, int booksPageNo);

        AuthorDetailsVM GetDetails(int id, int booksPageSize, int booksPageNo, int seriesPageSize, int seriesPageNo, int genresPageSize, int genresPageNo);

        ListAuthorForListVM GetList(SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        ListAuthorForBookVM GetListForBook(int bookId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        PartialList<Author> GetPartialListForGenre(int genreId, int pageSize, int pageNo);

        void SelectBooks(int authorId, List<int> selectedIds);

        int Upsert(AuthorVM author);
    }
}