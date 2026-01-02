using UnityEngine;

[CreateAssetMenu(fileName = "bossDataList", menuName =("Game/Enemy/bossDataList SO"))]
public class BossData : bossData
{
    public string bossName;

    [Header("Barricade Setting")]
    public Transform barricadeCenter;
    public BarricadePatternSO barricadePattern;
}
