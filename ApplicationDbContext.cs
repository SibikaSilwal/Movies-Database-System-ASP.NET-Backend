using Microsoft.EntityFrameworkCore;
using MoviesAPI.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MoviesAPI
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext([NotNullAttribute]DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // indicating the composite primary key for MoviesActors table
            modelBuilder.Entity<MoviesActors>().HasKey(x=> new {x.ActorId, x.MovieId});

            modelBuilder.Entity<MoviesGenres>().HasKey(x => new { x.GenreID, x.MovieId });

            modelBuilder.Entity<MovieTheatersMovie>().HasKey(x => new { x.MovieTheaterId, x.MovieID });

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieTheater> Theaters { get; set; }
        public DbSet<MoviesActors> MoviesActors { get; set; }
        public DbSet<MoviesGenres> MoviesGenres { get; set; }
        public DbSet<MovieTheatersMovie> MovieTheatersMovies { get; set; }

    }
}
