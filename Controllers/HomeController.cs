using Microsoft.AspNetCore.Mvc;
using MusicStoreApp.Models;
using MusicStoreApp.Services;

namespace MusicStoreApp.Controllers;

public class HomeController : Controller
{
    private readonly IDataGeneratorService _dataGenerator;
    private readonly IMusicGeneratorService _musicGenerator;

    public HomeController(IDataGeneratorService dataGenerator, IMusicGeneratorService musicGenerator)
    {
        _dataGenerator = dataGenerator;
        _musicGenerator = musicGenerator;
    }

    public IActionResult Index() => View();

    [HttpGet]
    public IActionResult GetSongs(int page = 1, int pageSize = 10, long seed = 123456, string language = "en-US", double averageLikes = 3.5)
    {
        var songs = _dataGenerator.GenerateSongPage(page, pageSize, seed, language, averageLikes);
        return Json(new { Songs = songs, CurrentPage = page, TotalPages = 100 });
    }

    [HttpGet]
    public IActionResult GetAudio(int index, long seed)
    {
        var audioData = _musicGenerator.GenerateSong((int)(seed + index));
        return File(audioData, "audio/wav");
    }
}
