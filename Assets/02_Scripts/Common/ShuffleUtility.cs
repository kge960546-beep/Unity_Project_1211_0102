using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ShuffleUtility
{
    public static List<T> FisherYatesShuffle<T>(List<T> source, int count)
    {
        RandomService rs = GameManager.Instance.GetService<RandomService>();
        List<T> result = new List<T>(source);
        for (int i = result.Count - 1; i > 0; i--)
        {
            // NextInt(int max) picks exclusive range from [0, max)
            // see https://docs.unity3d.com/Packages/com.unity.mathematics@1.3/api/Unity.Mathematics.Random.NextInt.html#Unity_Mathematics_Random_NextInt_System_Int32_
            int j = rs.random.NextInt(i + 1);
            (result[i], result[j]) = (result[j], result[i]);
        }
        return result.GetRange(0, Mathf.Min(result.Count, count));
    }
}
