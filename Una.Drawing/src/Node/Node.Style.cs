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
            _style.OnLayoutPropertyChanged -= SignalReflowRecursive;
            _style.OnPaintPropertyChanged  -= SignalRepaint;

            _style = value ?? throw new ArgumentNullException(nameof(value));

            _style.OnLayoutPropertyChanged += SignalReflowRecursive;
            _style.OnPaintPropertyChanged  += SignalRepaint;

            OnPropertyChanged?.Invoke("Style", value);
        }
    }

    private Style _style = new();

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
