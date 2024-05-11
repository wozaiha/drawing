using System;

namespace Una.Drawing;

public class BorderColor(Color? top = null, Color? right = null, Color? bottom = null, Color? left = null)
{
    internal Action? OnChanged;

    /// <summary>
    /// The size of the top edge in pixels.
    /// </summary>
    public Color? Top {
        get => top;
        set {
            if (top == value) return;
            top = value;
            OnChanged?.Invoke();
        }
    }

    /// <summary>
    /// The size of the right edge in pixels.
    /// </summary>
    public Color? Right {
        get => right;
        set {
            if (right == value) return;
            right = value;
            OnChanged?.Invoke();
        }
    }

    /// <summary>
    /// The size of the bottom edge in pixels.
    /// </summary>
    public Color? Bottom {
        get => bottom;
        set {
            if (bottom == value) return;
            bottom = value;
            OnChanged?.Invoke();
        }
    }

    /// <summary>
    /// The size of the left edge in pixels.
    /// </summary>
    public Color? Left {
        get => left;
        set {
            if (left == value) return;
            left = value;
            OnChanged?.Invoke();
        }
    }

    public BorderColor(Color? all) : this(all, all, all, all) { }

    public BorderColor(Color? topRight, Color? bottomLeft) : this(topRight, topRight, bottomLeft, bottomLeft) { }
}
