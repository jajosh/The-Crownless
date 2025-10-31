using System;
using NAudio.Wave;

public class ObjectAudio
{
    public Dictionary<string, int> notes = new Dictionary<string, int>
    {
        { "C4", 261 },
        { "C#4", 277 }, { "Db4", 277 },
        { "D4", 293 },
        { "D#4", 311 }, { "Eb4", 311 },
        { "E4", 330 },
        { "F4", 349 },
        { "F#4", 370 }, { "Gb4", 370 },
        { "G4", 392 },
        { "G#4", 415 }, { "Ab4", 415 },
        { "A4", 440 },
        { "A#4", 466 }, { "Bb4", 466 },
        { "B4", 494 },
        { "C5", 523 },
        { "C#5", 554 }, { "Db5", 554 },
        { "D5", 587 },
        { "D#5", 622 }, { "Eb5", 622 },
        { "E5", 659 }
    };
    public Dictionary<string, int> noteLengths = new Dictionary<string, int>
    {
        { "whole", 2000 },     // 4 beats
        { "half", 1000 },      // 2 beats
        { "quarter", 500 },    // 1 beat
        { "eighth", 250 },     // 1/2 beat
        { "sixteenth", 125 }   // 1/4 beat
    };
    public ObjectAudio()
	{
	}
    #region === Songs
    public void BeepTester()
    {
        WaveformProvider.BeepPlus(440, 500, WaveformType.Sine);     // smooth tone
        WaveformProvider.BeepPlus(440, 500, WaveformType.Square);   // buzzy tone
        WaveformProvider.BeepPlus(440, 500, WaveformType.Triangle); // hollow tone
        WaveformProvider.BeepPlus(440, 500, WaveformType.Sawtooth); // bright tone
    }
    public static void HappyBirthday()
    {
        
        // Phrase 1
        Parallel.Invoke(
            () => Beeper(notes["C4"], noteLengths["eighth"]),
            () => Beeper(notes["E4"], noteLengths["eighth"])
        );
        Beeper(notes["D4"], noteLengths["quarter"]);
        Beeper(notes["C4"], noteLengths["quarter"]);
        Parallel.Invoke(
            () => Beeper(notes["F4"], noteLengths["quarter"]),
            () => Beeper(notes["A4"], noteLengths["quarter"])
        );
        Beeper(notes["E4"], noteLengths["half"]);

        // Phrase 2
        Beeper(notes["C4"], noteLengths["eighth"]);
        Beeper(notes["C4"], noteLengths["eighth"]);
        Beeper(notes["D4"], noteLengths["quarter"]);
        Beeper(notes["C4"], noteLengths["quarter"]);
        Parallel.Invoke(
            () => Beeper(notes["G4"], noteLengths["quarter"]),
            () => Beeper(notes["B4"], noteLengths["quarter"])
        );
        Beeper(notes["F4"], noteLengths["half"]);

        // Phrase 3
        Beeper(notes["C4"], noteLengths["eighth"]);
        Beeper(notes["C4"], noteLengths["eighth"]);
        Parallel.Invoke(
            () => Beeper(notes["C5"], noteLengths["quarter"]),
            () => Beeper(notes["E4"], noteLengths["quarter"])
        );
        Beeper(notes["A4"], noteLengths["quarter"]);
        Beeper(notes["F4"], noteLengths["quarter"]);
        Beeper(notes["E4"], noteLengths["quarter"]);
        Beeper(notes["D4"], noteLengths["half"]);

        // Phrase 4
        Parallel.Invoke(
            () => Beeper(notes["Bb4"], noteLengths["eighth"]),
            () => Beeper(notes["D4"], noteLengths["eighth"])
        );
        Parallel.Invoke(
            () => Beeper(notes["Bb4"], noteLengths["eighth"]),
            () => Beeper(notes["D4"], noteLengths["eighth"])
        );
        Beeper(notes["A4"], noteLengths["quarter"]);
        Beeper(notes["F4"], noteLengths["quarter"]);
        Parallel.Invoke(
            () => Beeper(notes["G4"], noteLengths["quarter"]),
            () => Beeper(notes["C4"], noteLengths["quarter"])
        );
        Beeper(notes["F4"], noteLengths["half"]);
    }
    public static void TheCrownlessCry()
    {
        
        Parallel.Invoke(
            () => Beeper(notes["C4"], noteLengths["quarter"]),
            () => Beeper(notes["E4"], noteLengths["quarter"])
        );
        Beeper(notes["F4"], noteLengths["quarter"]);
        Beeper(notes["G4"], noteLengths["quarter"]);
        Parallel.Invoke(
            () => Beeper(notes["C4"], noteLengths["quarter"]),
            () => Beeper(notes["G#4"], noteLengths["quarter"])
        );
        Parallel.Invoke(
            () => Beeper(notes["C4"], noteLengths["whole"]),
            () => Beeper(notes["C5"], noteLengths["whole"])
        );
        Beeper(notes["C4"], noteLengths["quarter"]);
        Beeper(notes["F4"], noteLengths["quarter"]);
        Beeper(notes["A#4"], noteLengths["quarter"]);
        Parallel.Invoke(
            () => Beeper(notes["C4"], noteLengths["whole"]),
            () => Beeper(notes["G4"], noteLengths["whole"]),
            () => Beeper(notes["C5"], noteLengths["whole"])
        );
    }
    #endregion
}
