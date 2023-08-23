﻿namespace TitlesOrganizer.Domain.Models
{
    public class BookSeries
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}