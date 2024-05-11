/* Una.Drawing                                                 ____ ___
 *   A declarative drawing library for FFXIV.                 |    |   \____ _____        ____                _
 *                                                            |    |   /    \\__  \      |    \ ___ ___ _ _ _|_|___ ___
 * By Una. Licensed under AGPL-3.                             |    |  |   |  \/ __ \_    |  |  |  _| .'| | | | |   | . |
 * https://github.com/una-xiv/drawing                         |______/|___|  (____  / [] |____/|_| |__,|_____|_|_|_|_  |
 * ----------------------------------------------------------------------- \/ --- \/ ----------------------------- |__*/

namespace Una.Drawing;

internal enum QuerySelectorTokenType
{
    /// <summary>
    /// An identifier of a node.
    /// </summary>
    Identifier,

    /// <summary>
    /// A class-name of a node.
    /// </summary>
    Class,

    /// <summary>
    /// The next tokens represent direct children of the current node.
    /// </summary>
    Child,

    /// <summary>
    /// The next tokens represent nested children (recursively) of the current node.
    /// </summary>
    DeepChild,

    /// <summary>
    /// Represents a pseudo-class of the current node.
    /// </summary>
    TagList,

    /// <summary>
    /// Represents a separator in the current node.
    /// </summary>
    Separator,
}
