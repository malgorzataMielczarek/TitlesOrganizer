﻿namespace TitlesOrganizer.Domain.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }

        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}