using UnityEngine;

public enum Rarity
{
    Common,
    Rare,
    Epic,
    Legandary
}

[System.Serializable]
public class EquipmentLevelData
{
    public int level;

    [TextArea]
    public string description;
}

[CreateAssetMenu(fileName = "EquipmentData", menuName ="Game/Skill/EquipmentData")]
public class EquipmentData : ScriptableObject
{
    public int id;

    [Header("UI")]
    public string name;
    public Sprite icon;

    [TextArea]
    public string description;

    [Header("Skill Binding")]
    public SkillDescriptor skill;

    [Header("Progression")]
    public int maxLevel = 6;
    public Rarity rarity;

    public EquipmentLevelData[] levels;
}