using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Domain.Interfaces
{
    public interface IVideoRepository
    {
        void DeleteGenre(int genreId);

        IQueryable<Creator> GetAllDirectors();

        IQueryable<Genre> GetAllGenres();

        Creator? GetDirectorById(int directorId);

        Genre? GetGenreById(int genreId);

        int UpdateDirector(Creator director);

        int UpdateGenre(Genre genre);
    }
}