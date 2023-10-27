using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Domain.Interfaces
{
    public interface IBookCommandsRepository
    {
        void Delete(Author author);

        void Delete(Book book);

        void Delete(BookSeries series);

        void Delete(LiteratureGenre genre);

        void UpdateAuthorBooksRelation(Author author);

        void UpdateBookAuthorsRelation(Book book);

        void UpdateBookGenresRelation(Book book);

        void UpdateBookSeriesBooksRelation(BookSeries series);

        void UpdateBookSeriesRelation(Book book);

        void UpdateLiteratureGenreBooksRelation(LiteratureGenre genre);
    }
}