using SkiaSharp;

namespace Una.Drawing.Generator;

internal interface IGenerator
{
    /// <summary>
    /// Defines the order in which the generator should be rendered.
    /// </summary>
    public int RenderOrder { get; }

    /// <summary>
    /// Generates a part of the texture for the given node.
    /// </summary>
    /// <param name="canvas">The drawing canvas.</param>
    /// <param name="node">The source node.</param>
    public void Generate(SKCanvas canvas, Node node);
}
