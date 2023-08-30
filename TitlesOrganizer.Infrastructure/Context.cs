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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relations
            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Countries)
                .WithMany();
            modelBuilder.Entity<TvSeries>()
                .HasMany(s => s.Countries)
                .WithMany();

            // Data types
            modelBuilder.Entity<Author>().Property(a => a.Name).HasMaxLength(50);
            modelBuilder.Entity<Author>().Property(a => a.LastName).HasMaxLength(50);

            modelBuilder.Entity<Book>().Property(b => b.Title).HasMaxLength(450);
            modelBuilder.Entity<Book>().Property(b => b.OriginalTitle).HasMaxLength(450);
            modelBuilder.Entity<Book>().Property(b => b.Edition).HasMaxLength(50);
            modelBuilder.Entity<Book>().Property(b => b.Description).HasMaxLength(4000);

            modelBuilder.Entity<BookSeries>().Property(bs => bs.Title).HasMaxLength(450);
            modelBuilder.Entity<BookSeries>().Property(bs => bs.OriginalTitle).HasMaxLength(450);
            modelBuilder.Entity<BookSeries>().Property(bs => bs.Description).HasMaxLength(4000);

            modelBuilder.Entity<Country>().Property(c => c.Code).HasColumnType("char(3)").HasMaxLength(3);
            modelBuilder.Entity<Country>().Property(c => c.Name).HasColumnType("varchar(25)").HasMaxLength(25);

            modelBuilder.Entity<Director>().Property(d => d.Name).HasMaxLength(50);
            modelBuilder.Entity<Director>().Property(d => d.LastName).HasMaxLength(50);

            modelBuilder.Entity<Episode>().Property(e => e.Title).HasMaxLength(450);
            modelBuilder.Entity<Episode>().Property(e => e.OriginalTitle).HasMaxLength(450);
            modelBuilder.Entity<Episode>().Property(e => e.Description).HasMaxLength(4000);

            modelBuilder.Entity<Language>().Property(lang => lang.Code).HasColumnType("char(3)").HasMaxLength(3);
            modelBuilder.Entity<Language>().Property(lang => lang.Name).HasColumnType("varchar(25)").HasMaxLength(25);

            modelBuilder.Entity<LiteratureGenre>().Property(lg => lg.Name).HasMaxLength(50);

            modelBuilder.Entity<Movie>().Property(m => m.Title).HasMaxLength(450);
            modelBuilder.Entity<Movie>().Property(m => m.OriginalTitle).HasMaxLength(450);
            modelBuilder.Entity<Movie>().Property(m => m.Description).HasMaxLength(4000);

            modelBuilder.Entity<MovieSeries>().Property(ms => ms.Title).HasMaxLength(450);
            modelBuilder.Entity<MovieSeries>().Property(ms => ms.OriginalTitle).HasMaxLength(450);
            modelBuilder.Entity<MovieSeries>().Property(ms => ms.Description).HasMaxLength(4000);

            modelBuilder.Entity<Season>().Property(s => s.Title).HasMaxLength(450);
            modelBuilder.Entity<Season>().Property(s => s.OriginalTitle).HasMaxLength(450);
            modelBuilder.Entity<Season>().Property(s => s.Description).HasMaxLength(4000);

            modelBuilder.Entity<TvSeries>().Property(ts => ts.Title).HasMaxLength(450);
            modelBuilder.Entity<TvSeries>().Property(ts => ts.OriginalTitle).HasMaxLength(450);
            modelBuilder.Entity<TvSeries>().Property(ts => ts.Description).HasMaxLength(4000);

            modelBuilder.Entity<VideoGenre>().Property(vg => vg.Name).HasMaxLength(50);
        }
    }
}