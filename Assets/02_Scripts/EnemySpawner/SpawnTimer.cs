using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawn Timer SO", menuName = "Game/Enemy/EnemySpawnTimer")]
public class SpawnTimer : ScriptableObject
{
    public float playTime;

    void FixedUpdate()
    {
        playTime += Time.fixedDeltaTime;
    }
}


