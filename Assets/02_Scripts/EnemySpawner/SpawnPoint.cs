using UnityEngine;
using System;
using UnityEditor;


public class SpawnPoint : MonoBehaviour
{
    public static SpawnPoint Instance;

    void Awake()
    {
        Instance = this;
    }

    public Vector3 GetSpawnPoint()
    {
        float radius = 12f;
        Vector3 playerPos = transform.position;

        float a = playerPos.x;
        float b = playerPos.y;

        float x = UnityEngine.Random.Range(-radius + a, radius + a);
        float y_b = Mathf.Sqrt(Mathf.Pow(radius, 2) - Mathf.Pow(x - a, 2));
        y_b *= UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;
        float y = y_b + b;


        Vector3 randomPostion = new Vector3(x, y, 0);

        return randomPostion;
    }
}
