namespace MusicStoreApp.Models;

public class Song
{
    public int Index { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Artist { get; set; } = string.Empty;
    public string Album { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public int Likes { get; set; }
    public string? CoverImageBase64 { get; set; }
    public string? ReviewText { get; set; }
    public byte[]? AudioData { get; set; }
}

public class SongPageRequest
{
    public string Language { get; set; } = "en-US";
    public long Seed { get; set; }
    public double AverageLikes { get; set; } = 3.7;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class SongPageResponse
{
    public List<Song> Songs { get; set; } = new();
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
}
