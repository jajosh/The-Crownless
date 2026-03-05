using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;

namespace MyGame.Controls
{
    public class OverlayStep
    {
        public ColorComponent Color { get; set; } = new ColorComponent(0, 0, 0, 0);
        public string FontFamily { get; set; }
        public FontStyle? FontStyle { get; set; }
        public float SizeMultiplier { get; set; } = 1f;
    }
}
