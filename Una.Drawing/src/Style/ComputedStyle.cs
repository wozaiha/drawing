/* Una.Drawing                                                 ____ ___
 *   A declarative drawing library for FFXIV.                 |    |   \____ _____        ____                _
 *                                                            |    |   /    \\__  \      |    \ ___ ___ _ _ _|_|___ ___
 * By Una. Licensed under AGPL-3.                             |    |  |   |  \/ __ \_    |  |  |  _| .'| | | | |   | . |
 * https://github.com/una-xiv/drawing                         |______/|___|  (____  / [] |____/|_| |__,|_____|_|_|_|_  |
 * ----------------------------------------------------------------------- \/ --- \/ ----------------------------- |__*/

using System;
using System.Numerics;

namespace Una.Drawing;

internal class ComputedStyle
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
    internal bool IsVisible { get; set; } = true;

    /// <inheritdoc cref="Style.Anchor"/>
    internal Anchor Anchor { get; set; } = Anchor.TopLeft;

    /// <inheritdoc cref="Style.Size"/>
    internal Size Size { get; set; } = new();

    /// <inheritdoc cref="Style.Flow"/>
    internal Flow Flow { get; set; } = Flow.Horizontal;

    /// <inheritdoc cref="Style.Gap"/>
    internal int Gap { get; set; }

    /// <inheritdoc cref="Style.Padding"/>
    internal EdgeSize Padding { get; set; } = new();

    /// <inheritdoc cref="Style.Margin"/>
    internal EdgeSize Margin { get; set; } = new();

    internal bool Stretch { get; set; }

    /// <inheritdoc cref="Style.Color"/>
    internal Color Color { get; set; } = new(0xFFC0C0C0);

    /// <inheritdoc cref="Style.Font"/>
    internal string Font { get; set; } = "default";

    /// <inheritdoc cref="Style.FontSize"/>
    internal float FontSize { get; set; }

    /// <inheritdoc cref="Style.LineHeight"/>
    internal float LineHeight { get; set; }

    /// <inheritdoc cref="Style.WordWrap"/>
    internal bool WordWrap { get; set; }

    /// <inheritdoc cref="Style.TextAlign"/>
    internal Anchor TextAlign { get; set; } = Anchor.TopLeft;

    /// <inheritdoc cref="Style.OutlineSize"/>
    internal float OutlineSize { get; set; }

    /// <inheritdoc cref="Style.TextOffset"/>
    internal Vector2 TextOffset { get; set; }

    /// <inheritdoc cref="Style.BackgroundColor"/>
    internal Color? BackgroundColor { get; set; }

    /// <inheritdoc cref="Style.BorderColor"/>
    internal BorderColor? BorderColor { get; set; }

    /// <inheritdoc cref="Style.BorderInset"/>
    internal int BorderInset { get; set; }

    /// <inheritdoc cref="Style.BorderRadius"/>
    internal int BorderRadius { get; set; }

    /// <inheritdoc cref="Style.BorderWidth"/>
    internal EdgeSize BorderWidth { get; set; } = new();

    /// <inheritdoc cref="Style.StrokeColor"/>
    internal Color? StrokeColor { get; set; }

    /// <inheritdoc cref="Style.StrokeWidth"/>
    internal int StrokeWidth { get; set; }

    /// <inheritdoc cref="Style.BackgroundGradient"/>
    internal GradientColor? BackgroundGradient { get; set; }

    /// <inheritdoc cref="Style.OutlineColor"/>
    internal Color? OutlineColor { get; set; }

    /// <inheritdoc cref="Style.TextShadowSize"/>
    internal float TextShadowSize { get; set; }

    /// <inheritdoc cref="Style.TextShadowColor"/>
    internal Color? TextShadowColor { get; set; }

    internal void Apply(Style style)
    {
        IsVisible          = style.IsVisible ?? IsVisible;
        Anchor             = style.Anchor ?? Anchor;
        Size               = style.Size ?? Size;
        Flow               = style.Flow ?? Flow;
        Gap                = style.Gap ?? Gap;
        Stretch            = style.Stretch ?? Stretch;
        Padding            = style.Padding ?? Padding;
        Margin             = style.Margin ?? Margin;
        Font               = style.Font ?? Font;
        FontSize           = style.FontSize ?? FontSize;
        LineHeight         = style.LineHeight ?? LineHeight;
        WordWrap           = style.WordWrap ?? WordWrap;
        TextAlign          = style.TextAlign ?? TextAlign;
        OutlineSize        = style.OutlineSize ?? OutlineSize;
        TextOffset         = style.TextOffset ?? TextOffset;
        BackgroundColor    = style.BackgroundColor ?? BackgroundColor;
        BorderColor        = style.BorderColor ?? BorderColor;
        BorderInset        = style.BorderInset ?? BorderInset;
        BorderRadius       = style.BorderRadius ?? BorderRadius;
        BorderWidth        = style.BorderWidth ?? BorderWidth;
        StrokeColor        = style.StrokeColor ?? StrokeColor;
        StrokeWidth        = style.StrokeWidth ?? StrokeWidth;
        BackgroundGradient = style.BackgroundGradient ?? BackgroundGradient;
        OutlineColor       = style.OutlineColor ?? OutlineColor;
        TextShadowSize     = style.TextShadowSize ?? TextShadowSize;
        TextShadowColor    = style.TextShadowColor ?? TextShadowColor;
    }

    internal void Reset()
    {
        IsVisible          = true;
        Anchor             = Anchor.TopLeft;
        Size               = new();
        Flow               = Flow.Horizontal;
        Gap                = 0;
        Stretch            = false;
        Padding            = new();
        Margin             = new();
        Color              = new(0xFFC0C0C0);
        Font               = "default";
        FontSize           = 12;
        LineHeight         = 1.2f;
        WordWrap           = false;
        TextAlign          = Anchor.TopLeft;
        OutlineSize        = 0;
        TextOffset         = Vector2.Zero;
        BackgroundColor    = null;
        BorderColor        = null;
        BorderInset        = 0;
        BorderRadius       = 0;
        BorderWidth        = new();
        StrokeColor        = null;
        StrokeWidth        = 0;
        BackgroundGradient = null;
        OutlineColor       = null;
        TextShadowSize     = 0;
        TextShadowColor    = null;
    }

    private LayoutStyle _prevLayoutStyle;
    private PaintStyle  _prevPaintStyle;

    internal int Commit()
    {
        LayoutStyle ls = new() {
            Anchor     = Anchor,
            IsVisible  = IsVisible,
            Size       = Size,
            Flow       = Flow,
            Gap        = Gap,
            Stretch    = Stretch,
            Padding    = Padding,
            Margin     = Margin,
            WordWrap   = WordWrap,
            Font       = Font,
            FontSize   = FontSize,
            LineHeight = LineHeight
        };

        PaintStyle ps = new() {
            Color              = Color,
            TextAlign          = TextAlign,
            OutlineSize        = OutlineSize,
            TextOffset         = TextOffset,
            BackgroundColor    = BackgroundColor,
            BorderColor        = BorderColor,
            BorderInset        = BorderInset,
            BorderRadius       = BorderRadius,
            BorderWidth        = BorderWidth,
            StrokeColor        = StrokeColor,
            StrokeWidth        = StrokeWidth,
            BackgroundGradient = BackgroundGradient,
            OutlineColor       = OutlineColor,
            TextShadowSize     = TextShadowSize,
            TextShadowColor    = TextShadowColor
        };

        var result = 0;

        if (ls != _prevLayoutStyle) {
            _prevLayoutStyle = ls;
            OnLayoutPropertyChanged?.Invoke();
            result = 1;
        }

        if (ps != _prevPaintStyle) {
            _prevPaintStyle = ps;
            OnPaintPropertyChanged?.Invoke();
            result += 2;
        }

        return result;
    }
}

internal record struct LayoutStyle
{
    internal Anchor   Anchor;
    internal bool     IsVisible;
    internal Size     Size;
    internal Flow     Flow;
    internal int      Gap;
    internal bool     Stretch;
    internal EdgeSize Padding;
    internal EdgeSize Margin;
    internal bool     WordWrap;
    internal string   Font;
    internal float    FontSize;
    internal float    LineHeight;
}

internal record struct PaintStyle
{
    internal Anchor         TextAlign;
    internal float          OutlineSize;
    internal Vector2        TextOffset;
    internal Color          Color;
    internal Color?         BackgroundColor;
    internal BorderColor?   BorderColor;
    internal int            BorderInset;
    internal int            BorderRadius;
    internal EdgeSize       BorderWidth;
    internal Color?         StrokeColor;
    internal int            StrokeWidth;
    internal GradientColor? BackgroundGradient;
    internal Color?         OutlineColor;
    internal float          TextShadowSize;
    internal Color?         TextShadowColor;
}
