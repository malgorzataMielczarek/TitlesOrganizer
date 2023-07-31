using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Infrastructure
{
    public class Context : IdentityDbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<BookAuthor> BookAuthor { get; set; }
        public DbSet<BookGenre> BookGenre { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Creator> Creators { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<EpisodeCountry> EpisodeCounty { get; set; }
        public DbSet<EpisodeDirector> EpisodeDirector { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieCountry> MovieCountry { get; set; }
        public DbSet<MovieDirector> MovieDirector { get; set; }
        public DbSet<MovieGenre> MovieGenre { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<Series> Serieses { get; set; }

        public Context(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<BookAuthor>()
                .HasKey(it => new { it.AuthorId, it.BookId });
            builder.Entity<BookAuthor>()
                .HasOne<Book>(it => it.Book)
                .WithMany(i => i.BookAuthors)
                .HasForeignKey(it => it.BookId);
            builder.Entity<BookAuthor>()
                .HasOne<Creator>(it => it.Author)
                .WithMany(i => i.AuthorBooks)
                .HasForeignKey(it => it.AuthorId);

            builder.Entity<BookGenre>()
                .HasKey(it => new { it.BookId, it.GenreId });
            builder.Entity<BookGenre>()
                .HasOne<Book>(it => it.Book)
                .WithMany(i => i.BookGenres)
                .HasForeignKey(it => it.BookId);
            builder.Entity<BookGenre>()
                .HasOne<Genre>(it => it.Genre)
                .WithMany(i => i.GenreBooks)
                .HasForeignKey(it => it.GenreId);

            builder.Entity<EpisodeCountry>()
                .HasKey(it => new { it.CountryId, it.EpisodeId });
            builder.Entity<EpisodeCountry>()
                .HasOne<Episode>(it => it.Episode)
                .WithMany(i => i.EpisodeCountries)
                .HasForeignKey(it => it.EpisodeId);
            builder.Entity<EpisodeCountry>()
                .HasOne<Country>(it => it.Country)
                .WithMany(i => i.CountryEpisodes)
                .HasForeignKey(it => it.CountryId);

            builder.Entity<EpisodeDirector>()
                .HasKey(it => new { it.EpisodeId, it.DirectorId });
            builder.Entity<EpisodeDirector>()
                .HasOne<Episode>(it => it.Episode)
                .WithMany(i => i.EpisodeDirectors)
                .HasForeignKey(it => it.EpisodeId);
            builder.Entity<EpisodeDirector>()
                .HasOne<Creator>(it => it.Director)
                .WithMany(i => i.DirectorEpisodes)
                .HasForeignKey(it => it.DirectorId);

            builder.Entity<MovieCountry>()
                .HasKey(it => new { it.CountryId, it.MovieId });
            builder.Entity<MovieCountry>()
                .HasOne<Movie>(it => it.Movie)
                .WithMany(i => i.MovieCountries)
                .HasForeignKey(it => it.MovieId);
            builder.Entity<MovieCountry>()
                .HasOne<Country>(it => it.Country)
                .WithMany(i => i.CountryMovies)
                .HasForeignKey(it => it.CountryId);

            builder.Entity<MovieDirector>()
                .HasKey(it => new { it.MovieId, it.DirectorId });
            builder.Entity<MovieDirector>()
                .HasOne<Movie>(it => it.Movie)
                .WithMany(i => i.MovieDirectors)
                .HasForeignKey(it => it.MovieId);
            builder.Entity<MovieDirector>()
                .HasOne<Creator>(it => it.Director)
                .WithMany(i => i.DirectorMovies)
                .HasForeignKey(it => it.DirectorId);

            builder.Entity<MovieGenre>()
                .HasKey(it => new { it.MovieId, it.GenreId });
            builder.Entity<MovieGenre>()
                .HasOne<Movie>(it => it.Movie)
                .WithMany(i => i.MovieGenres)
                .HasForeignKey(it => it.MovieId);
            builder.Entity<MovieGenre>()
                .HasOne<Genre>(it => it.Genre)
                .WithMany(i => i.GenreMovies)
                .HasForeignKey(it => it.GenreId);
        }
    }
}