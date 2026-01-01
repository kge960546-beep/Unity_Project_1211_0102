using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Stage/SpawnShape/BatSwarmShape",fileName = "BatSwarmShape")]
public class BatSwarmShapeSO : SpawnShapeSO
{
    [Header("Swarm Direction")]
    public float angleRange = 60f;
    public float distance = 12f;
    public float width = 6f;

    // 2D 벡터의 수직 벡터를 구하는 헬퍼 함수 추가
    private static Vector3 Perpendicular(Vector3 v)
    {
        // z축 회전이므로 (x, y, z) -> (-y, x, z)
        return new Vector3(-v.y, v.x, v.z);
    }

    public override Vector3[] GetSpawnPositions(SpawnContext context)
    {
        Vector3[] result = new Vector3[context.spawnCount];

        Vector3 forward = 
            context.playerMoveDir.sqrMagnitude > 0.01f
            ? context.playerMoveDir.normalized
            : Random.insideUnitCircle.normalized;

        float halfAngle = angleRange * 0.5f;

        for (int i = 0; i < context.spawnCount; i++)
        {
            float t = context.spawnCount <= 1 ? 0.5f : (float)i / (context.spawnCount - 1);
            float angle = Mathf.Lerp(-halfAngle, halfAngle, t);

            Vector3 dir = Quaternion.Euler(0, 0, angle) * forward;

            float offset = Random.Range(-width * 0.5f, width * 0.5f);

            Vector3 pos =
                context.playerPosition
                + dir.normalized * distance
                + Perpendicular(dir) * offset;

            result[i] = pos;
        }
        return result;
    }
}
