using System;

namespace Una.Drawing;

/// <summary>
/// Defines the size of an object in pixels. A size of 0 on either axis is
/// considered undefined, meaning that the object will be sized based on its
/// content and child nodes.
/// </summary>
public class Size(int width = 0, int height = 0)
{
    internal event Action? OnChanged;

    public int Width {
        get => _width;
        set {
            if (_width.Equals(value)) return;
            _width = value;
            OnChanged?.Invoke();
        }
    }

    public int Height {
        get => _height;
        set {
            if (_height.Equals(value)) return;
            _height = value;
            OnChanged?.Invoke();
        }
    }

    private int _width  = width;
    private int _height = height;

    public Size() : this(0, 0) { }
    public Size(int size) : this(size, size) { }

    /// <summary>
    /// True if both sides have an undefined size, meaning that the box should
    /// be sized both horizontally and vertically based on its content and
    /// child nodes.
    /// </summary>
    public bool IsAuto => _width == 0 && _height == 0;

    /// <summary>
    /// True if both sides have a defined size, meaning that the box has a
    /// definitive size and should not be auto-sized based on its content and
    /// child nodes.
    /// </summary>
    public bool IsFixed => _width > 0 && _height > 0;

    /// <summary>
    /// True if the width is 0, meaning that the box should only be auto-sized
    /// horizontally based on its content and child nodes.
    /// </summary>
    public bool IsAutoWidth => _width == 0;

    /// <summary>
    /// True if the height is undefined, meaning that the box should only be
    /// auto-sized vertically based on its content and child nodes.
    /// </summary>
    public bool IsAutoHeight => _height == 0;

    public override string ToString()          => $"Size({_width}, {_height})";
    public override bool   Equals(object? obj) => obj is Size size && _width == size._width && _height == size._height;
    public override int    GetHashCode()       => HashCode.Combine(Width, Height);

    public static bool operator ==(Size left, Size right) => left.Equals(right);
    public static bool operator !=(Size left, Size right) => !left.Equals(right);
    public static Size operator +(Size left, Size right) => new(left.Width + right.Width, left.Height + right.Height);
    public static Size operator -(Size left, Size right) => new(left.Width - right.Width, left.Height - right.Height);

    /// <summary>
    /// Creates a copy of this size object.
    /// </summary>
    /// <returns></returns>
    public Size Copy()
    {
        return new(Width, Height);
    }
}
