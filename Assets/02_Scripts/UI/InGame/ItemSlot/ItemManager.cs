using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    public List<ItemData> itemDatas = new List<ItemData>();
    
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void GetItem(string name, Sprite sprite, int amount, ItemClass itemClass)
    {
        ItemData existingItem = itemDatas.Find(x => x.itemName == name);

        if(existingItem != null)
        {
            existingItem.count += amount;
        }
        else
        {
            ItemData newItem = new ItemData();
            newItem.itemName = name;
            newItem.icon = sprite;
            newItem.count = amount;
            newItem.itemClass = itemClass;

            itemDatas.Add(newItem);
        }
#if UNITY_EDITOR
        Debug.Log($"아이템 획득: 이름: {name} 개수: {amount}");
        Debug.Log($"[장바구니] {name} 획득! 현재 총 아이템 종류: {itemDatas.Count}개");
#endif
    }
    public List<ItemData> GetResultItems()
    {
        return itemDatas;
    }
}
