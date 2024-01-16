using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Domain.Interfaces
{
    public interface IVideoRepository
    {
        void DeleteGenre(int genreId);

        IQueryable<Creator> GetAllDirectors();

        IQueryable<VideoGenre> GetAllGenres();

        Creator? GetDirectorById(int directorId);

        VideoGenre? GetGenreById(int genreId);

        int UpdateDirector(Creator director);

        int UpdateGenre(VideoGenre genre);
    }
}