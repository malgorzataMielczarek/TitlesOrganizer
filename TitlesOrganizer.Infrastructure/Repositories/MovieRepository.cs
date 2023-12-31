﻿using Microsoft.IdentityModel.Tokens;
using TitlesOrganizer.Domain.Interfaces;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Infrastructure.Repositories
{
    public class MovieRepository : VideoRepository, IMovieRepository
    {
        public MovieRepository(Context context) : base(context)
        {
        }

        public string? AddCountry(int movieId, string countryCode)
        {
            var movie = GetMovieById(movieId);
            var country = _context.Countries.Find(countryCode);
            if (movie != null && country != null)
            {
                if (!movie.Countries.Contains(country))
                {
                    movie.Countries.Add(country);
                    _context.SaveChanges();
                }

                return countryCode;
            }

            return null;
        }

        public int AddExistingDirector(int movieId, int directorId)
        {
            var movie = GetMovieById(movieId);
            var director = GetDirectorById(directorId);
            if (movie != null && director != null)
            {
                if (!movie.Directors.Contains(director))
                {
                    movie.Directors.Add(director);
                    _context.SaveChanges();
                }

                return directorId;
            }

            return -1;
        }

        public int AddExistingGenre(int movieId, int genreId)
        {
            var movie = GetMovieById(movieId);
            var genre = GetGenreById(genreId);
            if (movie != null && genre != null)
            {
                if (!movie.Genres.Contains(genre))
                {
                    movie.Genres.Add(genre);
                    _context.SaveChanges();
                }

                return genreId;
            }

            return -1;
        }

        public int AddMovie(Movie movie)
        {
            if (!_context.Movies.Contains(movie))
            {
                _context.Movies.Add(movie);
                _context.SaveChanges();
            }

            return movie.Id;
        }

        public int AddNewDirector(int movieId, Director director)
        {
            var movie = GetMovieById(movieId);
            if (!_context.Directors.Contains(director))
            {
                _context.Directors.Add(director);
            }

            if (movie != null)
            {
                movie.Directors.Add(director);

                _context.SaveChanges();

                return director.Id;
            }

            return -1;
        }

        public int AddNewGenre(int movieId, VideoGenre genre)
        {
            if (!_context.VideoGenres.Contains(genre))
            {
                _context.VideoGenres.Add(genre);
            }

            var movie = GetMovieById(movieId);
            if (movie != null)
            {
                movie.Genres.Add(genre);
            }

            _context.SaveChanges();

            return genre.Id;
        }

        public int AddSeries(MovieSeries series)
        {
            if (!_context.MovieSeries.Contains(series))
            {
                _context.MovieSeries.Add(series);
                _context.SaveChanges();
            }

            return series.Id;
        }

        public void DeleteMovie(int movieId)
        {
            var movie = GetMovieById(movieId);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                _context.SaveChanges();
            }
        }

        public void DeleteSeries(int seriesId)
        {
            var series = GetSeriesById(seriesId);
            if (series != null)
            {
                _context.MovieSeries.Remove(series);
                _context.SaveChanges();
            }
        }

        public IQueryable<Movie> GetAllMovies() => _context.Movies;

        public IQueryable<MovieSeries> GetAllSeries() => _context.MovieSeries;

        public IQueryable<Director>? GetDirectorsOfSeries(int seriesId) => GetSeriesById(seriesId)?.Movies.SkipWhile(m => m.Directors.IsNullOrEmpty()).SelectMany(m => m.Directors).AsQueryable();

        public IQueryable<VideoGenre>? GetGenresOfSeries(int seriesId) => GetSeriesById(seriesId)?.Movies.SkipWhile(m => m.Genres.IsNullOrEmpty()).SelectMany(m => m.Genres).AsQueryable();

        public Movie? GetMovieById(int movieId) => _context.Movies.Find(movieId);

        public IQueryable<Movie>? GetMoviesByDirector(int directorId) => GetDirectorById(directorId)?.Movies?.AsQueryable();

        public IQueryable<Movie>? GetMoviesByGenre(int genreId) => GetGenreById(genreId)?.Movies?.AsQueryable();

        public IQueryable<Movie>? GetMoviesInSeries(int seriesId) => GetSeriesById(seriesId)?.Movies.AsQueryable();

        public IQueryable<MovieSeries>? GetSeriesByDirector(int directorId) => GetDirectorById(directorId)?.Movies?.SkipWhile(m => m.Series == null).Select(m => m.Series!).AsQueryable();

        public IQueryable<MovieSeries>? GetSeriesByGenre(int genreId) => GetGenreById(genreId)?.Movies?.SkipWhile(m => m.Series == null).Select(m => m.Series!).AsQueryable();

        public MovieSeries? GetSeriesById(int seriesId) => _context.MovieSeries.Find(seriesId);

        public void RemoveCountry(int movieId, string countryCode)
        {
            var movie = GetMovieById(movieId);
            var country = _context.Countries.Find(countryCode);

            if (movie != null && country != null)
            {
                movie.Countries.Remove(country);
                _context.SaveChanges();
            }
        }

        public void RemoveDirector(int movieId, int directorId)
        {
            var movie = GetMovieById(movieId);
            if (movie != null)
            {
                var director = movie.Directors.FirstOrDefault(d => d.Id == directorId);
                if (director != null)
                {
                    movie.Directors.Remove(director);
                    director.Movies?.Remove(movie);

                    if (director.TvSeries.IsNullOrEmpty() && director.Movies.IsNullOrEmpty())
                    {
                        _context.Directors.Remove(director);
                    }

                    _context.SaveChanges();
                }
            }
        }

        public void RemoveGenre(int movieId, int genreId)
        {
            var movie = GetMovieById(movieId);
            var genre = GetGenreById(genreId);
            if (movie != null && genre != null)
            {
                movie.Genres.Remove(genre);
                _context.SaveChanges();
            }
        }

        public int UpdateMovie(Movie movie)
        {
            _context.Movies.Update(movie);
            if (_context.SaveChanges() == 1)
            {
                return movie.Id;
            }

            return -1;
        }

        public int UpdateSeries(MovieSeries series)
        {
            _context.MovieSeries.Update(series);
            if (_context.SaveChanges() == 1)
            {
                return series.Id;
            }

            return -1;
        }
    }
}