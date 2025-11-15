using NAudio.Wave;
using System;
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
