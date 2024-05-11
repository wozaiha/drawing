using SkiaSharp;

namespace Una.Drawing.Generator;

public class TextGenerator : IGenerator
{
    /// <inheritdoc/>
    public int RenderOrder => 999;

    /// <inheritdoc/>
    public void Generate(SKCanvas canvas, Node node)
    {
        if (string.IsNullOrWhiteSpace(node.NodeValue) || node.NodeValueLines.Count == 0) return;

        using SKPaint paint = new();
        using SKFont  font  = new();

        font.Typeface     = TypefaceRegistry.Get(node.Style.Font);
        font.Size         = node.Style.FontSize;

        paint.IsAntialias = true;

        float y = font.Spacing + node.Style.Padding.Top;
        float x = node.Style.Padding.Left;

        foreach (string line in node.NodeValueLines) {
            PrintLine(canvas, paint, font, node, line, x, y);
            y += font.Spacing * node.Style.LineHeight;
            Logger.Log($"Line: {line}");
        }
    }

    private static void PrintLine(SKCanvas canvas, SKPaint paint, SKFont font, Node node, string line, float x, float y)
    {
        if (node.Style.OutlineSize > 0) {
            paint.Style       = SKPaintStyle.Stroke;
            paint.StrokeWidth = node.Style.OutlineSize * 3;
            paint.Color       = Color.ToSkColor(node.Style.OutlineColor ?? new(0xFF000000));
            canvas.DrawText(line, new(x, y), font, paint);
        }

        paint.Color = Color.ToSkColor(node.Style.Color ?? new(0xFFFFFFFF));
        paint.Style = SKPaintStyle.Fill;

        canvas.DrawText(line, new(x, y), font, paint);
    }
}
