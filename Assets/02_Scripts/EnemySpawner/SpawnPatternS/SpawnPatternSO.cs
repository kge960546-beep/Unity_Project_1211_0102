using UnityEngine;

[CreateAssetMenu(menuName = "Game/Stage/SpawnPattern/SpawnPattern SO")]
public abstract class SpawnPatternSO : ScriptableObject
{
    public int enemyID;
    public SpawnShapeSO shape;

    [Header("SpawnTimer")]
    public float startTime;
    public float endTime;
    public float tick;

    public float radius;
    public int spawnCount;

    public bool isAllowParallel;
    public abstract ISpawnShape CreateShape();
}
