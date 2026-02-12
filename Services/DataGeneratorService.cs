using Bogus;
using MusicStoreApp.Models;

namespace MusicStoreApp.Services;

public interface IDataGeneratorService
{
    Song GenerateSong(int index, long baseSeed, string language, double averageLikes);
    List<Song> GenerateSongPage(int page, int pageSize, long baseSeed, string language, double averageLikes);
}

public class DataGeneratorService : IDataGeneratorService
{
    private readonly ICoverGeneratorService _coverGenerator;

    public DataGeneratorService(ICoverGeneratorService coverGenerator)
    {
        _coverGenerator = coverGenerator;
    }

    private readonly string[] Words = { "Dream", "Night", "Shadow", "Echo", "Fading", "Burning", "Electric", "Velvet", "Silver", "Crimson" };
    private readonly string[] Artists = { "Orchestra", "Band", "Hearts", "Giants", "Drivers", "Parade" };
    private readonly string[] Albums = { "Polaroid", "Signals", "Voyage", "Downtown", "Blueprint", "Transit" };
    private readonly string[] Genres = { "Ambient", "Progressive Rock", "Dream Pop", "Disco Funk", "Neo Soul", "Electro House" };

    public Song GenerateSong(int index, long baseSeed, string language, double averageLikes)
    {
        var songSeed = baseSeed + index;
        var random = new Random((int)(songSeed & 0x7FFFFFFF));

        var title = $"{Words[random.Next(Words.Length)]} {Words[random.Next(Words.Length)]}";
        var artist = $"{Words[random.Next(Words.Length)]} {Artists[random.Next(Artists.Length)]}";
        var album = $"{Albums[random.Next(Albums.Length)]} {Words[random.Next(Words.Length)]}";
        var genre = Genres[random.Next(Genres.Length)];

        var likes = (int)Math.Round(random.NextDouble() * 10);

        var song = new Song
        {
            Index = index,
            Title = title,
            Artist = artist,
            Album = album,
            Genre = genre,
            Likes = likes,
            CoverImageBase64 = _coverGenerator.GenerateCover(title, artist, (int)songSeed)
        };

        return song;
    }

    public List<Song> GenerateSongPage(int page, int pageSize, long baseSeed, string language, double averageLikes)
    {
        var songs = new List<Song>();
        var startIndex = (page - 1) * pageSize + 1;
        for (int i = 0; i < pageSize; i++)
        {
            songs.Add(GenerateSong(startIndex + i, baseSeed, language, averageLikes));
        }
        return songs;
    }
}
