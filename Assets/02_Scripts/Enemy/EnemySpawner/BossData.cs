using UnityEngine;

[CreateAssetMenu(fileName = "bossDataList", menuName =("Game/Enemy/bossDataList SO"))]
public class BossData : EnemyData
{
    [Header("Barricade Setting")]
    public Transform barricadeCenter;
    public BarricadePatternSO barricadePattern;
}
