/* Una.Drawing                                                 ____ ___
 *   A declarative drawing library for FFXIV.                 |    |   \____ _____        ____                _
 *                                                            |    |   /    \\__  \      |    \ ___ ___ _ _ _|_|___ ___
 * By Una. Licensed under AGPL-3.                             |    |  |   |  \/ __ \_    |  |  |  _| .'| | | | |   | . |
 * https://github.com/una-xiv/drawing                         |______/|___|  (____  / [] |____/|_| |__,|_____|_|_|_|_  |
 * ----------------------------------------------------------------------- \/ --- \/ ----------------------------- |__*/

using Una.Drawing.Font;

namespace Una.Drawing;

public partial class Node
{
    private EdgeSize _textCachedPadding = new();
    private string?  _textCachedNodeValue;
    private uint?    _textCachedFontId;
    private float?   _textCachedFontSize;
    private bool?    _textCachedWordWrap;

    internal MeasuredText? NodeValueMeasurement { get; private set; }

    /// <summary>
    /// <para>
    /// Computes the <see cref="NodeBounds.ContentSize"/> of this node based on
    /// the text content of the node and returns a <see cref="Size"/> object
    /// containing the width and height of the content.
    /// </para>
    /// <para>
    /// If the node value is empty, the returned size is zero.
    /// </para>
    /// <para>
    /// This function returns cached values if any style properties that affect
    /// the text content have not changed since the last computation.
    /// </para>
    /// </summary>
    private Size ComputeContentSizeFromText()
    {
        if (string.IsNullOrEmpty(_nodeValue)) {
            return new(0, 0);
        }

        if (false == MustRecomputeNodeValue()) {
            return NodeValueMeasurement?.Size ?? new();
        }

        _texture = null;

        _textCachedNodeValue = _nodeValue;
        _textCachedWordWrap  = ComputedStyle.WordWrap;
        _textCachedPadding   = ComputedStyle.Padding;
        _textCachedFontId    = ComputedStyle.Font;
        _textCachedFontSize  = ComputedStyle.FontSize;

        var font = FontRegistry.Typefaces[ComputedStyle.Font];

        NodeValueMeasurement = font.MeasureText(
            _nodeValue,
            ComputedStyle.FontSize,
            ComputedStyle.Size.Width,
            ComputedStyle.WordWrap
        );

        return NodeValueMeasurement.Value.Size;
    }

    /// <summary>
    /// Returns true if the computed text content and bounding box based on the
    /// node value must be recomputed.
    /// </summary>
    private bool MustRecomputeNodeValue()
    {
        if (_nodeValue == null && !(NodeValueMeasurement?.Size.IsZero ?? false)) {
            NodeValueMeasurement = new();
            return false;
        }

        return _nodeValue != null
            && (
                _textCachedFontId != ComputedStyle.Font
                || _textCachedFontSize != ComputedStyle.FontSize
                || _textCachedNodeValue != _nodeValue
                || _textCachedWordWrap != ComputedStyle.WordWrap
                || _textCachedPadding != ComputedStyle.Padding
            );
    }
}
