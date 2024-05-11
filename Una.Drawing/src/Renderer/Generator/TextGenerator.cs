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

        font.Typeface = TypefaceRegistry.Get(node.Style.Font);
        font.Size     = node.Style.FontSize;
        font.Hinting  = SKFontHinting.Slight;
        font.Edging   = SKFontEdging.SubpixelAntialias;

        paint.IsAntialias = true;

        float y = font.Spacing + node.Style.TextOffset.Y - 1;
        float x = node.Style.TextOffset.X;

        if (node.Style.TextAlign.IsLeft) x   += node.Style.Padding.Left;
        if (node.Style.TextAlign.IsRight) x  += -node.Style.Padding.Right;
        if (node.Style.TextAlign.IsTop) y    += node.Style.Padding.Top;
        if (node.Style.TextAlign.IsMiddle) y += (node.Bounds.ContentSize.Height - node.NodeValueSize.Y) / 2;

        if (node.Style.TextAlign.IsBottom)
            y += (node.Bounds.ContentSize.Height - node.NodeValueSize.Y) - node.Style.Padding.Bottom;

        foreach (string line in node.NodeValueLines) {
            PrintLine(canvas, paint, font, node, line, x, y);
            y += font.Spacing * node.Style.LineHeight;
        }
    }

    private static void PrintLine(SKCanvas canvas, SKPaint paint, SKFont font, Node node, string line, float x, float y)
    {
        float lineWidth = font.MeasureText(line, paint);

        if (node.Style.TextAlign.IsCenter) x += (node.Bounds.ContentSize.Width - lineWidth) / 2;
        if (node.Style.TextAlign.IsRight) x  += node.Bounds.ContentSize.Width - lineWidth;

        SKPoint point = new(x, y);

        if (node.Style.OutlineSize > 0) {
            paint.Color       = Color.ToSkColor(node.Style.OutlineColor ?? new(0xFF000000));
            paint.Style       = SKPaintStyle.Stroke;
            paint.StrokeWidth = node.Style.OutlineSize * 2;
            paint.MaskFilter  = SKMaskFilter.CreateBlur(SKBlurStyle.Solid, node.Style.OutlineSize);
            canvas.DrawText(line, point, font, paint);
        }

        // Shadow
        // paint.ImageFilter = paint.ImageFilter = SKImageFilter.CreateDropShadow(
        // 0,
        // 0,
        // node.Style.OutlineSize * 2,
        // node.Style.OutlineSize * 2,
        // Color.ToSkColor(node.Style.OutlineColor ?? new(0xFF000000))
        //     );

        paint.Color       = Color.ToSkColor(node.Style.Color ?? new(0xFFFFFFFF));
        paint.Style       = SKPaintStyle.Fill;
        paint.StrokeWidth = 0;
        paint.MaskFilter  = null;

        canvas.DrawText(line, point, font, paint);
    }
}
