using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;

namespace TitlesOrganizer.Application.Interfaces
{
    public interface IAuthorService
    {
        void Delete(int id);

        AuthorVM Get(int id, int booksPageSize, int booksPageNo);

        AuthorDetailsVM GetDetails(int id, int booksPageSize, int booksPageNo, int authorsPageSize, int authorsPageNo, int seriesPageSize, int seriesPageNo);

        ListAuthorForListVM GetList(SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        ListAuthorForBookVM GetListForBook(int bookId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        PartialList<AuthorForListVM> GetPartialListForGenre(int genreId, int pageSize, int pageNo);

        void SelectForBook(int bookId, List<int> selectedIds);

        int Upsert(AuthorVM author);
    }
}