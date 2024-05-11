/* Una.Drawing                                                 ____ ___
 *   A declarative drawing library for FFXIV.                 |    |   \____ _____        ____                _
 *                                                            |    |   /    \\__  \      |    \ ___ ___ _ _ _|_|___ ___
 * By Una. Licensed under AGPL-3.                             |    |  |   |  \/ __ \_    |  |  |  _| .'| | | | |   | . |
 * https://github.com/una-xiv/drawing                         |______/|___|  (____  / [] |____/|_| |__,|_____|_|_|_|_  |
 * ----------------------------------------------------------------------- \/ --- \/ ----------------------------- |__*/

using System;

namespace Una.Drawing;

/// <summary>
/// Defines the properties that specify the presentation of an element.
/// </summary>
public partial class Style
{
    /// <summary>
    /// Margins surround the border edge of an element, providing spacing
    /// between elements within the same flow context.
    /// </summary>
    /// <remarks>
    /// Modifying this property will trigger a reflow of the layout. This is a
    /// computationally expensive operation and should be done sparingly.
    /// </remarks>
    public EdgeSize Margin {
        get => _margin;
        set {
            if (_margin.Equals(value)) return;

            _margin.OnChanged -= SignalLayoutPropertyChanged;
            _margin           =  value;
            _margin.OnChanged += SignalLayoutPropertyChanged;

            OnLayoutPropertyChanged?.Invoke();
        }
    }

    /// <summary>
    /// <para>
    /// Padding is inserted between the content edge and the padding edge of an
    /// element, providing spacing between the content and its border.
    /// </para>
    /// <para>
    /// If the padding is 0, the content will touch the border edge of the
    /// element. If the padding is negative, the content will overflow the
    /// border edge of the element, allowing the content to be rendered outside
    /// the bounding box of the element.
    /// </para>
    /// <para>
    /// If the padding is larger than the definitive size of the element that
    /// has been set by the <see cref="Style.Size"/> property, the definitive
    /// size will be floored to 0, meaning that content and children will not
    /// be rendered.
    /// </para>
    /// </summary>
    /// <remarks>
    /// Modifying this property will trigger a reflow of the layout. This is a
    /// computationally expensive operation and should be done sparingly.
    /// </remarks>
    public EdgeSize Padding {
        get => _padding;
        set {
            if (_padding.Equals(value)) return;

            _padding.OnChanged -= SignalLayoutPropertyChanged;
            _padding           =  value;
            _padding.OnChanged += SignalLayoutPropertyChanged;

            OnLayoutPropertyChanged?.Invoke();
        }
    }

    /// <summary>
    /// <para>
    /// Specifies the definitive inner dimensions of the content area of an
    /// element, including padding but not margin edges.
    /// </para>
    /// <para>
    /// The size of an element must be large enough to fit the padding edge
    /// size. If the size is too small, the padding will overflow the content
    /// size will be floored to 0, meaning that content and children will not
    /// be rendered.
    /// </para>
    /// <para>
    /// A size of 0 on either axis will let the element automatically adjust
    /// its size to fit its content and child nodes.
    /// </para>
    /// </summary>
    /// <remarks>
    /// Modifying this property will trigger a reflow of the layout. This is a
    /// computationally expensive operation and should be done sparingly.
    /// </remarks>
    public Size Size {
        get => _size;
        set {
            if (_size.Equals(value)) return;

            _size.OnChanged -= SignalLayoutPropertyChanged;
            _size           =  value;
            _size.OnChanged += SignalLayoutPropertyChanged;

            OnLayoutPropertyChanged?.Invoke();
        }
    }

    private EdgeSize _margin  = new();
    private EdgeSize _padding = new();
    private Size     _size    = new();

    private void SignalLayoutPropertyChanged()
    {
        OnLayoutPropertyChanged?.Invoke();
    }
}
