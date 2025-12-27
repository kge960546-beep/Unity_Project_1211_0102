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

    [SerializeField] private string sceneLoad = "Inventory_Grid";

    [SerializeField] private Transform slotRoot;
    [SerializeField] private GameObject itemPrefab;
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
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;        
    }
    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        GameObject grid = GameObject.Find(sceneLoad);

        if(grid != null)
        {
            slotRoot = grid.transform;

            UpdateSlot();
        }
        else
        {
            slotRoot = null;
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