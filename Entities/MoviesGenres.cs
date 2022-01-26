namespace MoviesAPI.Entities
{
    public class MoviesGenres
    {
        public int GenreID { get; set; }
        public int MovieId { get; set; }
        public Genre Genre { get; set; }
        public Movie Movie { get; set; }

    }
}
