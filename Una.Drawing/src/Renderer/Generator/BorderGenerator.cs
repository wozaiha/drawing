﻿/* Una.Drawing                                                 ____ ___
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
        Size          size  = node.Bounds.PaddingSize;
        ComputedStyle style = node.ComputedStyle;

        DrawStroke(canvas, size, style);

        if (null == style.BorderColor) return;
        if (style.BorderWidth is { HorizontalSize: 0, VerticalSize: 0 }) return;

        float inset        = style.BorderInset;
        int   topWidth     = style.BorderWidth.Top;
        int   rightWidth   = style.BorderWidth.Right;
        int   bottomWidth  = style.BorderWidth.Bottom;
        int   leftWidth    = style.BorderWidth.Left;
        float cornerRadius = Math.Max(0, (style.BorderRadius) - (style.BorderInset));

        var rect = new SKRect(
            inset + ((float)leftWidth / 2),
            inset + ((float)topWidth / 2),
            size.Width - 1 - inset - ((float)rightWidth / 2),
            size.Height - 1 - inset - ((float)bottomWidth / 2)
        );

        Color? topColor    = style.BorderColor.Value.Top;
        Color? rightColor  = style.BorderColor.Value.Right;
        Color? leftColor   = style.BorderColor.Value.Left;
        Color? bottomColor = style.BorderColor.Value.Bottom;

        using SKPaint paint = new SKPaint();
        paint.IsAntialias = node.ComputedStyle.IsAntialiased;
        paint.Style       = SKPaintStyle.Stroke;

        if (topWidth > 0 && topColor is not null) {
            paint.Color       = Color.ToSkColor(topColor);
            paint.StrokeWidth = topWidth;

            canvas.DrawLine(
                rect.Left + (style.RoundedCorners.HasFlag(RoundedCorners.TopLeft) ? cornerRadius : 0),
                rect.Top,
                rect.Right - (style.RoundedCorners.HasFlag(RoundedCorners.TopRight) ? cornerRadius : 0),
                rect.Top,
                paint
            );

            if (style.RoundedCorners.HasFlag(RoundedCorners.TopLeft)) {
                canvas.DrawArc(
                    new(rect.Left, rect.Top, rect.Left + 2 * cornerRadius, rect.Top + 2 * cornerRadius),
                    -135,
                    45,
                    false,
                    paint
                );
            }

            if (style.RoundedCorners.HasFlag(RoundedCorners.TopRight)) {
                canvas.DrawArc(
                    new(rect.Right - 2 * cornerRadius, rect.Top, rect.Right, rect.Top + 2 * cornerRadius),
                    -90,
                    45,
                    false,
                    paint
                );
            }
        }

        if (rightWidth > 0 && rightColor is not null) {
            paint.Color       = Color.ToSkColor(rightColor);
            paint.StrokeWidth = rightWidth;

            canvas.DrawLine(
                rect.Right,
                rect.Top + (style.RoundedCorners.HasFlag(RoundedCorners.TopRight) ? cornerRadius : 0),
                rect.Right,
                rect.Bottom - (style.RoundedCorners.HasFlag(RoundedCorners.BottomRight) ? cornerRadius : 0),
                paint
            );

            if (style.RoundedCorners.HasFlag(RoundedCorners.TopRight)) {
                canvas.DrawArc(
                    new(rect.Right - 2 * cornerRadius, rect.Top, rect.Right, rect.Top + 2 * cornerRadius),
                    -45,
                    45,
                    false,
                    paint
                );
            }

            if (style.RoundedCorners.HasFlag(RoundedCorners.BottomRight)) {
                canvas.DrawArc(
                    new(rect.Right - 2 * cornerRadius, rect.Bottom - 2 * cornerRadius, rect.Right, rect.Bottom),
                    0,
                    45,
                    false,
                    paint
                );
            }
        }

        if (bottomWidth > 0 && bottomColor is not null) {
            paint.Color       = Color.ToSkColor(bottomColor);
            paint.StrokeWidth = bottomWidth;

            canvas.DrawLine(
                rect.Left + (style.RoundedCorners.HasFlag(RoundedCorners.BottomLeft) ? cornerRadius : 0),
                rect.Bottom,
                rect.Right - (style.RoundedCorners.HasFlag(RoundedCorners.BottomRight) ? cornerRadius : 0),
                rect.Bottom,
                paint
            );

            if (style.RoundedCorners.HasFlag(RoundedCorners.BottomRight)) {
                canvas.DrawArc(
                    new(rect.Right - 2 * cornerRadius, rect.Bottom - 2 * cornerRadius, rect.Right, rect.Bottom),
                    45,
                    45,
                    false,
                    paint
                );
            }

            if (style.RoundedCorners.HasFlag(RoundedCorners.BottomLeft)) {
                canvas.DrawArc(
                    new(rect.Left, rect.Bottom - 2 * cornerRadius, rect.Left + 2 * cornerRadius, rect.Bottom),
                    90,
                    45,
                    false,
                    paint
                );
            }
        }

        if (leftWidth > 0 && leftColor is not null) {
            paint.Color       = Color.ToSkColor(leftColor);
            paint.StrokeWidth = leftWidth;

            canvas.DrawLine(
                rect.Left,
                rect.Top + (style.RoundedCorners.HasFlag(RoundedCorners.TopLeft) ? cornerRadius : 0),
                rect.Left,
                rect.Bottom - (style.RoundedCorners.HasFlag(RoundedCorners.BottomLeft) ? cornerRadius : 0),
                paint
            );

            if (style.RoundedCorners.HasFlag(RoundedCorners.TopLeft)) {
                canvas.DrawArc(
                    new(rect.Left, rect.Top, rect.Left + 2 * cornerRadius, rect.Top + 2 * cornerRadius),
                    180,
                    45,
                    false,
                    paint
                );
            }

            if (style.RoundedCorners.HasFlag(RoundedCorners.BottomLeft)) {
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

    private static void DrawStroke(SKCanvas canvas, Size size, ComputedStyle style)
    {
        if (!(style.StrokeColor?.IsVisible ?? false) || style.StrokeWidth == 0) return;

        using var paint = new SKPaint();

        float  inset = style.StrokeInset + (style.StrokeWidth / 2f);
        SKRect rect  = new(inset, inset, size.Width - inset, size.Height - inset);

        paint.IsAntialias = style.IsAntialiased;
        paint.Color       = Color.ToSkColor(style.StrokeColor);
        paint.Style       = SKPaintStyle.Stroke;
        paint.StrokeWidth = style.StrokeWidth;

        if (style.BorderRadius == 0) {
            canvas.DrawRect(rect, paint);
            return;
        }

        float   radius   = style.StrokeRadius ?? style.BorderRadius;
        SKPoint topLeft  = style.RoundedCorners.HasFlag(RoundedCorners.TopLeft) ? new(radius, radius) : new(0, 0);
        SKPoint topRight = style.RoundedCorners.HasFlag(RoundedCorners.TopRight) ? new(radius, radius) : new(0, 0);

        SKPoint bottomRight =
            style.RoundedCorners.HasFlag(RoundedCorners.BottomRight) ? new(radius, radius) : new(0, 0);

        SKPoint bottomLeft = style.RoundedCorners.HasFlag(RoundedCorners.BottomLeft) ? new(radius, radius) : new(0, 0);

        SKRoundRect roundRect = new SKRoundRect(rect, radius, radius);
        roundRect.SetRectRadii(rect, [topLeft, topRight, bottomRight, bottomLeft]);

        canvas.DrawRoundRect(roundRect, paint);
    }
}
