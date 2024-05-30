using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Dalamud.Interface.Internal;
using Lumina.Data.Files;
using SkiaSharp;

namespace Una.Drawing.Texture;

internal static class TextureLoader
{
    private static readonly Dictionary<uint, TexFile>               IconToTexFileCache   = [];
    private static readonly Dictionary<uint, SKImage>               IconToImageCache     = [];
    private static readonly Dictionary<string, IDalamudTextureWrap> EmbeddedTextureCache = [];

    /// <summary>
    /// Loads an embedded texture from one of the plugin assemblies.
    /// </summary>
    /// <param name="name">The logical name of the resource.</param>
    /// <returns>An instance of <see cref="IDalamudTextureWrap"/> that wraps the resource.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static IDalamudTextureWrap GetEmbeddedTexture(string name)
    {
        if (EmbeddedTextureCache.TryGetValue(name, out var cachedTexture)) return cachedTexture;

        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
        if (stream == null) throw new InvalidOperationException($"Failed to load embedded texture \"{name}\".");

        var imageData = new byte[stream.Length];
        int _         = stream.Read(imageData, 0, imageData.Length);

        IDalamudTextureWrap texture = DalamudServices.UiBuilder.LoadImage(imageData);
        EmbeddedTextureCache[name] = texture;

        return texture;
    }

    internal static SKImage? LoadFromBytes(byte[] bytes)
    {
        using MemoryStream stream = new(bytes);
        using SKImage      image  = SKImage.FromEncodedData(stream);
        using SKBitmap     bitmap = SKBitmap.FromImage(image);

        // We need to do some fuckery to swap from BGRA to RGBA...
        SKImageInfo info = new(image.Width, image.Height, SKColorType.Rgba8888, SKAlphaType.Premul);

        IntPtr pixelPtr = Marshal.AllocHGlobal(bitmap.ByteCount);

        Marshal.Copy(bitmap.Bytes, 0, pixelPtr, bitmap.ByteCount);
        SKImage? output = SKImage.FromPixels(info, pixelPtr);

        return output;
    }

    internal static SKImage? LoadIcon(uint iconId)
    {
        if (IconToImageCache.TryGetValue(iconId, out SKImage? cachedImage)) return cachedImage;

        TexFile? iconFile = GetIconFile(iconId);
        if (null == iconFile) return null;

        SKImageInfo info = new(iconFile.Header.Width, iconFile.Header.Height, SKColorType.Rgba8888, SKAlphaType.Premul);

        IntPtr pixelPtr = Marshal.AllocHGlobal(iconFile.ImageData.Length);
        Marshal.Copy(iconFile.ImageData, 0, pixelPtr, iconFile.ImageData.Length);

        using SKPixmap pixmap = new(info, pixelPtr);
        SKImage?       image  = SKImage.FromPixels(pixmap);

        IconToImageCache[iconId] = image;

        return image;
    }

    internal static SKImage? LoadTexture(string path)
    {
        if (DalamudServices.DataManager == null || DalamudServices.TextureProvider == null)
            throw new InvalidOperationException("Una.Drawing.DrawingLib has not been set-up.");

        path = DalamudServices.TextureSubstitutionProvider.GetSubstitutedPath(path);

        TexFile? texFile = Path.IsPathRooted(path)
            ? DalamudServices.DataManager.GameData.GetFileFromDisk<TexFile>(path)
            : DalamudServices.DataManager.GetFile<TexFile>(path);

        if (null == texFile) return null;

        SKImageInfo info = new(texFile.Header.Width, texFile.Header.Height, SKColorType.Rgba8888, SKAlphaType.Unpremul);

        IntPtr pixelPtr = Marshal.AllocHGlobal(texFile.ImageData.Length);
        Marshal.Copy(texFile.ImageData, 0, pixelPtr, texFile.ImageData.Length);

        using SKPixmap pixmap = new(info, pixelPtr);
        SKImage?       image  = SKImage.FromPixels(pixmap);

        return image;
    }

    /// <summary>
    /// Returns a <see cref="TexFile"/> for the given icon ID.
    /// </summary>
    private static TexFile GetIconFile(uint iconId)
    {
        if (IconToTexFileCache.TryGetValue(iconId, out var cachedIconFile)) return cachedIconFile;

        if (DalamudServices.DataManager == null || DalamudServices.TextureProvider == null)
            throw new InvalidOperationException("Una.Drawing.DrawingLib has not been set-up.");

        string originalIconPath = DalamudServices.TextureProvider.GetIconPath(iconId)
            ?? throw new InvalidOperationException($"Failed to get icon path for #{iconId}.");

        string iconPath = DalamudServices.TextureSubstitutionProvider.GetSubstitutedPath(originalIconPath);

        TexFile? iconFile = Path.IsPathRooted(iconPath)
            ? DalamudServices.DataManager.GameData.GetFileFromDisk<TexFile>(iconPath)
            : DalamudServices.DataManager.GetFile<TexFile>(iconPath);

        IconToTexFileCache[iconId] = iconFile
            ?? throw new InvalidOperationException($"Failed to load icon file for #{iconId}.");

        return iconFile;
    }
}
