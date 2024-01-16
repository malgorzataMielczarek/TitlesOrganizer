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

        int AddNewGenre(int movieId, VideoGenre genre);

        int AddSeries(MovieSeries series);

        void DeleteMovie(int movieId);

        void DeleteSeries(int seriesId);

        IQueryable<Movie> GetAllMovies();

        IQueryable<MovieSeries> GetAllSeries();

        IQueryable<Creator>? GetDirectorsOfSeries(int seriesId);

        IQueryable<VideoGenre>? GetGenresOfSeries(int seriesId);

        Movie? GetMovieById(int movieId);

        IQueryable<Movie>? GetMoviesByDirector(int directorId);

        IQueryable<Movie>? GetMoviesByGenre(int genreId);

        IQueryable<Movie>? GetMoviesInSeries(int seriesId);

        IQueryable<MovieSeries>? GetSeriesByDirector(int directorId);

        IQueryable<MovieSeries>? GetSeriesByGenre(int genreId);

        MovieSeries? GetSeriesById(int seriesId);

        void RemoveCountry(int movieId, string countryCode);

        void RemoveCreator(int movieId, int creatorId);

        void RemoveGenre(int movieId, int genreId);

        int UpdateMovie(Movie movie);

        int UpdateSeries(MovieSeries series);
    }
}