/* Una.Drawing                                                 ____ ___
 *   A declarative drawing library for FFXIV.                 |    |   \____ _____        ____                _
 *                                                            |    |   /    \\__  \      |    \ ___ ___ _ _ _|_|___ ___
 * By Una. Licensed under AGPL-3.                             |    |  |   |  \/ __ \_    |  |  |  _| .'| | | | |   | . |
 * https://github.com/una-xiv/drawing                         |______/|___|  (____  / [] |____/|_| |__,|_____|_|_|_|_  |
 * ----------------------------------------------------------------------- \/ --- \/ ----------------------------- |__*/

using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Una.Drawing;

public sealed class ComputedStyle
{
    /// <summary>
    /// Invoked when a property that affects the layout has changed.
    /// </summary>
    internal Action? OnLayoutPropertyChanged;

    /// <summary>
    /// Invoked when a property that affects the graphical aspects of the node
    /// have been changed.
    /// </summary>
    internal Action? OnPaintPropertyChanged;

    internal ComputedStyle()
    {
        Reset();
    }

    /// <inheritdoc cref="Style.IsVisible"/>
    public bool IsVisible { get; private set; } = true;

    /// <inheritdoc cref="Style.Anchor"/>
    public Anchor Anchor { get; private set; } = Anchor.TopLeft;

    /// <inheritdoc cref="Style.Size"/>
    public Size Size { get; private set; } = new();

    /// <inheritdoc cref="Style.Flow"/>
    public Flow Flow { get; private set; } = Flow.Horizontal;

    /// <inheritdoc cref="Style.Gap"/>
    public int Gap { get; private set; }

    /// <inheritdoc cref="Style.Padding"/>
    public EdgeSize Padding { get; private set; } = new();

    /// <inheritdoc cref="Style.Margin"/>
    public EdgeSize Margin { get; private set; } = new();

    public bool Stretch { get; private set; }

    /// <inheritdoc cref="Style.Color"/>
    public Color Color { get; private set; } = new(0xFFC0C0C0);

    /// <inheritdoc cref="Style.Font"/>
    public uint Font { get; private set; }

    /// <inheritdoc cref="Style.FontSize"/>
    public int FontSize { get; private set; }

    /// <inheritdoc cref="Style.LineHeight"/>
    public float LineHeight { get; private set; }

    /// <inheritdoc cref="Style.WordWrap"/>
    public bool WordWrap { get; private set; }

    /// <inheritdoc cref="Style.TextAlign"/>
    public Anchor TextAlign { get; private set; } = Anchor.TopLeft;

    /// <inheritdoc cref="Style.OutlineSize"/>
    public float OutlineSize { get; private set; }

    /// <inheritdoc cref="Style.TextOffset"/>
    public Vector2 TextOffset { get; private set; }

    /// <inheritdoc cref="Style.TextOverflow"/>
    public bool TextOverflow { get; private set; } = true;

    /// <inheritdoc cref="Style.BackgroundColor"/>
    public Color? BackgroundColor { get; private set; }

    /// <inheritdoc cref="Style.BorderColor"/>
    public BorderColor? BorderColor { get; private set; }

    /// <inheritdoc cref="Style.BorderInset"/>
    public EdgeSize BorderInset { get; private set; } = new();

    /// <inheritdoc cref="Style.BorderRadius"/>
    public int BorderRadius { get; private set; }

    /// <inheritdoc cref="Style.BorderWidth"/>
    public EdgeSize BorderWidth { get; private set; } = new();

    /// <inheritdoc cref="Style.StrokeColor"/>
    public Color? StrokeColor { get; private set; }

    /// <inheritdoc cref="Style.StrokeWidth"/>
    public int StrokeWidth { get; private set; }

    /// <inheritdoc cref="Style.StrokeInset"/>
    public float StrokeInset { get; private set; }

    /// <inheritdoc cref="Style.RoundedCorners"/>
    public float? StrokeRadius { get; private set; }

    /// <inheritdoc cref="Style.RoundedCorners"/>
    public RoundedCorners RoundedCorners { get; private set; }

    /// <inheritdoc cref="Style.BackgroundGradient"/>
    public GradientColor? BackgroundGradient { get; private set; }

    /// <inheritdoc cref="Style.BackgroundGradientInset"/>
    public EdgeSize BackgroundGradientInset { get; private set; } = new();

    /// <inheritdoc cref="Style.BackgroundImage"/>
    public object? BackgroundImage { get; set; }

    /// <inheritdoc cref="Style.BackgroundImageInset"/>
    public EdgeSize BackgroundImageInset { get; set; } = new(0, 0);

    /// <inheritdoc cref="Style.BackgroundImageScale"/>
    public Vector2 BackgroundImageScale { get; set; } = new(1, 1);

    /// <inheritdoc cref="Style.BackgroundImageColor"/>
    public Color BackgroundImageColor { get; set; } = new(0xFFFFFFFF);

    /// <inheritdoc cref="Style.BackgroundImageRotation"/>
    public short BackgroundImageRotation { get; set; }

    /// <inheritdoc cref="Style.BackgroundImageBlendMode"/>
    public BlendMode BackgroundImageBlendMode { get; set; }

    /// <inheritdoc cref="Style.OutlineColor"/>
    public Color? OutlineColor { get; private set; }

    /// <inheritdoc cref="Style.TextShadowSize"/>
    public float TextShadowSize { get; private set; }

    /// <inheritdoc cref="Style.TextShadowColor"/>
    public Color? TextShadowColor { get; private set; }

    /// <inheritdoc cref="Style.IconId"/>
    public uint? IconId { get; private set; }

    /// <inheritdoc cref="Style.ImageBytes"/>
    public byte[]? ImageBytes { get; private set; }

    /// <inheritdoc cref="Style.ImageInset"/>
    public EdgeSize? ImageInset { get; private set; }

    /// <inheritdoc cref="Style.ImageOffset"/>
    public Vector2? ImageOffset { get; private set; }

    /// <inheritdoc cref="Style.ImageRounding"/>
    public float ImageRounding { get; private set; }

    /// <inheritdoc cref="Style.ImageRoundedCorners"/>
    public RoundedCorners ImageRoundedCorners { get; private set; }

    /// <inheritdoc cref="Style.ImageGrayscale"/>
    public bool ImageGrayscale { get; private set; }

    /// <inheritdoc cref="Style.ImageContrast"/>
    public float ImageContrast { get; private set; }

    /// <inheritdoc cref="Style.ImageRotation"/>
    public short ImageRotation { get; private set; }

    /// <inheritdoc cref="Style.ImageColor"/>
    public Color ImageColor { get; private set; } = new(0xFFFFFFFF);

    /// <inheritdoc cref="Style.ImageBlendMode"/>
    public BlendMode ImageBlendMode { get; private set; }

    /// <inheritdoc cref="Style.Opacity"/>
    public float Opacity { get; private set; }

    /// <inheritdoc cref="Style.ShadowSize"/>
    public EdgeSize ShadowSize { get; private set; } = new();

    /// <inheritdoc cref="Style.ShadowInset"/>
    public int ShadowInset { get; private set; }

    /// <inheritdoc cref="Style.ShadowOffset"/>
    public Vector2 ShadowOffset { get; private set; }

    /// <inheritdoc cref="Style.IsAntialiased"/>
    public bool IsAntialiased { get; private set; }

    /// <inheritdoc cref="Style.ScrollbarTrackColor"/>
    public Color ScrollbarTrackColor { get; private set; } = new(0xFF404040);

    /// <inheritdoc cref="Style.ScrollbarThumbColor"/>
    public Color ScrollbarThumbColor { get; private set; } = new(0xFFA0A0A0);

    /// <inheritdoc cref="Style.ScrollbarThumbHoverColor"/>
    public Color ScrollbarThumbHoverColor { get; private set; } = new(0xFFC0C0C0);

    /// <inheritdoc cref="Style.ScrollbarThumbActiveColor"/>
    public Color ScrollbarThumbActiveColor { get; private set; } = new(0xFFFFFFFF);

    internal bool HasDrawables()
    {
        return BackgroundColor is not null
            || BorderColor is not null
            || StrokeColor is not null
            || BackgroundGradient is not null
            || BackgroundImage is not null
            || OutlineColor is not null
            || TextShadowColor is not null
            || IconId is not null
            || ImageBytes is not null;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    internal void Apply(Style style)
    {
        IsVisible                 = style.IsVisible ?? IsVisible;
        Anchor                    = style.Anchor ?? Anchor;
        Size                      = style.Size ?? Size;
        Flow                      = style.Flow ?? Flow;
        Gap                       = style.Gap ?? Gap;
        Stretch                   = style.Stretch ?? Stretch;
        Padding                   = style.Padding ?? Padding;
        Margin                    = style.Margin ?? Margin;
        Color                     = style.Color ?? Color;
        Font                      = style.Font ?? Font;
        FontSize                  = style.FontSize ?? FontSize;
        LineHeight                = style.LineHeight ?? LineHeight;
        WordWrap                  = style.WordWrap ?? WordWrap;
        TextAlign                 = style.TextAlign ?? TextAlign;
        OutlineSize               = style.OutlineSize ?? OutlineSize;
        TextOffset                = style.TextOffset ?? TextOffset;
        TextOverflow              = style.TextOverflow ?? TextOverflow;
        BackgroundColor           = style.BackgroundColor ?? BackgroundColor;
        BorderColor               = style.BorderColor ?? BorderColor;
        BorderInset               = style.BorderInset ?? BorderInset;
        BorderRadius              = style.BorderRadius ?? BorderRadius;
        BorderWidth               = style.BorderWidth ?? BorderWidth;
        StrokeColor               = style.StrokeColor ?? StrokeColor;
        StrokeWidth               = style.StrokeWidth ?? StrokeWidth;
        StrokeInset               = style.StrokeInset ?? StrokeInset;
        StrokeRadius              = style.StrokeRadius ?? StrokeRadius;
        RoundedCorners            = style.RoundedCorners ?? RoundedCorners;
        BackgroundGradient        = style.BackgroundGradient ?? BackgroundGradient;
        BackgroundGradientInset   = style.BackgroundGradientInset ?? BackgroundGradientInset;
        BackgroundImage           = style.BackgroundImage ?? BackgroundImage;
        BackgroundImageInset      = style.BackgroundImageInset ?? BackgroundImageInset;
        BackgroundImageScale      = style.BackgroundImageScale ?? BackgroundImageScale;
        BackgroundImageColor      = style.BackgroundImageColor ?? BackgroundImageColor;
        BackgroundImageRotation   = style.BackgroundImageRotation ?? BackgroundImageRotation;
        BackgroundImageBlendMode  = style.BackgroundImageBlendMode ?? BackgroundImageBlendMode;
        OutlineColor              = style.OutlineColor ?? OutlineColor;
        TextShadowSize            = style.TextShadowSize ?? TextShadowSize;
        TextShadowColor           = style.TextShadowColor ?? TextShadowColor;
        IconId                    = style.IconId ?? IconId;
        ImageBytes                = style.ImageBytes ?? ImageBytes;
        ImageInset                = style.ImageInset ?? ImageInset;
        ImageOffset               = style.ImageOffset ?? ImageOffset;
        ImageRounding             = style.ImageRounding ?? ImageRounding;
        ImageRoundedCorners       = style.ImageRoundedCorners ?? ImageRoundedCorners;
        ImageGrayscale            = style.ImageGrayscale ?? ImageGrayscale;
        ImageContrast             = style.ImageContrast ?? ImageContrast;
        ImageRotation             = style.ImageRotation ?? ImageRotation;
        ImageColor                = style.ImageColor ?? ImageColor;
        ImageBlendMode            = style.ImageBlendMode ?? ImageBlendMode;
        Opacity                   = style.Opacity ?? Opacity;
        ShadowSize                = style.ShadowSize ?? ShadowSize;
        ShadowInset               = style.ShadowInset ?? ShadowInset;
        ShadowOffset              = style.ShadowOffset ?? ShadowOffset;
        IsAntialiased             = style.IsAntialiased ?? IsAntialiased;
        ScrollbarTrackColor       = style.ScrollbarTrackColor ?? ScrollbarTrackColor;
        ScrollbarThumbColor       = style.ScrollbarThumbColor ?? ScrollbarThumbColor;
        ScrollbarThumbHoverColor  = style.ScrollbarThumbHoverColor ?? ScrollbarThumbHoverColor;
        ScrollbarThumbActiveColor = style.ScrollbarThumbActiveColor ?? ScrollbarThumbActiveColor;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    internal void ApplyScaleFactor()
    {
        Size         *= Node.ScaleFactor;
        Padding      *= Node.ScaleFactor;
        Margin       *= Node.ScaleFactor;
        FontSize     =  (int)(FontSize * Node.ScaleFactor);
        TextOffset   *= Node.ScaleFactor;
        BorderInset  *= Node.ScaleFactor;
        OutlineSize  *= Node.ScaleFactor;
        BorderRadius =  (int)(BorderRadius * Node.ScaleFactor);

        BackgroundGradientInset *= Node.ScaleFactor;

        if (Node.ScaleAffectsBorders) {
            BorderWidth *= Node.ScaleFactor;
            StrokeWidth =  (int)Math.Ceiling(StrokeWidth * Node.ScaleFactor);
        }

        StrokeInset    *= Node.ScaleFactor;
        StrokeRadius   *= Node.ScaleFactor;
        TextShadowSize *= Node.ScaleFactor;
        ImageInset     *= Node.ScaleFactor;
        ImageRounding  *= Node.ScaleFactor;
        ImageOffset    *= Node.ScaleFactor;
        ShadowSize     *= Node.ScaleFactor;
        ShadowOffset   *= Node.ScaleFactor;
        ShadowInset    =  (int)(ShadowInset * Node.ScaleFactor);
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    internal void Reset()
    {
        IsVisible                 = true;
        Anchor                    = Anchor.TopLeft;
        Size                      = new();
        Flow                      = Flow.Horizontal;
        Gap                       = 0;
        Stretch                   = false;
        Padding                   = new();
        Margin                    = new();
        Color                     = new(0xFFC0C0C0);
        Font                      = 0;
        FontSize                  = 12;
        LineHeight                = 1.2f;
        WordWrap                  = false;
        TextAlign                 = Anchor.TopLeft;
        OutlineColor              = new(0x00000000);
        OutlineSize               = 1;
        TextOffset                = Vector2.Zero;
        TextOverflow              = true;
        BackgroundColor           = null;
        BorderColor               = null;
        BorderInset               = new();
        BorderRadius              = 0;
        BorderWidth               = new();
        StrokeColor               = null;
        StrokeWidth               = 0;
        StrokeInset               = 0;
        StrokeRadius              = null;
        RoundedCorners            = RoundedCorners.All;
        BackgroundGradient        = null;
        BackgroundGradientInset   = new();
        BackgroundImage           = null;
        BackgroundImageInset      = new(0, 0);
        BackgroundImageScale      = new(1, 1);
        BackgroundImageColor      = new(0xFFFFFFFF);
        BackgroundImageBlendMode  = BlendMode.SrcIn;
        OutlineColor              = null;
        TextShadowSize            = 0;
        TextShadowColor           = null;
        IconId                    = null;
        ImageBytes                = null;
        ImageInset                = null;
        ImageOffset               = null;
        ImageRounding             = 0;
        ImageRoundedCorners       = RoundedCorners.All;
        ImageGrayscale            = false;
        ImageContrast             = 0;
        ImageRotation             = 0;
        ImageColor                = new(0xFFFFFFFF);
        ImageBlendMode            = BlendMode.SrcIn;
        Opacity                   = 1;
        ShadowSize                = new();
        ShadowInset               = 0;
        ShadowOffset              = Vector2.Zero;
        IsAntialiased             = true;
        ScrollbarTrackColor       = new(0xFF404040);
        ScrollbarThumbColor       = new(0xFFA0A0A0);
        ScrollbarThumbHoverColor  = new(0xFFC0C0C0);
        ScrollbarThumbActiveColor = new(0xFFFFFFFF);
    }

    internal LayoutStyle CommittedLayoutStyle;
    internal PaintStyle  CommittedPaintStyle;

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    internal int Commit()
    {
        LayoutStyle ls = new() {
            Anchor        = Anchor.Point,
            IsVisible     = IsVisible,
            Width         = Size.Width,
            Height        = Size.Height,
            Flow          = Flow,
            Gap           = Gap,
            Stretch       = Stretch,
            PaddingTop    = Padding.Top,
            PaddingRight  = Padding.Right,
            PaddingBottom = Padding.Bottom,
            PaddingLeft   = Padding.Left,
            MarginTop     = Margin.Top,
            MarginRight   = Margin.Right,
            MarginBottom  = Margin.Bottom,
            MarginLeft    = Margin.Left,
            WordWrap      = WordWrap,
            Font          = Font,
            FontSize      = FontSize,
            LineHeight    = LineHeight,
            TextOverflow  = TextOverflow
        };

        PaintStyle ps = new() {
            Color                         = Color.ToUInt(),
            TextAlign                     = TextAlign.Point,
            OutlineSize                   = OutlineSize,
            TextOffset                    = TextOffset,
            BackgroundColor               = BackgroundColor?.ToUInt(),
            BorderTopColor                = BorderColor?.Top?.ToUInt(),
            BorderRightColor              = BorderColor?.Right?.ToUInt(),
            BorderBottomColor             = BorderColor?.Bottom?.ToUInt(),
            BorderLeftColor               = BorderColor?.Left?.ToUInt(),
            BorderInsetTop                = BorderInset.Top,
            BorderInsetRight              = BorderInset.Right,
            BorderInsetBottom             = BorderInset.Bottom,
            BorderInsetLeft               = BorderInset.Left,
            BorderRadius                  = BorderRadius,
            BorderTopWidth                = BorderWidth.Top,
            BorderRightWidth              = BorderWidth.Right,
            BorderBottomWidth             = BorderWidth.Bottom,
            BorderLeftWidth               = BorderWidth.Left,
            StrokeColor                   = StrokeColor?.ToUInt(),
            StrokeWidth                   = StrokeWidth,
            StrokeInset                   = StrokeInset,
            StrokeRadius                  = StrokeRadius,
            BackgroundGradient1           = BackgroundGradient?.Color1?.ToUInt(),
            BackgroundGradient2           = BackgroundGradient?.Color2?.ToUInt(),
            BackgroundGradientInsetTop    = BackgroundGradientInset.Top,
            BackgroundGradientInsetRight  = BackgroundGradientInset.Right,
            BackgroundGradientInsetBottom = BackgroundGradientInset.Bottom,
            BackgroundGradientInsetLeft   = BackgroundGradientInset.Left,
            BackgroundImageIconId         = BackgroundImage is uint image ? image : null,
            BackgroundImageByteSize       = BackgroundImage is byte[] bytes ? (uint)bytes.Length : null,
            BackgroundImageInsetBottom    = BackgroundImageInset.Bottom,
            BackgroundImageInsetLeft      = BackgroundImageInset.Left,
            BackgroundImageInsetRight     = BackgroundImageInset.Right,
            BackgroundImageInsetTop       = BackgroundImageInset.Top,
            BackgroundImageRepeatX        = BackgroundImageScale.X,
            BackgroundImageRepeatY        = BackgroundImageScale.Y,
            BackgroundImageColor          = BackgroundImageColor.ToUInt(),
            BackgroundImageRotation       = BackgroundImageRotation,
            BackgroundImageBlendMode      = (byte)BackgroundImageBlendMode,
            OutlineColor                  = OutlineColor?.ToUInt(),
            TextShadowSize                = TextShadowSize,
            TextShadowColor               = TextShadowColor?.ToUInt(),
            IconId                        = IconId,
            ImageInsetTop                 = ImageInset?.Top,
            ImageInsetRight               = ImageInset?.Right,
            ImageInsetBottom              = ImageInset?.Bottom,
            ImageInsetLeft                = ImageInset?.Left,
            ImageOffsetX                  = ImageOffset?.X,
            ImageOffsetY                  = ImageOffset?.Y,
            ImageRounding                 = ImageRounding,
            ImageRoundedCorners           = ImageRoundedCorners,
            ImageGrayscale                = ImageGrayscale,
            ImageContrast                 = ImageContrast,
            ImageRotation                 = ImageRotation,
            ImageColor                    = ImageColor.ToUInt(),
            ImageBlendMode                = (byte)ImageBlendMode,
            IsAntialiased                 = IsAntialiased
        };

        var result = 0;

        if (AreStructsEqual(ref CommittedLayoutStyle, ref ls) is false) {
            CommittedLayoutStyle = ls;
            OnLayoutPropertyChanged?.Invoke();
            result = 1;
        }

        if (AreStructsEqual(ref CommittedPaintStyle, ref ps) is false) {
            CommittedPaintStyle = ps;
            OnPaintPropertyChanged?.Invoke();
            result += 2;
        }

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static bool AreStructsEqual<T>(ref readonly T a, ref readonly T b) where T : unmanaged
    {
        return MemoryMarshal
            .AsBytes(new ReadOnlySpan<T>(in a))
            .SequenceEqual(MemoryMarshal.AsBytes(new ReadOnlySpan<T>(in b)));
    }
}

[StructLayout(LayoutKind.Sequential)]
internal struct LayoutStyle
{
    internal Anchor.AnchorPoint Anchor;
    internal bool               IsVisible;
    internal int                Width;
    internal int                Height;
    internal Flow               Flow;
    internal int                Gap;
    internal bool               Stretch;
    internal int                PaddingTop;
    internal int                PaddingRight;
    internal int                PaddingBottom;
    internal int                PaddingLeft;
    internal int                MarginTop;
    internal int                MarginRight;
    internal int                MarginBottom;
    internal int                MarginLeft;
    internal bool               WordWrap;
    internal uint               Font;
    internal int                FontSize;
    internal float              LineHeight;
    internal bool               TextOverflow;
}

[StructLayout(LayoutKind.Sequential)]
internal struct PaintStyle
{
    internal Anchor.AnchorPoint TextAlign;
    internal float              OutlineSize;
    internal Vector2            TextOffset;
    internal uint               Color;
    internal uint?              BackgroundColor;
    internal uint?              BorderTopColor;
    internal uint?              BorderRightColor;
    internal uint?              BorderBottomColor;
    internal uint?              BorderLeftColor;
    internal float              BorderInsetTop;
    internal float              BorderInsetRight;
    internal float              BorderInsetBottom;
    internal float              BorderInsetLeft;
    internal int                BorderRadius;
    internal int                BorderTopWidth;
    internal int                BorderRightWidth;
    internal int                BorderBottomWidth;
    internal int                BorderLeftWidth;
    internal uint?              StrokeColor;
    internal int                StrokeWidth;
    internal float              StrokeInset;
    internal float?             StrokeRadius;
    internal RoundedCorners?    RoundedCorners;
    internal uint?              BackgroundGradient1;
    internal uint?              BackgroundGradient2;
    internal float?             BackgroundGradientInsetTop;
    internal float?             BackgroundGradientInsetRight;
    internal float?             BackgroundGradientInsetBottom;
    internal float?             BackgroundGradientInsetLeft;
    internal uint?              BackgroundImageIconId;
    internal uint?              BackgroundImageByteSize;
    internal float?             BackgroundImageInsetTop;
    internal float?             BackgroundImageInsetRight;
    internal float?             BackgroundImageInsetBottom;
    internal float?             BackgroundImageInsetLeft;
    internal float?             BackgroundImageRepeatX;
    internal float?             BackgroundImageRepeatY;
    internal uint?              BackgroundImageColor;
    internal short?             BackgroundImageRotation;
    internal byte?              BackgroundImageBlendMode;
    internal uint?              OutlineColor;
    internal float              TextShadowSize;
    internal uint?              TextShadowColor;
    internal uint?              IconId;
    internal int?               ImageInsetTop;
    internal int?               ImageInsetRight;
    internal int?               ImageInsetBottom;
    internal int?               ImageInsetLeft;
    internal float?             ImageOffsetX;
    internal float?             ImageOffsetY;
    internal float?             ImageRounding;
    internal RoundedCorners?    ImageRoundedCorners;
    internal bool?              ImageGrayscale;
    internal float?             ImageContrast;
    internal short?             ImageRotation;
    internal uint?              ImageColor;
    internal byte?              ImageBlendMode;
    internal bool               IsAntialiased;
}
