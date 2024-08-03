/* Una.Drawing                                                 ____ ___
 *   A declarative drawing library for FFXIV.                 |    |   \____ _____        ____                _
 *                                                            |    |   /    \\__  \      |    \ ___ ___ _ _ _|_|___ ___
 * By Una. Licensed under AGPL-3.                             |    |  |   |  \/ __ \_    |  |  |  _| .'| | | | |   | . |
 * https://github.com/una-xiv/drawing                         |______/|___|  (____  / [] |____/|_| |__,|_____|_|_|_|_  |
 * ----------------------------------------------------------------------- \/ --- \/ ----------------------------- |__*/

using System.Linq;
using Una.Drawing.Font;

namespace Una.Drawing;

public class FontRegistry : IDisposable
{
    internal event Action? FontChanged;

    private static FontRegistry? _instance;

    public static FontRegistry Instance => _instance ??=
        DalamudServices.PluginInterface.GetOrCreateData($"Una.Drawing.FontRegistry:{DrawingLib.Version}", () => new FontRegistry());

    internal Dictionary<uint, IFont> Fonts  { get; }      = [];
    internal SKTypeface              Glyphs { get; set; } = SKTypeface.Default;

    public IEnumerable<string> GetFontFamilies()
    {
        // Remove duplicates and sort alphabetically.
        List<string> families = SKFontManager.Default.FontFamilies.Distinct().ToList();
        families.Sort();

        return families;
    }

    internal void RelinquishDataShare()
    {
        DalamudServices.PluginInterface.RelinquishData("Una.Drawing.FontRegistry");
    }

    /// <summary>
    /// Creates a font from the given font family and registers it with the
    /// given ID. Existing fonts with the same ID will be disposed of.
    /// </summary>
    /// <example>
    /// Register: <code>FontRegistry.Instance.SetNativeFontFamily(1, "Arial");</code>
    /// Usage: <code>new Style() { Font = 1 }</code>
    /// </example>
    /// <param name="id"></param>
    /// <param name="fontFamily"></param>
    /// <param name="weight"></param>
    /// <param name="sizeOffset"></param>
    public void SetNativeFontFamily(
        uint              id,
        string            fontFamily,
        SKFontStyleWeight weight     = SKFontStyleWeight.Normal, // TODO: Remove me.
        float             sizeOffset = 0
    )
    {
        if (Fonts.TryGetValue(id, out IFont? existingFont)) existingFont.Dispose();

        Fonts[id] = FontFactory.CreateFromFontFamily(fontFamily, sizeOffset);
        FontChanged?.Invoke();
    }

    /// <summary>
    /// Creates a font from the given font file and registers it with the
    /// given ID. Existing fonts with the same ID will be disposed of.
    /// </summary>
    /// <example>
    /// Register: <code>FontRegistry.Instance.SetNativeFontFamily(1, "/path/to/font.otf");</code>
    /// Usage: <code>new Style() { Font = 1 }</code>
    /// </example>
    /// <param name="id"></param>
    /// <param name="fontFile"></param>
    /// <param name="sizeOffset"></param>
    /// <exception cref="FileNotFoundException"></exception>
    public void SetNativeFontFamily(uint id, FileInfo fontFile, float sizeOffset = 0)
    {
        if (!fontFile.Exists) throw new FileNotFoundException("Font file not found.", fontFile.FullName);

        if (Fonts.TryGetValue(id, out IFont? existingFont)) existingFont.Dispose();

        Fonts[id] = FontFactory.CreateFromFontFile(fontFile, sizeOffset);
        FontChanged?.Invoke();
    }

    /// <summary>
    /// Creates a font from the given font file stream and registers it with the
    /// given ID. Existing fonts with the same ID will be disposed of.
    /// </summary>
    /// <example>
    /// Register: <code>FontRegistry.Instance.SetNativeFontFamily(1, stream);</code>
    /// Usage: <code>new Style() { Font = 1 }</code>
    /// </example>
    /// <param name="id"></param>
    /// <param name="fontStream"></param>
    /// <param name="sizeOffset"></param>
    /// <exception cref="FileNotFoundException"></exception>
    public void SetNativeFontFamily(uint id, Stream fontStream, float sizeOffset = 0)
    {
        if (!fontStream.CanRead) throw new EndOfStreamException($"The stream for font #{id} is not readable.");

        if (Fonts.TryGetValue(id, out IFont? existingFont)) existingFont.Dispose();

        Fonts[id] = FontFactory.CreateFromFontStream(fontStream, sizeOffset);
        FontChanged?.Invoke();
    }

    internal void SetupGlyphFont()
    {
        Glyphs = SKTypeface.FromFile(GameGlyphProvider.GlyphsFile.FullName);
    }

    public void Dispose()
    {
        foreach (var font in Fonts.Values) font.Dispose();

        Glyphs.Dispose();
        Fonts.Clear();
    }
}