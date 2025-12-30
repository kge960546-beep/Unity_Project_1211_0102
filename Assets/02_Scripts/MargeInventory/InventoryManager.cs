using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class InventoryItemData
{
    public string uid;
    public EquipmentSO scriptableObjectData;
    public EquipmentSO.EquipmentClassType classType;
    public int step;
}
[System.Serializable]
public class ItemSaveData
{
    public string uid;
    public int scriptableObjectData;
    public int classType;
    public int step;
}
[System.Serializable]
public class InventorySaveFile
{
    public List<ItemSaveData> saveItems = new List<ItemSaveData>();
}
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    private event Action onInventoryChanged;

    public List<EquipmentSO> acquisitionItemsInStage = new List<EquipmentSO>();
    [SerializeField] private List<InventoryItemData> inventoryItems = new List<InventoryItemData>();

    public List<EquipmentSO> allItems;
    private Dictionary<int, EquipmentSO> itemById;

    [Header("Test")]
    public EquipmentSO debugWeapon;
    public EquipmentSO debugArmor;
    [ContextMenu("테스트: 기본아이템 10개 획득")]
    public void TestAddRandomitems()
    {
        if (debugWeapon == null)
        {
            Debug.LogError("인스펙터에서 Debug Weapon에 SO 파일을 연결해주세요!");
            return;
        }

        // 쿠나이 5개 추가 (합성 테스트용)
        for (int i = 0; i < 5; i++)
        {
            AddItem(debugWeapon);
        }

        // 갑옷 5개 추가
        if (debugArmor != null)
        {
            for (int i = 0; i < 5; i++) AddItem(debugArmor);
        }

        Debug.Log("테스트 아이템 10개가 인벤토리에 추가되었습니다.");
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            ConversionItemData();
            LoadInventory();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    private void OnApplicationQuit()
    {
        SaveInventory();
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
            SaveInventory();
    }
    private void OnApplicationFocus(bool focus)
    {
        if(!focus)
            SaveInventory();
    }
    private void InventoryChanged()
    {
        onInventoryChanged?.Invoke();
    }
    public void SubscribeOnInventoryChanged(Action action)
    {
        onInventoryChanged += action;
    }
    public void UnsubscribeOnInventoryChanged(Action action)
    {
        onInventoryChanged -= action;
    }
    public void ConversionItemData()
    {
        if(allItems == null || allItems.Count == 0)
        {
            itemById = null;
            return;
        }

        itemById = new Dictionary<int, EquipmentSO>(allItems.Count);

        foreach(var item in allItems)
        {
            if(item == null) continue;

            if (itemById.ContainsKey(item.equipmentID)) continue;
            itemById.Add(item.equipmentID, item);
        }
    }
    public void AddItem(EquipmentSO itemData)
    {
        InventoryItemData newData = new InventoryItemData();
        newData.uid = System.Guid.NewGuid().ToString();
        newData.scriptableObjectData = itemData;
        //newData.classType = EquipmentSO.EquipmentClassType.Normal;
        newData.classType = itemData.classType;
        newData.step = 0;

        inventoryItems.Add(newData);
        InventoryChanged();
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
        Debug.Log($"{itemData.equipmentName} 획득");
#endif
    }
    public InventoryItemData FindUid(string uid) => inventoryItems.Find(x => x.uid == uid);
    public bool RemoveUid(string uid, bool mergeEvent = true)
    {
        int remove = inventoryItems.RemoveAll(x => x.uid == uid);
        if (remove > 0 && mergeEvent)
            InventoryChanged();
        return remove > 0;
    }
    public bool UpdateUid(string uid, EquipmentSO.EquipmentClassType classType, int step)
    {
        var data = FindUid(uid);
        if (data == null) return false;

        data.classType = classType;
        data.step = step;

        InventoryChanged();
        return true;
    }
    public void SaveInventory()
    {
        InventorySaveFile basket = new InventorySaveFile();

        foreach(var item in inventoryItems)
        {
            if (item.scriptableObjectData == null) continue;

            ItemSaveData saveData = new ItemSaveData();
            saveData.uid = item.uid;
            saveData.scriptableObjectData = item.scriptableObjectData.equipmentID;
            saveData.classType = (int)item.classType;
            saveData.step = item.step;

            
            basket.saveItems.Add(saveData);
        }

        string json = JsonUtility.ToJson(basket);
        GoldService.SaveEncryptedData("myEquipment", json);
        Debug.Log("Json 저장완료");
    }
    public void LoadInventory()
    {
        if (itemById == null || itemById.Count == 0) return;

        string json = GoldService.LoadEncryptedData("myEquipment");

        if (string.IsNullOrEmpty(json)) return;

        InventorySaveFile basket = JsonUtility.FromJson<InventorySaveFile>(json);

        if (basket == null) return;

        inventoryItems.Clear();

        foreach(var item in basket.saveItems)
        {
            EquipmentSO originalSO = null;

            itemById.TryGetValue(item.scriptableObjectData,  out originalSO);

            if(originalSO != null)
            {
                InventoryItemData loadItem = new InventoryItemData();
                loadItem.uid = item.uid;
                loadItem.scriptableObjectData = originalSO;
                loadItem.classType = (EquipmentSO.EquipmentClassType)item.classType;
                loadItem.step = item.step;

                inventoryItems.Add(loadItem);
            }
            else
            {
                Debug.Log("없다 찾아라 세상끝에 두고 왔으니");
            }
        }
        InventoryChanged();
    }
    public void ClearStageData()
    {
        acquisitionItemsInStage.Clear();
        InventoryChanged();
    }
}