/* Una.Drawing                                                 ____ ___
 *   A declarative drawing library for FFXIV.                 |    |   \____ _____        ____                _
 *                                                            |    |   /    \\__  \      |    \ ___ ___ _ _ _|_|___ ___
 * By Una. Licensed under AGPL-3.                             |    |  |   |  \/ __ \_    |  |  |  _| .'| | | | |   | . |
 * https://github.com/una-xiv/drawing                         |______/|___|  (____  / [] |____/|_| |__,|_____|_|_|_|_  |
 * ----------------------------------------------------------------------- \/ --- \/ ----------------------------- |__*/

using System;
using SkiaSharp;
using Una.Drawing.Font;

namespace Una.Drawing.Generator;

internal class TextGenerator : IGenerator
{
    /// <inheritdoc/>
    public int RenderOrder => 999;

    /// <inheritdoc/>
    public void Generate(SKCanvas canvas, Node node)
    {
        if (node.NodeValue is not string str || string.IsNullOrWhiteSpace(str)) return;

        MeasuredText? measurement = node.NodeValueMeasurement;
        if (null == measurement || measurement.Value.LineCount == 0) return;

        Size  size       = node.NodeValueMeasurement!.Value.Size;
        IFont font       = FontRegistry.Fonts[node.ComputedStyle.Font];
        int   fontSize   = node.ComputedStyle.FontSize;
        var   lineHeight = (int)Math.Ceiling(font.GetLineHeight(fontSize));
        var   metrics    = font.GetMetrics(node.ComputedStyle.FontSize);

        var y = (int)(metrics.CapHeight + (int)node.ComputedStyle.TextOffset.Y + 1);
        var x = (int)node.ComputedStyle.TextOffset.X;

        if (node.ComputedStyle.TextAlign.IsTop) y    += node.ComputedStyle.Padding.Top;
        if (node.ComputedStyle.TextAlign.IsLeft) x   += node.ComputedStyle.Padding.Left;
        if (node.ComputedStyle.TextAlign.IsRight) x  += -node.ComputedStyle.Padding.Right;
        if (node.ComputedStyle.TextAlign.IsMiddle) y += (node.Height - size.Height) / 2 + 1;
        if (node.ComputedStyle.TextAlign.IsBottom) y =  node.Height - size.Height;

        foreach (string line in node.NodeValueMeasurement!.Value.Lines) {
            PrintLine(canvas, font, node, line, x, y, size);
            y += lineHeight;
        }
    }

    private static void PrintLine(SKCanvas canvas, IFont font, Node node, string line, float x, float y, Size size)
    {
        MeasuredText measurement = font.MeasureText(line, node.ComputedStyle.FontSize);
        float        lineWidth   = measurement.Size.Width;

        if (node.ComputedStyle.TextAlign.IsCenter) x += (node.Bounds.PaddingSize.Width - lineWidth) / 2;
        if (node.ComputedStyle.TextAlign.IsRight) x  += node.Bounds.PaddingSize.Width - lineWidth;

        using SKPaint sp = new();
        sp.IsStroke    = true;
        sp.Style       = SKPaintStyle.Stroke;
        sp.Color       = SKColors.White;
        sp.StrokeWidth = 2;

        using SKPaint paint = new();
        SKPoint       point = new(x, y);

        if (node.ComputedStyle.OutlineSize > 0) {
            paint.ImageFilter = null;
            paint.Color       = Color.ToSkColor(node.ComputedStyle.OutlineColor ?? new(0xFF000000));
            paint.Style       = SKPaintStyle.Stroke;
            paint.StrokeWidth = node.ComputedStyle.OutlineSize * 2;
            paint.MaskFilter  = SKMaskFilter.CreateBlur(SKBlurStyle.Solid, node.ComputedStyle.OutlineSize);
            font.DrawText(canvas, paint, point, node.ComputedStyle.FontSize, line);
        }

        if (node.ComputedStyle.TextShadowSize > 0) {
            paint.ImageFilter = paint.ImageFilter = SKImageFilter.CreateDropShadow(
                0,
                0,
                node.ComputedStyle.TextShadowSize * 2,
                node.ComputedStyle.TextShadowSize * 2,
                Color.ToSkColor(node.ComputedStyle.TextShadowColor ?? new(0xFF000000))
            );
        }

        paint.Color       = Color.ToSkColor(node.ComputedStyle.Color);
        paint.Style       = SKPaintStyle.Fill;
        paint.StrokeWidth = 0;
        paint.MaskFilter  = null;

        font.DrawText(canvas, paint, point, node.ComputedStyle.FontSize, line);
    }
}
