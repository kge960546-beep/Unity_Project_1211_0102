using UnityEngine;
using System;
using UnityEditor;


public class SpawnPoint : MonoBehaviour
{
    public static SpawnPoint Instance;

    public Transform[] spawnPoints;

    void Awake()
    {
        Instance = this;
    }

    public Vector3 GetSpawnPoint(int id)
    {
        return spawnPoints[id].position;
    }
}
