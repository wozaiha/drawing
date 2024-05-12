/* Una.Drawing                                                 ____ ___
 *   A declarative drawing library for FFXIV.                 |    |   \____ _____        ____                _
 *                                                            |    |   /    \\__  \      |    \ ___ ___ _ _ _|_|___ ___
 * By Una. Licensed under AGPL-3.                             |    |  |   |  \/ __ \_    |  |  |  _| .'| | | | |   | . |
 * https://github.com/una-xiv/drawing                         |______/|___|  (____  / [] |____/|_| |__,|_____|_|_|_|_  |
 * ----------------------------------------------------------------------- \/ --- \/ ----------------------------- |__*/

using System;
using SkiaSharp;

namespace Una.Drawing.Generator;

internal class BorderGenerator : IGenerator
{
    /// <inheritdoc/>
    public int RenderOrder => 10;

    /// <inheritdoc/>
    public void Generate(SKCanvas canvas, Node node)
    {
        if (null == node.ComputedStyle.BorderColor) return;

        if (node.ComputedStyle.BorderWidth is { HorizontalSize: 0, VerticalSize: 0 })
            return;

        Size          size  = node.Bounds.PaddingSize;
        ComputedStyle style = node.ComputedStyle;

        int inset        = style.BorderInset;
        int topWidth     = style.BorderWidth.Top;
        int rightWidth   = style.BorderWidth.Right;
        int bottomWidth  = style.BorderWidth.Bottom;
        int leftWidth    = style.BorderWidth.Left;
        var insetTop     = (int)Math.Floor((float)topWidth / 2);
        var insetRight   = (int)Math.Floor((float)rightWidth / 2);
        var insetBottom  = (int)Math.Floor((float)bottomWidth / 2);
        var insetLeft    = (int)Math.Floor((float)leftWidth / 2);
        int cornerRadius = Math.Max(0, (style.BorderRadius) - (style.BorderInset));

        var rect = new SKRect(
            insetLeft + inset,
            insetTop + inset,
            size.Width - insetRight - inset,
            size.Height - insetBottom - inset
        );

        Color? topColor    = style.BorderColor.Value.Top;
        Color? rightColor  = style.BorderColor.Value.Right;
        Color? leftColor   = style.BorderColor.Value.Left;
        Color? bottomColor = style.BorderColor.Value.Bottom;

        if (topWidth > 0 && topColor is not null) {
            using SKPaint paint = new SKPaint();

            paint.IsAntialias = true;
            paint.Style       = SKPaintStyle.Stroke;
            paint.Color       = Color.ToSkColor(topColor);
            paint.StrokeWidth = topWidth;

            canvas.DrawLine(rect.Left + cornerRadius, rect.Top, rect.Right - cornerRadius, rect.Top, paint);

            canvas.DrawArc(
                new(rect.Left, rect.Top, rect.Left + 2 * cornerRadius, rect.Top + 2 * cornerRadius),
                -135,
                45,
                false,
                paint
            );

            canvas.DrawArc(
                new(rect.Right - 2 * cornerRadius, rect.Top, rect.Right, rect.Top + 2 * cornerRadius),
                -90,
                45,
                false,
                paint
            );
        }

        if (rightWidth > 0 && rightColor is not null) {
            using SKPaint paint = new SKPaint();

            paint.IsAntialias = true;
            paint.Style       = SKPaintStyle.Stroke;
            paint.Color       = Color.ToSkColor(rightColor);
            paint.StrokeWidth = rightWidth;

            canvas.DrawLine(rect.Right, rect.Top + cornerRadius, rect.Right, rect.Bottom - cornerRadius, paint);

            canvas.DrawArc(
                new(rect.Right - 2 * cornerRadius, rect.Top, rect.Right, rect.Top + 2 * cornerRadius),
                -45,
                45,
                false,
                paint
            );

            canvas.DrawArc(
                new(rect.Right - 2 * cornerRadius, rect.Bottom - 2 * cornerRadius, rect.Right, rect.Bottom),
                0,
                45,
                false,
                paint
            );
        }

        if (bottomWidth > 0 && bottomColor is not null) {
            using SKPaint paint = new SKPaint();

            paint.IsAntialias = true;
            paint.Style       = SKPaintStyle.Stroke;
            paint.Color       = Color.ToSkColor(bottomColor);
            paint.StrokeWidth = bottomWidth;

            canvas.DrawLine(rect.Left + cornerRadius, rect.Bottom, rect.Right - cornerRadius, rect.Bottom, paint);

            canvas.DrawArc(
                new(rect.Right - 2 * cornerRadius, rect.Bottom - 2 * cornerRadius, rect.Right, rect.Bottom),
                45,
                45,
                false,
                paint
            );

            canvas.DrawArc(
                new(rect.Left, rect.Bottom - 2 * cornerRadius, rect.Left + 2 * cornerRadius, rect.Bottom),
                90,
                45,
                false,
                paint
            );
        }

        if (leftWidth > 0 && leftColor is not null) {
            using SKPaint paint = new SKPaint();

            paint.IsAntialias = true;
            paint.Style       = SKPaintStyle.Stroke;
            paint.Color       = Color.ToSkColor(leftColor);
            paint.StrokeWidth = leftWidth;

            canvas.DrawLine(rect.Left, rect.Top + cornerRadius, rect.Left, rect.Bottom - cornerRadius, paint);

            canvas.DrawArc(
                new SKRect(rect.Left, rect.Top, rect.Left + 2 * cornerRadius, rect.Top + 2 * cornerRadius),
                180,
                45,
                false,
                paint
            );

            canvas.DrawArc(
                new(rect.Left, rect.Bottom - 2 * cornerRadius, rect.Left + 2 * cornerRadius, rect.Bottom),
                135,
                45,
                false,
                paint
            );
        }
    }
}
