using System.Numerics;

namespace Una.Drawing;

public partial class Style
{
    /// <summary>
    /// Draws a game icon in the node.
    /// </summary>
    public uint? IconId { get; set; }

    /// <summary>
    /// Defines the space between the icon and the border edges of the node.
    /// </summary>
    public EdgeSize? IconInset { get; set; }

    /// <summary>
    /// A translation offset for the icon.
    /// </summary>
    public Vector2? IconOffset { get; set; }

    /// <summary>
    /// Specifies the rounding of the icon.
    /// </summary>
    public float? IconRounding { get; set; }

    /// <summary>
    /// Specifies the corners of the icon that should be rounded.
    /// </summary>
    public RoundedCorners? IconRoundedCorners { get; set; }

    /// <summary>
    /// Whether the icon should be displayed in black and white.
    /// </summary>
    public bool? IconGrayscale { get; set; }

    /// <summary>
    /// A contrast value for the icon. Must range between 0 and 1.
    /// </summary>
    public float? IconContrast { get; set; }
}
