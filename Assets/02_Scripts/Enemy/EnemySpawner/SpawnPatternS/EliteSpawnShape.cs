using UnityEngine;

[CreateAssetMenu(fileName = "EliteSpawnShape", menuName = "Game/Stage/SpawnShape/EliteSpawnShape")]
public class EliteSpawnShape : SpawnShapeSO
{
    public float radius = 8f;

    public override Vector3[] GetSpawnPositions(SpawnContext context)
    {
        Vector3[] result = new Vector3[1];

        float angle = Random.Range(0f, 360f);
        Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.right;

        result[0] = context.playerPosition + dir * radius;

        return result;
    }
}
