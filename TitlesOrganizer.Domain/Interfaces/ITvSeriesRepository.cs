using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Domain.Interfaces
{
    public interface ITvSeriesRepository : IVideoRepository
    {
        string? AddCountry(int seriesId, string countryCode);

        int AddEpisode(Episode episode);

        int AddExistingDirector(int seriesId, int directorId);

        int AddExistingGenre(int seriesId, int genreId);

        int AddNewCreator(int seriesId, Creator creator);

        int AddNewGenre(int seriesId, VideoGenre genre);

        int AddSeason(Season season);

        int AddSeries(TvSeries series);

        void DeleteEpisode(int episodeId);

        void DeleteSeason(int seasonId);

        void DeleteSeries(int seriesId);

        IQueryable<TvSeries> GetAllSeries();

        IQueryable<Episode>? GetEpisodesOfSeason(int seasonId);

        Episode? GetEpisodeById(int episodeId);

        IQueryable<Season>? GetSeasonsOfSeries(int seriesId);

        Season? GetSeasonById(int seasonId);

        TvSeries? GetSeriesById(int seriesId);

        IQueryable<TvSeries>? GetSeriesByDirector(int directorId);

        IQueryable<TvSeries>? GetSeriesByGenre(int genreId);

        bool IsMoreThenOneSeason(int seriesId);

        void RemoveCountry(int seriesId, string countryCode);

        void RemoveCreator(int seriesId, int creatorId);

        void RemoveGenre(int seriesId, int genreId);

        int UpdateEpisode(Episode episode);

        int UpdateSeason(Season season);

        int UpdateSeries(TvSeries series);
    }
}