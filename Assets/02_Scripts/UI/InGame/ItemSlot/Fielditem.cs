using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fielditem : MonoBehaviour
{
    public enum ItemType {field,RandomBox }

    [Header("아이템 설정")]
    public ItemType type = ItemType.field;
    public ItemClass itemClass = ItemClass.Normal;

    [Header("필드 아이템 정보")]
    public string itemName;
    public Sprite itemIcon;
    public int amount;

    public List<ItemData> randomItems;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (ItemManager.instance == null) return;

        if(type == ItemType.field)
        {
            ItemManager.instance.GetItem(itemName, itemIcon, amount, itemClass);
        }
        else if(type == ItemType.RandomBox)
        {
            if(randomItems.Count > 0)
            {
                int randomIndex = Random.Range(0, randomItems.Count);
                ItemData selectItem = randomItems[randomIndex];

                ItemManager.instance.GetItem(selectItem.itemName, selectItem.icon, 1, itemClass);
            }
        }
        Destroy(gameObject);
    }
}