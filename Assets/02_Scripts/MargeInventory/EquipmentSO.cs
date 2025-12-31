using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentSO", menuName = "Game/Equipment/EquipmentSO")]
public class EquipmentSO : ScriptableObject
{
    public enum EquipmentClassType
    {
        Normal = 0,    // È¸»ö
        Good = 1,      // ÃÊ·Ï
        Better = 2,    // ÆÄ¶û
        Excellent = 3, // º¸¶ó
        Epic = 4,      // ³ë¶û
        Legend = 5     // »¡°­
    }
    public enum EquipmentPart
    {
        Weapon,
        Armor,
        Necklace,
        Belt,
        Gloves,
        Shoes
    }
    public enum PercentTarget
    {
        Hp,
        Attack
    }

    [Header("Identity")]
    public string equipmentName;
    public int equipmentID;
    public EquipmentClassType classType;

    [Header("Stats")]
    public int plusEquimentHP;
    public int plusEquimentAttack;
    public EquipmentPart partType;
    public Sprite itemSprite;
    public int weight;    

    [Header("SkillID")]
    public int skillId;

    [Header("Upgrade")]
    [SerializeField] private PercentTarget percentTargetStat = PercentTarget.Attack;
    public GameObject[] upgradePrefabs = new GameObject[18];
    [SerializeField] private float[] precentUpgradeStep = new float[18];

    public int GetUpgradeStep(EquipmentClassType type, int step)
    {
        if(type <= EquipmentClassType.Better)
        {
            step = 0;
        }

        step = Mathf.Clamp(step, 0, 2);

        return ((int)type * 3) + step;
    }
    public GameObject GetUpgradePrefab(EquipmentClassType type, int step)
    {        
        int index = GetUpgradeStep(type, step);

        if (upgradePrefabs == null) return null;
        if (index < 0 || index >= upgradePrefabs.Length) return null;

        return upgradePrefabs[index];
    }
    private float GetPercent(EquipmentClassType type, int step)
    {
        int index = GetUpgradeStep(type, step);

        if (precentUpgradeStep == null || precentUpgradeStep.Length < 18)
            return 0;

        if (index < 0 || index >= precentUpgradeStep.Length)
            return 0;

        return precentUpgradeStep[index];        
    }
    public void GetFinalStats(EquipmentClassType type, int step, out int finalHp, out int fianAttack)
    {
        float per = GetPercent(type, step);

        if(percentTargetStat == PercentTarget.Hp)
        {
            finalHp = Mathf.RoundToInt(plusEquimentHP * (1.0f + per));
            fianAttack = plusEquimentAttack;
        }
        else
        {
            finalHp = plusEquimentHP;
            fianAttack = Mathf.RoundToInt(plusEquimentAttack * (1.0f + per));
        }
    }
}