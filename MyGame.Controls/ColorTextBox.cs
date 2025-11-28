using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.VisualStyles;

namespace MyGame.Controls
{
    // ========================================================================
    // COLOR COMPONENT (Your Custom Color Class)
    // ========================================================================
    public class ColorComponent
    {
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
        public int A { get; set; } = 255;

        public ColorComponent(int r, int g, int b) : this(255, r, g, b) { }

        public ColorComponent(int a, int r, int g, int b)
        {
            A = Math.Clamp(a, 0, 255);
            R = Math.Clamp(r, 0, 255);
            G = Math.Clamp(g, 0, 255);
            B = Math.Clamp(b, 0, 255);
        }

        public Color ToColor() => Color.FromArgb(A, R, G, B);

        public static ColorComponent FromColor(Color color) =>
            new ColorComponent(color.A, color.R, color.G, color.B);

        public static ColorComponent Lerp(ColorComponent a, ColorComponent b, float t) =>
            new ColorComponent(
                (int)(a.A + (b.A - a.A) * t),
                (int)(a.R + (b.R - a.R) * t),
                (int)(a.G + (b.G - a.G) * t),
                (int)(a.B + (b.B - a.B) * t)
            );

        public static ColorComponent Multiply(ColorComponent a, float factor) =>
            new ColorComponent(
                a.A,
                (int)(a.R * factor),
                (int)(a.G * factor),
                (int)(a.B * factor)
            );
    }

    // ========================================================================
    // STRING ARRAY CONVERTER (For Editable DesignTimeLines)
    // ========================================================================
    public class StringArrayConverter : TypeConverter
    {
        public override bool GetPropertiesSupported(ITypeDescriptorContext context) => true;
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
            => new PropertyDescriptorCollection(new PropertyDescriptor[0]);

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string s)
                return s.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is string[] arr)
                return string.Join(Environment.NewLine, arr);
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    // ========================================================================
    // MAIN CONTROL: ColorTextBox
    // ========================================================================
    [ToolboxItem(true)]
    [Designer(typeof(ControlDesigner))]
    public class ColorTextBox : Control
    {
        // --------------------------------------------------------------------
        // Per-character data
        // --------------------------------------------------------------------
        public class CharData
        {
            public char Char { get; set; }
            public ColorComponent Color { get; set; } = new ColorComponent(255, 255, 255);
            public bool IsSelected { get; set; }

            public float MainScale { get; set; } = 1f;
            public float ShadowScale { get; set; } = 1f;
            public float OverlayScale { get; set; } = 1f;

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

            public ColorComponent? ColorShiftOverride { get; set; }
            // Parameterless constructor (required for JSON deserialization, etc.)
            public CharData()
            {
                Color = new ColorComponent(255, 255, 255); // Default white
            }
        }

        public class OverlayStep
        {
            public ColorComponent Color { get; set; } = new ColorComponent(0, 0, 0, 0);
            public string FontFamily { get; set; }
            public FontStyle? FontStyle { get; set; }
            public float SizeMultiplier { get; set; } = 1f;
            // Parameterless constructor
            public OverlayStep()
            {
                Color = new ColorComponent(0, 0, 0, 0); // Fully transparent
            }
        }

        // --------------------------------------------------------------------
        // Core fields
        // --------------------------------------------------------------------
        private readonly List<CharData> _chars = new List<CharData>();
        private readonly Dictionary<(string Family, float Size, FontStyle Style), Font> _fontCache = new();

        private float _charWidth;
        private float _lineHeight;
        private int _firstVisibleLine;
        private int _letterSpacing;
        private HorizontalAlignment _textAlign = HorizontalAlignment.Left;
        private VerticalAlignment _verticalAlign = VerticalAlignment.Top;

        private int _selectionStart = -1;
        private int _selectionLength = 0;

        private System.Windows.Forms.Timer _shimmerTimer;
        private int _shimmerIndex = 0;

        private System.Windows.Forms.Timer _pulseTimer;
        private float _totalTime = 0f;
        private DateTime _lastFrame = DateTime.MinValue;

        private System.Windows.Forms.Timer _backAnimTimer;
        private ColorComponent _currentBackColor = new ColorComponent(0, 0, 0);
        private ColorComponent _targetBackColor = new ColorComponent(0, 0, 0);
        private Image _currentBackgroundImage;
        private Image _targetBackgroundImage;
        private float _backgroundAlpha = 0f;

        private ColorComponent _colorShiftCurrent = new ColorComponent(255, 255, 255);
        private ColorComponent _colorShiftTarget = new ColorComponent(255, 255, 255);
        private System.Windows.Forms.Timer _colorShiftTimer;

        // --------------------------------------------------------------------
        // Appearance (Fixed: Added DesignerSerialization)
        // --------------------------------------------------------------------
        private ColorComponent _overlayTint = new ColorComponent(0, 0, 0, 0);
        private ColorComponent _shadowColor = new ColorComponent(120, 0, 0, 0);
        private ColorComponent _selectionBackground = ColorComponent.FromColor(SystemColors.Highlight);
        private ColorComponent _selectionColor = ColorComponent.FromColor(SystemColors.HighlightText);

        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ColorComponent OverlayTint
        {
            get => _overlayTint;
            set { _overlayTint = value; Invalidate(); }
        }

        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ColorComponent ShadowColor
        {
            get => _shadowColor;
            set { _shadowColor = value; Invalidate(); }
        }

        [Category("Appearance"), DefaultValue(typeof(Point), "1,1")]
        public Point ShadowOffset { get; set; } = new Point(1, 1);

        [Category("Appearance"), DefaultValue(typeof(Point), "0,0")]
        public Point OverlayOffset { get; set; } = Point.Empty;

        [Category("Appearance"), DefaultValue(0)]
        public int LetterSpacing
        {
            get => _letterSpacing;
            set { _letterSpacing = value; if (AutoWidthInChars) RecalculateWidthInChars(); Invalidate(); }
        }

        [Category("Appearance"), DefaultValue(HorizontalAlignment.Left)]
        public HorizontalAlignment TextAlign { get => _textAlign; set { _textAlign = value; Invalidate(); } }

        [Category("Appearance"), DefaultValue(VerticalAlignment.Top)]
        public VerticalAlignment VerticalAlign { get => _verticalAlign; set { _verticalAlign = value; Invalidate(); } }

        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ColorComponent SelectionBackground
        {
            get => _selectionBackground;
            set { _selectionBackground = value; Invalidate(); }
        }

        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ColorComponent SelectionColor
        {
            get => _selectionColor;
            set { _selectionColor = value; Invalidate(); }
        }

        [Category("Appearance"), DefaultValue(1f)]
        public float GlobalMainScale { get; set; } = 1f;

        [Category("Appearance"), DefaultValue(1f)]
        public float GlobalShadowScale { get; set; } = 1f;

        [Category("Appearance"), DefaultValue(1f)]
        public float GlobalOverlayScale { get; set; } = 1f;

        [Category("Appearance"), DefaultValue(1f)]
        public float ShadowSquishX { get; set; } = 1f;

        [Category("Appearance"), DefaultValue(1f)]
        public float ShadowSquishY { get; set; } = 1f;

        [Category("Appearance"), DefaultValue(null)]
        public string ShadowFontFamily { get; set; }

        [Category("Appearance"), DefaultValue(FontStyle.Regular)]
        public FontStyle ShadowFontStyle { get; set; } = FontStyle.Regular;

        [Category("Appearance"), DefaultValue(null)]
        public string OverlayFontFamily { get; set; }

        [Category("Appearance"), DefaultValue(FontStyle.Regular)]
        public FontStyle OverlayFontStyle { get; set; } = FontStyle.Regular;

        // --------------------------------------------------------------------
        // Effects
        // --------------------------------------------------------------------
        [Category("Effects"), DefaultValue(true)]
        public bool EnableRipple { get; set; } = true;

        [Category("Effects"), DefaultValue(3f)]
        public float RippleAmplitude { get; set; } = 3f;

        [Category("Effects"), DefaultValue(0.8f)]
        public float RippleFrequency { get; set; } = 0.8f;

        [Category("Effects"), DefaultValue(2.5f)]
        public float RippleSpeed { get; set; } = 2.5f;

        [Category("Effects"), DefaultValue(false)]
        public bool RippleHorizontal { get; set; } = false;

        [Category("Effects"), DefaultValue(true)]
        public bool EnableNeonPulse { get; set; } = true;

        [Category("Effects"), DefaultValue(1.3f)]
        public float NeonPulseMaxScale { get; set; } = 1.3f;

        [Category("Effects"), DefaultValue(0.7f)]
        public float NeonPulseMinScale { get; set; } = 0.7f;

        [Category("Effects"), DefaultValue(1200)]
        public int NeonPulseDurationMs { get; set; } = 1200;

        // --------------------------------------------------------------------
        // Color Shift
        // --------------------------------------------------------------------
        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ColorComponent ColorShiftTarget
        {
            get => _colorShiftTarget;
            set
            {
                _colorShiftTarget = value;
                if (ColorShiftStrength > 0f) StartColorShiftAnimation();
            }
        }

        [Category("Appearance"), DefaultValue(0f)]
        public float ColorShiftStrength { get; set; } = 0f;

        // --------------------------------------------------------------------
        // Shimmer
        // --------------------------------------------------------------------
        public List<OverlayStep> OverlaySteps { get; } = new List<OverlayStep>();

        [Category("Appearance"), DefaultValue(100)]
        public int ShimmerIntervalMs { get; set; } = 100;

        // --------------------------------------------------------------------
        // Design
        // --------------------------------------------------------------------
        private string[] _designTimeLines = new[]
        {
            "ColorTextBox Ready!",
            "Ripple + Neon Pulse",
            "ColorComponent Power!"
        };

        [Category("Design")]
        [Description("Multi-line preview text shown in the designer. Click [...] to edit.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [TypeConverter(typeof(StringArrayConverter))]
        [Editor(typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [RefreshProperties(RefreshProperties.All)]
        public string[] DesignTimeLines
        {
            get => _designTimeLines;
            set
            {
                _designTimeLines = value ?? Array.Empty<string>();
                Invalidate();
            }
        }

        // --------------------------------------------------------------------
        // Layout
        // --------------------------------------------------------------------
        [Category("Layout"), DefaultValue(51)]
        public int WidthInChars { get; set; } = 51;

        [Category("Layout"), DefaultValue(true)]
        public bool AutoWidthInChars { get; set; } = true;

        // --------------------------------------------------------------------
        // Constructor
        // --------------------------------------------------------------------
        public ColorTextBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.DoubleBuffer |
                     ControlStyles.Selectable |
                     ControlStyles.StandardClick |
                     ControlStyles.ResizeRedraw, true);

            base.BackColor = Color.Black;
            TabStop = true;

            UpdateMetrics();
            if (AutoWidthInChars) RecalculateWidthInChars();

            OverlaySteps.Add(new OverlayStep { Color = _overlayTint });

            _pulseTimer = new System.Windows.Forms.Timer { Interval = 16 };
            _pulseTimer.Tick += (s, e) => Invalidate();
            _pulseTimer.Start();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var f in _fontCache.Values) f.Dispose();
                _fontCache.Clear();
                _shimmerTimer?.Dispose();
                _pulseTimer?.Dispose();
                _backAnimTimer?.Dispose();
                _colorShiftTimer?.Dispose();
            }
            base.Dispose(disposing);
        }

        // --------------------------------------------------------------------
        // Metrics
        // --------------------------------------------------------------------
        private Font GetCachedFont(string familyName, float sizePoints, FontStyle style)
        {
            string family = familyName ?? Font.FontFamily.Name;
            var key = (family, sizePoints, style);
            if (!_fontCache.TryGetValue(key, out var font))
            {
                font = new Font(family, sizePoints, style);
                _fontCache[key] = font;
            }
            return font;
        }

        private void UpdateMetrics()
        {
            using Graphics g = CreateGraphics();
            _charWidth = g.MeasureString("M", Font).Width;
            _lineHeight = Font.Height;
        }

        private void RecalculateWidthInChars()
        {
            float advance = CharAdvance;
            if (advance <= 0) advance = 1f;
            float available = ClientSize.Width - Padding.Horizontal;
            WidthInChars = Math.Max(1, (int)Math.Floor(available / advance));
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            UpdateMetrics();
            if (AutoWidthInChars) RecalculateWidthInChars();
            Invalidate();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (AutoWidthInChars) RecalculateWidthInChars();
            Invalidate();
        }

        protected override void OnPaddingChanged(EventArgs e)
        {
            base.OnPaddingChanged(e);
            if (AutoWidthInChars) RecalculateWidthInChars();
            Invalidate();
        }

        public override Font Font
        {
            get => base.Font;
            set
            {
                if (value == null) return;
                using Graphics g = CreateGraphics();
                float wi = g.MeasureString("i", value).Width;
                float wM = g.MeasureString("M", value).Width;
                base.Font = Math.Abs(wi - wM) > 1f
                    ? new Font("Consolas", value.SizeInPoints, value.Style)
                    : value;
                foreach (var f in _fontCache.Values) f.Dispose();
                _fontCache.Clear();
            }
        }

        private int TotalLines => (_chars.Count + WidthInChars - 1) / WidthInChars;
        private int VisibleLines => _lineHeight > 0 ? Math.Max(1, (int)Math.Floor((ClientSize.Height - Padding.Vertical) / _lineHeight)) : 1;
        private float CharAdvance => _charWidth + LetterSpacing;

        // --------------------------------------------------------------------
        // Animation Helpers
        // --------------------------------------------------------------------
        public void RefreshShimmer()
        {
            if (OverlaySteps.Count > 1 && ShimmerIntervalMs > 0)
            {
                _shimmerTimer ??= new System.Windows.Forms.Timer();
                _shimmerTimer.Interval = ShimmerIntervalMs;
                _shimmerTimer.Tick -= ShimmerTimer_Tick;
                _shimmerTimer.Tick += ShimmerTimer_Tick;
                _shimmerTimer.Start();
            }
            else _shimmerTimer?.Stop();
        }

        private void ShimmerTimer_Tick(object sender, EventArgs e)
        {
            _shimmerIndex = (_shimmerIndex + 1) % OverlaySteps.Count;
            Invalidate();
        }

        private void StartColorShiftAnimation()
        {
            _colorShiftTimer?.Dispose();
            _colorShiftTimer = new System.Windows.Forms.Timer { Interval = 16 };
            _colorShiftCurrent = GetCurrentGlobalTint();
            _colorShiftTimer.Tick += (s, e) =>
            {
                float t = 0.1f;
                _colorShiftCurrent = ColorComponent.Lerp(_colorShiftCurrent, _colorShiftTarget, t);
                Invalidate();
                if (ColorShiftStrength == 0f || NearlyEqual(_colorShiftCurrent, _colorShiftTarget))
                    _colorShiftTimer.Stop();
            };
            _colorShiftTimer.Start();
        }

        private bool NearlyEqual(ColorComponent a, ColorComponent b) =>
            Math.Abs(a.R - b.R) + Math.Abs(a.G - b.G) + Math.Abs(a.B - b.B) < 10;

        private ColorComponent GetCurrentGlobalTint() =>
            ColorShiftStrength > 0f ? _colorShiftCurrent : new ColorComponent(255, 255, 255);

        private ColorComponent ApplyColorShift(ColorComponent baseColor, CharData cd)
        {
            if (ColorShiftStrength <= 0f) return baseColor;
            if (cd.ColorShiftOverride is ColorComponent ov) return ov;
            return ColorComponent.Lerp(baseColor, GetCurrentGlobalTint(), ColorShiftStrength);
        }

        // --------------------------------------------------------------------
        // Background Animation
        // --------------------------------------------------------------------
        public void SetBackColor(ColorComponent color, int fadeMs = 300)
        {
            _targetBackColor = color;
            if (fadeMs <= 0)
            {
                _currentBackColor = color;
                base.BackColor = color.ToColor();
                Invalidate();
                return;
            }

            var start = _currentBackColor;
            int steps = Math.Max(1, fadeMs / 16);
            int step = 0;

            StopBackAnim();
            _backAnimTimer = new System.Windows.Forms.Timer { Interval = 16 };
            _backAnimTimer.Tick += (s, e) =>
            {
                step++;
                float t = step / (float)steps;
                _currentBackColor = ColorComponent.Lerp(start, color, t);
                base.BackColor = _currentBackColor.ToColor();
                Invalidate();
                if (step >= steps) StopBackAnim();
            };
            _backAnimTimer.Start();
        }

        public void SetBackgroundImage(Image image, int fadeMs = 300)
        {
            _targetBackgroundImage = image;
            if (fadeMs <= 0)
            {
                _currentBackgroundImage = image;
                _backgroundAlpha = image != null ? 1f : 0f;
                Invalidate();
                return;
            }

            Image startImg = _currentBackgroundImage;
            float startAlpha = _backgroundAlpha;
            float targetAlpha = image != null ? 1f : 0f;

            int steps = Math.Max(1, fadeMs / 16);
            int step = 0;

            StopBackAnim();
            _backAnimTimer = new System.Windows.Forms.Timer { Interval = 16 };
            _backAnimTimer.Tick += (s, e) =>
            {
                step++;
                float t = step / (float)steps;
                _backgroundAlpha = startAlpha + (targetAlpha - startAlpha) * t;
                if (image != null && startImg == null) _currentBackgroundImage = image;
                Invalidate();
                if (step >= steps) { _currentBackgroundImage = image; StopBackAnim(); }
            };
            _backAnimTimer.Start();
        }

        private void StopBackAnim()
        {
            _backAnimTimer?.Stop();
            _backAnimTimer?.Dispose();
            _backAnimTimer = null;
        }

        // --------------------------------------------------------------------
        // DrawChar with Ripple & Squish
        // --------------------------------------------------------------------
        private void DrawChar(Graphics g, float baseX, float baseY, char ch, ColorComponent colorComp, Font font,
                              float scale, float squishX, float squishY, Point offset,
                              float rippleX, float rippleY, StringFormat sf)
        {
            if (ch == ' ') return;

            float drawX = baseX + offset.X + rippleX;
            float drawY = baseY + offset.Y + rippleY;

            var state = g.Save();
            float centerX = baseX + CharAdvance / 2f + offset.X;
            float centerY = baseY + _lineHeight / 2f + offset.Y;

            g.TranslateTransform(centerX, centerY);
            g.ScaleTransform(scale * squishX, scale * squishY);
            g.TranslateTransform(-centerX, -centerY);

            using var b = new SolidBrush(colorComp.ToColor());
            g.DrawString(ch.ToString(), font, b, drawX, drawY, sf);

            g.Restore(state);
        }

        // --------------------------------------------------------------------
        // OnPaint
        // --------------------------------------------------------------------
        protected override void OnPaint(PaintEventArgs e)
        {
            var now = DateTime.Now;
            if (_lastFrame != DateTime.MinValue)
            {
                float delta = (float)(now - _lastFrame).TotalSeconds;
                _totalTime += delta;
            }
            _lastFrame = now;

            // === BACKGROUND ===
            e.Graphics.Clear(_currentBackColor.ToColor());
            if (_currentBackgroundImage != null && _backgroundAlpha > 0.01f)
            {
                var rect = new Rectangle(0, 0, Width, Height);
                using var attributes = new ImageAttributes();
                var matrix = new ColorMatrix { Matrix33 = _backgroundAlpha };
                attributes.SetColorMatrix(matrix);
                e.Graphics.DrawImage(_currentBackgroundImage, rect, 0, 0,
                    _currentBackgroundImage.Width, _currentBackgroundImage.Height,
                    GraphicsUnit.Pixel, attributes);
            }

            e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            var sf = StringFormat.GenericTypographic;

            // === DESIGN MODE ===
            if (DesignMode && _chars.Count == 0)
            {
                if (_designTimeLines == null || _designTimeLines.Length == 0) return;
                float y = Padding.Top;
                foreach (var line in _designTimeLines)
                {
                    if (string.IsNullOrWhiteSpace(line)) { y += _lineHeight; continue; }
                    float x = Padding.Left;
                    for (int i = 0; i < line.Length; i++)
                    {
                        char c = line[i];
                        using var brush = new SolidBrush(Color.FromArgb(255, (i * 40) % 255, (i * 60) % 255, 200));
                        e.Graphics.DrawString(c.ToString(), Font, brush, x, y, sf);
                        x += CharAdvance;
                    }
                    y += _lineHeight;
                }
                return;
            }

            // === NEON PULSE ===
            if (EnableNeonPulse && NeonPulseDurationMs > 0)
            {
                float pulseT = (float)Math.Sin(_totalTime / NeonPulseDurationMs * Math.PI * 2) * 0.5f + 0.5f;
                float scale = NeonPulseMinScale + (NeonPulseMaxScale - NeonPulseMinScale) * pulseT;
                GlobalOverlayScale = scale;
                GlobalShadowScale = scale * 0.8f;
                byte a = (byte)(150 + pulseT * 105);
                OverlayTint = new ColorComponent(a, 0, 255, 255);
            }

            // === PAINT LOOP ===
            float yOffset = 0f;
            if (TotalLines <= VisibleLines && _firstVisibleLine == 0)
            {
                float extra = ClientSize.Height - Padding.Vertical - TotalLines * _lineHeight;
                if (extra > 0)
                    yOffset = VerticalAlign == VerticalAlignment.Center ? extra / 2f :
                              VerticalAlign == VerticalAlignment.Bottom ? extra : 0f;
            }

            float yPos = Padding.Top + yOffset;
            int row = _firstVisibleLine;

            while (yPos < ClientSize.Height - Padding.Bottom && row < TotalLines)
            {
                int startIdx = row * WidthInChars;
                float charTimeOffset = row * 0.3f;

                int effectiveLength = 0;
                for (int col = 0; col < WidthInChars; col++)
                {
                    int idx = startIdx + col;
                    if (idx >= _chars.Count) break;
                    if (_chars[idx].Char != ' ' || _chars[idx].IsSelected)
                        effectiveLength = col + 1;
                }

                float offsetX = 0f;
                if (TextAlign != HorizontalAlignment.Left && effectiveLength > 0)
                {
                    float unused = WidthInChars - effectiveLength;
                    offsetX = TextAlign == HorizontalAlignment.Center ? unused / 2f * CharAdvance :
                              unused * CharAdvance;
                }

                float baseX = Padding.Left + offsetX;

                float x = baseX;
                float xMain = baseX;

                for (int col = 0; col < WidthInChars; col++)
                {
                    int idx = startIdx + col;
                    if (idx >= _chars.Count) break;
                    var cd = _chars[idx];

                    // === RIPPLE ===
                    float rippleX = 0f, rippleY = 0f;
                    if (EnableRipple)
                    {
                        float phase = (_totalTime * RippleSpeed + col * RippleFrequency + charTimeOffset) * 2 * (float)Math.PI;
                        rippleY = (float)Math.Sin(phase) * RippleAmplitude;
                        if (RippleHorizontal)
                            rippleX = (float)Math.Cos(phase) * RippleAmplitude * 0.5f;
                    }

                    // === SHADOW PASS ===
                    if (ShadowColor.A > 0 && cd.Char != ' ' && !cd.IsSelected)
                    {
                        string family = cd.ShadowFontFamily ?? ShadowFontFamily;
                        FontStyle style = cd.ShadowFontStyle ?? ShadowFontStyle;
                        float sizeMult = cd.ShadowFontSizeMultiplier * GlobalShadowScale;
                        float scale = cd.ShadowScale * GlobalShadowScale;
                        float squishX = cd.ShadowSquishX * ShadowSquishX;
                        float squishY = cd.ShadowSquishY * ShadowSquishY;

                        using Font font = GetCachedFont(family, Font.SizeInPoints * sizeMult, style);
                        var shadowCol = ApplyColorShift(ShadowColor, cd);
                        DrawChar(e.Graphics, x, yPos, cd.Char, shadowCol, font, scale, squishX, squishY, ShadowOffset, rippleX, rippleY, sf);
                    }

                    // === MAIN PASS ===
                    if (cd.Char != ' ')
                    {
                        if (cd.IsSelected)
                        {
                            using var bg = new SolidBrush(SelectionBackground.ToColor());
                            e.Graphics.FillRectangle(bg, xMain, yPos, CharAdvance, _lineHeight);
                        }

                        string family = cd.MainFontFamily;
                        FontStyle style = cd.MainFontStyle ?? Font.Style;
                        float sizeMult = cd.MainFontSizeMultiplier * GlobalMainScale;
                        float scale = cd.MainScale * GlobalMainScale;

                        using Font font = GetCachedFont(family, Font.SizeInPoints * sizeMult, style);
                        var baseCol = cd.IsSelected ? SelectionColor : cd.Color;
                        var finalCol = ApplyColorShift(baseCol, cd);
                        DrawChar(e.Graphics, xMain, yPos, cd.Char, finalCol, font, scale, 1f, 1f, Point.Empty, rippleX, rippleY, sf);
                    }

                    // === OVERLAY PASS ===
                    if (OverlaySteps.Count > 0)
                    {
                        var step = OverlaySteps[_shimmerIndex % OverlaySteps.Count];
                        if (step.Color.A > 0 && cd.Char != ' ')
                        {
                            string family = cd.OverlayFontFamily ?? step.FontFamily ?? OverlayFontFamily;
                            FontStyle style = cd.OverlayFontStyle ?? step.FontStyle ?? OverlayFontStyle;
                            float sizeMult = cd.OverlayFontSizeMultiplier * (step.SizeMultiplier * GlobalOverlayScale);
                            float scale = cd.OverlayScale * GlobalOverlayScale;

                            using Font font = GetCachedFont(family, Font.SizeInPoints * sizeMult, style);
                            var finalCol = ApplyColorShift(step.Color, cd);
                            DrawChar(e.Graphics, x, yPos, cd.Char, finalCol, font, scale, 1f, 1f, OverlayOffset, rippleX, rippleY, sf);
                        }
                    }

                    x += CharAdvance;
                    xMain += CharAdvance;
                }

                yPos += _lineHeight;
                row++;
            }
        }

        // --------------------------------------------------------------------
        // Public API
        // --------------------------------------------------------------------
        public void Append(char c, ColorComponent color,
                           float mainScale = 1f, float shadowScale = 1f, float overlayScale = 1f,
                           float shadowSquishX = 1f, float shadowSquishY = 1f)
        {
            _chars.Add(new CharData
            {
                Char = c,
                Color = color,
                MainScale = mainScale,
                ShadowScale = shadowScale,
                OverlayScale = overlayScale,
                ShadowSquishX = shadowSquishX,
                ShadowSquishY = shadowSquishY
            });
            Invalidate();
        }

        public void Append(CharData charData)
        {
            if (charData == null) throw new ArgumentNullException(nameof(charData));
            _chars.Add(charData);
            Invalidate();
        }

        public void Clear()
        {
            _chars.Clear();
            _firstVisibleLine = 0;
            ClearSelection();
            Invalidate();
        }

        public void Select(int start, int length)
        {
            start = Math.Max(0, Math.Min(start, _chars.Count));
            length = Math.Max(0, Math.Min(length, _chars.Count - start));

            if (start == _selectionStart && length == _selectionLength) return;

            ClearSelection();
            _selectionStart = start;
            _selectionLength = length;

            if (length > 0)
                for (int i = start; i < start + length; i++)
                    _chars[i].IsSelected = true;

            Invalidate();
        }

        public void ClearSelection()
        {
            if (_selectionLength == 0) return;
            for (int i = _selectionStart; i < _selectionStart + _selectionLength && i < _chars.Count; i++)
                _chars[i].IsSelected = false;
            _selectionStart = -1;
            _selectionLength = 0;
            Invalidate();
        }
    }
}