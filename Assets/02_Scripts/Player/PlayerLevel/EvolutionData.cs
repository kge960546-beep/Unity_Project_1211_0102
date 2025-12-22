using UnityEngine;

[CreateAssetMenu(fileName = "EvolutionData",menuName = "Game/Equipment/EvolutionData")]
public class EvolutionData : ScriptableObject
{
    public EquipmentData baseEquipment;
    public EquipmentData result;

    public string description;
}
