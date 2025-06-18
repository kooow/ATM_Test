using System;
using System.Collections.Generic;
using System.Linq;

namespace ATM_Test.Helpers;

/// <summary>
/// Extensions for Dictionary<TKey, TValue>
/// </summary>
public static class DictionaryExtensionsClass
{
    /// <summary>
    /// ToLogString extension method for Dictionary<TKey, TValue> to convert it to a string representation.
    /// </summary>
    /// <typeparam name="TKey">Tkey</typeparam>
    /// <typeparam name="TValue">TValue</typeparam>
    /// <param name="dictionary">Dictionary</param>
    /// <returns>Concated string</returns>
    public static string ToLogString<TKey, TValue>(this Dictionary<TKey, TValue> dictionary) where TKey : notnull
    {
        var dictionaryKeyAndValueRows = dictionary.Select(kvp => $"{kvp.Key}: {kvp.Value}");
        return string.Join(Environment.NewLine, dictionaryKeyAndValueRows);
    }
}
