/* Una.Drawing                                                 ____ ___
 *   A declarative drawing library for FFXIV.                 |    |   \____ _____        ____                _
 *                                                            |    |   /    \\__  \      |    \ ___ ___ _ _ _|_|___ ___
 * By Una. Licensed under AGPL-3.                             |    |  |   |  \/ __ \_    |  |  |  _| .'| | | | |   | . |
 * https://github.com/una-xiv/drawing                         |______/|___|  (____  / [] |____/|_| |__,|_____|_|_|_|_  |
 * ----------------------------------------------------------------------- \/ --- \/ ----------------------------- |__*/

using Dalamud.Interface.GameFonts;

namespace Una.Drawing.Texture.GameGlyph;

internal class GameGlyphEntry(GameGlyphTexture texture, FdtReader.FontTableHeader header, FdtReader.FontTableEntry info)
{
    public int    Width     { get; } = info.BoundingWidth;
    public int    Height    { get; } = info.BoundingHeight;
    public byte[] ImageData { get; } = texture.Extract(header, info);
}
