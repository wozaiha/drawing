/* Una.Drawing                                                 ____ ___
 *   A declarative drawing library for FFXIV.                 |    |   \____ _____        ____                _
 *                                                            |    |   /    \\__  \      |    \ ___ ___ _ _ _|_|___ ___
 * By Una. Licensed under AGPL-3.                             |    |  |   |  \/ __ \_    |  |  |  _| .'| | | | |   | . |
 * https://github.com/una-xiv/drawing                         |______/|___|  (____  / [] |____/|_| |__,|_____|_|_|_|_  |
 * ----------------------------------------------------------------------- \/ --- \/ ----------------------------- |__*/

using System.Buffers;
using System.Linq;
using System.Reflection;
using Dalamud.Interface.Textures;
using Dalamud.Interface.Textures.TextureWraps;
using ImGuiNET;
using Una.Drawing.Generator;

namespace Una.Drawing;

internal class Renderer : IDisposable
{
    private readonly List<IGenerator> _generators;
    private readonly SKSurface        _skSurface;
    private readonly SKCanvas         _skCanvas;

    private static Renderer? _instance;
    internal static Renderer Instance => _instance ??=
        DalamudServices.PluginInterface.GetOrCreateData($"Una.Drawing.Renderer:{DrawingLib.Version}", () => new Renderer());

    internal Renderer()
    {
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

        // Create the SKSurface and SKCanvas.
        SKImageInfo         info  = new(8192, 8192);
        SKSurfaceProperties props = new(SKSurfacePropsFlags.None, SKPixelGeometry.Unknown);

        _skSurface = SKSurface.Create(info, props);
        _skCanvas  = _skSurface.Canvas;
    }

    /// <summary>
    /// Relinquish the shared instance.
    /// <para>
    /// Once all plugins have relinquished the shared instance, the instance Dispose method will be called by Dalamud.
    /// </para>
    /// </summary>
    internal void RelinquishDataShare()
    {
        DalamudServices.PluginInterface.RelinquishData("Una.Drawing.Renderer");
    }

    public void Dispose()
    {
        _skCanvas.Dispose();
        _skSurface.Dispose();
    }

    /// <summary>
    /// Creates a texture for the given node.
    /// </summary>
    internal unsafe IDalamudTextureWrap? CreateTexture(Node node)
    {
        if (node.Width == 0 || node.Height == 0) return null;

        using SKPaint paint = new();
        paint.Color     = SKColor.Empty;
        paint.Style     = SKPaintStyle.Fill;
        paint.BlendMode = SKBlendMode.Clear;

        _skCanvas.DrawRect(0, 0, node.Width, node.Height, paint);

        bool hasDrawn = false;

        foreach (IGenerator generator in _generators)
        {
            if (generator.Generate(_skCanvas, node))
            {
                hasDrawn = true;
            }
        }

        if (!hasDrawn) return null;

        byte[] targetData = ArrayPool<byte>.Shared.Rent(node.Width * node.Height * 4);

        fixed (void* ptr = targetData)
        {
            _skSurface.ReadPixels(
                new()
                {
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

        IDalamudTextureWrap texture = DalamudServices.TextureProvider.CreateFromRaw(
            RawImageSpecification.Rgba32(node.Width, node.Height),
            targetData
        );

        // IDalamudTextureWrap texture = DalamudServices.UiBuilder.LoadImageRaw(targetData, node.Width, node.Height, 4);

        ArrayPool<byte>.Shared.Return(targetData);

        return texture;
    }
}