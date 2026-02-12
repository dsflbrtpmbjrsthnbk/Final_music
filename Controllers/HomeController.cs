using Microsoft.AspNetCore.Mvc;
using MusicStoreApp.Models;
using MusicStoreApp.Services;

namespace MusicStoreApp.Controllers;

public class HomeController : Controller
{
    private readonly IDataGeneratorService _dataGenerator;
    private readonly ICoverGeneratorService _coverGenerator;
    private readonly IMusicGeneratorService _musicGenerator;

    public HomeController(IDataGeneratorService dataGenerator, ICoverGeneratorService coverGenerator, IMusicGeneratorService musicGenerator)
    {
        _dataGenerator = dataGenerator;
        _coverGenerator = coverGenerator;
        _musicGenerator = musicGenerator;
    }

    public IActionResult Index() => View();

    [HttpGet]
    public IActionResult GetSongs([FromQuery] SongPageRequest request)
    {
        var songs = _dataGenerator.GenerateSongPage(request.Page, request.PageSize, request.Seed, request.Language, request.AverageLikes);

        var response = new SongPageResponse
        {
            Songs = songs,
            CurrentPage = request.Page,
            TotalPages = 100
        };

        return Json(response);
    }

    [HttpGet]
    public IActionResult GetSongDetails(int index, long seed, string language, double averageLikes)
    {
        var song = _dataGenerator.GenerateSong(index, seed, language, averageLikes);
        song.CoverImageBase64 = _coverGenerator.GenerateCover(song.Title, song.Artist, (int)((seed + index) & 0x7FFFFFFF));
        return Json(song);
    }

    [HttpGet]
    public IActionResult GetAudio(int index, long seed)
    {
        var audioData = _musicGenerator.GenerateSong((int)((seed + index) & 0x7FFFFFFF));
        return File(audioData, "audio/wav");
    }
}
