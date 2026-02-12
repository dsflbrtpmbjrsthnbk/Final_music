using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Fonts;

namespace MusicStoreApp.Services;

public interface ICoverGeneratorService
{
    string GenerateCover(string title, string artist, int seed);
}

public class CoverGeneratorService : ICoverGeneratorService
{
    public string GenerateCover(string title, string artist, int seed)
    {
        var random = new Random(seed);

        // Создаем изображение 400x400
        using var image = new Image<Rgba32>(400, 400);

        // Генерация градиентного фона
        var r1 = random.Next(50, 255);
        var g1 = random.Next(50, 255);
        var b1 = random.Next(50, 255);

        var r2 = random.Next(50, 255);
        var g2 = random.Next(50, 255);
        var b2 = random.Next(50, 255);

        image.Mutate(ctx =>
        {
            for (int y = 0; y < 400; y++)
            {
                float ratio = y / 400f;
                var r = (byte)(r1 + (r2 - r1) * ratio);
                var g = (byte)(g1 + (g2 - g1) * ratio);
                var b = (byte)(b1 + (b2 - b1) * ratio);

                ctx.Fill(new Rgba32(r, g, b), new RectangleF(0, y, 400, 1));
            }

            // Полупрозрачный прямоугольник для текста
            ctx.Fill(new Rgba32(0, 0, 0, 128), new RectangleF(20, 320, 360, 60));
        });

        // Шрифты
        var titleFont = SystemFonts.CreateFont("Arial", 24, FontStyle.Bold);
        var artistFont = SystemFonts.CreateFont("Arial", 18, FontStyle.Regular);

        // Рисуем текст напрямую через правильный вызов DrawText
        image.Mutate(ctx =>
        {
            // Title
            ctx.DrawText(title, titleFont, Color.White, new PointF(30, 330));

            // Artist
            ctx.DrawText(artist, artistFont, Color.LightGray, new PointF(30, 360));
        });

        // Конвертируем в Base64
        using var ms = new MemoryStream();
        image.SaveAsPng(ms);
        return Convert.ToBase64String(ms.ToArray());
    }
}
