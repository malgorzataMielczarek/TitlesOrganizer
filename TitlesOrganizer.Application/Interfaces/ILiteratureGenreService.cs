﻿using TitlesOrganizer.Application.ViewModels.Base;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.Interfaces
{
    public interface ILiteratureGenreService
    {
        void Delete(int id);

        GenreVM Get(int id, int booksPageSize, int booksPageNo);

        GenreDetailsVM GetDetails(int id, int booksPageSize, int booksPageNo, int authorsPageSize, int authorsPageNo, int seriesPageSize, int seriesPageNo);

        ListGenreForListVM GetList(SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        ListGenreForBookVM GetListForBook(int bookId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        PartialList<LiteratureGenre> GetPartialListForAuthor(int authorId, int pageSize, int pageNo);

        void SelectBooks(int genreId, int[] selectedIds);

        int Upsert(GenreVM genre);
    }
}