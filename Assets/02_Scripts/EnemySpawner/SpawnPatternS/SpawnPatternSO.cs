using UnityEngine;

[CreateAssetMenu(menuName = "Game/Stage/SpawnPattern/SpawnPattern SO",fileName = "SpawnPattern")]
public class SpawnPatternSO : ScriptableObject
{
    [Header("Spawn Type")]
    public EnemyData enemyData;
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
