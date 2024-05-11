/* Una.Drawing                                                 ____ ___
 *   A declarative drawing library for FFXIV.                 |    |   \____ _____        ____                _
 *                                                            |    |   /    \\__  \      |    \ ___ ___ _ _ _|_|___ ___
 * By Una. Licensed under AGPL-3.                             |    |  |   |  \/ __ \_    |  |  |  _| .'| | | | |   | . |
 * https://github.com/una-xiv/drawing                         |______/|___|  (____  / [] |____/|_| |__,|_____|_|_|_|_  |
 * ----------------------------------------------------------------------- \/ --- \/ ----------------------------- |__*/

namespace Una.Drawing;

/// <summary>
/// Defines the properties that specify the presentation of an element.
/// </summary>
public partial class Style
{
    /// <summary>
    /// Determines whether this node is visible.
    /// </summary>
    public bool IsVisible {
        get => _isVisible;
        set {
            if (_isVisible == value) return;

            _isVisible = value;
            OnLayoutPropertyChanged?.Invoke();
        }
    }

    /// <summary>
    /// <para>
    /// The anchor defines the point of origin of the element.
    /// </para>
    /// </summary>
    /// <remarks>
    /// Modifying this property will trigger a reflow of the layout. This is a
    /// computationally expensive operation and should be done sparingly.
    /// </remarks>
    public Anchor Anchor {
        get => _anchor;
        set {
            if (_anchor == value) return;

            _anchor = value;
            OnLayoutPropertyChanged?.Invoke();
        }
    }

    /// <summary>
    /// Defines the flow direction of the node.
    /// </summary>
    public Flow Flow {
        get => _flow;
        set {
            if (_flow == value) return;

            _flow = value;
            OnLayoutPropertyChanged?.Invoke();
        }
    }

    /// <summary>
    /// Stretches the node to fill the remaining space in the current flow. For
    /// example, if the flow of the parent node is set to Vertical, the node
    /// will stretch horizontally to fill the parent node, and vice versa.
    /// </summary>
    public bool Stretch {
        get => _stretch;
        set {
            if (_stretch == value) return;

            _stretch = value;
            OnLayoutPropertyChanged?.Invoke();
        }
    }

    /// <summary>
    /// Defines the gap between nodes.
    /// </summary>
    public int Gap {
        get => _gap;
        set {
            if (_gap == value) return;

            _gap = value;
            OnLayoutPropertyChanged?.Invoke();
        }
    }

    private bool   _isVisible = true;
    private Anchor _anchor    = Anchor.TopLeft;
    private Flow   _flow      = Flow.Horizontal;
    private bool   _stretch;
    private int    _gap;
}
