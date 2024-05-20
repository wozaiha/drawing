/* Una.Drawing                                                 ____ ___
 *   A declarative drawing library for FFXIV.                 |    |   \____ _____        ____                _
 *                                                            |    |   /    \\__  \      |    \ ___ ___ _ _ _|_|___ ___
 * By Una. Licensed under AGPL-3.                             |    |  |   |  \/ __ \_    |  |  |  _| .'| | | | |   | . |
 * https://github.com/una-xiv/drawing                         |______/|___|  (____  / [] |____/|_| |__,|_____|_|_|_|_  |
 * ----------------------------------------------------------------------- \/ --- \/ ----------------------------- |__*/

using Dalamud.Game.Text;
using SkiaSharp;
using Una.Drawing.Texture.GameGlyph;

namespace Una.Drawing.Generator;

public class GlyphGenerator : IGenerator
{
    /// <inheritdoc/>
    public int RenderOrder { get; } = 4;

    /// <inheritdoc/>
    public void Generate(SKCanvas canvas, Node node)
    {
        if (node.ComputedStyle.Glyph is null) return;

        SKImage glyph = GameGlyphRegistry.GetGlyphTexture(node.ComputedStyle.Glyph.Value.ToIconChar());
        SKRect  rect  = new(0, 0, node.Width, node.Height);

        if (rect.IsEmpty) return;

        SKMatrix matrix = SKMatrix.CreateScale(
            rect.Width / glyph.Width,
            rect.Height / glyph.Height
        );

        matrix.TransX = node.ComputedStyle.GlyphOffset.X;
        matrix.TransY = node.ComputedStyle.GlyphOffset.Y;

        using SKPaint  paint  = new();
        using SKShader shader = SKShader.CreateImage(glyph, SKShaderTileMode.Clamp, SKShaderTileMode.Clamp, matrix);

        paint.Shader      = shader;
        paint.IsAntialias = node.ComputedStyle.IsAntialiased;

        var color = node.ComputedStyle.GlyphColor;

        if (color.ToUInt() != 0xFFFFFFFF) {
            paint.ColorFilter = CreateColorFilter(color);
        }

        canvas.DrawRect(rect, paint);
    }

    private static SKColorFilter CreateColorFilter(Color color)
    {
        var r = new byte[256];
        var g = new byte[256];
        var b = new byte[256];

        for (var i = 0; i < 256; i++)
        {
            r[i] = (byte)(color.R * i / 255);
            g[i] = (byte)(color.G * i / 255);
            b[i] = (byte)(color.B * i / 255);
        }

        return SKColorFilter.CreateTable(null, r, g, b);
    }
}
