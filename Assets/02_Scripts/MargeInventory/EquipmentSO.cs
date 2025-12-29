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

    [Header("Identity")]
    public string equipmentName;
    public int equipmentID;

    [Header("Stats")]
    public int plusEquimentHP;
    public int plusEquimentAttack;
    public EquipmentPart partType;
    public Sprite itemSprite;

    [Header("SkillID")]
    public int skillId;

    [Header("Upgrade")]
    public GameObject[] upgradePrefabs = new GameObject[18];

    public GameObject GetUpgradePrefab(EquipmentClassType type, int step)
    {
        step = Mathf.Clamp(step, 0, 2);
        int index = ((int)type * 3) + step;

        if (upgradePrefabs == null) return null;
        if (index < 0 || index >= upgradePrefabs.Length) return null;

        return upgradePrefabs[index];
    }    
}
