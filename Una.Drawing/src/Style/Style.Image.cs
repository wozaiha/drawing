/* Una.Drawing                                                 ____ ___
 *   A declarative drawing library for FFXIV.                 |    |   \____ _____        ____                _
 *                                                            |    |   /    \\__  \      |    \ ___ ___ _ _ _|_|___ ___
 * By Una. Licensed under AGPL-3.                             |    |  |   |  \/ __ \_    |  |  |  _| .'| | | | |   | . |
 * https://github.com/una-xiv/drawing                         |______/|___|  (____  / [] |____/|_| |__,|_____|_|_|_|_  |
 * ----------------------------------------------------------------------- \/ --- \/ ----------------------------- |__*/

using System.Numerics;
using Dalamud.Game.Text;

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
    /// Draws the given game glyph in the node.
    /// </summary>
    public SeIconChar? Glyph { get; set; }

    /// <summary>
    /// A translation offset for the glyph.
    /// </summary>
    public Vector2? GlyphOffset { get; set; }

    /// <summary>
    /// Defines the color of the glyph. Defaults to white.
    /// </summary>
    public Color? GlyphColor { get; set; }

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
