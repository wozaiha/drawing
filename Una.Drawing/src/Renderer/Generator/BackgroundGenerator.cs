using SkiaSharp;

namespace Una.Drawing.Generator;

internal class BackgroundGenerator : IGenerator
{
    /// <inheritdoc/>
    public int RenderOrder => 0;

    /// <inheritdoc/>
    public void Generate(SKCanvas canvas, Node node)
    {
        if (null == node.Style.BackgroundColor) return;

        using var paint = new SKPaint();

        Size size = node.Bounds.PaddingSize;

        paint.Color = Color.ToSkColor(node.Style.BackgroundColor);
        paint.Style = SKPaintStyle.Fill;

        if (node.Style.BorderRadius is null or 0) {
            canvas.DrawRect(0, 0, size.Width, size.Height, paint);
            DrawStroke(canvas, size, node.Style);
            return;
        }

        var radius = (float)node.Style.BorderRadius;
        canvas.DrawRoundRect(0, 0, size.Width, size.Height, radius, radius, paint);
        DrawStroke(canvas, size, node.Style);
    }

    private static void DrawStroke(SKCanvas canvas, Size size, Style style)
    {
        if (!(style.StrokeColor?.IsVisible ?? false) || style.StrokeWidth is not > 0) return;

        using var paint = new SKPaint();

        float  half = (float)style.StrokeWidth.Value / 2;
        SKRect rect = new(half, half, size.Width - half, size.Height - half);

        paint.Color       = Color.ToSkColor(style.StrokeColor);
        paint.Style       = SKPaintStyle.Stroke;
        paint.StrokeWidth = style.StrokeWidth.Value;

        if (style.BorderRadius is null or 0) {
            canvas.DrawRect(rect, paint);
            return;
        }

        var radius = (float)style.BorderRadius;
        canvas.DrawRoundRect(rect, radius, radius, paint);
    }
}
