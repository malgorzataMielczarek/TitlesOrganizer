using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Domain.Interfaces
{
    public interface IVideoRepository
    {
        void DeleteGenre(int genreId);

        IQueryable<Director> GetAllDirectors();

        IQueryable<VideoGenre> GetAllGenres();

        Director? GetDirectorById(int directorId);

        VideoGenre? GetGenreById(int genreId);

        int UpdateDirector(Director director);

        int UpdateGenre(VideoGenre genre);
    }
}