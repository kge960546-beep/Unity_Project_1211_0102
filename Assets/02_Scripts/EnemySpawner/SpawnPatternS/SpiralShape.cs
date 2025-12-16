using UnityEngine;

public class SpiralShape : ISpawnShape
{
    public float rotateSpeed = 2f;

    public Vector3[] GetSpawnPositions(SpawnContext context)
    {
        Vector3[] result = new Vector3[context.spawnCount];

        for(int i = 0; i< context.spawnCount; i++)
        {
            float angle = context.patternTime * rotateSpeed + i * 0.5f;
            Vector3 dir = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
            result[i] = context.playerPosition + dir * context.radius;
        }
        return result;
    }
}
