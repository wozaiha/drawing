/* Una.Drawing                                                 ____ ___
 *   A declarative drawing library for FFXIV.                 |    |   \____ _____        ____                _
 *                                                            |    |   /    \\__  \      |    \ ___ ___ _ _ _|_|___ ___
 * By Una. Licensed under AGPL-3.                             |    |  |   |  \/ __ \_    |  |  |  _| .'| | | | |   | . |
 * https://github.com/una-xiv/drawing                         |______/|___|  (____  / [] |____/|_| |__,|_____|_|_|_|_  |
 * ----------------------------------------------------------------------- \/ --- \/ ----------------------------- |__*/

using System.IO;
using SkiaSharp;

namespace Una.Drawing.Font;

internal class FontFactory
{
    internal static IFont CreateFromFontFamily(string fontFamily)
    {
        return new FontNativeImpl(SKTypeface.FromFamilyName(fontFamily));
    }

    internal static IFont CreateFromFontFile(FileInfo file)
    {
        return new FontNativeImpl(SKTypeface.FromFile(file.FullName));
    }
}
