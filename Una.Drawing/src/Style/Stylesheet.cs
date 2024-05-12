/* Una.Drawing                                                 ____ ___
 *   A declarative drawing library for FFXIV.                 |    |   \____ _____        ____                _
 *                                                            |    |   /    \\__  \      |    \ ___ ___ _ _ _|_|___ ___
 * By Una. Licensed under AGPL-3.                             |    |  |   |  \/ __ \_    |  |  |  _| .'| | | | |   | . |
 * https://github.com/una-xiv/drawing                         |______/|___|  (____  / [] |____/|_| |__,|_____|_|_|_|_  |
 * ----------------------------------------------------------------------- \/ --- \/ ----------------------------- |__*/

using System.Collections.Generic;

namespace Una.Drawing;

public static class Stylesheet
{
    internal static readonly Dictionary<string, Style> ClassRules = [];
    internal static readonly Dictionary<string, Style> TagRules   = [];

    /// <summary>
    /// Defines a style that is applied to all nodes that match the specified
    /// class name.
    /// </summary>
    public static void SetClassRule(string className, Style style)
    {
        ClassRules[className] = style;
    }

    /// <summary>
    /// Defines a style that is applied to all nodes that match the specified
    /// tag name.
    /// </summary>
    public static void SetTagRule(string tagName, Style style)
    {
        TagRules[tagName] = style;
    }

    /// <summary>
    /// Returns a <see cref="Style"/> object that matches the given class name
    /// or NULL if no such style was found.
    /// </summary>
    public static Style? GetStyleForClass(string className)
    {
        return ClassRules.GetValueOrDefault(className);
    }

    /// <summary>
    /// Returns a <see cref="Style"/> object that matches the given tag name
    /// or NULL if no such style was found.
    /// </summary>
    public static Style? GetStyleForTag(string tagName)
    {
        return TagRules.GetValueOrDefault(tagName);
    }

    /// <summary>
    /// Deletes a class rule from the stylesheet.
    /// </summary>
    public static void DeleteClassRule(string className)
    {
        ClassRules.Remove(className);
    }

    /// <summary>
    /// Deletes a tag rule from the stylesheet.
    /// </summary>
    public static void DeleteTagRule(string tagName)
    {
        TagRules.Remove(tagName);
    }
}
