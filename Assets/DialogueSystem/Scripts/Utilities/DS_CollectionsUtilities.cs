using DS.Runtime.Utilities;
using System.Collections.Generic;

namespace DS.Editor.Utilities
{
    /// <summary>
    /// Utilities for handling custom colletions in the Dialogue System.
    /// </summary>
    public static class DS_CollectionsUtilities
    {
        /// <summary>
        /// Add both Key and Value in a SerializableDictionary, if key already exists add only the value in the value list.
        /// </summary>
        /// <typeparam name="K">The Key type.</typeparam>
        /// <typeparam name="V">The Value type.</typeparam>
        /// <param name="serializableDictionary"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
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
