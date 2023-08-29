using Microsoft.IdentityModel.Tokens;
using TitlesOrganizer.Domain.Interfaces;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Infrastructure.Repositories
{
    public class TvSeriesRepository : VideoRepository, ITvSeriesRepository
    {
        public TvSeriesRepository(Context context) : base(context)
        {
        }

        public string? AddCountry(int seriesId, string countryCode)
        {
            var series = GetSeriesById(seriesId);
            var country = _context.Countries.Find(countryCode);
            if (series != null && country != null)
            {
                if (!series.Countries.Contains(country))
                {
                    series.Countries.Add(country);
                    _context.SaveChanges();
                }

                return countryCode;
            }

            return null;
        }

        public int AddEpisode(Episode episode)
        {
            if (!_context.Episodes.Contains(episode))
            {
                _context.Episodes.Add(episode);
                _context.SaveChanges();
            }

            return episode.Id;
        }

        public int AddExistingDirector(int seriesId, int directorId)
        {
            var series = GetSeriesById(seriesId);
            var director = GetDirectorById(directorId);
            if (series != null && director != null)
            {
                if (!series.Directors.Contains(director))
                {
                    series.Directors.Add(director);
                    _context.SaveChanges();
                }

                return directorId;
            }

            return -1;
        }

        public int AddExistingGenre(int seriesId, int genreId)
        {
            var series = GetSeriesById(seriesId);
            var genre = GetGenreById(genreId);
            if (series != null && genre != null)
            {
                if (!series.Genres.Contains(genre))
                {
                    series.Genres.Add(genre);
                    _context.SaveChanges();
                }

                return genreId;
            }

            return -1;
        }

        public int AddNewDirector(int seriesId, Director director)
        {
            var series = GetSeriesById(seriesId);
            if (!_context.Directors.Contains(director))
            {
                _context.Directors.Add(director);
            }

            if (series != null)
            {
                series.Directors.Add(director);

                _context.SaveChanges();

                return director.Id;
            }

            return -1;
        }

        public int AddNewGenre(int seriesId, VideoGenre genre)
        {
            if (!_context.VideoGenres.Contains(genre))
            {
                _context.VideoGenres.Add(genre);
            }

            var series = GetSeriesById(seriesId);
            if (series != null)
            {
                series.Genres.Add(genre);
            }

            _context.SaveChanges();

            return genre.Id;
        }

        public int AddSeason(Season season)
        {
            if (!_context.Seasons.Contains(season))
            {
                _context.Seasons.Add(season);
                _context.SaveChanges();
            }

            return season.Id;
        }

        public int AddSeries(TvSeries series)
        {
            if (!_context.TvSeries.Contains(series))
            {
                _context.TvSeries.Add(series);
                _context.SaveChanges();
            }

            return series.Id;
        }

        public void DeleteEpisode(int episodeId)
        {
            var episode = GetEpisodeById(episodeId);
            if (episode != null)
            {
                _context.Episodes.Remove(episode);
                _context.SaveChanges();
            }
        }

        public void DeleteSeason(int seasonId)
        {
            var season = GetSeasonById(seasonId);
            if (season != null)
            {
                _context.Seasons.Remove(season);
                _context.SaveChanges();
            }
        }

        public void DeleteSeries(int seriesId)
        {
            var series = GetSeriesById(seriesId);
            if (series != null)
            {
                _context.TvSeries.Remove(series);
                _context.SaveChanges();
            }
        }

        public IQueryable<TvSeries> GetAllSeries() => _context.TvSeries;

        public Episode? GetEpisodeById(int episodeId) => _context.Episodes.Find(episodeId);

        public IQueryable<Episode>? GetEpisodesOfSeason(int seasonId) => GetSeasonById(seasonId)?.Episodes.AsQueryable();

        public Season? GetSeasonById(int seasonId) => _context.Seasons.Find(seasonId);

        public IQueryable<Season>? GetSeasonsOfSeries(int seriesId) => GetSeriesById(seriesId)?.Seasons.AsQueryable();

        public IQueryable<TvSeries>? GetSeriesByDirector(int directorId) => GetDirectorById(directorId)?.TvSeries?.AsQueryable();

        public IQueryable<TvSeries>? GetSeriesByGenre(int genreId) => GetGenreById(genreId)?.TvSeries?.AsQueryable();

        public TvSeries? GetSeriesById(int seriesId) => _context.TvSeries.Find(seriesId);

        public bool IsMoreThenOneSeason(int seriesId)
        {
            return (GetSeasonsOfSeries(seriesId)?.Count() ?? 0) > 1;
        }

        public void RemoveCountry(int seriesId, string countryCode)
        {
            var series = GetSeriesById(seriesId);
            var country = _context.Countries.Find(countryCode);

            if (series != null && country != null)
            {
                series.Countries.Remove(country);
                _context.SaveChanges();
            }
        }

        public void RemoveDirector(int seriesId, int directorId)
        {
            var series = GetSeriesById(seriesId);
            if (series != null)
            {
                var director = series.Directors.FirstOrDefault(d => d.Id == directorId);
                if (director != null)
                {
                    series.Directors.Remove(director);
                    director.TvSeries?.Remove(series);

                    if (director.Movies.IsNullOrEmpty() && director.TvSeries.IsNullOrEmpty())
                    {
                        _context.Directors.Remove(director);
                    }

                    _context.SaveChanges();
                }
            }
        }

        public void RemoveGenre(int seriesId, int genreId)
        {
            var series = GetSeriesById(seriesId);
            var genre = GetGenreById(genreId);
            if (series != null && genre != null)
            {
                series.Genres.Remove(genre);
                _context.SaveChanges();
            }
        }

        public int UpdateEpisode(Episode episode)
        {
            _context.Episodes.Update(episode);
            if (_context.SaveChanges() == 1)
            {
                return episode.Id;
            }

            return -1;
        }

        public int UpdateSeason(Season season)
        {
            _context.Seasons.Update(season);
            if (_context.SaveChanges() == 1)
            {
                return season.Id;
            }

            return -1;
        }

        public int UpdateSeries(TvSeries series)
        {
            _context.TvSeries.Update(series);
            if (_context.SaveChanges() == 1)
            {
                return series.Id;
            }

            return -1;
        }
    }
}