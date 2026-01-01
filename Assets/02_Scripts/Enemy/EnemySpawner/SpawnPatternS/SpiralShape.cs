using UnityEngine;

[CreateAssetMenu(menuName = "Game/Stage/SpawnShape/SpiralShape",fileName = "SpiralShape")]
public class SpiralShape : SpawnShapeSO
{
    public float rotateSpeed = 2f;
    public float spiralSpeed = 1f;

    public override Vector3[] GetSpawnPositions(SpawnContext context)
    {
        Vector3[] result = new Vector3[context.spawnCount];

        for(int i = 0; i< context.spawnCount; i++)
        {
            float angle = context.patternTime * rotateSpeed + i * 0.5f;
            //반지름이 점점 증가하도록 변경
            float radius = context.radius + (context.patternTime + i) * spiralSpeed;

            Vector3 dir = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
            result[i] = context.playerPosition + dir * context.radius;
        }
        return result;
    }
}
