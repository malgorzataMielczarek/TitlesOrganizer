﻿namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class AuthorForList
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Books { get; set; } = null!;
    }
}