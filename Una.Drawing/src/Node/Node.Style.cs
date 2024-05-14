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
    public Style Style { get; set; } = new();

    /// <summary>
    /// Defines the final computed style of this node.
    /// </summary>
    internal ComputedStyle ComputedStyle { get; private set; } = new();

    /// <summary>
    /// Generates the computed style of this node and its descendants.
    /// </summary>
    private bool ComputeStyle()
    {
        ComputedStyle.Reset();

        foreach (string className in _classList) {
            Style? cStyle = Stylesheet.GetStyleForClass(className);
            if (cStyle is not null) ComputedStyle.Apply(cStyle);
        }

        foreach (string tagName in _tagsList) {
            Style? tStyle = Stylesheet.GetStyleForTag(tagName);
            if (tStyle is not null) ComputedStyle.Apply(tStyle);
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
