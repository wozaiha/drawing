/* Una.Drawing                                                 ____ ___
 *   A declarative drawing library for FFXIV.                 |    |   \____ _____        ____                _
 *                                                            |    |   /    \\__  \      |    \ ___ ___ _ _ _|_|___ ___
 * By Una. Licensed under AGPL-3.                             |    |  |   |  \/ __ \_    |  |  |  _| .'| | | | |   | . |
 * https://github.com/una-xiv/drawing                         |______/|___|  (____  / [] |____/|_| |__,|_____|_|_|_|_  |
 * ----------------------------------------------------------------------- \/ --- \/ ----------------------------- |__*/

using System.Collections.Generic;
using System.Numerics;
using SkiaSharp;

namespace Una.Drawing;

public partial class Node
{
    private Vector2  _textCachedNodeSize = Vector2.Zero;
    private EdgeSize _textCachedPadding  = new();
    private string?  _textCachedNodeValue;
    private string?  _textCachedFontName;
    private float?   _textCachedFontSize;
    private bool?    _textCachedWordWrap;

    internal List<string> NodeValueLines { get; private set; } = [];

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
            if (NodeValueLines.Count > 0) NodeValueLines.Clear();
            return new(0, 0);
        }

        if (false == MustRecomputeNodeValue()) {
            return new((int)_textCachedNodeSize.X, (int)_textCachedNodeSize.Y);
        }

        NodeValueLines.Clear();
        _textCachedNodeValue = _nodeValue;
        _textCachedWordWrap  = _style.WordWrap;
        _textCachedPadding   = _style.Padding;
        _textCachedFontName  = _style.Font;
        _textCachedFontSize  = _style.FontSize;

        SKTypeface    tf    = TypefaceRegistry.Get(_style.Font);
        using SKFont  font  = new(tf, _style.FontSize);
        using SKPaint paint = new();

        if (_style.WordWrap == false || Style.Size.IsAutoWidth) {
            _textCachedNodeSize = new(font.MeasureText(NodeValue, paint), font.Metrics.XHeight);
            NodeValueLines      = [_nodeValue!];
            return new((int)_textCachedNodeSize.X, (int)_textCachedNodeSize.Y);
        }

        List<string> lines = [];
        List<string> words = [];

        var lineWidth = 0f;
        var maxWidth  = (float)(Style.Size.Width - Style.Padding.HorizontalSize);

        foreach (string word in NodeValue!.Split(' ')) {
            string wordWithSpace = word + " ";
            float  wordWidth     = font.MeasureText(wordWithSpace, paint);

            if (lineWidth + wordWidth > maxWidth) {
                if (words.Count > 0) {
                    lines.Add(string.Join(" ", words));
                    words.Clear();
                    lineWidth = 0f;
                }
            }

            words.Add(word);
            lineWidth += wordWidth;
        }

        if (words.Count > 0) {
            lines.Add(string.Join(" ", words));
            words.Clear();
        }

        NodeValueLines      = lines;
        _textCachedNodeSize = new(maxWidth, lines.Count * (font.Spacing * _style.LineHeight));

        return new((int)_textCachedNodeSize.X, (int)_textCachedNodeSize.Y);
    }

    /// <summary>
    /// Returns true if the computed text content and bounding box based on the
    /// node value must be recomputed.
    /// </summary>
    private bool MustRecomputeNodeValue()
    {
        // if (_textCachedFontName != Style.Font && _textCachedFontSize != Style.FontSize) {
        //     || _textCachedGlyphSize == Vector2.Zero) {
        //     _textCachedFontHandle = fontPtr.NativePtr;
        //     _textCachedGlyphSize  = ImGui.CalcTextSize("W");
        // }

        if (_nodeValue == null && _textCachedNodeSize != Vector2.Zero) {
            _textCachedNodeSize = Vector2.Zero;
            return false;
        }

        return _nodeValue != null
            && (
                _textCachedFontName != _style.Font
                || _textCachedFontSize != _style.FontSize
                || _textCachedNodeValue != _nodeValue
                || _textCachedNodeSize == Vector2.Zero
                || _textCachedWordWrap != _style.WordWrap
                || _textCachedPadding != _style.Padding
            );
    }
}
