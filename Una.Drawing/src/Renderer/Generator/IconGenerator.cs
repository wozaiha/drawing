/* Una.Drawing                                                 ____ ___
 *   A declarative drawing library for FFXIV.                 |    |   \____ _____        ____                _
 *                                                            |    |   /    \\__  \      |    \ ___ ___ _ _ _|_|___ ___
 * By Una. Licensed under AGPL-3.                             |    |  |   |  \/ __ \_    |  |  |  _| .'| | | | |   | . |
 * https://github.com/una-xiv/drawing                         |______/|___|  (____  / [] |____/|_| |__,|_____|_|_|_|_  |
 * ----------------------------------------------------------------------- \/ --- \/ ----------------------------- |__*/

using SkiaSharp;
using Una.Drawing.Texture;

namespace Una.Drawing.Generator;

internal class IconGenerator : IGenerator
{
    /// <inheritdoc/>
    public int RenderOrder => 3;

    /// <inheritdoc/>
    public void Generate(SKCanvas canvas, Node node)
    {
        if (node.ComputedStyle.IconId is null) return;

        SKImage? image = TextureLoader.LoadIcon(node.ComputedStyle.IconId.Value);
        if (image == null) return;

        SKRect rect = new(
            node.ComputedStyle.IconInset?.Left ?? 0,
            node.ComputedStyle.IconInset?.Top ?? 0,
            node.Bounds.PaddingSize.Width - (node.ComputedStyle.IconInset?.Right ?? 0),
            node.Bounds.PaddingSize.Height - (node.ComputedStyle.IconInset?.Bottom ?? 0)
        );

        if (rect.IsEmpty) return;

        SKMatrix matrix = SKMatrix.CreateScale(
            rect.Width / image.Width,
            rect.Height / image.Height
        );

        matrix.TransX = (node.ComputedStyle.IconOffset?.X ?? 0) + (node.ComputedStyle.IconInset?.Left ?? 0);
        matrix.TransY = (node.ComputedStyle.IconOffset?.Y ?? 0) + (node.ComputedStyle.IconInset?.Top ?? 0);

        using SKPaint  paint  = new();
        using SKShader shader = SKShader.CreateImage(image, SKShaderTileMode.Clamp, SKShaderTileMode.Clamp, matrix);

        paint.Shader      = shader;
        paint.IsAntialias = node.ComputedStyle.IsAntialiased;
        paint.Style       = SKPaintStyle.Fill;

        paint.Shader = SKShader.CreateColorFilter(
            SKShader.CreateImage(image, SKShaderTileMode.Clamp, SKShaderTileMode.Clamp, matrix),
            SKColorFilter.CreateHighContrast(
                new() {
                    Grayscale = node.ComputedStyle.IconGrayscale,
                    Contrast  = node.ComputedStyle.IconContrast
                }
            )
        );

        float radius = node.ComputedStyle.IconRounding;

        if (radius < 0.01f) {
            canvas.DrawRect(rect, paint);
            return;
        }

        var style = node.ComputedStyle;

        RoundedCorners corners     = style.IconRoundedCorners;
        SKPoint        topLeft     = corners.HasFlag(RoundedCorners.TopLeft) ? new(radius, radius) : new(0, 0);
        SKPoint        topRight    = corners.HasFlag(RoundedCorners.TopRight) ? new(radius, radius) : new(0, 0);
        SKPoint        bottomRight = corners.HasFlag(RoundedCorners.BottomRight) ? new(radius, radius) : new(0, 0);
        SKPoint        bottomLeft  = corners.HasFlag(RoundedCorners.BottomLeft) ? new(radius, radius) : new(0, 0);

        using SKRoundRect roundRect = new SKRoundRect(rect, radius, radius);

        roundRect.SetRectRadii(rect, [topLeft, topRight, bottomRight, bottomLeft]);
        canvas.DrawRoundRect(roundRect, paint);
    }
}
