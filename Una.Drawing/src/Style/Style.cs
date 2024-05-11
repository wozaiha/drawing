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
    /// Invoked when a property that affects the layout has changed.
    /// </summary>
    internal event Action? OnLayoutPropertyChanged;

    /// <summary>
    /// Invoked when a property that affects the graphical aspects of the node
    /// have been changed.
    /// </summary>
    internal event Action? OnPaintPropertyChanged;

    public Style()
    {
        _margin.OnChanged  += SignalLayoutPropertyChanged;
        _padding.OnChanged += SignalLayoutPropertyChanged;
        _size.OnChanged    += SignalLayoutPropertyChanged;
    }

    /// <summary>
    /// Applies properties from the specified style if they are undefined in
    /// this style.
    /// </summary>
    /// <param name="from"></param>
    internal void InheritProperties(Style from)
    {
        InheritTextProperties(from);
    }
}
