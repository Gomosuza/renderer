using System;
using System.Collections.Generic;

namespace Terrain
{
    /// <summary>
    /// Extensions for the <see cref="Dictionary{TKey,TValue}"/> class.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Gets a value from the dictionary based on the specific key. If the value does not yet exist, creator func is called and the created value is stored back in the dicitonary before the value is returned.
        /// This method is not thread safe.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="creator">Called when no matching key exists in the dictionary. The created object is stored in the dictionary and then returned.</param>
        /// <returns>The existing or newly created value that belongs to the specific key.</returns>
        public static TValue GetOrCreateValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> creator)
        {
            TValue value;
            if (!dictionary.TryGetValue(key, out value))
            {
                value = creator(key);
                dictionary.Add(key, value);
            }
            return value;
        }
    }
}