using UnityEngine;
[CreateAssetMenu(fileName = " BossBaseDataSo", menuName = "Monster/ Boss")]
public class BossBaseDataSO : ScriptableObject
{
    public string bossName;
    [Header("BaseAbility")]
    public int bossHp;
}