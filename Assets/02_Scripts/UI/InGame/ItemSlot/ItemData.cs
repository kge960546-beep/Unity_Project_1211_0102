using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public enum ItemClass 
{
    Normal,    // È¸»ö
    Good,      // ÃÊ·Ï
    Better,    // ÆÄ¶û
    Excellent, // º¸¶ó
    Epic,      // ³ë¶û
    Legend     // »¡°­
}
public class ItemData
{
    public string itemName;
    public Sprite icon;
    public int count;
    public ItemClass itemClass;
}
