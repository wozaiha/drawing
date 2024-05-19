/* Una.Drawing                                                 ____ ___
 *   A declarative drawing library for FFXIV.                 |    |   \____ _____        ____                _
 *                                                            |    |   /    \\__  \      |    \ ___ ___ _ _ _|_|___ ___
 * By Una. Licensed under AGPL-3.                             |    |  |   |  \/ __ \_    |  |  |  _| .'| | | | |   | . |
 * https://github.com/una-xiv/drawing                         |______/|___|  (____  / [] |____/|_| |__,|_____|_|_|_|_  |
 * ----------------------------------------------------------------------- \/ --- \/ ----------------------------- |__*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using SkiaSharp;
using Una.Drawing.Font;

namespace Una.Drawing;

public static class FontRegistry
{
    internal static event Action? FontChanged;

    internal static readonly Dictionary<uint, IFont> Fonts = [];

    public static List<string> GetFontFamilies()
    {
        List<string> result = [];

        InstalledFontCollection installedFontCollection = new();
        FontFamily[]            fontFamilies            = installedFontCollection.Families;

        result.AddRange(fontFamilies.Select(fontFamily => fontFamily.Name));

        return result;
    }

    /// <summary>
    /// Creates a font from the given font family and registers it with the
    /// given ID. Existing fonts with the same ID will be disposed of.
    /// </summary>
    /// <example>
    /// Register: <code>FontRegistry.SetNativeFontFamily(1, "Arial");</code>
    /// Usage: <code>new Style() { Font = 1 }</code>
    /// </example>
    /// <param name="id"></param>
    /// <param name="fontFamily"></param>
    /// <param name="weight"></param>
    public static void SetNativeFontFamily(
        uint id, string fontFamily, SKFontStyleWeight weight = SKFontStyleWeight.Normal
    )
    {
        if (Fonts.TryGetValue(id, out IFont? existingFont)) existingFont.Dispose();

        Fonts[id] = FontFactory.CreateFromFontFamily(fontFamily, weight);
        FontChanged?.Invoke();
    }

    /// <summary>
    /// Creates a font from the given font file and registers it with the
    /// given ID. Existing fonts with the same ID will be disposed of.
    /// </summary>
    /// <example>
    /// Register: <code>FontRegistry.SetNativeFontFamily(1, "/path/to/font.otf");</code>
    /// Usage: <code>new Style() { Font = 1 }</code>
    /// </example>
    /// <param name="id"></param>
    /// <param name="fontFile"></param>
    /// <exception cref="FileNotFoundException"></exception>
    public static void SetNativeFontFamily(uint id, FileInfo fontFile)
    {
        if (!fontFile.Exists) throw new FileNotFoundException("Font file not found.", fontFile.FullName);

        if (Fonts.TryGetValue(id, out IFont? existingFont)) existingFont.Dispose();

        Fonts[id] = FontFactory.CreateFromFontFile(fontFile);
    }

    internal static void Dispose()
    {
        foreach (var font in Fonts.Values) font.Dispose();
        Fonts.Clear();
    }
}
