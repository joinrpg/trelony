using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using JetBrains.Annotations;

namespace Joinrpg.Common.Helpers
{
    public static class DictionaryHelpers
    {
        [PublicAPI]
        public static TValue GetValueOrDefault<TKey, TValue>([NotNull] this IReadOnlyDictionary<TKey, TValue> data, TKey key)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            return data.ContainsKey(key) ? data[key] : default(TValue);
        }

        [PublicAPI]
        public static IReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(
            [NotNull] this IDictionary<TKey, TValue> dictionary)
            => new ReadOnlyDictionary<TKey, TValue>(dictionary);
    }
}
