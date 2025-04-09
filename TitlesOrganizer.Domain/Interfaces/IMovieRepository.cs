using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Domain.Interfaces
{
    public interface IMovieRepository : IVideoRepository
    {
        string? AddCountry(int movieId, string countryCode);

        int AddExistingDirector(int movieId, int directorId);

        int AddExistingGenre(int movieId, int genreId);

        int AddMovie(Movie movie);

        int AddNewCreator(int movieId, Creator creator);

        int AddNewGenre(int movieId, Genre genre);

        int AddSeries(Series series);

        void DeleteMovie(int movieId);

        void DeleteSeries(int seriesId);

        IQueryable<Movie> GetAllMovies();

        IQueryable<Series> GetAllSeries();

        IQueryable<Creator>? GetDirectorsOfSeries(int seriesId);

        IQueryable<Genre>? GetGenresOfSeries(int seriesId);

        Movie? GetMovieById(int movieId);

        IQueryable<Movie>? GetMoviesByDirector(int directorId);

        IQueryable<Movie>? GetMoviesByGenre(int genreId);

        IQueryable<Movie>? GetMoviesInSeries(int seriesId);

        IQueryable<Series>? GetSeriesByDirector(int directorId);

        IQueryable<Series>? GetSeriesByGenre(int genreId);

        Series? GetSeriesById(int seriesId);

        void RemoveCountry(int movieId, string countryCode);

        void RemoveCreator(int movieId, int creatorId);

        void RemoveGenre(int movieId, int genreId);

        int UpdateMovie(Movie movie);

        int UpdateSeries(Series series);
    }
}