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
        
        // Musical scales and chord progressions
        var scale = GetScale(random);
        var chordProgression = GetChordProgression(random);
        var tempo = 80 + random.Next(60); // 80-140 BPM
        
        using var ms = new MemoryStream();
        using var writer = new WaveFileWriter(ms, new WaveFormat(_sampleRate, 16, 2));
        
        var totalSamples = _sampleRate * durationSeconds;
        var beatDuration = (60.0 / tempo) * _sampleRate;
        
        for (int i = 0; i < totalSamples; i++)
        {
            var time = (double)i / _sampleRate;
            var beat = (int)(i / beatDuration) % chordProgression.Length;
            var chord = chordProgression[beat];
            
            // Generate melody note
            var melodyNote = scale[random.Next(scale.Length)];
            var melodyFreq = GetFrequency(melodyNote + 12); // Octave up
            
            // Generate bass note from chord root
            var bassFreq = GetFrequency(chord[0]);
            
            // Generate chord harmony
            var harmonyFreq1 = GetFrequency(chord[1]);
            var harmonyFreq2 = GetFrequency(chord[2]);
            
            // Mix all parts with envelope
            var envelope = GetEnvelope(i % (int)beatDuration, (int)beatDuration);
            
            var melody = Math.Sin(2 * Math.PI * melodyFreq * time) * 0.15 * envelope;
            var bass = Math.Sin(2 * Math.PI * bassFreq * time) * 0.3;
            var harmony1 = Math.Sin(2 * Math.PI * harmonyFreq1 * time) * 0.1 * envelope;
            var harmony2 = Math.Sin(2 * Math.PI * harmonyFreq2 * time) * 0.1 * envelope;
            
            var sample = (short)((melody + bass + harmony1 + harmony2) * 32767 * 0.5);
            
            // Write stereo
            writer.WriteSample(sample);
            writer.WriteSample(sample);
        }
        
        writer.Flush();
        return ms.ToArray();
    }

    private int[] GetScale(Random random)
    {
        // Major scale pattern
        var root = 60 + random.Next(12); // C4 to B4
        return new[] { root, root + 2, root + 4, root + 5, root + 7, root + 9, root + 11, root + 12 };
    }

    private int[][] GetChordProgression(Random random)
    {
        // Common chord progressions in scale degrees
        var progressions = new[]
        {
            new[] { 0, 4, 7, 5 }, // I-IV-V-IV
            new[] { 0, 5, 7, 4 }, // I-vi-V-IV
            new[] { 0, 4, 5, 7 }, // I-IV-vi-V
            new[] { 0, 7, 4, 5 }  // I-V-IV-vi
        };
        
        var pattern = progressions[random.Next(progressions.Length)];
        var scale = GetScale(random);
        
        return pattern.Select(degree => new[]
        {
            scale[degree % scale.Length],
            scale[(degree + 2) % scale.Length],
            scale[(degree + 4) % scale.Length]
        }).ToArray();
    }

    private double GetFrequency(int midiNote)
    {
        return 440.0 * Math.Pow(2, (midiNote - 69) / 12.0);
    }

    private double GetEnvelope(int position, int duration)
    {
        var attack = duration * 0.1;
        var decay = duration * 0.2;
        var sustain = 0.7;
        var release = duration * 0.3;
        
        if (position < attack)
            return position / attack;
        else if (position < attack + decay)
            return 1.0 - (1.0 - sustain) * ((position - attack) / decay);
        else if (position < duration - release)
            return sustain;
        else
            return sustain * (1.0 - (position - (duration - release)) / release);
    }
}
