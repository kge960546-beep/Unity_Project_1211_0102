using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSO : ScriptableObject
{
    public enum equipmentClassType
    {
        Normal,    // È¸»ö
        Good,      // ÃÊ·Ï
        Better,    // ÆÄ¶û
        Excellent, // º¸¶ó
        Epic,      // ³ë¶û
        Legend     // »¡°­
    }

    public int equipmentID;
    public int ClassColorID;

    public Color ClassColor;

    public int plusEquimentHP;
    public int plusEquimentAttack;

    
}
