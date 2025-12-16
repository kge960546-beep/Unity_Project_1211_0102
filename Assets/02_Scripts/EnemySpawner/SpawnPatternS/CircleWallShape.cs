using UnityEngine;

//원형 벽 스폰 패턴 모양
public class CircleWallShape : ISpawnShape
{
    public Vector3[] GetSpawnPositions(SpawnContext context)
    {
        Vector3[] result = new Vector3[context.spawnCount];
        float wallStep = 360f / context.spawnCount;

        for(int i = 0; i< context.spawnCount; i++)
        {
            float angle = wallStep * i * Mathf.Deg2Rad;   //Mathf.Deg2Rad : This is equal to (PI * 2) / 360
            float x = Mathf.Cos(angle) * context.radius;
            float y = Mathf.Sin(angle) * context.radius;
            result[i] = context.playerPosition + new Vector3(x, y, 0);
        }
        return result;
    }
}
