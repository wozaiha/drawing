/* Una.Drawing                                                 ____ ___
 *   A declarative drawing library for FFXIV.                 |    |   \____ _____        ____                _
 *                                                            |    |   /    \\__  \      |    \ ___ ___ _ _ _|_|___ ___
 * By Una. Licensed under AGPL-3.                             |    |  |   |  \/ __ \_    |  |  |  _| .'| | | | |   | . |
 * https://github.com/una-xiv/drawing                         |______/|___|  (____  / [] |____/|_| |__,|_____|_|_|_|_  |
 * ----------------------------------------------------------------------- \/ --- \/ ----------------------------- |__*/

using System;
using System.Collections.Generic;
using SkiaSharp;

namespace Una.Drawing.Font;

internal sealed class FontNativeImpl : IFont
{
    private readonly SKTypeface              _typeface;
    private readonly Dictionary<int, SKFont> _fontCache = [];

    internal FontNativeImpl(SKTypeface typeface)
    {
        _typeface = typeface;
    }

    /// <inheritdoc/>
    public float GetLineHeight(int fontSize) => (96 / 72) * GetFont(fontSize).Size;

    /// <inheritdoc/>
    public MeasuredText MeasureText(string text, int fontSize = 14, float? maxLineWidth = null, bool wordWrap = false)
    {
        var maxWidth   = 0;
        var maxHeight  = 0;
        var font       = GetFont(fontSize);
        var lineHeight = (int)Math.Ceiling(GetLineHeight(fontSize));

        if (maxLineWidth is null or 0) {
            maxWidth  = (int)Math.Ceiling(font.MeasureText(text));

            return new() {
                Size      = new(maxWidth, lineHeight),
                Lines     = [text],
                LineCount = 1,
            };
        }

        List<string> lines           = [];
        var          line            = string.Empty;
        float        totalLineLength = 0;

        foreach (string word in text.Split(' ')) {
            string wordWithSpace = word + ' ';
            float  wordLength    = font.MeasureText(line + wordWithSpace);

            if (totalLineLength + wordLength > maxLineWidth) {
                maxWidth        =  (int)Math.Ceiling(Math.Max(maxWidth, totalLineLength + wordLength));
                maxHeight       += lineHeight;
                totalLineLength =  0;

                if (wordWrap == false) {
                    lines.Add(line + "…");
                    line = "";
                    break;
                }

                lines.Add(line);
                line = wordWithSpace;
            } else {
                line            += word + ' ';
                totalLineLength += wordLength;
            }
        }

        if (line.Length > 0) {
            lines.Add(line);
            maxHeight += lineHeight;
        }

        return new() {
            Size      = new(maxWidth, maxHeight),
            Lines     = lines.ToArray(),
            LineCount = (uint)lines.Count,
        };
    }

    /// <inheritdoc/>
    public void DrawText(SKCanvas canvas, SKPaint paint, SKPoint pos, int fontSize, string text)
    {
        canvas.DrawText(text, pos.X, pos.Y, GetFont(fontSize), paint);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        foreach (SKFont font in _fontCache.Values) {
            font.Dispose();
        }

        _fontCache.Clear();
    }

    public SKFontMetrics GetMetrics(int fontSize) => GetFont(fontSize).Metrics;

    private SKFont GetFont(int fontSize)
    {
        if (_fontCache.TryGetValue(fontSize, out SKFont? cachedFont)) return cachedFont;

        var font = new SKFont(_typeface, (int)((float)fontSize));
        font.Hinting  = SKFontHinting.Full;
        font.Edging   = SKFontEdging.SubpixelAntialias;
        font.Subpixel = true;

        _fontCache[fontSize] = font;

        return font;
    }
}
