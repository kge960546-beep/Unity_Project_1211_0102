using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItemData
{
    public EquipmentSO soData;
    public EquipmentSO.EquipmentClassType type;
    public int step;
}
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [SerializeField] private Transform slotRoot;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private List<InventoryItemData> inventoryItems = new List<InventoryItemData>();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    public void AddItem(EquipmentSO itemData)
    {
        InventoryItemData newData = new InventoryItemData();
        newData.soData = itemData;
        newData.type = EquipmentSO.EquipmentClassType.Normal;
        newData.step = 0;

        inventoryItems.Add(newData);

        if(slotRoot != null)
            UpdateSlot();
    }
    public void UpdateSlot()
    {
        if (slotRoot == null) return;

        foreach(Transform child in slotRoot)
        {
            Destroy(child.gameObject);
        }

        foreach(InventoryItemData data in inventoryItems)
        {
            GameObject newSlot = Instantiate(itemPrefab, slotRoot);

            EquipmentItem item = newSlot.GetComponent<EquipmentItem>();

            if(item != null)
            {
                item.Initialize(data.soData, data.type, data.step);
            }
        }
    }
}
