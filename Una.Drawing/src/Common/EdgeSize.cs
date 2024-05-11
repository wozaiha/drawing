/* Una.Drawing                                                 ____ ___
 *   A declarative drawing library for FFXIV.                 |    |   \____ _____        ____                _
 *                                                            |    |   /    \\__  \      |    \ ___ ___ _ _ _|_|___ ___
 * By Una. Licensed under AGPL-3.                             |    |  |   |  \/ __ \_    |  |  |  _| .'| | | | |   | . |
 * https://github.com/una-xiv/drawing                         |______/|___|  (____  / [] |____/|_| |__,|_____|_|_|_|_  |
 * ----------------------------------------------------------------------- \/ --- \/ ----------------------------- |__*/

using System;

namespace Una.Drawing;

/// <summary>
/// Defines the size of each of the four sides of a rectangle.
/// </summary>
public class EdgeSize
{
    internal Action? OnChanged;

    /// <summary>
    /// The size of the top edge in pixels.
    /// </summary>
    public int Top {
        get => _top;
        set {
            if (_top.Equals(value)) return;
            _top = value;
            OnChanged?.Invoke();
        }
    }

    /// <summary>
    /// The size of the right edge in pixels.
    /// </summary>
    public int Right {
        get => _right;
        set {
            if (_right.Equals(value)) return;
            _right = value;
            OnChanged?.Invoke();
        }
    }

    /// <summary>
    /// The size of the bottom edge in pixels.
    /// </summary>
    public int Bottom {
        get => _bottom;
        set {
            if (_bottom.Equals(value)) return;
            _bottom = value;
            OnChanged?.Invoke();
        }
    }

    /// <summary>
    /// The size of the left edge in pixels.
    /// </summary>
    public int Left {
        get => _left;
        set {
            if (_left.Equals(value)) return;
            _left = value;
            OnChanged?.Invoke();
        }
    }

    /// <summary>
    /// Returns the combined horizontal size of the left and right edges.
    /// </summary>
    public int HorizontalSize => Left + Right;

    /// <summary>
    /// Returns the combined vertical size of the top and bottom edges.
    /// </summary>
    public int VerticalSize => Top + Bottom;

    /// <summary>
    /// Returns a new Size object with the combined horizontal and vertical
    /// sizes of the defined edges.
    /// </summary>
    public Size Size => new(HorizontalSize, VerticalSize);

    private int _top;
    private int _right;
    private int _bottom;
    private int _left;

    /// <summary>
    /// Defines a uniform perimeter size for all four edges that is zero.
    /// </summary>
    public EdgeSize() : this(0, 0, 0, 0) { }

    /// <summary>
    /// Defines a uniform perimeter size for all four edges.
    /// </summary>
    /// <param name="size">The edge size in pixels.</param>
    public EdgeSize(int size) : this(size, size, size, size) { }

    /// <summary>
    /// Defines the sizes for the vertical and horizontal edges.
    /// </summary>
    /// <param name="vertical">The size of vertical edges in pixels.</param>
    /// <param name="horizontal">The size of horizontal edges in pixels.</param>
    public EdgeSize(int vertical, int horizontal) : this(vertical, horizontal, vertical, horizontal) { }

    /// <summary>
    /// Defines the sizes for the top, right, bottom, and left edges.
    /// </summary>
    /// <param name="top">The size of the top edge in pixels.</param>
    /// <param name="right">The size of the right edge in pixels.</param>
    /// <param name="bottom">The size of the bottom edge in pixels.</param>
    /// <param name="left">The size of the left edge in pixels.</param>
    public EdgeSize(int top = 0, int right = 0, int bottom = 0, int left = 0)
    {
        Top    = top;
        Right  = right;
        Bottom = bottom;
        Left   = left;
    }

    public override string ToString()          => $"EdgeSize({Top}, {Right}, {Bottom}, {Left})";
    public override bool   Equals(object? obj) => obj is EdgeSize size && size.GetHashCode() == GetHashCode();
    public override int    GetHashCode()       => HashCode.Combine(Top, Right, Bottom, Left);

    public static bool operator ==(EdgeSize? left, EdgeSize? right) => left is not null && left.Equals(right);
    public static bool operator !=(EdgeSize? left, EdgeSize? right) => !(left == right);
}
