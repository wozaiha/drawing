using System;
using SkiaSharp;

namespace Una.Drawing.Generator;

public class GradientGenerator : IGenerator
{
    public int RenderOrder => 1;

    public void Generate(SKCanvas canvas, Node node)
    {
        Style style = node.Style;
        Size  size  = node.Bounds.PaddingSize;

        if (null == style.BackgroundGradient || style.BackgroundGradient.IsEmpty) return;

        int inset = style.BackgroundGradient.Inset;

        using var paint = new SKPaint();
        SKRect    rect  = new(inset, inset, size.Width - inset, size.Height - inset);

        paint.IsAntialias = true;
        paint.Style       = SKPaintStyle.Fill;
        paint.Shader      = CreateShader(size, style.BackgroundGradient);

        if (style.BorderRadius is null or 0) {
            canvas.DrawRect(rect, paint);
            return;
        }

        int cornerRadius = Math.Max(0, (style.BorderRadius ?? 0) - (style.BorderInset ?? 0));
        canvas.DrawRoundRect(rect, cornerRadius, cornerRadius, paint);
    }

    private static SKShader CreateShader(Size size, GradientColor gradientColor)
    {
        return gradientColor.Type switch {
            GradientType.Horizontal => SKShader.CreateLinearGradient(
                new(gradientColor.Inset, gradientColor.Inset),
                new(size.Width - gradientColor.Inset, gradientColor.Inset),
                new[] { Color.ToSkColor(gradientColor.Color1), Color.ToSkColor(gradientColor.Color2) },
                null,
                SKShaderTileMode.Clamp
            ),
            GradientType.Vertical => SKShader.CreateLinearGradient(
                new(gradientColor.Inset, gradientColor.Inset),
                new(gradientColor.Inset, size.Height - gradientColor.Inset),
                new[] { Color.ToSkColor(gradientColor.Color1), Color.ToSkColor(gradientColor.Color2) },
                null,
                SKShaderTileMode.Clamp
            ),
            GradientType.Radial => SKShader.CreateRadialGradient(
                new(size.Width / 2, size.Height / 2),
                (size.Width - gradientColor.Inset) / 2,
                new[] { Color.ToSkColor(gradientColor.Color1), Color.ToSkColor(gradientColor.Color2) },
                null,
                SKShaderTileMode.Clamp
            ),
            _ => throw new InvalidOperationException(nameof(gradientColor.Type))
        };
    }
}
