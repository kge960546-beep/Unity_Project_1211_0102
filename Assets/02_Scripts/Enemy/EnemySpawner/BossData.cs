using UnityEngine;

[CreateAssetMenu(fileName = "BossData", menuName =("Game/Enemy/BossData SO"))]
public class BossData : EnemyData
{
    public SpawnShapeSO shape;
    public SpawnContext context;

    public Transform barricadeCenter;
}
