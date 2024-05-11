/* Una.Drawing                                                 ____ ___
 *   A declarative drawing library for FFXIV.                 |    |   \____ _____        ____                _
 *                                                            |    |   /    \\__  \      |    \ ___ ___ _ _ _|_|___ ___
 * By Una. Licensed under AGPL-3.                             |    |  |   |  \/ __ \_    |  |  |  _| .'| | | | |   | . |
 * https://github.com/una-xiv/drawing                         |______/|___|  (____  / [] |____/|_| |__,|_____|_|_|_|_  |
 * ----------------------------------------------------------------------- \/ --- \/ ----------------------------- |__*/

using System.Collections.Generic;
using System.Linq;

namespace Una.Drawing;

public partial class Node
{
    private readonly Dictionary<string, Node?>      _querySelectorCache    = [];
    private readonly Dictionary<string, List<Node>> _querySelectorAllCache = [];

    /// <summary>
    /// Returns the first node that matches the given query selector, or NULL
    /// if no matching node was found.
    /// </summary>
    /// <param name="querySelectorString">The query selector string.</param>
    /// <returns>A <see cref="Node"/> object or NULL if no such node exists.</returns>
    public Node? QuerySelector(string querySelectorString)
    {
        if (_querySelectorCache.TryGetValue(querySelectorString, out var cachedResult)) return cachedResult;
        var querySelectors = QuerySelectorParser.Parse(querySelectorString);

        foreach (var querySelector in querySelectors) {
            Node? node = FindChildrenMatching(this, querySelector, true).FirstOrDefault();

            if (node != null) {
                _querySelectorCache[querySelectorString] = node;
                return node;
            }
        }

        _querySelectorCache[querySelectorString] = null;
        return null;
    }

    /// <summary>
    /// Returns the first node that matches the given query selector and type
    /// argument, or NULL if no such node exists.
    /// </summary>
    /// <param name="querySelectorString">The query selector string.</param>
    /// <returns>An instance of T that inherits <see cref="Node"/> or NULL.</returns>
    public T? QuerySelector<T>(string querySelectorString) where T : Node
    {
        return QuerySelector(querySelectorString) as T;
    }

    /// <summary>
    /// Returns a list of nodes that match the given query selector.
    /// </summary>
    /// <param name="querySelectorString">The query selector string.</param>
    /// <returns>A list of matching <see cref="Node"/> instances.</returns>
    public IEnumerable<Node> QuerySelectorAll(string querySelectorString)
    {
        if (_querySelectorAllCache.TryGetValue(querySelectorString, out var cachedResult)) return cachedResult;

        List<QuerySelector> querySelectors = QuerySelectorParser.Parse(querySelectorString);
        List<Node>          nodeListResult = [];

        foreach (var querySelector in querySelectors) {
            nodeListResult.AddRange(FindChildrenMatching(this, querySelector, true));
        }

        _querySelectorAllCache[querySelectorString] = nodeListResult;

        return nodeListResult;
    }

    /// <summary>
    /// Returns a list of nodes that match the given query selector and type
    /// argument.
    /// </summary>
    /// <param name="querySelectorString">The query selector string.</param>
    /// <returns>A list of matching nodes.</returns>
    public IEnumerable<T> QuerySelectorAll<T>(string querySelectorString) where T : Node
    {
        return QuerySelectorAll(querySelectorString).Cast<T>().ToList();
    }

    /// <summary>
    /// Invalidates the query selector cache entries.
    /// </summary>
    private void InvalidateQuerySelectorCache(object? _ = null)
    {
        _querySelectorCache.Clear();
        _querySelectorAllCache.Clear();
    }

    private static List<Node> FindChildrenMatching(Node node, QuerySelector querySelector, bool recursive)
    {
        List<Node> nodeListResult = [];

        foreach (var child in node._childNodes) {
            if (!MatchesQuerySelector(child, querySelector)) {
                continue;
            }

            if (querySelector.DirectChild != null) {
                var result = FindChildrenMatching(child, querySelector.DirectChild, false);
                if (result.Count > 0) nodeListResult.AddRange(result);
                continue;
            }

            if (querySelector.NestedChild != null) {
                var result = FindChildrenMatching(child, querySelector.NestedChild, true);
                if (result.Count > 0) nodeListResult.AddRange(result);
                continue;
            }

            nodeListResult.Add(child);
        }

        if (recursive && nodeListResult.Count == 0 && node._childNodes.Count > 0) {
            foreach (var child in node._childNodes) {
                nodeListResult.AddRange(FindChildrenMatching(child, querySelector, true));
            }
        }

        return nodeListResult;
    }

    private static bool MatchesQuerySelector(Node node, QuerySelector querySelector)
    {
        if (querySelector.Identifier != null && node.Id != querySelector.Identifier) {
            return false;
        }

        if (querySelector.ClassList.Count > 0) {
            if (node.ClassList.Count == 0) return false;

            foreach (string className in querySelector.ClassList) {
                if (!node.ClassList.Contains(className)) {
                    return false;
                }
            }
        }

        if (querySelector.TagList.Count > 0) {
            if (node.TagsList.Count == 0) return false;

            foreach (string tag in querySelector.TagList) {
                if (!node.TagsList.Contains(tag)) {
                    return false;
                }
            }
        }

        return true;
    }
}
