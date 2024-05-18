using System.IO;
using System.Numerics;
using Dalamud.Interface.Internal;

namespace Una.Drawing;

public partial class Style
{
    /// <summary>
    /// Draws a game icon in the node.
    /// </summary>
    public uint? IconId { get; set; }

    /// <summary>
    /// A byte array of an image file to be displayed in the node.
    /// This property takes precedence over <see cref="IconId"/>.
    /// </summary>
    public byte[]? ImageBytes { get; set; }

    /// <summary>
    /// Defines the space between the icon and the border edges of the node.
    /// </summary>
    public EdgeSize? ImageInset { get; set; }

    /// <summary>
    /// A translation offset for the icon.
    /// </summary>
    public Vector2? ImageOffset { get; set; }

    /// <summary>
    /// Specifies the rounding of the icon.
    /// </summary>
    public float? ImageRounding { get; set; }

    /// <summary>
    /// Specifies the corners of the icon that should be rounded.
    /// </summary>
    public RoundedCorners? ImageRoundedCorners { get; set; }

    /// <summary>
    /// Whether the icon should be displayed in black and white.
    /// </summary>
    public bool? ImageGrayscale { get; set; }

    /// <summary>
    /// A contrast value for the icon. Must range between 0 and 1.
    /// </summary>
    public float? ImageContrast { get; set; }
}
