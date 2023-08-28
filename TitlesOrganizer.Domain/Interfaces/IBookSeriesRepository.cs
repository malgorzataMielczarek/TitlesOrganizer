using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Domain.Interfaces
{
    public interface IBookSeriesRepository
    {
        int AddSeries(BookSeries series);

        int AddExistingBook(int seriesId, int bookId);

        int AddNewBook(int seriesId, Book book);

        void DeleteSeries(int seriesId);

        BookSeries GetSeriesById(int seriesId);

        void RemoveBookFromSeries(int seriesId, int bookId);

        int UpdateSeries(BookSeries series);
    }
}