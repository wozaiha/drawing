using System.IO;
using SkiaSharp;

namespace Una.Drawing;

public readonly struct Typeface(string name, SKTypeface typeface)
{
    public string     Name       { get; } = name;
    public SKTypeface SkTypeface { get; } = typeface;

    public static Typeface Default { get; } = new("monospace", SKTypeface.FromFamilyName("Arial"));

    public static Typeface FromFile(string name, string path) => new(name, SKTypeface.FromFile(path));

    public static Typeface FromStream(string name, Stream stream) => new(name, SKTypeface.FromStream(stream));

    public static Typeface FromBytes(string name, byte[] bytes) =>
        new(name, SKTypeface.FromData(SKData.CreateCopy(bytes)));
}
