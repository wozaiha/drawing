/* Una.Drawing                                                 ____ ___
 *   A declarative drawing library for FFXIV.                 |    |   \____ _____        ____                _
 *                                                            |    |   /    \\__  \      |    \ ___ ___ _ _ _|_|___ ___
 * By Una. Licensed under AGPL-3.                             |    |  |   |  \/ __ \_    |  |  |  _| .'| | | | |   | . |
 * https://github.com/una-xiv/drawing                         |______/|___|  (____  / [] |____/|_| |__,|_____|_|_|_|_  |
 * ----------------------------------------------------------------------- \/ --- \/ ----------------------------- |__*/

using SkiaSharp;

namespace Una.Drawing;

public class Color(byte r, byte g, byte b, float a = 1.0f)
{
    public string Name { get; set; } = string.Empty;

    public byte  R { get; set; } = r;
    public byte  G { get; set; } = g;
    public byte  B { get; set; } = b;
    public float A { get; set; } = a;

    /// <summary>
    /// Returns a UInt32 representation of the color in ABGR format.
    /// </summary>
    /// <returns></returns>
    public uint ToUInt() => (uint)((byte)(A * 255) << 24 | B << 16 | G << 8 | R);

    public Color(byte r, byte g, byte b, byte a) : this(r, g, b, a * 255) { }

    public Color(uint color) : this(
        (byte)(color & 0xFF),
        (byte)((color >> 8) & 0xFF),
        (byte)((color >> 16) & 0xFF),
        (byte)((color >> 24) & 0xFF)
    ) { }

    public Color(string name) : this(0, 0, 0, 0)
    {
        Name = name;
    }

    /// <summary>
    /// Returns true if this color is visible.
    /// </summary>
    public bool IsVisible => A > 0;

    public override string ToString()          => $"Color(#{ToUInt():x8})";
    public override bool   Equals(object? obj) => obj is Color color && color.ToUInt() == ToUInt();
    public override int    GetHashCode()       => ToUInt().GetHashCode();

    public static bool operator ==(Color? left, Color? right) => left is not null && left.Equals(right);
    public static bool operator !=(Color? left, Color? right) => !(left == right);

    internal static SKColor ToSkColor(Color? color)
    {
        return color is null
            ? SKColor.Empty
            : new(color.R, color.G, color.B, (byte)(color.A * 255));
    }
}
