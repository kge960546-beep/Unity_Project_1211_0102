using UnityEngine;

[CreateAssetMenu(menuName = "Game/Stage/SpawnShape/SpiralShape",fileName = "SpiralShape")]
public class SpiralShape : SpawnShapeSO
{
    public float rotateSpeed = 2f;

    public override Vector3[] GetSpawnPositions(SpawnContext context)
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
