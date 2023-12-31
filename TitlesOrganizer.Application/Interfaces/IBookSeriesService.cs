﻿using TitlesOrganizer.Application.ViewModels.Base;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.Interfaces
{
    public interface IBookSeriesService
    {
        void Delete(int id);

        SeriesVM Get(int id, int booksPageSize, int booksPageNo);

        SeriesDetailsVM GetDetails(int id, int booksPageSize, int booksPageNo);

        ListSeriesForListVM GetList(SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        ListSeriesForBookVM GetListForBook(int bookId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        PartialList<BookSeries> GetPartialListForAuthor(int authorId, int pageSize, int pageNo);

        PartialList<BookSeries> GetPartialListForGenre(int genreId, int pageSize, int pageNo);

        void SelectBooks(int seriesId, int[] selectedIds);

        int Upsert(SeriesVM series);
    }
}