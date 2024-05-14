using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Dalamud.Interface.Internal;
using ImGuiNET;
using Una.Drawing.Texture;

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
    public bool IsVisible => ComputedStyle.IsVisible && !Bounds.PaddingSize.IsZero;

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
        RenderShadow(drawList);

        if (null != _texture) {
            drawList.AddImage(
                _texture.ImGuiHandle,
                Bounds.PaddingRect.TopLeft,
                Bounds.PaddingRect.BottomRight,
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
            ValueWidth  = NodeValueMeasurement?.Size.Width ?? 0,
            ValueHeight = NodeValueMeasurement?.Size.Height ?? 0,
            LayoutStyle = ComputedStyle.CommittedLayoutStyle,
            PaintStyle  = ComputedStyle.CommittedPaintStyle,
        };
    }

    private uint GetRenderColor()
    {
        float opacity = ComputedStyle.Opacity;

        Node? parent = ParentNode;

        while (parent is not null
               && opacity > 0.0f) {
            opacity *= parent.ComputedStyle.Opacity;
            parent  =  parent.ParentNode;
        }

        opacity = Math.Clamp(opacity, 0.0f, 1.0f);

        return (uint)(opacity * 255) << 24 | 0x00FFFFFF;
    }

    private void RenderShadow(ImDrawListPtr drawList)
    {
        if (ComputedStyle.ShadowSize.IsZero) return;

        uint color = GetRenderColor();
        if (color == 0) return;

        var  rect  = Bounds.PaddingRect.Copy();

        if (ComputedStyle.ShadowInset > 0) rect.Shrink(new(ComputedStyle.ShadowInset));

        rect.X1 += (int)ComputedStyle.ShadowOffset.X;
        rect.Y1 += (int)ComputedStyle.ShadowOffset.Y;
        rect.X2 += (int)ComputedStyle.ShadowOffset.X;
        rect.Y2 += (int)ComputedStyle.ShadowOffset.Y;

        const float uv0  = 0.0f;
        const float uv1  = 0.333333f;
        const float uv2  = 0.666666f;
        const float uv3  = 1.0f;

        ImDrawListPtr dl    = drawList;
        IntPtr        id    = TextureLoader.GetEmbeddedTexture("Shadow.png").ImGuiHandle;
        Vector2       p     = rect.TopLeft;
        Vector2       s     = new(rect.Width, rect.Height);
        Vector2       m     = new(p.X + s.X, p.Y + s.Y);
        EdgeSize      side  = ComputedStyle.ShadowSize;

        if (IsInWindowDrawList(dl)) dl.PushClipRectFullScreen();

        if (side.Top > 0 || side.Left > 0)
            dl.AddImage(id, new(p.X - side.Left, p.Y - side.Top), new(p.X, p.Y), new(uv0, uv0), new(uv1, uv1), color);

        if (side.Top > 0)
            dl.AddImage(id, p with { Y = p.Y - side.Top }, new(m.X, p.Y), new(uv1, uv0), new(uv2, uv1), color);

        if (side.Top > 0 || side.Right > 0)
            dl.AddImage(id, m with { Y = p.Y - side.Top }, p with { X = m.X + side.Right }, new(uv2, uv0), new(uv3, uv1), color);

        if (side.Left > 0)
            dl.AddImage(id, p with { X = p.X - side.Left }, new(p.X, m.Y), new(uv0, uv1), new(uv1, uv2), color);

        if (side.Right > 0)
            dl.AddImage(id, new(m.X, p.Y), m with { X = m.X + side.Right }, new(uv2, uv1), new(uv3, uv2), color);

        if (side.Bottom > 0 || side.Left > 0)
            dl.AddImage(id, m with { X = p.X - side.Left }, p with { Y = m.Y + side.Bottom }, new(uv0, uv2), new(uv1, uv3), color);

        if (side.Bottom > 0)
            dl.AddImage(id, new(p.X, m.Y), m with { Y = m.Y + side.Bottom }, new(uv1, uv2), new(uv2, uv3), color);

        if (side.Bottom > 0 || side.Right > 0)
            dl.AddImage(id, new(m.X, m.Y), new(m.X + side.Right, m.Y + side.Bottom), new(uv2, uv2), new(uv3, uv3), color);

        if (IsInWindowDrawList(drawList)) dl.PopClipRect();
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
