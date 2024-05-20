/* Una.Drawing                                                 ____ ___
 *   A declarative drawing library for FFXIV.                 |    |   \____ _____        ____                _
 *                                                            |    |   /    \\__  \      |    \ ___ ___ _ _ _|_|___ ___
 * By Una. Licensed under AGPL-3.                             |    |  |   |  \/ __ \_    |  |  |  _| .'| | | | |   | . |
 * https://github.com/una-xiv/drawing                         |______/|___|  (____  / [] |____/|_| |__,|_____|_|_|_|_  |
 * ----------------------------------------------------------------------- \/ --- \/ ----------------------------- |__*/

using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using SkiaSharp;
using Una.Drawing.Font;
using Una.Drawing.Texture;

namespace Una.Drawing.Generator;

public class SeStringGenerator : IGenerator
{
    /// <inheritdoc/>
    public int RenderOrder => 999;

    /// <inheritdoc/>
    public void Generate(SKCanvas canvas, Node node)
    {
        if (node.NodeValue is not SeString seString || seString.Payloads.Count == 0) return;

        Size  size       = node.NodeValueMeasurement!.Value.Size;
        IFont font       = FontRegistry.Fonts[node.ComputedStyle.Font];
        var   metrics    = font.GetMetrics(node.ComputedStyle.FontSize);
        int   spaceWidth = font.MeasureText(" ", node.ComputedStyle.FontSize).Size.Width;

        var y = (int)(metrics.CapHeight + node.ComputedStyle.TextOffset.Y);
        var x = (int)node.ComputedStyle.TextOffset.X;

        if (node.ComputedStyle.TextAlign.IsTop) y    += node.ComputedStyle.Padding.Top;
        if (node.ComputedStyle.TextAlign.IsLeft) x   += node.ComputedStyle.Padding.Left;
        if (node.ComputedStyle.TextAlign.IsRight) x  += -node.ComputedStyle.Padding.Right;
        if (node.ComputedStyle.TextAlign.IsMiddle) y += (node.Height - size.Height) / 2;
        if (node.ComputedStyle.TextAlign.IsBottom) y =  node.Height - size.Height;
        if (node.ComputedStyle.TextAlign.IsCenter) x += (node.Width - size.Width) / 2;

        SKColor prevColor = Color.ToSkColor(node.ComputedStyle.Color);
        SKColor color     = prevColor;

        foreach (var payload in seString.Payloads) {
            switch (payload) {
                case UIForegroundPayload fg:
                    uint rgb = fg.RGB;

                    if (rgb == 0) {
                        color = prevColor;
                    } else {
                        prevColor = color;
                        color     = Color.ToSkColor(new(RgbaToAbgr(fg.UIColor.UIForeground)));
                    }

                    continue;
                case TextPayload text:
                    x += DrawText(canvas, new(x, y + node.ComputedStyle.TextOffset.Y), color, node, text.Text ?? "");
                    continue;
                case IconPayload icon: {
                    x += spaceWidth;

                    GfdIcon gfdIcon = GfdIconRepository.GetIcon(icon.Icon);

                    using SKPaint gfdPaint = new();

                    canvas.DrawImage(
                        gfdIcon.Texture,
                        new(gfdIcon.Uv0.X, gfdIcon.Uv0.Y, gfdIcon.Uv1.X, gfdIcon.Uv1.Y),
                        new SKRect(
                            x - 4,
                            y - (metrics.CapHeight) - 6,
                            x + 22,
                            y - (metrics.CapHeight) + 19
                        ),
                        gfdPaint
                    );

                    x += 20 + spaceWidth;
                    continue;
                }
            }
        }
    }

    private static int DrawText(SKCanvas canvas, SKPoint point, SKColor color, Node node, string text)
    {
        if (string.IsNullOrEmpty(text)) return 0;

        IFont font = FontRegistry.Fonts[node.ComputedStyle.Font];

        MeasuredText measurements = font.MeasureText(text, node.ComputedStyle.FontSize);

        if (measurements.Size.Width == 0) return 0;

        using SKPaint paint = new();

        if (node.ComputedStyle.OutlineSize > 0) {
            paint.Color       = Color.ToSkColor(node.ComputedStyle.OutlineColor ?? new(0xFF000000));
            paint.Style       = SKPaintStyle.Stroke;
            paint.StrokeWidth = node.ComputedStyle.OutlineSize * 2;
            paint.MaskFilter  = SKMaskFilter.CreateBlur(SKBlurStyle.Solid, node.ComputedStyle.OutlineSize);
            font.DrawText(canvas, paint, point, node.ComputedStyle.FontSize, text);
        }

        if (node.ComputedStyle.TextShadowSize > 0) {
            paint.ImageFilter = paint.ImageFilter = SKImageFilter.CreateDropShadow(
                0,
                0,
                node.ComputedStyle.TextShadowSize * 2,
                node.ComputedStyle.TextShadowSize * 2,
                Color.ToSkColor(node.ComputedStyle.TextShadowColor ?? new(0xFF000000))
            );
        } else {
            paint.ImageFilter = null;
        }

        paint.MaskFilter  = null;
        paint.StrokeWidth = 0;
        paint.Style       = SKPaintStyle.Fill;
        paint.Color       = color;
        font.DrawText(canvas, paint, point, node.ComputedStyle.FontSize, text);

        return measurements.Size.Width;
    }

    internal static uint RgbaToAbgr(uint rgba)
    {
        uint tmp = ((rgba << 8) & 0xFF00FF00) | ((rgba >> 8) & 0xFF00FF);
        return (tmp << 16) | (tmp >> 16);
    }
}
