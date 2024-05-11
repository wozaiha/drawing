using System;
using System.Collections.Generic;
using SkiaSharp;

namespace Una.Drawing;

public static class TypefaceRegistry
{
    private static readonly Dictionary<string, SKTypeface> Typefaces = new();

    private static string _defaultFamilyName = "";

    internal static SKTypeface Get(string? familyName)
    {
        if (Typefaces.TryGetValue(familyName ?? _defaultFamilyName, out SKTypeface? typeface)) return typeface;

        if (_defaultFamilyName == "") Register(Typeface.Default, true);

        return Typefaces[_defaultFamilyName];
    }

    /// <summary>
    /// <para>
    /// Registers a typeface to be used when rendering text.
    /// </para>
    /// <para>
    /// The <see cref="Typeface.Name"/> defined in the typeface is referenced
    /// using the <see cref="Style.Font"/> style property. If the referenced
    /// typeface does not exist, the default typeface is used instead.
    /// </para>
    /// </summary>
    /// <param name="typeface">A typeface that defines the font.</param>
    /// <param name="isDefault">Whether this typeface should be used as default.</param>
    /// <exception cref="ArgumentException"></exception>
    public static void Register(Typeface typeface, bool isDefault = false)
    {
        if (Typefaces.ContainsKey(typeface.Name))
            throw new ArgumentException($"Typeface '{typeface.Name}' already exists.");

        Typefaces.Add(typeface.Name, typeface.SkTypeface);

        if (isDefault || _defaultFamilyName == "") _defaultFamilyName = typeface.Name;
    }

    /// <summary>
    /// Disposes of all typefaces in the registry.
    /// </summary>
    public static void Dispose()
    {
        foreach (SKTypeface typeface in Typefaces.Values) typeface.Dispose();
    }
}
