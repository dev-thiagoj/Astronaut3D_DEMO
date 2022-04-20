using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class DevUtills
{
    public static T DevGetRandom<T>(this List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    public static T DevGetRandomButNotTheSame<T>(this List<T> list, T unique)
    {
        if (list.Count == 1) return unique;

        int randomIndex = Random.Range(0, list.Count);
        return list[randomIndex];
    }
}
