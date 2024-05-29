using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DS.Utilities
{
    public static class DS_CollectionsUtilities
    {
        public static void AddItem<K, V>(this SerializableDictionary<K, List<V>> serializableDictionary, K key, V value)
        {
            if(serializableDictionary.ContainsKey(key))
            {
                serializableDictionary[key].Add(value);
                return;
            }
            else
            {
                serializableDictionary.Add(key, new List<V>() { value });
            }
        }
    }
}
