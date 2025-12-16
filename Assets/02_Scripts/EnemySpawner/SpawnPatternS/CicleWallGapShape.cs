using UnityEngine;

public class CicleWallGapShape : ISpawnShape
{
    [Range(0f, 1f)] public float gapRatio = 0.2f;

    public Vector3[] GetSpawnPositions(SpawnContext context)
    {
        int gapCount = Mathf.RoundToInt(context.spawnCount *  gapRatio);
        int realCount = context.spawnCount - gapCount;

        Vector3[] result = new Vector3[realCount];
        int index = 0;

        for(int i=0; i < context.spawnCount; i++)
        {
            if (i < gapCount) continue;

            float angle = i * Mathf.PI * 2 / context.spawnCount;
            Vector3 dir = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
            result[index++] = context.playerPosition + dir * context.radius;
        }
        return result;
    }
}
