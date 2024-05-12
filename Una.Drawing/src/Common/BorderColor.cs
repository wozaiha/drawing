/* Una.Drawing                                                 ____ ___
 *   A declarative drawing library for FFXIV.                 |    |   \____ _____        ____                _
 *                                                            |    |   /    \\__  \      |    \ ___ ___ _ _ _|_|___ ___
 * By Una. Licensed under AGPL-3.                             |    |  |   |  \/ __ \_    |  |  |  _| .'| | | | |   | . |
 * https://github.com/una-xiv/drawing                         |______/|___|  (____  / [] |____/|_| |__,|_____|_|_|_|_  |
 * ----------------------------------------------------------------------- \/ --- \/ ----------------------------- |__*/

namespace Una.Drawing;

public record struct BorderColor(Color? Top = null, Color? Right = null, Color? Bottom = null, Color? Left = null)
{
    public BorderColor(Color? all) : this(all, all, all, all) { }

    public BorderColor(Color? topRight, Color? bottomLeft) : this(topRight, topRight, bottomLeft, bottomLeft) { }
}
