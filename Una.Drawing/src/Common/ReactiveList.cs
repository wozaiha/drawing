/* Una.Drawing                                                 ____ ___
 *   A declarative drawing library for FFXIV.                 |    |   \____ _____        ____                _
 *                                                            |    |   /    \\__  \      |    \ ___ ___ _ _ _|_|___ ___
 * By Una. Licensed under AGPL-3.                             |    |  |   |  \/ __ \_    |  |  |  _| .'| | | | |   | . |
 * https://github.com/una-xiv/drawing                         |______/|___|  (____  / [] |____/|_| |__,|_____|_|_|_|_  |
 * ----------------------------------------------------------------------- \/ --- \/ ----------------------------- |__*/

using System;
using System.Collections.Generic;

namespace Una.Drawing;

internal class ReactiveList<T> : List<T>
{
    public event Action<T>? OnItemAdded;
    public event Action<T>? OnItemRemoved;

    public new void Add(T item)
    {
        base.Add(item);
        OnItemAdded?.Invoke(item);
    }

    public new void Remove(T item)
    {
        if (base.Remove(item)) OnItemRemoved?.Invoke(item);
    }

    public new void Clear()
    {
        foreach (var item in this) {
            OnItemRemoved?.Invoke(item);
        }

        base.Clear();
    }

    public new void AddRange(IEnumerable<T> collection)
    {
        foreach (var item in collection) {
            Add(item);
        }
    }

    public new void RemoveAll(Predicate<T> match)
    {
        foreach (var item in this) {
            if (match(item)) Remove(item);
        }
    }

    public new void RemoveAt(int index)
    {
        var item = this[index];
        Remove(item);
    }

    public new void RemoveRange(int index, int count)
    {
        for (int i = index; i < index + count; i++) {
            RemoveAt(i);
        }
    }

    public new void Insert(int index, T item)
    {
        base.Insert(index, item);
        OnItemAdded?.Invoke(item);
    }

    public new void InsertRange(int index, IEnumerable<T> collection)
    {
        foreach (var item in collection) {
            Insert(index++, item);
        }
    }
}
