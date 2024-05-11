/* Una.Drawing                                                 ____ ___
 *   A declarative drawing library for FFXIV.                 |    |   \____ _____        ____                _
 *                                                            |    |   /    \\__  \      |    \ ___ ___ _ _ _|_|___ ___
 * By Una. Licensed under AGPL-3.                             |    |  |   |  \/ __ \_    |  |  |  _| .'| | | | |   | . |
 * https://github.com/una-xiv/drawing                         |______/|___|  (____  / [] |____/|_| |__,|_____|_|_|_|_  |
 * ----------------------------------------------------------------------- \/ --- \/ ----------------------------- |__*/

namespace Una.Drawing;

public partial class Node
{
    /// <summary>
    /// Defines the sizes and positions of the content, padding, border, and
    /// margin areas of this node.
    /// </summary>
    public NodeBounds Bounds { get; private set; } = new();

    #region Box Model Properties

    /// <summary>
    /// A read-only property that represents the outer width of this node,
    /// based on the total width of the content, padding and margin areas.
    /// </summary>
    public int OuterWidth => Style.IsVisible ? Bounds.MarginSize.Width : 0;

    /// <summary>
    /// A read-only property that represents the outer height of this node,
    /// based on the total height of the content, padding and margin areas.
    /// </summary>
    public int OuterHeight => Style.IsVisible ? Bounds.MarginSize.Height : 0;

    /// <summary>
    /// A read-only property that represents the inner width of this node,
    /// based on the total width of the contents of the node.
    /// </summary>
    public int InnerWidth => Style.IsVisible ? Bounds.ContentSize.Width : 0;

    /// <summary>
    /// A read-only property that represents the inner height of this node,
    /// based on the total height of the contents of the node.
    /// </summary>
    public int InnerHeight => Style.IsVisible ? Bounds.ContentSize.Height : 0;

    /// <summary>
    /// A read-only property that represents the width of this node that is
    /// made up of the content and padding areas.
    /// </summary>
    public int Width => Style.IsVisible ? Bounds.PaddingSize.Width : 0;

    /// <summary>
    /// A read-only property that represents the height of this node that is
    /// made up of the content and padding areas.
    /// </summary>
    public int Height => Style.IsVisible ? Bounds.PaddingSize.Height : 0;

    #endregion
}
