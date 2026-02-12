using NAudio.Wave;
using NAudio.Wave.SampleProviders;

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
        using var writer = new WaveFileWriter(ms, new WaveFormat(_sampleRate, 16, 2));

        int totalSamples = _sampleRate * durationSeconds;

        for (int i = 0; i < totalSamples; i++)
        {
            var sampleValue = Math.Sin(2 * Math.PI * 440 * i / _sampleRate); // Simple tone
            short sample = (short)(sampleValue * 32767 * 0.5);
            writer.WriteSample(sample);
            writer.WriteSample(sample);
        }

        writer.Flush();
        return ms.ToArray();
    }
}
