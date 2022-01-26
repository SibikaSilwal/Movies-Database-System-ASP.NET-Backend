namespace MoviesAPI.Entities
{
    public class MovieTheatersMovie
    {
        public int MovieID { get; set; }
        public int MovieTheaterId { get; set; }
        public Movie Movie { get; set; }
        public MovieTheater MovieTheater { get; set; }
    }
}
