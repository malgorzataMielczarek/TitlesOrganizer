using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Domain.Interfaces
{
    public interface IBookCommandsRepository
    {
        void Delete(Author author);

        void Delete(Book book);

        void Delete(BookSeries series);

        void Delete(LiteratureGenre genre);

        Author? GetAuthor(int id);

        Book? GetBook(int id);

        BookSeries? GetBookSeries(int id);

        LiteratureGenre? GetLiteratureGenre(int id);

        void UpdateAuthorBooksRelation(Author author);

        void UpdateBookAuthorsRelation(Book book);

        void UpdateBookGenresRelation(Book book);

        void UpdateBookSeriesBooksRelation(BookSeries series);

        void UpdateBookSeriesRelation(Book book);

        void UpdateLiteratureGenreBooksRelation(LiteratureGenre genre);
    }
}