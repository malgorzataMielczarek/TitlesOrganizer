using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Concrete.BookVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;

namespace TitlesOrganizer.Application.Interfaces
{
    public interface IBookSeriesService
    {
        void Delete(int id);

        SeriesVM Get(int id, int booksPageSize, int booksPageNo);

        SeriesDetailsVM GetDetails(int id, int booksPageSize, int booksPageNo);

        IListVM GetList(SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        IListForItemVM GetListForBook(int bookId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        IPartialListVM GetPartialListForAuthor(int authorId, int pageSize, int pageNo);

        IPartialListVM GetPartialListForGenre(int genreId, int pageSize, int pageNo);

        void SelectBooks(int seriesId, int[] selectedIds);

        int Upsert(SeriesVM series);
    }
}