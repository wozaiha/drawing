/* Una.Drawing                                                 ____ ___
 *   A declarative drawing library for FFXIV.                 |    |   \____ _____        ____                _
 *                                                            |    |   /    \\__  \      |    \ ___ ___ _ _ _|_|___ ___
 * By Una. Licensed under AGPL-3.                             |    |  |   |  \/ __ \_    |  |  |  _| .'| | | | |   | . |
 * https://github.com/una-xiv/drawing                         |______/|___|  (____  / [] |____/|_| |__,|_____|_|_|_|_  |
 * ----------------------------------------------------------------------- \/ --- \/ ----------------------------- |__*/

using SkiaSharp;

namespace Una.Drawing.Generator;

internal class TextGenerator : IGenerator
{
    /// <inheritdoc/>
    public int RenderOrder => 999;

    /// <inheritdoc/>
    public void Generate(SKCanvas canvas, Node node)
    {
        if (string.IsNullOrWhiteSpace(node.NodeValue) || node.NodeValueLines.Count == 0) return;

        using SKPaint paint = new();
        using SKFont  font  = new();

        font.Typeface = TypefaceRegistry.Get(node.ComputedStyle.Font);
        font.Size     = node.ComputedStyle.FontSize;
        font.Hinting  = SKFontHinting.Slight;
        font.Edging   = SKFontEdging.Antialias;

        paint.IsAntialias = true;

        float y = (font.Spacing * 1.5f) + node.ComputedStyle.TextOffset.Y;
        float x = node.ComputedStyle.TextOffset.X;

        if (node.ComputedStyle.TextAlign.IsTop) y    -= 1;
        if (node.ComputedStyle.TextAlign.IsLeft) x   += node.ComputedStyle.Padding.Left;
        if (node.ComputedStyle.TextAlign.IsRight) x  += -node.ComputedStyle.Padding.Right;

        if (node.ComputedStyle.TextAlign.IsMiddle)
            y += ((node.Bounds.PaddingSize.Height - node.NodeValueSize.Y) / 2) - (font.Spacing / 1.5f) - 2;

        if (node.ComputedStyle.TextAlign.IsBottom) y = (node.Bounds.PaddingSize.Height - node.NodeValueSize.Y) + 1;

        foreach (string line in node.NodeValueLines) {
            PrintLine(canvas, paint, font, node, line, x, y);
            y += font.Spacing;
        }
    }

    private static void PrintLine(SKCanvas canvas, SKPaint paint, SKFont font, Node node, string line, float x, float y)
    {
        float lineWidth = font.MeasureText(line, paint);

        if (node.ComputedStyle.TextAlign.IsCenter) x += (node.Bounds.PaddingSize.Width - lineWidth) / 2;
        if (node.ComputedStyle.TextAlign.IsRight) x  += node.Bounds.PaddingSize.Width - lineWidth;

        SKPoint point = new(x, y);

        if (node.ComputedStyle.OutlineSize > 0) {
            paint.ImageFilter = null;
            paint.Color       = Color.ToSkColor(node.ComputedStyle.OutlineColor ?? new(0xFF000000));
            paint.Style       = SKPaintStyle.Stroke;
            paint.StrokeWidth = node.ComputedStyle.OutlineSize * 2;
            paint.MaskFilter  = SKMaskFilter.CreateBlur(SKBlurStyle.Solid, node.ComputedStyle.OutlineSize);
            canvas.DrawText(line, point, font, paint);
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

        canvas.DrawText(line, point, font, paint);
    }
}
