using AutoMapper;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using NetTopologySuite.Geometries;

namespace MoviesAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
       
        public AutoMapperProfiles(GeometryFactory geometryFactory)
        {
            CreateMap<GenreDTO, Genre>().ReverseMap();
            CreateMap<GenreCreationDTO, Genre>();
            CreateMap<ActorDTO, Actor>().ReverseMap();
            CreateMap<ActorCreationDTO, Actor>().ForMember(x => x.Picture, options => options.Ignore());

            CreateMap<MovieTheater, MovieTheaterDTO>()
                .ForMember(x => x.Latitude, dto => dto.MapFrom(prop => prop.Location.Y))
                .ForMember(x => x.Longitude, dto => dto.MapFrom(prop => prop.Location.X));

            CreateMap<MovieTheaterCreationDTO, MovieTheater>()
                .ForMember(x=> x.Location, x=> x.MapFrom(dto=> geometryFactory.CreatePoint(new Coordinate(dto.Longitude, dto.Latitude))));

            CreateMap<MovieCreationDTO, Movie>()
                .ForMember(x => x.Poster, options => options.Ignore())
                .ForMember(x => x.MoviesGenres, options => options.MapFrom(MapMoviesGenres))
                .ForMember(x => x.MovieTheatersMovie, options => options.MapFrom(MapMovieTheatersMovies))
                .ForMember(x => x.MoviesActors, options => options.MapFrom(MapMoviesActors));

            CreateMap<Movie, MovieDTO>()
                .ForMember(x => x.Genres, options => options.MapFrom(MapMoviesGenres))
                .ForMember(x => x.MovieTheaters, options => options.MapFrom(MapMoviesTheaters))
                .ForMember(x => x.Actors, options => options.MapFrom(MapMoviesActors));
        }

        private List<GenreDTO> MapMoviesGenres(Movie movie, MovieDTO movieDTO)
        {
            var result = new List<GenreDTO>();

            if(movie.MoviesGenres != null)
            {
                foreach(var genre in movie.MoviesGenres)
                {
                    result.Add(new GenreDTO() { Id = genre.GenreID, Name = genre.Genre.Name });
                }
                return result;
            }
            return result;
        }

        private List<MovieTheaterDTO> MapMoviesTheaters(Movie movie, MovieDTO movieDTO)
        {
            var result = new List<MovieTheaterDTO>();

            if (movie.MoviesGenres != null)
            {
                foreach (var theater in movie.MovieTheatersMovie)
                {
                    result.Add(new MovieTheaterDTO() { Id = theater.MovieTheaterId, 
                        Name = theater.MovieTheater.Name,
                        Latitude = theater.MovieTheater.Location.Y,
                        Longitude = theater.MovieTheater.Location.X});

                }
                return result;
            }
            return result;
        }

        private List<ActorsMovieDTO> MapMoviesActors(Movie movie, MovieDTO movieDTO)
        {
            var result = new List<ActorsMovieDTO>();

            if (movie.MoviesActors != null)
            {
                foreach (var actor in movie.MoviesActors)
                {
                    result.Add(new ActorsMovieDTO() { 
                        Id = actor.ActorId, 
                        Name = actor.Actor.Name,
                        Character = actor.Character,
                        Picture = actor.Actor.Picture,
                        Order = actor.Order});
                }
                return result;
            }
            return result;
        }

        private List<MoviesGenres> MapMoviesGenres(MovieCreationDTO movieCreationDTO, Movie movie)
        {
            var result = new List<MoviesGenres>();

            if(movieCreationDTO.GenresIds == null) return result;

            foreach(var id in movieCreationDTO.GenresIds)
            {
                result.Add(new MoviesGenres() { GenreID = id });
            }
            return result;
        }

        private List<MovieTheatersMovie> MapMovieTheatersMovies(MovieCreationDTO movieCreationDTO, Movie movie)
        {
            var result = new List<MovieTheatersMovie>();

            if (movieCreationDTO.MovieTheaterIds == null) return result;

            foreach (var id in movieCreationDTO.MovieTheaterIds)
            {
                result.Add(new MovieTheatersMovie() { MovieTheaterId = id });
            }
            return result;
        }

        private List<MoviesActors> MapMoviesActors(MovieCreationDTO movieCreationDTO, Movie movie)
        {
            var result = new List<MoviesActors>();

            if (movieCreationDTO.Actors == null) return result;

            foreach (var actor in movieCreationDTO.Actors)
            {
                result.Add(new MoviesActors() { ActorId = actor.Id, Character = actor.Character });
            }
            return result;
        }
    }
}
