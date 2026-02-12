using NAudio.Wave;

namespace MusicStoreApp.Services;

public interface IMusicGeneratorService
{
    byte[] GenerateSong(int seed, int durationSeconds = 6);
}

public class MusicGeneratorService : IMusicGeneratorService
{
    private readonly int _sampleRate = 44100;

    public byte[] GenerateSong(int seed, int durationSeconds = 6)
    {
        var random = new Random(seed);
        using var ms = new MemoryStream();
        using var writer = new WaveFileWriter(ms, new WaveFormat(_sampleRate, 16, 1));

        // Простая мажорная гамма C4-B4
        int[] scale = { 60, 62, 64, 65, 67, 69, 71, 72 };

        int totalSamples = _sampleRate * durationSeconds;
        int beatSamples = _sampleRate / 2; // 0.5 сек на ноту

        for (int i = 0; i < totalSamples; i++)
        {
            double t = (double)i / _sampleRate;
            int beat = i / beatSamples;
            int noteIndex = beat % scale.Length;
            double freq = 440.0 * Math.Pow(2, (scale[noteIndex] - 69) / 12.0);

            double posInBeat = i % beatSamples;
            double env = 1.0;
            if (posInBeat < beatSamples * 0.1) env = posInBeat / (beatSamples * 0.1);
            else if (posInBeat > beatSamples * 0.8) env = 1.0 - (posInBeat - beatSamples * 0.8) / (beatSamples * 0.2);

            short sample = (short)(Math.Sin(2 * Math.PI * freq * t) * env * 3000);
            writer.WriteSample(sample);
        }

        writer.Flush();
        return ms.ToArray();
    }
}
