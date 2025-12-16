using UnityEngine;

//기본 스폰 패턴
public class CircleShape : ISpawnShape
{
    public Vector3[] GetSpawnPositions(SpawnContext context)
    {
        Vector3[] result = new Vector3[context.spawnCount];
        {
            for(int i =0; i< context.spawnCount; i++)
            {
                float angle = i * Mathf.PI * 2 / context.spawnCount;
                Vector3 dir = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
                result[i] = context.playerPosition + dir * context.radius;
            }
            return result;
        }
    }
}
