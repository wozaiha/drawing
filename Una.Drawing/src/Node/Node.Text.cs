/* Una.Drawing                                                 ____ ___
 *   A declarative drawing library for FFXIV.                 |    |   \____ _____        ____                _
 *                                                            |    |   /    \\__  \      |    \ ___ ___ _ _ _|_|___ ___
 * By Una. Licensed under AGPL-3.                             |    |  |   |  \/ __ \_    |  |  |  _| .'| | | | |   | . |
 * https://github.com/una-xiv/drawing                         |______/|___|  (____  / [] |____/|_| |__,|_____|_|_|_|_  |
 * ----------------------------------------------------------------------- \/ --- \/ ----------------------------- |__*/

using System;
using System.Collections.Generic;
using System.Numerics;
using SkiaSharp;

namespace Una.Drawing;

public partial class Node
{
    private EdgeSize _textCachedPadding = new();
    private string?  _textCachedNodeValue;
    private string?  _textCachedFontName;
    private float?   _textCachedFontSize;
    private bool?    _textCachedWordWrap;

    internal Vector2      NodeValueSize  { get; private set; } = Vector2.Zero;
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
            return new((int)NodeValueSize.X, (int)NodeValueSize.Y);
        }

        _texture = null;

        NodeValueLines.Clear();
        _textCachedNodeValue = _nodeValue;
        _textCachedWordWrap  = ComputedStyle.WordWrap;
        _textCachedPadding   = ComputedStyle.Padding;
        _textCachedFontName  = ComputedStyle.Font;
        _textCachedFontSize  = ComputedStyle.FontSize;

        SKTypeface    tf    = TypefaceRegistry.Get(ComputedStyle.Font);
        using SKFont  font  = new(tf, ComputedStyle.FontSize);
        using SKPaint paint = new();

        if (ComputedStyle.WordWrap == false || ComputedStyle.Size.IsAutoWidth) {
            NodeValueSize  = new(font.MeasureText(NodeValue, paint), font.Spacing);
            NodeValueLines = [_nodeValue!];

            return new(
                (int)NodeValueSize.X,
                (int)NodeValueSize.Y
            );
        }

        List<string> lines = [];
        List<string> words = [];

        var lineWidth = 0f;
        var maxWidth  = (float)(ComputedStyle.Size.Width - ComputedStyle.Padding.HorizontalSize);

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

        NodeValueLines = lines;

        NodeValueSize = new(
            maxWidth,
            font.Spacing + Math.Max(0, font.Spacing * (lines.Count - 1))
        );

        return new(
            (int)NodeValueSize.X,
            (int)NodeValueSize.Y
        );
    }

    /// <summary>
    /// Returns true if the computed text content and bounding box based on the
    /// node value must be recomputed.
    /// </summary>
    private bool MustRecomputeNodeValue()
    {
        // if (_textCachedFontName != ComputedStyle.Font && _textCachedFontSize != ComputedStyle.FontSize) {
        //     || _textCachedGlyphSize == Vector2.Zero) {
        //     _textCachedFontHandle = fontPtr.NativePtr;
        //     _textCachedGlyphSize  = ImGui.CalcTextSize("W");
        // }

        if (_nodeValue == null && NodeValueSize != Vector2.Zero) {
            NodeValueSize = Vector2.Zero;
            return false;
        }

        return _nodeValue != null
            && (
                _textCachedFontName != ComputedStyle.Font
                || _textCachedFontSize != ComputedStyle.FontSize
                || _textCachedNodeValue != _nodeValue
                || NodeValueSize == Vector2.Zero
                || _textCachedWordWrap != ComputedStyle.WordWrap
                || _textCachedPadding != ComputedStyle.Padding
            );
    }
}
