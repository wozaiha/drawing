using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

namespace Una.Drawing;

public class DrawingLib
{
    private static Renderer? _renderer = null;

    /// <summary>
    /// Setup the drawing library. Make sure to call this method in your plugin
    /// before using any of the drawing library's features.
    /// </summary>
    public static void Setup(DalamudPluginInterface pluginInterface)
    {
        pluginInterface.Create<DalamudServices>();

        _renderer = Renderer.Create(pluginInterface);
    }

    /// <summary>
    /// Disposes of the allocated resources in the drawing library. Make sure
    /// to call invoke this in your plugin's Dispose method.
    /// </summary>
    public static void Dispose()
    {
        _renderer?.Dispose();
    }
}

internal class DalamudServices
{
    [PluginService] public static IDataManager                 DataManager                 { get; set; } = null!;
    [PluginService] public static ITextureProvider             TextureProvider             { get; set; } = null!;
    [PluginService] public static ITextureSubstitutionProvider TextureSubstitutionProvider { get; set; } = null!;
}
