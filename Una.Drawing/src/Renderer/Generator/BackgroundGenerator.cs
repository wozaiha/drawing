/* Una.Drawing                                                 ____ ___
 *   A declarative drawing library for FFXIV.                 |    |   \____ _____        ____                _
 *                                                            |    |   /    \\__  \      |    \ ___ ___ _ _ _|_|___ ___
 * By Una. Licensed under AGPL-3.                             |    |  |   |  \/ __ \_    |  |  |  _| .'| | | | |   | . |
 * https://github.com/una-xiv/drawing                         |______/|___|  (____  / [] |____/|_| |__,|_____|_|_|_|_  |
 * ----------------------------------------------------------------------- \/ --- \/ ----------------------------- |__*/

using SkiaSharp;

namespace Una.Drawing.Generator;

internal class BackgroundGenerator : IGenerator
{
    /// <inheritdoc/>
    public int RenderOrder => 0;

    /// <inheritdoc/>
    public void Generate(SKCanvas canvas, Node node)
    {
        if (null == node.ComputedStyle.BackgroundColor) return;

        using var paint = new SKPaint();

        Size size = node.Bounds.PaddingSize;

        paint.Color = Color.ToSkColor(node.ComputedStyle.BackgroundColor);
        paint.Style = SKPaintStyle.Fill;

        if (node.ComputedStyle.BorderRadius == 0) {
            canvas.DrawRect(0, 0, size.Width, size.Height, paint);
            DrawStroke(canvas, size, node.ComputedStyle);
            return;
        }

        var radius = (float)node.ComputedStyle.BorderRadius;
        canvas.DrawRoundRect(0, 0, size.Width, size.Height, radius, radius, paint);
        DrawStroke(canvas, size, node.ComputedStyle);
    }

    private static void DrawStroke(SKCanvas canvas, Size size, ComputedStyle style)
    {
        if (!(style.StrokeColor?.IsVisible ?? false) || style.StrokeWidth == 0) return;

        using var paint = new SKPaint();

        float  half = (float)style.StrokeWidth / 2;
        SKRect rect = new(half, half, size.Width - half, size.Height - half);

        paint.Color       = Color.ToSkColor(style.StrokeColor);
        paint.Style       = SKPaintStyle.Stroke;
        paint.StrokeWidth = style.StrokeWidth;

        if (style.BorderRadius == 0) {
            canvas.DrawRect(rect, paint);
            return;
        }

        var radius = (float)style.BorderRadius;
        canvas.DrawRoundRect(rect, radius, radius, paint);
    }
}
