/* Una.Drawing                                                 ____ ___
 *   A declarative drawing library for FFXIV.                 |    |   \____ _____        ____                _
 *                                                            |    |   /    \\__  \      |    \ ___ ___ _ _ _|_|___ ___
 * By Una. Licensed under AGPL-3.                             |    |  |   |  \/ __ \_    |  |  |  _| .'| | | | |   | . |
 * https://github.com/una-xiv/drawing                         |______/|___|  (____  / [] |____/|_| |__,|_____|_|_|_|_  |
 * ----------------------------------------------------------------------- \/ --- \/ ----------------------------- |__*/

using System.IO;
using SkiaSharp;

namespace Una.Drawing;

public readonly struct Typeface(string name, SKTypeface typeface)
{
    public string     Name       { get; } = name;
    public SKTypeface SkTypeface { get; } = typeface;

    public static Typeface Default { get; } = new("default", SKTypeface.FromFamilyName("Consolas"));

    public static Typeface FromFile(string name, string path) => new(name, SKTypeface.FromFile(path));

    public static Typeface FromStream(string name, Stream stream) => new(name, SKTypeface.FromStream(stream));

    public static Typeface FromBytes(string name, byte[] bytes) =>
        new(name, SKTypeface.FromData(SKData.CreateCopy(bytes)));
}
