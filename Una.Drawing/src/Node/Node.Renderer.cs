using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Dalamud.Interface.Internal;
using ImGuiNET;

namespace Una.Drawing;

public partial class Node
{
    private IDalamudTextureWrap? _texture;
    private NodeSnapshot         _snapshot;

    public void Render(ImDrawListPtr drawList, Point position)
    {
        if (ParentNode is not null)
            throw new InvalidOperationException("Cannot render a node that has a parent or is not a root node.");

        ComputeStyle();
        Reflow(position);
        Draw(drawList);
    }

    /// <summary>
    /// Returns true if this node is visible and its bounds are not empty.
    /// </summary>
    public bool IsVisible =>  ComputedStyle.IsVisible && !Bounds.PaddingSize.IsZero;

    private void Draw(ImDrawListPtr drawList)
    {
        if (!IsVisible) {
            _isVisibleSince = 0;
            return;
        }
        if (_isVisibleSince == 0) _isVisibleSince = DateTimeOffset.Now.ToUnixTimeMilliseconds();

        NodeSnapshot snapshot = CreateSnapshot();

        if (_texture is null || NodeSnapshot.AreEqual(ref snapshot, ref _snapshot) is false) {
            _texture  = Renderer.CreateTexture(this);
            _snapshot = snapshot;
        }

        SetupInteractive(drawList);

        if (null != _texture) {
            drawList.AddImage(
                _texture.ImGuiHandle,
                Bounds.PaddingRect.TopLeft, Bounds.PaddingRect.BottomRight,
                Vector2.Zero,
                Vector2.One,
                GetRenderColor()
            );
        }

        DrawDebugBounds();

        foreach (var childNode in ChildNodes) {
            childNode.Draw(drawList);
        }

        EndInteractive();
    }

    private NodeSnapshot CreateSnapshot()
    {
        return new() {
            Width       = OuterWidth,
            Height      = OuterHeight,
            // ValueWidth  = NodeValueMeasurement.Size.Width,
            // ValueHeight = NodeValueMeasurement.Size.Height,
            LayoutStyle = ComputedStyle.CommittedLayoutStyle,
            PaintStyle  = ComputedStyle.CommittedPaintStyle,
        };
    }

    private uint GetRenderColor()
    {
        float opacity = ComputedStyle.Opacity;

        Node? parent = ParentNode;

        while (parent is not null && opacity > 0.0f) {
            opacity *= parent.ComputedStyle.Opacity;
            parent = parent.ParentNode;
        }

        opacity = Math.Clamp(opacity, 0.0f, 1.0f);

        return (uint)(opacity * 255) << 24 | 0x00FFFFFF;
    }
}

[StructLayout(LayoutKind.Sequential)]
internal struct NodeSnapshot
{
    internal int         Width;
    internal int         Height;
    internal int         ValueWidth;
    internal int         ValueHeight;
    internal LayoutStyle LayoutStyle;
    internal PaintStyle  PaintStyle;

    internal static bool AreEqual<T>(ref readonly T a, ref readonly T b) where T : unmanaged
    {
        return MemoryMarshal
            .AsBytes(new ReadOnlySpan<T>(in a))
            .SequenceEqual(MemoryMarshal.AsBytes(new ReadOnlySpan<T>(in b)));
    }
}
