/* Una.Drawing                                                 ____ ___
 *   A declarative drawing library for FFXIV.                 |    |   \____ _____        ____                _
 *                                                            |    |   /    \\__  \      |    \ ___ ___ _ _ _|_|___ ___
 * By Una. Licensed under AGPL-3.                             |    |  |   |  \/ __ \_    |  |  |  _| .'| | | | |   | . |
 * https://github.com/una-xiv/drawing                         |______/|___|  (____  / [] |____/|_| |__,|_____|_|_|_|_  |
 * ----------------------------------------------------------------------- \/ --- \/ ----------------------------- |__*/

namespace Una.Drawing;

public partial class Style
{
    /// <summary>
    /// <para>
    /// Defines the color of the border around the node.
    /// </para>
    /// This property has no effect if <see cref="BorderWidth"/> is left at 0.
    /// </summary>
    public BorderColor? BorderColor {
        get => _borderColor;
        set {
            if (_borderColor == value) return;

            if (_borderColor is not null) _borderColor.OnChanged -= OnPaintPropertyChanged;

            _borderColor = value;

            if (_borderColor is not null) _borderColor.OnChanged += OnPaintPropertyChanged;

            OnPaintPropertyChanged?.Invoke();
        }
    }

    /// <summary>
    /// Defines the thickness of the border around the node.
    /// </summary>
    /// <remarks>
    /// This property has no effect if <see cref="BorderColor"/> is left
    /// undefined.
    /// </remarks>
    public EdgeSize? BorderWidth {
        get => _borderWidth;
        set {
            if (_borderWidth == value) return;

            _borderWidth = value;
            OnPaintPropertyChanged?.Invoke();
        }
    }

    /// <summary>
    /// Defines the roundness of the corners of the node.
    /// </summary>
    public int? BorderRadius {
        get => _borderRadius;
        set {
            if (_borderRadius == value) return;

            _borderRadius = value;
            OnPaintPropertyChanged?.Invoke();
        }
    }

    /// <summary>
    /// Defines the inset of the border around the node, allowing the border to
    /// be drawn inside the node's bounds.
    /// </summary>
    public int? BorderInset {
        get => _borderInset;
        set {
            if (_borderInset == value) return;

            _borderInset = value;
            OnPaintPropertyChanged?.Invoke();
        }
    }

    /// <summary>
    /// <para>
    /// Defines the stroke color of the node. This property has no effect
    /// if <see cref="StrokeWidth"/> is left at 0.
    /// </para>
    /// <para>
    /// Compared to <see cref="BorderColor"/>, this property is used to define
    /// a single color stroke around the background of the node, while a border
    /// allows for different colors on each edge, as well as a different inset
    /// value.
    /// </para>
    /// </summary>
    public Color? StrokeColor {
        get => _strokeColor;
        set {
            if (_strokeColor == value) return;

            _strokeColor = value;
            OnPaintPropertyChanged?.Invoke();
        }
    }

    /// <summary>
    /// <para>
    /// Defines the thickness of the stroke around the node.
    /// </para>
    /// <para>
    /// Compared to <see cref="BorderWidth"/>, this property is used to define
    /// a fixed stroke width around the background of the node, while a border
    /// border allows for different sizes on each edge, as well as a different
    /// inset value.
    /// </para>
    /// </summary>
    public int? StrokeWidth {
        get => _strokeWidth;
        set {
            if (_strokeWidth == value) return;

            _strokeWidth = value;
            OnPaintPropertyChanged?.Invoke();
        }
    }

    private BorderColor? _borderColor;
    private EdgeSize?    _borderWidth;
    private int?         _borderRadius;
    private int?         _borderInset;
    private Color?       _strokeColor;
    private int?         _strokeWidth;
}
