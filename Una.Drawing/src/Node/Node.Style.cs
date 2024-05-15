/* Una.Drawing                                                 ____ ___
 *   A declarative drawing library for FFXIV.                 |    |   \____ _____        ____                _
 *                                                            |    |   /    \\__  \      |    \ ___ ___ _ _ _|_|___ ___
 * By Una. Licensed under AGPL-3.                             |    |  |   |  \/ __ \_    |  |  |  _| .'| | | | |   | . |
 * https://github.com/una-xiv/drawing                         |______/|___|  (____  / [] |____/|_| |__,|_____|_|_|_|_  |
 * ----------------------------------------------------------------------- \/ --- \/ ----------------------------- |__*/

using System;

namespace Una.Drawing;

public partial class Node
{
    /// <summary>
    /// Signals the listener that a property that affects the layout of this
    /// node or any of its descendants been modified. This event can be
    /// triggered multiple times during a single reflow operation. It is up
    /// to the listener to keep track of the changes and perform the reflow
    /// operation as efficiently as possible.
    /// </summary>
    public Action? OnReflow;

    /// <summary>
    /// Defines the properties that specify the visual representation of this
    /// element.
    /// </summary>
    /// <remarks>
    /// Modifying this property directly will trigger a reflow. The same
    /// applies to modifications made to some of the properties of the given
    /// <see cref="Style"/> object.
    /// </remarks>
    /// <exception cref="ArgumentNullException"></exception>
    public Style Style {
        get => _style;
        set {
            _style = value ?? throw new ArgumentNullException(nameof(value));
            SignalReflowRecursive();
        }
    }

    public Stylesheet? Stylesheet {
        get => _stylesheet ?? ParentNode?.Stylesheet;
        set {
            _stylesheet = value;
            SignalReflowRecursive();
        }
    }

    /// <summary>
    /// Defines the final computed style of this node.
    /// </summary>
    internal ComputedStyle ComputedStyle { get; private set; } = new();

    private Style       _style = new();
    private Stylesheet? _stylesheet;

    /// <summary>
    /// Generates the computed style of this node and its descendants.
    /// </summary>
    private bool ComputeStyle()
    {
        ComputedStyle.Reset();

        if (Stylesheet is not null) {
            foreach ((Stylesheet.Rule rule, Style style) in Stylesheet.Rules) {
                if (rule.Matches(this)) ComputedStyle.Apply(style);
            }
        }

        ComputedStyle.Apply(Style);

        int res     = ComputedStyle.Commit();
        var updated = false;

        foreach (Node child in _childNodes) {
            updated |= child.ComputeStyle();
        }

        if (updated) {
            ReassignAnchorNodes();
        }

        return _textCachedNodeValue != _nodeValue || res is 1 or 3;
    }

    /// <summary>
    /// Invokes the reflow event, signaling that the layout of this element
    /// or any of its descendants has changed.
    /// </summary>
    private void SignalReflow()
    {
        if (_isInReflow) return;

        OnReflow?.Invoke();
        _mustReflow = true;
    }

    /// <summary>
    /// Performs a recursive reflow to all children and parent nodes.
    /// </summary>
    private void SignalReflowRecursive()
    {
        if (_isInReflow) return;

        _isInReflow = true;
        _mustReflow = true;

        foreach (Node child in _childNodes) child.SignalReflowRecursive();

        ParentNode?.SignalReflowRecursive();

        _isInReflow = false;
    }

    /// <summary>
    /// Forces a repaint of the texture for this node on the next frame.
    /// </summary>
    private void SignalRepaint()
    {
        _texture?.Dispose();
        _texture = null;
    }
}
