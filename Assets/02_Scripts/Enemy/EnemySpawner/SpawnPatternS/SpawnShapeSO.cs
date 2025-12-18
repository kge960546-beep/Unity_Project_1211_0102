using UnityEngine;

public abstract class SpawnShapeSO : ScriptableObject
{
    public abstract Vector3[] GetSpawnPositions(SpawnContext context);
}
