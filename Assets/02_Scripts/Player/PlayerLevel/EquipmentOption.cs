using UnityEngine;

public enum EquipmentOptionType
{ 
    Upgrade,
    Evolution
}

public class EquipmentOption
{
    public EquipmentOptionType type;
    public EquipmentData equipment;
    public EvolutionData evolution;

    public static EquipmentOption Upgrade(EquipmentData data)
    {
        return new EquipmentOption
        {
            type = EquipmentOptionType.Upgrade,
            equipment = data
        };
    }

    public static EquipmentOption Evolution(EvolutionData evo)
    {
        return new EquipmentOption
        {
            type = EquipmentOptionType.Evolution,
            equipment = evo.result,
            evolution = evo
        };
    }
}
