/* Una.Drawing                                                 ____ ___
 *   A declarative drawing library for FFXIV.                 |    |   \____ _____        ____                _
 *                                                            |    |   /    \\__  \      |    \ ___ ___ _ _ _|_|___ ___
 * By Una. Licensed under AGPL-3.                             |    |  |   |  \/ __ \_    |  |  |  _| .'| | | | |   | . |
 * https://github.com/una-xiv/drawing                         |______/|___|  (____  / [] |____/|_| |__,|_____|_|_|_|_  |
 * ----------------------------------------------------------------------- \/ --- \/ ----------------------------- |__*/

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Dalamud.Interface.GameFonts;
using SkiaSharp;

namespace Una.Drawing.Texture.GameGlyph;

internal static class GameGlyphRegistry
{
    private static readonly Dictionary<int, GameGlyphTexture>         TextureCache = [];
    private static readonly Dictionary<int, GameGlyphEntry>           GlyphCache   = [];
    private static readonly Dictionary<int, FdtReader.FontTableEntry> Glyphs       = [];
    private static readonly Dictionary<int, SKImage>                  ImageCache   = [];

    private static FdtReader.FontTableHeader Header { get; set; }

    public static void Setup()
    {
        var file   = DalamudServices.DataManager.GetFile("common/font/AXIS_36.fdt");
        var reader = new FdtReader(file!.Data);

        Header = reader.FontHeader;

        foreach (var glyph in reader.Glyphs) {
            if (false == TextureCache.ContainsKey(glyph.TextureFileIndex)) {
                TextureCache[glyph.TextureFileIndex] = new(glyph.TextureFileIndex);
            }

            Glyphs[glyph.CharInt] = glyph;
        }
    }

    public static void Dispose()
    {
        foreach (var image in ImageCache.Values) {
            image.Dispose();
        }

        ImageCache.Clear();
        Glyphs.Clear();
        GlyphCache.Clear();
        TextureCache.Clear();
    }

    public static SKImage GetGlyphTexture(int c)
    {
        if (ImageCache.TryGetValue(c, out SKImage? cachedImage)) return cachedImage;

        var g = GetGlyph(c);

        SKImageInfo info = new(g.Width, g.Height, SKColorType.Rgba8888, SKAlphaType.Unpremul);

        IntPtr pixelPtr = Marshal.AllocHGlobal(g.ImageData.Length);
        Marshal.Copy(g.ImageData, 0, pixelPtr, g.ImageData.Length);

        using SKPixmap pixmap = new(info, pixelPtr);
        SKImage?       image  = SKImage.FromPixels(pixmap);

        return ImageCache[c] = image;
    }

    private static GameGlyphEntry GetGlyph(int c)
    {
        if (GlyphCache.TryGetValue(c, out GameGlyphEntry? cachedEntry)) return cachedEntry;

        FdtReader.FontTableEntry info = Glyphs.TryGetValue(c, out FdtReader.FontTableEntry glyph)
            ? glyph
            : Glyphs['?'];

        return GlyphCache[c] = new(TextureCache[info.TextureFileIndex], Header, info);
    }
}
