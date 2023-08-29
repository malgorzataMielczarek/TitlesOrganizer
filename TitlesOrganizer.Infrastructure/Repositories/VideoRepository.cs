using TitlesOrganizer.Domain.Interfaces;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Infrastructure.Repositories
{
    public abstract class VideoRepository : IVideoRepository
    {
        protected readonly Context _context;

        public VideoRepository(Context context)
        {
            _context = context;
        }

        public void DeleteGenre(int genreId)
        {
            var genre = GetGenreById(genreId);
            if (genre != null)
            {
                _context.VideoGenres.Remove(genre);
                _context.SaveChanges();
            }
        }

        public IQueryable<Director> GetAllDirectors() => _context.Directors;

        public IQueryable<VideoGenre> GetAllGenres() => _context.VideoGenres;

        public Director? GetDirectorById(int directorId) => _context.Directors.Find(directorId);

        public VideoGenre? GetGenreById(int genreId) => _context.VideoGenres.Find(genreId);

        public int UpdateDirector(Director director)
        {
            _context.Directors.Update(director);
            if (_context.SaveChanges() == 1)
            {
                return director.Id;
            }

            return -1;
        }

        public int UpdateGenre(VideoGenre genre)
        {
            _context.VideoGenres.Update(genre);
            if (_context.SaveChanges() == 1)
            {
                return genre.Id;
            }

            return -1;
        }
    }
}