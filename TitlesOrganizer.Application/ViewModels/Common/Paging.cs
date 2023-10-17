﻿namespace TitlesOrganizer.Application.ViewModels.Common
{
    public class Paging
    {
        public int Count { get; set; }
        public int PageSize { get; set; } = 5;
        public int CurrentPage { get; set; }
    }
}