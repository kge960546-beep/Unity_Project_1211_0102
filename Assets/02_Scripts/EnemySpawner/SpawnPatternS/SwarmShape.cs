using UnityEngine;

//박쥐 스폰 패턴 모양 
[CreateAssetMenu(menuName = "Game/Stage/SpawnShape/SwarmShape", fileName = "SwarmShape")]
public class SwarmShape : SpawnShapeSO
{
    public override Vector3[] GetSpawnPositions(SpawnContext context)
    {
        Vector3[] result = new Vector3[context.spawnCount];

        float startAngle = Random.Range(0f, 360f);
        float arcAngle = 90f; //부채꼴 각도
        float step = arcAngle / (context.spawnCount - 1);

        for(int i = 0; i< context.spawnCount; i++)
        {
            float angle = (startAngle + step * i) * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
            result[i] = context.playerPosition + dir * context.radius;
        }

        return result;
    }
}
