/* Una.Drawing                                                 ____ ___
 *   A declarative drawing library for FFXIV.                 |    |   \____ _____        ____                _
 *                                                            |    |   /    \\__  \      |    \ ___ ___ _ _ _|_|___ ___
 * By Una. Licensed under AGPL-3.                             |    |  |   |  \/ __ \_    |  |  |  _| .'| | | | |   | . |
 * https://github.com/una-xiv/drawing                         |______/|___|  (____  / [] |____/|_| |__,|_____|_|_|_|_  |
 * ----------------------------------------------------------------------- \/ --- \/ ----------------------------- |__*/

using System.Collections.Generic;
using Dalamud.Interface.GameFonts;
using Lumina.Data.Files;

namespace Una.Drawing.Texture.GameGlyph;

internal class GameGlyphTexture
{
    private readonly Dictionary<int, byte[]> _channelMaps = [];

    public GameGlyphTexture(int textureId)
    {
        TexFile texFile = DalamudServices.DataManager.GetFile<TexFile>($"common/font/font{textureId + 1}.tex")!;
        byte[]  imgData = GetImageData(texFile);

        for (var channel = 0; channel < 4; channel++) {
            _channelMaps[channel] = GetChannelData(imgData, channel);
        }
    }

    /// <summary>
    /// Extracts the glyph data from the texture.
    /// </summary>
    public byte[] Extract(FdtReader.FontTableHeader header, FdtReader.FontTableEntry entry)
    {
        int x1     = entry.TextureOffsetX;
        int y1     = entry.TextureOffsetY;
        int x2     = entry.TextureOffsetX + entry.BoundingWidth;
        int y2     = entry.TextureOffsetY + entry.BoundingHeight;

        var glyphData = new byte[entry.BoundingWidth * entry.BoundingHeight * 4];

        for (int y = y1; y < y2; y++) {
            for (int x = x1; x < x2; x++) {
                long srcIndex = (y + entry.CurrentOffsetY - 2) * header.TextureWidth + x + 0;
                long dstIndex = (y - y1) * entry.BoundingWidth * 4 + (x - x1) * 4;
                byte pixel    = _channelMaps[entry.TextureChannelIndex][srcIndex];

                glyphData[dstIndex]     = pixel;
                glyphData[dstIndex + 1] = pixel;
                glyphData[dstIndex + 2] = pixel;
                glyphData[dstIndex + 3] = pixel;
            }
        }

        return glyphData;
    }

    /// <summary>
    /// Extracts a single channel from the image data.
    /// </summary>
    private static byte[] GetChannelData(in byte[] imgData, int channel)
    {
        var channelData = new byte[imgData.Length / 4];

        for (var index = 0; index < channelData.Length; index++) {
            channelData[index] = imgData[index * 4 + channel];
        }

        return channelData;
    }

    /// <summary>
    /// Returns the image data of the given TexFile in RGBA format.
    /// </summary>
    private static byte[] GetImageData(TexFile file)
    {
        byte[] imageData     = file.ImageData;
        var    rgbaImageData = new byte[imageData.Length];

        for (var index = 0; index < rgbaImageData.Length; index += 4) {
            rgbaImageData[index]     = imageData[index + 2];
            rgbaImageData[index + 1] = imageData[index + 1];
            rgbaImageData[index + 2] = imageData[index];
            rgbaImageData[index + 3] = imageData[index + 3];
        }

        return rgbaImageData;
    }
}
