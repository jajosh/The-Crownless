using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

class MyDictionary<TKey, TValue> : IDictionary<TKey, TValue>
{
    private Dictionary<TKey, TValue> backingDictionary;

    public ICollection<TKey> Keys => throw new NotImplementedException();

    public ICollection<TValue> Values => throw new NotImplementedException();

    public int Count => throw new NotImplementedException();

    public bool IsReadOnly => throw new NotImplementedException();

    public TValue this[TKey key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    //Implement the interface here
    //Delegating most of the logic to your backingDictionary

    public void Add(TKey key, TValue value)
    {
        
        throw new NotImplementedException();
    }

    public bool ContainsKey(TKey key)
    {
        throw new NotImplementedException();
    }

    public bool Remove(TKey key)
    {
        throw new NotImplementedException();
    }

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        throw new NotImplementedException();
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        throw new NotImplementedException();
    }

    public void Clear()
    {
        throw new NotImplementedException();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        throw new NotImplementedException();
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        throw new NotImplementedException();
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
