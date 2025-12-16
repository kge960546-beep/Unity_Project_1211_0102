using UnityEngine;

public interface ISpawnShape
{
    Vector3[] GetSpawnPositions(SpawnContext context);
}
public struct SpawnContext
{
    public Vector3 playerPosition;
    public Vector3 playerVelocity;
    public float patternTime;
    public int spawnCount;
    public float radius;

    public float difficultyMultipiler;
}

