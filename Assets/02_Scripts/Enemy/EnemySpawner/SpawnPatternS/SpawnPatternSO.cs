using UnityEngine;

public enum SpawnTargetType
{
    Normal,
    Elite,
    Boss
}

[CreateAssetMenu(menuName = "Game/Stage/SpawnPattern/SpawnPattern SO",fileName = "SpawnPattern")]
public class SpawnPatternSO : ScriptableObject
{
    [Header("Spawn Type")]
    public bossData enemyData;
    public SpawnTargetType targetType;
    public SpawnShapeSO shape;

    [Header("Spawn Timer")]
    public float startTime;
    public float endTime;
    public float tick;

    [Header("Spawn Info")]
    public float radius;
    public int spawnCount;

    [Header("control")]
    public bool isAllowParallel;
}
