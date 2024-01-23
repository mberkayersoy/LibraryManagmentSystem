using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<SerializedDictionarElement<TKey, TValue>> _elements = new();
    public void OnAfterDeserialize()
    {
        Clear();
        foreach (var item in _elements)
        {
            this[item.Key] = item.Value;
        }

    }

    public void OnBeforeSerialize()
    {
        _elements.Clear();

        foreach (var pair in this)
        {
            _elements.Add(new SerializedDictionarElement<TKey, TValue>(pair.Key, pair.Value));
        }
    }

}

[Serializable]
public class SerializedDictionarElement<TKey, TValue>
{
    public TKey Key;
    public TValue Value;

    public SerializedDictionarElement(TKey key, TValue value)
    {
        Key = key;
        Value = value;
    }

}