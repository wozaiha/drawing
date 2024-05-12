/* Una.Drawing                                                 ____ ___
 *   A declarative drawing library for FFXIV.                 |    |   \____ _____        ____                _
 *                                                            |    |   /    \\__  \      |    \ ___ ___ _ _ _|_|___ ___
 * By Una. Licensed under AGPL-3.                             |    |  |   |  \/ __ \_    |  |  |  _| .'| | | | |   | . |
 * https://github.com/una-xiv/drawing                         |______/|___|  (____  / [] |____/|_| |__,|_____|_|_|_|_  |
 * ----------------------------------------------------------------------- \/ --- \/ ----------------------------- |__*/

namespace Una.Drawing;

public partial class Style
{
    /// <summary>
    /// <para>
    /// Defines a single background color for the element.
    /// </para>
    /// <para>
    /// If both <see cref="BackgroundColor"/> and <see cref="BackgroundGradient"/>
    /// are defined, the gradient will be rendered on top of the background color.
    /// </para>
    /// </summary>
    public Color? BackgroundColor { get; set; }

    /// <summary>
    /// <para>
    /// Defines a 4-point gradient background for the element.
    /// </para>
    /// <para>
    /// If both <see cref="BackgroundColor"/> and <see cref="BackgroundGradient"/>
    /// are defined, the gradient will be rendered on top of the background color.
    /// </para>
    /// </summary>
    public GradientColor? BackgroundGradient { get; set; }
}
