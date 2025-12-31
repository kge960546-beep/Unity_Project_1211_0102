using UnityEngine;

[CreateAssetMenu(fileName = "BossData", menuName =("Game/Enemy/BossData SO"))]
public class BossData : EnemyData
{
    [Header("Barricade Setting")]
    public Transform barricadeCenter;
    public BarricadePatternSO barricadePattern;
}
