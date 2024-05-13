/* Una.Drawing                                                 ____ ___
 *   A declarative drawing library for FFXIV.                 |    |   \____ _____        ____                _
 *                                                            |    |   /    \\__  \      |    \ ___ ___ _ _ _|_|___ ___
 * By Una. Licensed under AGPL-3.                             |    |  |   |  \/ __ \_    |  |  |  _| .'| | | | |   | . |
 * https://github.com/una-xiv/drawing                         |______/|___|  (____  / [] |____/|_| |__,|_____|_|_|_|_  |
 * ----------------------------------------------------------------------- \/ --- \/ ----------------------------- |__*/

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dalamud.Interface;
using Dalamud.Interface.Internal;
using SkiaSharp;
using Una.Drawing.Generator;

namespace Una.Drawing;

internal static class Renderer
{
    private static List<IGenerator> _generators = [];

    private static UiBuilder _uiBuilder = null!;
    private static SKSurface _skSurface = null!;
    private static SKCanvas  _skCanvas  = null!;

    internal static void Setup(UiBuilder uiBuilder)
    {
        _uiBuilder = uiBuilder;

        // Collect generators.
        List<Type> generatorTypes = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass && t.IsAssignableTo(typeof(IGenerator)))
            .ToList();

        _generators = generatorTypes
            .Select(t => (IGenerator)Activator.CreateInstance(t)!)
            .OrderBy(g => g.RenderOrder)
            .ToList();

        _generators.ForEach(g => Logger.Log($"Got generator: {g.GetType().Name}"));

        // Create the SKSurface and SKCanvas.
        _skSurface = SKSurface.Create(new SKImageInfo(4096, 4096));
        _skCanvas  = _skSurface.Canvas;
    }

    internal static void Dispose()
    {
        _skCanvas.Dispose();
        _skSurface.Dispose();
    }

    /// <summary>
    /// Creates a texture for the given node.
    /// </summary>
    internal static unsafe IDalamudTextureWrap? CreateTexture(Node node)
    {
        if (node.Width == 0 || node.Height == 0) return null;

        using SKPaint paint = new();
        paint.Color     = SKColor.Empty;
        paint.Style     = SKPaintStyle.Fill;
        paint.BlendMode = SKBlendMode.Clear;
        _skCanvas.DrawRect(0, 0, node.Width, node.Height, paint);

        foreach (IGenerator generator in _generators) {
            generator.Generate(_skCanvas, node);
        }

        byte[] targetData = ArrayPool<byte>.Shared.Rent(node.Width * node.Height * 4);

        fixed (void* ptr = targetData) {
            _skSurface.ReadPixels(
                new() {
                    Width      = node.Width,
                    Height     = node.Height,
                    AlphaType  = SKAlphaType.Premul,
                    ColorType  = SKColorType.Bgra8888,
                    ColorSpace = SKColorSpace.CreateSrgb()
                },
                (nint)ptr,
                node.Width * 4,
                0,
                0
            );
        }

        IDalamudTextureWrap texture = _uiBuilder.LoadImageRaw(targetData, node.Width, node.Height, 4);

        ArrayPool<byte>.Shared.Return(targetData);

        return texture;
    }
}
