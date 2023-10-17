﻿using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Application.ViewModels.BookVMs.Details;
using TitlesOrganizer.Application.ViewModels.BookVMs.ForList;
using TitlesOrganizer.Application.ViewModels.Common;

namespace TitlesOrganizer.Application.Interfaces
{
    public interface IBookService
    {
        int AddAuthor(AuthorVM author);

        void SelectAuthorsForBook(ListAuthorForBookVM listAuthorForBook);

        int UpsertBook(BookVM book);

        int AddGenre(GenreVM genre);

        int AddGenre(int bookId, GenreVM genre);

        void AddGenresForBook(int bookId, List<int> genresIds);

        int AddNewSeries(NewSeriesVM newSeries);

        void AddSeriesForBook(int bookId, int seriesId);

        void DeleteBook(int id);

        ListAuthorForBookVM GetAllAuthorsForBookList(int bookId, ViewModels.Common.SortByEnum sortBy, int pageSize, int pageNo, string searchString);

        ListAuthorForListVM GetAllAuthorsForList(ViewModels.Common.SortByEnum sortBy, int pageSize, int pageNo, string searchString);

        ListBookForListVM GetAllBooksForList(ViewModels.Common.SortByEnum sortBy, int pageSize, int pageNo, string searchString);

        ListGenreVM GetAllGenres(ViewModels.Common.SortByEnum sortBy, int pageSize, int pageNo, string searchString);

        ListGenreForBookVM GetAllGenresForBookList(int bookId, ViewModels.Common.SortByEnum sortBy, int pageSize, int pageNo, string searchString);

        ListSeriesForBookVM GetAllSeriesForBookList(int bookId, SortByEnum sortBy, int pageSize, int pageNo, string searchString);

        ListSeriesForListVM GetAllSeriesForList(SortByEnum sortBy, int pageSize, int pageNo, string searchString);

        AuthorDetailsVM GetAuthorDetails(int id, ViewModels.Common.SortByEnum sortBy, int pageSize, int pageNo, string searchString);

        BookDetailsVM GetBookDetails(int id);

        GenreDetailsVM GetGenreDetails(int id, ViewModels.Common.SortByEnum sortBy, int pageSize, int pageNo, string searchString);

        SeriesDetailsVM GetSeriesDetails(int id, ViewModels.Common.SortByEnum sortBy, int pageSize, int pageNo, string searchString);

        BookVM GetBook(int id);

        ListAuthorForBookVM GetAllAuthorsForBookList(ListAuthorForBookVM listAuthorForBook);
    }
}