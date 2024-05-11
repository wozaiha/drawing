using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dalamud.Interface;
using Dalamud.Interface.Internal;
using Dalamud.Plugin;
using SkiaSharp;
using Una.Drawing.Generator;

namespace Una.Drawing;

public sealed class Renderer : IDisposable
{
    private static Renderer? _instance;

    private readonly UiBuilder        _uiBuilder;
    private readonly List<IGenerator> _generators;

    /// <summary>
    /// <para>
    /// Creates an instance of the node renderer that is responsible for
    /// rendering the graphical aspects of individual nodes.
    /// </para>
    /// <para>
    /// The returned instance MUST be disposed of when the plugin is being
    /// disposed of.
    /// </para>
    /// </summary>
    /// <param name="pluginInterface"></param>
    /// <returns></returns>
    public static Renderer Create(DalamudPluginInterface pluginInterface)
    {
        return _instance ??= new(pluginInterface);
    }

    /// <summary>
    /// Returns true if an instance of the renderer has been created.
    /// </summary>
    internal static bool HasInstance => _instance != null;

    /// <summary>
    /// Returns the instance of the renderer.
    /// </summary>
    /// <exception cref="InvalidOperationException">If no instance was created.</exception>
    internal static Renderer Instance =>
        _instance ?? throw new InvalidOperationException("Renderer has not been initialized.");

    internal Renderer(DalamudPluginInterface plugin)
    {
        _uiBuilder = plugin.UiBuilder;

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
    }

    /// <inheritdoc/>
    public void Dispose() { }

    /// <summary>
    /// Creates a texture for the given node.
    /// </summary>
    internal unsafe IDalamudTextureWrap CreateTexture(Node node)
    {
        using SKSurface skSurface = SKSurface.Create(new SKImageInfo(node.Width, node.Height));
        using SKCanvas  skCanvas  = skSurface.Canvas;

        foreach (IGenerator generator in _generators) {
            generator.Generate(skCanvas, node);
        }

        using SKImage skImage = skSurface.Snapshot();

        byte[] targetData = ArrayPool<byte>.Shared.Rent(skImage.Width * skImage.Height * 4);
        fixed (void* ptr = targetData) {
            skSurface.ReadPixels(skImage.Info, (nint)ptr, skImage.Width * 4, 0, 0);
        }

        ArrayPool<byte>.Shared.Return(targetData);

        return _uiBuilder.LoadImageRaw(targetData, skImage.Width, skImage.Height, 4);
    }
}
