using TitlesOrganizer.Application.ViewModels.BookVMs.CommandVMs;

namespace TitlesOrganizer.Application.Interfaces
{
    public interface IBookCommandsService
    {
        void DeleteAuthor(int id);

        void DeleteBook(int id);

        void DeleteGenre(int id);

        void DeleteSeries(int id);

        void SelectAuthorsForBook(int bookId, List<int> authorsIds);

        void SelectBooksForAuthor(int authorId, List<int> booksIds);

        void SelectBooksForGenre(int genreId, List<int> booksIds);

        void SelectBooksForSeries(int seriesId, List<int> booksIds);

        void SelectGenresForBook(int bookId, List<int> genresIds);

        void SelectSeriesForBook(int bookId, int? seriesIds);

        int UpsertAuthor(AuthorVM author);

        int UpsertBook(BookVM book);

        int UpsertGenre(GenreVM genre);

        int UpsertSeries(SeriesVM series);
    }
}