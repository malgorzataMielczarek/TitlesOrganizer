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

        public IQueryable<Creator> GetAllDirectors() => _context.Creators
            .Where(c => c.Profession.HasFlag(Domain.Models.Enums.Profession.Director));

        public IQueryable<VideoGenre> GetAllGenres() => _context.VideoGenres;

        public Creator? GetDirectorById(int directorId) => _context.Creators
            .FirstOrDefault(c => c.Id == directorId && c.Profession.HasFlag(Domain.Models.Enums.Profession.Director));

        public VideoGenre? GetGenreById(int genreId) => _context.VideoGenres.Find(genreId);

        public int UpdateDirector(Creator director)
        {
            _context.Creators.Update(director);
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