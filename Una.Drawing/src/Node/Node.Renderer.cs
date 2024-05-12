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

    private void Draw(ImDrawListPtr drawList)
    {
        NodeSnapshot snapshot = CreateSnapshot();

        if (_texture is null || NodeSnapshot.AreEqual(ref snapshot, ref _snapshot) is false) {
            _texture  = Renderer.Instance.CreateTexture(this);
            _snapshot = snapshot;
        }

        if (null != _texture) {
            drawList.AddImage(_texture.ImGuiHandle, Bounds.PaddingRect.TopLeft, Bounds.PaddingRect.BottomRight);
        }

        DrawDebugBounds();

        foreach (var childNode in ChildNodes) {
            childNode.Draw(drawList);
        }
    }

    private NodeSnapshot CreateSnapshot()
    {
        return new() {
            Width       = OuterWidth,
            Height      = OuterHeight,
            ValueWidth  = NodeValueSize.X,
            ValueHeight = NodeValueSize.Y,
            LayoutStyle = ComputedStyle.CommittedLayoutStyle,
            PaintStyle  = ComputedStyle.CommittedPaintStyle,
        };
    }

    private void Test<T>(in T x) where T : unmanaged { }
}

[StructLayout(LayoutKind.Sequential)]
internal struct NodeSnapshot
{
    internal int         Width;
    internal int         Height;
    internal float       ValueWidth;
    internal float       ValueHeight;
    internal LayoutStyle LayoutStyle;
    internal PaintStyle  PaintStyle;

    public static bool AreEqual<T>(ref readonly T a, ref readonly T b) where T : unmanaged
    {
        return MemoryMarshal
            .AsBytes(new ReadOnlySpan<T>(in a))
            .SequenceEqual(MemoryMarshal.AsBytes(new ReadOnlySpan<T>(in b)));
    }
}
