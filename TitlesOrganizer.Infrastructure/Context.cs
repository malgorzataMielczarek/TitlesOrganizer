using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Infrastructure
{
    public class Context : IdentityDbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookSeries> BookSeries { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<LiteratureGenre> LiteratureGenres { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieSeries> MovieSeries { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<TvSeries> TvSeries { get; set; }
        public DbSet<VideoGenre> VideoGenres { get; set; }

        public Context(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}