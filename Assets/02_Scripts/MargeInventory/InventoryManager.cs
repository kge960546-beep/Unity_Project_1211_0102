using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [SerializeField] private List<InventoryItemData> inventoryItems = new List<InventoryItemData>();

    public List<EquipmentSO> acquisitionItemsInStage = new List<EquipmentSO>();

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
    }
    public List<InventoryItemData> GetInventoryList()
    {
        return inventoryItems;
    }   
    public void AcquisitionItem(EquipmentSO itemData)
    {
        if (itemData == null) return;

        AddItem(itemData);        
        acquisitionItemsInStage.Add(itemData);

#if UNITY_EDITOR
        Debug.Log($"{itemData.equipmentName} È¹µæ");
#endif
    }
    public void ClearStageData()
    {
        acquisitionItemsInStage.Clear();
    }
}