using UnityEngine;

[CreateAssetMenu(fileName = "BossSpawnShape", menuName = "Game/Stage/SpawnShape/BossSpawnShape")]
public class BossSpawnShapeSO : SpawnShapeSO
{
    [SerializeField] private float verticalOffset = 8f; //플레이어 상부 거리
    [SerializeField] private float horizontalRange = 10f;  //좌우 랜덤 범위
    public override Vector3[] GetSpawnPositions(SpawnContext context)
    {
        Vector3[] result = new Vector3[1];

        //좌우 랜덤 값
        float randomX = Random.Range(-horizontalRange, horizontalRange);

        //항상 플레이어 위쪽
        Vector3 spawnPos = context.playerPosition;
        spawnPos.x += randomX;
        spawnPos.y += verticalOffset;

        result[0] = spawnPos;
        return result;
    }
}
