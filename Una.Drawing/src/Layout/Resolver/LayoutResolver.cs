namespace Una.Drawing.Layout.Resolver;

internal static class LayoutResolver
{
    public static void Resolve(Node node)
    {
        ComputeSizeOf(node);
    }

    /// <summary>
    /// Pre-compute the bounding box sizes of the given node.
    /// </summary>
    /// <param name="node"></param>
    private static void ComputeSizeOf(Node node)
    {
        foreach (var childNode in node.ChildNodes) ComputeSizeOf(childNode);

        if (node.Style.Size.IsFixed) {
            node.Bounds.ContentSize = node.Style.Size.Copy();
            node.Bounds.PaddingSize = node.Style.Size.Copy();
        } else if (node.Style.Size.IsAutoWidth) {
            node.Bounds.ContentSize = new(0, node.Style.Size.Height);
            node.Bounds.PaddingSize = new(node.Style.Padding.HorizontalSize, node.Style.Size.Height + node.Style.Padding.VerticalSize);
        } else if (node.Style.Size.IsAutoHeight) {
            node.Bounds.ContentSize = new(node.Style.Size.Width, 0);
            node.Bounds.PaddingSize = new(node.Style.Size.Width + node.Style.Padding.HorizontalSize, node.Style.Padding.VerticalSize);
        } else {
            node.Bounds.ContentSize = new(0, 0);
            node.Bounds.PaddingSize = node.Style.Padding.Size;
        }

        node.Bounds.MarginSize = node.Bounds.PaddingSize + node.Style.Margin.Size;
    }

    private static void ComputeContentSizeOf(Node node)
    {

    }
}
