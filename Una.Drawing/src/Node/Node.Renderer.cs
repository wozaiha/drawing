using System;
using System.Numerics;
using Dalamud.Interface.Internal;
using ImGuiNET;

namespace Una.Drawing;

public partial class Node
{
    private IDalamudTextureWrap? _texture;

    public void Render(ImDrawListPtr drawList, Point position)
    {
        if (ParentNode is not null)
            throw new InvalidOperationException("Cannot render a node that has a parent or is not a root node.");

        Reflow(position);
        Draw(drawList);
    }

    private void Draw(ImDrawListPtr drawList)
    {
        _texture ??= Renderer.Instance.CreateTexture(this);

        if (null != _texture) {
            drawList.AddImage(_texture.ImGuiHandle, Bounds.PaddingRect.TopLeft, Bounds.PaddingRect.BottomRight);
        }

        DrawDebugBounds();

        foreach (var childNode in ChildNodes) {
            childNode.Draw(drawList);
        }
    }
}
