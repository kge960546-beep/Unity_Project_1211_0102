using UnityEngine;

[CreateAssetMenu(fileName = "EvolutionData",menuName = "Game/Equipment/EvolutionData")]
public class EvolutionData : ScriptableObject
{
    [Header("Condition")]
    public EquipmentData baseEquipment;
    public int requiredLevel;

    [Header("Result")]
    public EquipmentData result;

    [TextArea]
    public string description;
}
