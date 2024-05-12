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
        // TODO: Only recompute style if needed:
        //  - If the style has changed. (make it reactive again)
        //  - If the class list has changed.

        ComputedStyle.Reset();

        ComputedStyle.Apply(Style);

        int res     = ComputedStyle.Commit();
        var updated = false;

        foreach (Node child in _childNodes) {
            updated |= child.ComputeStyle();
        }

        if (updated) {
            ReassignAnchorNodes();
        }

        return res is 1 or 3;
    }

    /// <summary>
    /// Invokes the reflow event, signaling that the layout of this element
    /// or any of its descendants has changed.
    /// </summary>
    private void SignalReflow()
    {
        if (_isReflowing) return;

        OnReflow?.Invoke();
        _mustReflow = true;

        _texture?.Dispose();
        _texture = null;
    }

    private void SignalReflowRecursive()
    {
        _isReflowing = true;

        _texture?.Dispose();
        _texture    = null;
        _mustReflow = true;

        foreach (Node child in _childNodes) child.SignalReflowRecursive();

        OnReflow?.Invoke();

        _isReflowing = false;
    }

    private void SignalRepaint()
    {
        _texture?.Dispose();
        _texture = null;
    }
}
