using NAudio.Wave;

public enum WaveformType
{
    Sine,
    Square,
    Triangle,
    Sawtooth
}

public class PersistentBeeper : IDisposable
{
    private readonly WaveOutEvent output;
    private readonly WaveformProvider provider;

    public PersistentBeeper()
    {
        provider = new WaveformProvider();
        output = new WaveOutEvent();
        output.Init(provider);
        output.Play();
    }

    public void PlayNote(int frequency, int durationMs, WaveformType waveform)
    {
        provider.Frequency = frequency;
        provider.Waveform = waveform;
        provider.NoteLengthMs = durationMs;
        provider.Reset();

        Thread.Sleep(durationMs + 50); // allow fade to finish
    }

    public void Dispose()
    {
        output.Stop();
        output.Dispose();
    }
}
public class WaveformProvider : WaveProvider32
{
    private double phase;
    private double phaseIncrement;
    private int totalSamples;
    private int currentSample;

    public float Frequency { get; set; } = 440f;
    public float Amplitude { get; set; } = 0.25f;
    public WaveformType Waveform { get; set; } = WaveformType.Sine;
    public int NoteLengthMs { get; set; } = 500;

    public WaveformProvider()
    {
        SetWaveFormat(44100, 1);
    }
    public void Reset()
    {
        currentSample = 0;
    }
    public override int Read(float[] buffer, int offset, int count)
    {
        int sampleRate = WaveFormat.SampleRate;
        totalSamples = (int)((NoteLengthMs / 1000.0) * sampleRate);
        phaseIncrement = (2 * Math.PI * Frequency) / sampleRate;

        // 5ms fade duration
        int fadeSamples = (int)(sampleRate * 0.005);

        for (int n = 0; n < count; n++)
        {
            if (currentSample >= totalSamples)
            {
                buffer[n + offset] = 0;
                continue;
            }

            double value = 0;
            double t = phase / (2 * Math.PI);

            // waveform generation
            switch (Waveform)
            {
                case WaveformType.Sine:
                    value = Math.Sin(phase);
                    break;

                case WaveformType.Square:
                    value = Math.Sign(Math.Sin(phase));
                    break;

                case WaveformType.Triangle:
                    value = 2 * Math.Abs(2 * (t % 1) - 1) - 1;
                    break;

                case WaveformType.Sawtooth:
                    value = 2 * (t % 1) - 1;
                    break;
            }

            // apply fade-in/out
            double gain = 1.0;
            if (currentSample < fadeSamples)
                gain = (double)currentSample / fadeSamples; // fade-in
            else if (currentSample > totalSamples - fadeSamples)
                gain = (double)(totalSamples - currentSample) / fadeSamples; // fade-out

            buffer[n + offset] = (float)(Amplitude * value * gain);

            // increment phase and wrap
            phase += phaseIncrement;
            if (phase >= 2 * Math.PI)
                phase -= 2 * Math.PI;

            currentSample++;
        }

        return count;
    }

    public static void BeepPlus(int frequency, int durationMs, WaveformType waveform)
    {
        var provider = new WaveformProvider
        {
            Frequency = frequency,
            Amplitude = 0.25f,
            Waveform = waveform,
            NoteLengthMs = durationMs
        };

        using var wo = new WaveOutEvent();
        wo.Init(provider);
        wo.Play();

        // Wait for the sound to finish naturally
        int extra = 50; // small buffer to ensure fade-out completes
        Thread.Sleep(durationMs + extra);

        // Let WaveOut play to completion, then stop
        while (wo.PlaybackState == PlaybackState.Playing)
            Thread.Sleep(5);

        wo.Stop();
    }
}