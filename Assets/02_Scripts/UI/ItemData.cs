using UnityEngine;

[CreateAssetMenu(menuName = "Game/Item/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("Main Item Info")]
    public Sprite itemIcon;
    public int skillID;
}
