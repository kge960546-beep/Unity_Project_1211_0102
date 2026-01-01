using UnityEngine;

[CreateAssetMenu(fileName = "PlayerPressureShape", menuName = "Game/Stage/SpawnShape/PlayerPressureShape")]
public class PlayerPressureShape : SpawnShapeSO
{
    public float forwardDistance = 8f;
    public float lateralSpread = 5f;

    public float angleSpread = 30f;
    public override Vector3[] GetSpawnPositions(SpawnContext context)
    {
        Vector3[] result = new Vector3[context.spawnCount];

        //플레이어 이동 방향
        Vector3 dir = context.playerMoveDir;
        if (dir == Vector3.zero)
            dir = Vector3.right;

        //직각 벡터 (2D 기준)
        Vector3 perp = new Vector3(-dir.y, dir.x, 0);

        for(int i = 0; i < context.spawnCount; ++i)
        {
            float t = context.spawnCount <= i ? 0.5f : (float)i / (context.spawnCount - 1);

            //좌우로 퍼짐
            float sideoffset = Mathf.Lerp(-lateralSpread, lateralSpread, t);

            //기본 위치 = 플레이어 앞
            Vector3 basePos = context.playerPosition + dir.normalized * forwardDistance + perp.normalized * sideoffset;

            //압박 느낌을 위한 약간의 랜덤 각도
            float angle = Random.Range(-angleSpread, angleSpread);
            Vector3 rotated = Quaternion.Euler(0, 0, angle) * dir;

            result[i] = basePos + rotated * 0.5f;
        }

        return result;
    }
}
