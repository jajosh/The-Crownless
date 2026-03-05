using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;

namespace MyGame.Controls
{
    public class CharacterData
    {
        public string Char { get; set; }
        public ColorComponent Color { get; set; } = new ColorComponent(255, 255, 255);
        public bool IsSelected { get; set; }

        // Aliases for game engine compatibility
        public string MainChar { get => Char; set => Char = value; }
        public string ShadowChar { get => Char; set => Char = value; }
        public string TintChar { get => Char; set => Char = value; }
        public ColorComponent MainColor { get => Color; set => Color = value; }

        // Effect Intensities (Simplified for now, can be hooked into rendering later)
        public float ShakeIntensity { get; set; }
        public float ShimmerIntensity { get; set; }
        public ColorComponent ShimmerColor { get; set; } = new ColorComponent(255, 255, 255, 0);
        public float WaveIntensity { get; set; }
        public bool IsFlicker { get; set; }

        public float MainScale { get; set; } = 1f;
        public float ShadowScale { get; set; } = 1f;

        public float ShadowSquishX { get; set; } = 1f;
        public float ShadowSquishY { get; set; } = 1f;

        public string MainFontFamily { get; set; }
        public FontStyle? MainFontStyle { get; set; }
        public float MainFontSizeMultiplier { get; set; } = 1f;

        public string ShadowFontFamily { get; set; }
        public FontStyle? ShadowFontStyle { get; set; }
        public float ShadowFontSizeMultiplier { get; set; } = 1f;

        public string OverlayFontFamily { get; set; }
        public FontStyle? OverlayFontStyle { get; set; }
        public float OverlayFontSizeMultiplier { get; set; } = 1f;

        public ColorComponent ColorShiftOverride { get; set; }

        public CharacterData() { }
        public CharacterData(string c) { Char = c; }
        public CharacterData(char c) { Char = c.ToString(); }
    }
}
