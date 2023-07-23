using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public static class RandomExtensions
{
    public static T RandomPickOne<T>(this List<T> list, Random random = null)
    {
        var rng = random == null? new Random() : random;
        if (list.Count == 0)
        {
            throw new InvalidOperationException("Cannot select a random element from an empty list.");
        }

        return list[rng.Next(list.Count)];
    }

    public static T RandomPickOne<T>(this T[] array, Random random = null)
    {
        var rng = random == null? new Random() : random;
        if (array.Length == 0)
        {
            throw new InvalidOperationException("Cannot select a random element from an empty array.");
        }
        return array[rng.Next(array.Length)];
    }
}
