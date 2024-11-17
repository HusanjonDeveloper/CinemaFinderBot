using System.IO;
using System.Text.Json;
using MovieBot.Models;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace MovieBot.Services;

public class MovieService
{
    private readonly string _apiKey;

    public MovieService()
    { 
        string appSettingsContent = File.ReadAllText("AppSettings.json");
        var appSettings = JsonSerializer.Deserialize<AppSettings>(appSettingsContent);
    }

    public async Task<string> SearchMovie(string movieName)
    {
        using var httpClient = new HttpClient();
        string requestUrl = $"https://api.themoviedb.org/3/search/movie?api_key={_apiKey}&query={Uri.EscapeDataString(movieName)}";

        var response = await httpClient.GetAsync(requestUrl);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<TmdbResponse>(content);
          
            if (result.Results.Any())
            {
                var movie = result.Results.First();
                return $"Kino nomi: {movie.Title}\n" +
                       $"Sanasi: {movie.ReleaseDate}\n" +
                       $"Reyting: {movie.VoteAverage}\n" +
                       $"Ma'lumot: https://www.themoviedb.org/movie/{movie.Id}";
            }
            return "Kino topilmadi.";
        }
        return "Xatolik yuz berdi. Keyinroq qayta urinib ko'ring.";
    }
}