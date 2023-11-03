﻿using System.ComponentModel;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Base;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class GenreForListVM : BaseForListVM<LiteratureGenre>, IForListVM<LiteratureGenre>
    {
        [DisplayName("Genre")]
        public override string Description { get; set; } = null!;
    }

    public class ListGenreForListVM : BaseListVM<LiteratureGenre>, IListVM<LiteratureGenre>
    {
        [DisplayName("Genres")]
        public override List<IForListVM<LiteratureGenre>> Values { get; set; } = new List<IForListVM<LiteratureGenre>>();
    }

    public static partial class MappingExtensions
    {
        public static GenreForListVM Map(this LiteratureGenre genre)
        {
            return new GenreForListVM()
            {
                Id = genre.Id,
                Description = genre.Name
            };
        }

        public static IForListVM<T> Map<T>(this LiteratureGenre genre)
            where T : LiteratureGenre
        {
            return (IForListVM<T>)genre.Map();
        }

        public static IQueryable<IForListVM<LiteratureGenre>> Map(this IQueryable<LiteratureGenre> items)
        {
            return items.Select(it => it.Map());
        }

        public static List<IForListVM<LiteratureGenre>> Map(this IEnumerable<LiteratureGenre> items)
        {
            return items.Select(it => it.Map<LiteratureGenre>()).ToList();
        }

        public static ListGenreForListVM MapToList(this IQueryable<LiteratureGenre> genres, Paging paging, Filtering filtering)
        {
            return (ListGenreForListVM)genres
                .Sort(filtering.SortBy, g => g.Name)
                .Map()
                .MapToList<LiteratureGenre, ListGenreForListVM>(paging, filtering);
        }

        public static IQueryable<IForListVM<LiteratureGenre>> MapToList(this IQueryable<LiteratureGenre> genres, ref Paging paging)
        {
            return genres
                .OrderBy(g => g.Name)
                .Map()
                .MapToList<LiteratureGenre>(ref paging);
        }

        public static List<IForListVM<LiteratureGenre>> MapToList(this IEnumerable<LiteratureGenre> genres, ref Paging paging)
        {
            return genres
                .OrderBy(g => g.Name)
                .Map()
                .MapToList<LiteratureGenre>(ref paging);
        }
    }
}