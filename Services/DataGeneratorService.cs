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
    private readonly Dictionary<string, string[]> _musicWords = new()
    {
        ["en-US"] = new[] { "Dream","Night","Shadow","Echo","Fading","Burning","Electric","Velvet","Silver","Crimson",
            "Lonely","Wild","Restless","Shallow","Radiant","Secret","Blue","Golden","Dark","Bright",
            "Summer","Winter","Highway","River","Ocean","Mountain","Sky","Star","Moon","Sun",
            "Love","Heart","Soul","Mind","Spirit","Frequency","Melody","Rhythm","Harmony","Symphony" },
        ["ru-RU"] = new[] { "Мечта","Ночь","Тень","Эхо","Угасание","Горящий","Электрический","Бархат","Серебро","Багровый",
            "Одинокий","Дикий","Беспокойный","Мелкий","Сияющий","Тайный","Синий","Золотой","Темный","Яркий",
            "Лето","Зима","Шоссе","Река","Океан","Гора","Небо","Звезда","Луна","Солнце",
            "Любовь","Сердце","Душа","Разум","Дух","Частота","Мелодия","Ритм","Гармония","Симфония" }
    };

    private readonly Dictionary<string, string[]> _artistTypes = new()
    {
        ["en-US"] = new[] { "Orchestra","Band","Theory","Hearts","Giants","Drivers","Parade","Coalition","Collective" },
        ["ru-RU"] = new[] { "Оркестр","Группа","Теория","Сердца","Гиганты","Драйверы","Парад","Коалиция","Коллектив" }
    };

    private readonly Dictionary<string, string[]> _albumWords = new()
    {
        ["en-US"] = new[] { "Polaroid","Signals","Voyage","Downtown","Blueprint","Transit","Mirrors","Midnight" },
        ["ru-RU"] = new[] { "Полароид","Сигналы","Путешествие","Центр","План","Транзит","Зеркала","Полночь" }
    };

    private readonly Dictionary<string, string[]> _genres = new()
    {
        ["en-US"] = new[] { "Ambient","Progressive Rock","Dream Pop","Disco Funk","Neo Soul","Electro House","Alt Country","Indie Pop" },
        ["ru-RU"] = new[] { "Эмбиент","Прогрессивный Рок","Дрим Поп","Диско Фанк","Нео Соул","Электро Хаус","Альт Кантри","Инди Поп" }
    };

    private readonly Dictionary<string, string[]> _reviewPhrases = new()
    {
        ["en-US"] = new[] { "the bridge adds real tension","the hook stays in your head","the chorus lands perfectly",
            "the groove feels effortless","the vocal tone is expressive","the arrangement keeps surprising",
            "the production is warm and spacious","the rhythm section feels tight" },
        ["ru-RU"] = new[] { "бридж добавляет настоящее напряжение","хук застревает в голове","припев звучит идеально",
            "грув ощущается легко","вокальный тон выразительный","аранжировка продолжает удивлять",
            "продакшн теплый и просторный","ритм-секция звучит плотно" }
    };

    public Song GenerateSong(int index, long baseSeed, string language, double averageLikes)
    {
        var songSeed = CombineSeeds(baseSeed, index);
        var songRandom = new Random((int)(songSeed & 0x7FFFFFFF));
        var lang = _musicWords.ContainsKey(language) ? language : "en-US";

        var faker = new Faker(lang.Split('-')[0])
        {
            Random = new Randomizer((int)(songSeed & 0x7FFFFFFF))
        };

        var words = _musicWords[lang];
        var title = $"{words[songRandom.Next(words.Length)]} {words[songRandom.Next(words.Length)]}";

        string artist = songRandom.Next(2) == 0
            ? $"{words[songRandom.Next(words.Length)]} {_artistTypes[lang][songRandom.Next(_artistTypes[lang].Length)]}"
            : faker.Name.FullName();

        var album = songRandom.Next(3) == 0 ? "Single" :
            $"{_albumWords[lang][songRandom.Next(_albumWords[lang].Length)]} {words[songRandom.Next(words.Length)]}";

        var genre = _genres[lang][songRandom.Next(_genres[lang].Length)];

        var likesRandom = new Random((int)(CombineSeeds(songSeed, 999999) & 0x7FFFFFFF));
        var likes = GenerateLikes(averageLikes, likesRandom);

        var reviewRandom = new Random((int)(CombineSeeds(songSeed, 777777) & 0x7FFFFFFF));
        var reviewText = _reviewPhrases[lang][reviewRandom.Next(_reviewPhrases[lang].Length)];

        return new Song
        {
            Index = index,
            Title = title,
            Artist = artist,
            Album = album,
            Genre = genre,
            Likes = likes,
            ReviewText = reviewText
        };
    }

    public List<Song> GenerateSongPage(int page, int pageSize, long baseSeed, string language, double averageLikes)
    {
        var songs = new List<Song>();
        var startIndex = (page - 1) * pageSize + 1;

        for (int i = 0; i < pageSize; i++)
        {
            var index = startIndex + i;
            songs.Add(GenerateSong(index, baseSeed, language, averageLikes));
        }

        return songs;
    }

    private long CombineSeeds(long seed, int modifier)
    {
        return (seed * 1103515245 + modifier) & 0x7FFFFFFFFFFFFFFF;
    }

    private int GenerateLikes(double averageLikes, Random random)
    {
        if (averageLikes == 0) return 0;
        if (averageLikes >= 10) return 10;

        var floor = (int)Math.Floor(averageLikes);
        var fractional = averageLikes - floor;
        return floor + (random.NextDouble() < fractional ? 1 : 0);
    }
}
