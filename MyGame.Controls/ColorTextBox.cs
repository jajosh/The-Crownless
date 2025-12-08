using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms; // This is the key namespace for the UI Timer
using System.Windows.Forms.Design;
using System.Windows.Forms.VisualStyles;

namespace MyGame.Controls
{
    // ========================================================================
    // ColorComponent (Helper Class)
    // ========================================================================
    #region 🎨 ColorComponent (Data)
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ColorComponent
    {
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
        public int A { get; set; } = 255;

        public ColorComponent() : this(255, 255, 255, 255) { }
        public ColorComponent(int r, int g, int b) : this(255, r, g, b) { }

        public ColorComponent(int a, int r, int g, int b)
        {
            A = Clamp(a, 0, 255);
            R = Clamp(r, 0, 255);
            G = Clamp(g, 0, 255);
            B = Clamp(b, 0, 255);
        }

        public Color ToColor() => Color.FromArgb(A, R, G, B);

        public static ColorComponent FromColor(Color color) =>
            new ColorComponent(color.A, color.R, color.G, color.B);

        public static ColorComponent Lerp(ColorComponent a, ColorComponent b, float t)
        {
            t = Clamp(t, 0f, 1f);
            return new ColorComponent(
                (int)(a.A + (b.A - a.A) * t),
                (int)(a.R + (b.R - a.R) * t),
                (int)(a.G + (b.G - a.G) * t),
                (int)(a.B + (b.B - a.B) * t)
            );
        }

        // Helper for .NET Framework compatibility
        private static int Clamp(int value, int min, int max) => (value < min) ? min : (value > max) ? max : value;
        private static float Clamp(float value, float min, float max) => (value < min) ? min : (value > max) ? max : value;

        public override string ToString() => $"A={A}, R={R}, G={G}, B={B}";
    }
    #endregion

    // ========================================================================
    // StringArrayConverter (Helper Class)
    // ========================================================================
    #region 🛠 StringArrayConverter (Designer)
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
    #endregion

    // ========================================================================
    // MAIN CONTROL: ColorTextBox
    // ========================================================================
    [ToolboxItem(true)]
    [Designer(typeof(ControlDesigner))]
    public class ColorTextBox : Control
    {
        // --------------------------------------------------------------------
        // Data Structures
        // --------------------------------------------------------------------
        #region 💾 Data Structures

        public class CharData
        {
            public char Char { get; set; }
            public ColorComponent Color { get; set; } = new ColorComponent(255, 255, 255);
            public bool IsSelected { get; set; }

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

            public CharData() { }
            public CharData(char c) { Char = c; }
        }

        public class OverlayStep
        {
            public ColorComponent Color { get; set; } = new ColorComponent(0, 0, 0, 0);
            public string FontFamily { get; set; }
            public FontStyle? FontStyle { get; set; }
            public float SizeMultiplier { get; set; } = 1f;
        }

        #endregion

        // --------------------------------------------------------------------
        // Fields & State
        // --------------------------------------------------------------------
        #region ⚙️ Fields & State

        private readonly List<CharData> _chars = new List<CharData>();
        private readonly Dictionary<(string, float, FontStyle), Font> _fontCache = new Dictionary<(string, float, FontStyle), Font>();

        // Metrics
        private float _charWidth;
        private float _lineHeight;
        private int _firstVisibleLine;
        private int _letterSpacing;
        private HorizontalAlignment _textAlign = HorizontalAlignment.Left;
        private VerticalAlignment _verticalAlign = VerticalAlignment.Top;

        // Animation Timers (FIXED: Explicitly using System.Windows.Forms.Timer)
        private System.Windows.Forms.Timer _shimmerTimer;
        private int _shimmerIndex = 0;
        private System.Windows.Forms.Timer _pulseTimer;
        private float _totalTime = 0f;
        private DateTime _lastFrame = DateTime.MinValue;
        private System.Windows.Forms.Timer _backAnimTimer;
        private System.Windows.Forms.Timer _colorShiftTimer;

        // Background State
        private ColorComponent _currentBackColor = new ColorComponent(0, 0, 0);
        private ColorComponent _colorShiftCurrent = new ColorComponent(255, 255, 255);
        private Image _currentBackgroundImage;
        private float _backgroundAlpha = 0f;

        // Target State
        private ColorComponent _targetBackColor = new ColorComponent(0, 0, 0);
        private ColorComponent _colorShiftTarget = new ColorComponent(255, 255, 255);

        private int _widthInChars = 51; // Default to grid width for consistency

        #endregion

        // --------------------------------------------------------------------
        // Control Properties
        // --------------------------------------------------------------------
        #region 🧱 Control Properties

        private ColorComponent _overlayTint = new ColorComponent(0, 0, 0, 0);
        private ColorComponent _shadowColor = new ColorComponent(120, 0, 0, 0);
        private ColorComponent _selectionBackground = ColorComponent.FromColor(SystemColors.Highlight);
        private ColorComponent _selectionColor = ColorComponent.FromColor(SystemColors.HighlightText);

        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ColorComponent OverlayTint { get => _overlayTint; set { _overlayTint = value; Invalidate(); } }

        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ColorComponent ShadowColor { get => _shadowColor; set { _shadowColor = value; Invalidate(); } }

        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ColorComponent SelectionBackground { get => _selectionBackground; set { _selectionBackground = value; Invalidate(); } }

        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ColorComponent SelectionColor { get => _selectionColor; set { _selectionColor = value; Invalidate(); } }

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

        // Global Scaling
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

        // Global Font Styles
        [Category("Appearance"), DefaultValue(null)]
        public string ShadowFontFamily { get; set; }

        [Category("Appearance"), DefaultValue(FontStyle.Regular)]
        public FontStyle ShadowFontStyle { get; set; } = FontStyle.Regular;

        [Category("Appearance"), DefaultValue(null)]
        public string OverlayFontFamily { get; set; }

        [Category("Appearance"), DefaultValue(FontStyle.Regular)]
        public FontStyle OverlayFontStyle { get; set; } = FontStyle.Regular;

        #endregion

        // --------------------------------------------------------------------
        // Layout & Grid Locking
        // --------------------------------------------------------------------
        #region 📏 Layout & Grid Locking

        [Category("Layout"), DefaultValue(51)]
        [Browsable(false)]
        public int FixedWidthInChars { get; } = 51;

        [Category("Layout"), DefaultValue(25)]
        [Browsable(false)]
        public int FixedHeightInChars { get; } = 25;

        [Category("Layout"), DefaultValue(true)]
        [Description("When true, the text buffer is locked to 51 columns and 25 rows.")]
        [RefreshProperties(RefreshProperties.All)]
        public bool LockToGrid { get; set; } = true;

        [Category("Layout"), DefaultValue(false)]
        public bool AutoWidthInChars { get; set; } = false;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int WidthInChars
        {
            get => LockToGrid ? FixedWidthInChars : _widthInChars;
            set
            {
                if (!LockToGrid)
                {
                    _widthInChars = value;
                    Invalidate();
                }
                // Ignored if LockToGrid is true
            }
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            if (LockToGrid)
            {
                // Calculate the required pixel width and height for a 51x25 grid
                int requiredWidth = (int)Math.Ceiling(FixedWidthInChars * CharAdvance) + Padding.Horizontal;
                int requiredHeight = (int)Math.Ceiling(FixedHeightInChars * _lineHeight) + Padding.Vertical;
                return new Size(requiredWidth, requiredHeight);
            }
            return base.GetPreferredSize(proposedSize);
        }

        #endregion

        // --------------------------------------------------------------------
        // Effects Properties
        // --------------------------------------------------------------------
        #region ✨ Effects Properties

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

        [Category("Effects"), DefaultValue(0f)]
        public float ColorShiftStrength { get; set; } = 0f;

        [Category("Effects")]
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

        public List<OverlayStep> OverlaySteps { get; } = new List<OverlayStep>();

        [Category("Effects"), DefaultValue(100)]
        public int ShimmerIntervalMs { get; set; } = 100;

        #endregion

        // --------------------------------------------------------------------
        // Design Mode Properties
        // --------------------------------------------------------------------
        #region 🖼️ Design Mode

        private string[] _designTimeLines = new[]
        {
            "ColorTextBox Ready!",
            "LockToGrid: 51x25",
            "Coordinate Editing Enabled"
        };

        [Category("Design")]
        [Description("Multi-line preview text shown in the designer.")]
        [TypeConverter(typeof(StringArrayConverter))]
        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        // FIXED: Added Content serialization to fix the design mode warning/error
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public string[] DesignTimeLines
        {
            get => _designTimeLines;
            set { _designTimeLines = value ?? Array.Empty<string>(); Invalidate(); }
        }

        #endregion

        // --------------------------------------------------------------------
        // Constructor, Dispose, and Overrides
        // --------------------------------------------------------------------
        #region 🏗️ Init & Overrides

        public ColorTextBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.DoubleBuffer |
                     ControlStyles.Selectable |
                     ControlStyles.StandardClick |
                     ControlStyles.ResizeRedraw, true);

            base.BackColor = Color.Black;
            base.ForeColor = Color.White;
            TabStop = true;

            UpdateMetrics();
            if (LockToGrid) InitializeGridBuffer(); // Initial padding
            else if (AutoWidthInChars) RecalculateWidthInChars();

            OverlaySteps.Add(new OverlayStep { Color = _overlayTint });

            // FIXED: Use System.Windows.Forms.Timer for UI thread safety
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
                // FIXED: Timer disposal now works correctly for System.Windows.Forms.Timer
                _shimmerTimer?.Dispose();
                _pulseTimer?.Dispose();
                _backAnimTimer?.Dispose();
                _colorShiftTimer?.Dispose();
            }
            base.Dispose(disposing);
        }

        [Category("Appearance")]
        [Description("The text associated with the control.")]
        public override string Text
        {
            get
            {
                // Returns string based on current CharData
                return string.Join("", _chars.Select(c => c.Char));
            }
            set
            {
                // Note: This wipes out custom colors/effects/etc. for simple text setting.
                _chars.Clear();
                if (!string.IsNullOrEmpty(value))
                {
                    foreach (char c in value)
                    {
                        _chars.Add(new CharData(c) { Color = ColorComponent.FromColor(this.ForeColor) });
                    }
                }
                if (LockToGrid) InitializeGridBuffer();
                Invalidate();
            }
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

                // Force Monospace if needed for grid alignment logic
                base.Font = Math.Abs(wi - wM) > 1f
                    ? new Font("Consolas", value.SizeInPoints, value.Style)
                    : value;

                // Clear cache on font change to save memory
                foreach (var f in _fontCache.Values) f.Dispose();
                _fontCache.Clear();
            }
        }

        #endregion

        // --------------------------------------------------------------------
        // Metrics & Helpers
        // --------------------------------------------------------------------
        #region 📊 Metrics & Helpers

        private void InitializeGridBuffer()
        {
            if (!LockToGrid) return;

            int totalSize = FixedWidthInChars * FixedHeightInChars;

            // Pad the buffer with spaces up to the fixed size
            while (_chars.Count < totalSize)
            {
                _chars.Add(new CharData(' ')); // Pad with empty CharData
            }

            // Truncate if buffer is too large
            if (_chars.Count > totalSize)
            {
                _chars.RemoveRange(totalSize, _chars.Count - totalSize);
            }
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
            // Only used when LockToGrid is false
            _widthInChars = Math.Max(1, (int)Math.Floor(available / advance));
        }

        private Font GetCachedFont(string familyName, float sizePoints, FontStyle style)
        {
            string family = familyName ?? Font.FontFamily.Name;
            if (sizePoints <= 0) sizePoints = 1f;

            var key = (family, sizePoints, style);
            if (!_fontCache.TryGetValue(key, out var font))
            {
                try
                {
                    font = new Font(family, sizePoints, style);
                }
                catch
                {
                    font = new Font(FontFamily.GenericMonospace, sizePoints, style);
                }
                _fontCache[key] = font;
            }
            return font;
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            UpdateMetrics();
            if (LockToGrid) InitializeGridBuffer();
            else if (AutoWidthInChars) RecalculateWidthInChars();
            Invalidate();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (LockToGrid)
            {
                // Do nothing to WidthInChars, but refresh the visual
            }
            else if (AutoWidthInChars) RecalculateWidthInChars();
            Invalidate();
        }

        protected override void OnPaddingChanged(EventArgs e)
        {
            base.OnPaddingChanged(e);
            if (LockToGrid)
            {
                // Refresh only
            }
            else if (AutoWidthInChars) RecalculateWidthInChars();
            Invalidate();
        }

        private int TotalLines => LockToGrid ? FixedHeightInChars : (_chars.Count + WidthInChars - 1) / WidthInChars;
        private int VisibleLines => _lineHeight > 0 ? Math.Max(1, (int)Math.Floor((ClientSize.Height - Padding.Vertical) / _lineHeight)) : 1;
        private float CharAdvance => _charWidth + LetterSpacing;

        #endregion

        // --------------------------------------------------------------------
        // Public API for Data Manipulation
        // --------------------------------------------------------------------
        #region ⌨️ Public API (Read/Write)

        public List<CharData> GetCharData() => _chars;

        /// <summary>
        /// Clears the internal buffer and sets new character data.
        /// </summary>
        public void SetCharData(IEnumerable<CharData> newChars)
        {
            _chars.Clear();
            if (newChars != null) _chars.AddRange(newChars);
            if (LockToGrid) InitializeGridBuffer();
            Invalidate();
        }

        /// <summary>
        /// Sets the CharData at a specific grid coordinate (X, Y).
        /// X is column (0 to Width-1), Y is row (0 to Height-1).
        /// </summary>
        public void SetCharDataAt(int x, int y, CharData data)
        {
            if (!LockToGrid)
            {
                throw new InvalidOperationException("SetCharDataAt requires LockToGrid to be true.");
            }

            if (x < 0 || x >= FixedWidthInChars || y < 0 || y >= FixedHeightInChars)
            {
                throw new ArgumentOutOfRangeException("Coordinates are outside the fixed 51x25 grid.");
            }

            InitializeGridBuffer(); // Ensure buffer is padded

            int index = y * FixedWidthInChars + x;

            _chars[index] = data ?? new CharData(' ');

            Invalidate();
        }

        /// <summary>
        /// Appends text to the buffer. Used primarily when LockToGrid is false.
        /// </summary>
        public void AppendText(string text, ColorComponent color = null)
        {
            if (string.IsNullOrEmpty(text)) return;
            var c = color ?? ColorComponent.FromColor(ForeColor);
            foreach (var ch in text)
            {
                _chars.Add(new CharData(ch) { Color = c });
            }
            if (LockToGrid) InitializeGridBuffer();
            Invalidate();
        }

        #endregion

        // --------------------------------------------------------------------
        // Animation Logic
        // --------------------------------------------------------------------
        #region 🌀 Animation Logic

        public void RefreshShimmer()
        {
            if (OverlaySteps.Count > 1 && ShimmerIntervalMs > 0)
            {
                // FIXED: Use System.Windows.Forms.Timer
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
            // FIXED: Use System.Windows.Forms.Timer
            _colorShiftTimer = new System.Windows.Forms.Timer { Interval = 16 };
            _colorShiftCurrent = GetCurrentGlobalTint();
            _colorShiftTimer.Tick += (s, e) =>
            {
                float t = 0.1f;
                _colorShiftCurrent = ColorComponent.Lerp(_colorShiftCurrent, _colorShiftTarget, t);
                Invalidate();
                if (ColorShiftStrength == 0f || Math.Abs(_colorShiftCurrent.R - _colorShiftTarget.R) < 1)
                    _colorShiftTimer.Stop();
            };
            _colorShiftTimer.Start();
        }

        private ColorComponent GetCurrentGlobalTint() =>
            ColorShiftStrength > 0f ? _colorShiftCurrent : new ColorComponent(255, 255, 255);

        private ColorComponent ApplyColorShift(ColorComponent baseColor, CharData cd)
        {
            if (ColorShiftStrength <= 0f) return baseColor;
            if (cd.ColorShiftOverride != null) return cd.ColorShiftOverride;
            return ColorComponent.Lerp(baseColor, GetCurrentGlobalTint(), ColorShiftStrength);
        }

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

            _backAnimTimer?.Stop();
            // FIXED: Use System.Windows.Forms.Timer
            _backAnimTimer = new System.Windows.Forms.Timer { Interval = 16 };
            _backAnimTimer.Tick += (s, e) =>
            {
                step++;
                float t = step / (float)steps;
                _currentBackColor = ColorComponent.Lerp(start, color, t);
                base.BackColor = _currentBackColor.ToColor();
                Invalidate();
                if (step >= steps) _backAnimTimer.Stop();
            };
            _backAnimTimer.Start();
        }

        #endregion

        // --------------------------------------------------------------------
        // Rendering
        // --------------------------------------------------------------------
        #region 🖌️ Rendering Core

        private void DrawChar(Graphics g, float baseX, float baseY, char ch, ColorComponent colorComp, Font font,
                              float scale, float squishX, float squishY, Point offset,
                              float rippleX, float rippleY, StringFormat sf)
        {
            if (ch == ' ' || colorComp.A <= 0) return;

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

        protected override void OnPaint(PaintEventArgs e)
        {
            // --- Time Management ---
            var now = DateTime.Now;
            if (_lastFrame != DateTime.MinValue)
            {
                float delta = (float)(now - _lastFrame).TotalSeconds;
                _totalTime += delta;
            }
            _lastFrame = now;

            // --- Background Drawing ---
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
            using var sf = (StringFormat)StringFormat.GenericTypographic.Clone();

            // --- Design Mode Preview ---
            if (DesignMode && _chars.Count == 0)
            {
                // 
                // (Design mode rendering logic remains as a fallback display)
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

            // --- Neon Pulse Effect Calculation ---
            if (EnableNeonPulse && NeonPulseDurationMs > 0)
            {
                float pulseT = (float)Math.Sin(_totalTime / (NeonPulseDurationMs / 1000f) * Math.PI * 2) * 0.5f + 0.5f;
                float scale = NeonPulseMinScale + (NeonPulseMaxScale - NeonPulseMinScale) * pulseT;
                GlobalOverlayScale = scale;
                GlobalShadowScale = scale * 0.8f;

                byte a = (byte)(100 + pulseT * 155);
                OverlayTint = new ColorComponent(a, OverlayTint.R, OverlayTint.G, OverlayTint.B);
            }

            // --- Vertical Alignment Calculation ---
            float yOffset = 0f;
            if (TotalLines <= VisibleLines && _firstVisibleLine == 0)
            {
                float extra = ClientSize.Height - Padding.Vertical - TotalLines * _lineHeight;
                if (extra > 0)
                {
                    yOffset = VerticalAlign == VerticalAlignment.Center ? extra / 2f :
                              VerticalAlign == VerticalAlignment.Bottom ? extra : 0f;
                }
            }

            // --- Main Draw Loop ---
            float yPos = Padding.Top + yOffset;
            int row = _firstVisibleLine;
            int maxChars = _chars.Count;

            while (yPos < ClientSize.Height - Padding.Bottom && row < TotalLines)
            {
                int startIdx = row * WidthInChars;
                float charTimeOffset = row * 0.3f;

                // --- Alignment Check (Ignored/simplified when locked) ---
                int effectiveRowWidth = LockToGrid ? WidthInChars : Math.Min(WidthInChars, maxChars - startIdx);
                float offsetX = 0f;
                float baseX = Padding.Left + offsetX;

                for (int col = 0; col < effectiveRowWidth; col++)
                {
                    int idx = startIdx + col;
                    if (idx >= maxChars) break;
                    var cd = _chars[idx];

                    float xPos = baseX + (col * CharAdvance);

                    // --- RIPPLE CALCULATION ---
                    float rippleX = 0f, rippleY = 0f;
                    if (EnableRipple)
                    {
                        float phase = (_totalTime * RippleSpeed + col * RippleFrequency + charTimeOffset) * 2 * (float)Math.PI;
                        rippleY = (float)Math.Sin(phase) * RippleAmplitude;
                        if (RippleHorizontal)
                            rippleX = (float)Math.Cos(phase) * RippleAmplitude * 0.5f;
                    }

                    // --- SHADOW PASS ---
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
                        DrawChar(e.Graphics, xPos, yPos, cd.Char, shadowCol, font, scale, squishX, squishY, ShadowOffset, rippleX, rippleY, sf);
                    }

                    // --- MAIN PASS ---
                    if (cd.Char != ' ')
                    {
                        if (cd.IsSelected)
                        {
                            using var bg = new SolidBrush(SelectionBackground.ToColor());
                            e.Graphics.FillRectangle(bg, xPos, yPos, CharAdvance, _lineHeight);
                        }

                        string family = cd.MainFontFamily;
                        FontStyle style = cd.MainFontStyle ?? Font.Style;
                        float sizeMult = cd.MainFontSizeMultiplier * GlobalMainScale;
                        float scale = cd.MainScale * GlobalMainScale;

                        using Font font = GetCachedFont(family, Font.SizeInPoints * sizeMult, style);
                        var baseCol = cd.IsSelected ? SelectionColor : cd.Color;
                        var finalCol = ApplyColorShift(baseCol, cd);
                        DrawChar(e.Graphics, xPos, yPos, cd.Char, finalCol, font, scale, 1f, 1f, Point.Empty, rippleX, rippleY, sf);
                    }

                    // --- OVERLAY/SHIMMER PASS ---
                    if (OverlaySteps.Count > 0)
                    {
                        int stepIdx = _shimmerIndex >= 0 && _shimmerIndex < OverlaySteps.Count ? _shimmerIndex : 0;
                        var step = OverlaySteps[stepIdx];

                        var blendedOverlayColor = step.Color;
                        if (OverlayTint.A > 0)
                        {
                            blendedOverlayColor = ColorComponent.Lerp(step.Color, OverlayTint, 0.5f);
                        }

                        if (blendedOverlayColor.A > 0 && cd.Char != ' ')
                        {
                            string family = cd.OverlayFontFamily ?? step.FontFamily ?? OverlayFontFamily;
                            FontStyle style = cd.OverlayFontStyle ?? step.FontStyle ?? OverlayFontStyle;
                            float sizeMult = cd.OverlayFontSizeMultiplier * step.SizeMultiplier * GlobalOverlayScale;
                            float scale = GlobalOverlayScale;

                            using Font font = GetCachedFont(family, Font.SizeInPoints * sizeMult, style);
                            DrawChar(e.Graphics, xPos, yPos, cd.Char, blendedOverlayColor, font, scale, 1f, 1f, OverlayOffset, rippleX, rippleY, sf);
                        }
                    }
                } // End Column Loop

                yPos += _lineHeight;
                row++;
            } // End Row Loop
        }

        #endregion
    }
}