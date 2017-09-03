using UnityEngine;
using System;
using System.Collections.Generic;

public static class Extensions
{
    public static Vector2 ToVector2(this Vector3 v3)
    {
        return new Vector2(v3.x, v3.y);
    }

    public static double ToRadians(this double val)
    {
        return (Mathf.PI / 180) * val;
    }

    public static float ToRadians(this float val)
    {
        return (Mathf.PI / 180) * val;
    }

    public static T ToEnum<T>(this string value, bool ignoreCase = true)
    {
        return (T)Enum.Parse(typeof(T), value, ignoreCase);
    }

    public static List<T> SwapValue<T>(this List<T> list, int x, int y)
    {
        T _tmp = list[x];
        list[x] = list[y];
        list[y] = _tmp;
        return list;
    }

    public static bool IsNullOrEmpty<T>(this ICollection<T> genericCollection)
    {
        if (genericCollection == null)
        {
            return true;
        }
        return genericCollection.Count < 1;
    }

    public static T GetRandomValue<T>(this List<T> genericCollection)
    {
        if (genericCollection.IsNullOrEmpty()) return default(T);
        int rdm = UnityEngine.Random.Range(0, genericCollection.Count);
        return genericCollection[rdm];
    }
}

public static class StringExtensions
{
    public static string RemoveLineBreaks(this string lines)
    {
        string replacement = "";
        return lines.Replace("\r\n", replacement)
                    .Replace("\r", replacement)
                    .Replace("\n", replacement);
    }

    public static string RemoveLineBreaksWithSpace(this string lines)
    {
        string replacement = " ";
        return lines.Replace("\r\n", replacement)
                    .Replace("\r", replacement)
                    .Replace("\n", replacement);
    }

    public static string ReplaceLineBreaksWithString(this string lines, string replacement)
    {
        return lines.Replace("\r\n", replacement)
                    .Replace("\r", replacement)
                    .Replace("\n", replacement);
    }
}