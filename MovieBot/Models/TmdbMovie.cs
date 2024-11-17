namespace MovieBot.Models;

public class TmdbMovie
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public string ReleaseDate { get; set; }
    
    public double VoteAverage { get; set; }
}