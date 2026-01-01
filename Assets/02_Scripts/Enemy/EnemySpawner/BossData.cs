using UnityEngine;

[CreateAssetMenu(fileName = "bossData", menuName =("Game/Enemy/bossData SO"))]
public class BossData : EnemyData
{
    [Header("Barricade Setting")]
    public Transform barricadeCenter;
    public BarricadePatternSO barricadePattern;
}
