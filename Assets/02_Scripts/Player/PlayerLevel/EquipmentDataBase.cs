using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentData",menuName = "Game/Equipment/EquipmentData")]
public class EquipmentDataBase : ScriptableObject
{
    public List<EquipmentData> equipmentDataList;
}
