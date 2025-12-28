using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class InventoryItemData
{
    public string uid;
    public EquipmentSO soData;
    public EquipmentSO.EquipmentClassType classType;
    public int step;
}
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;    

    [SerializeField] private List<InventoryItemData> inventoryItems = new List<InventoryItemData>();

    public List<EquipmentSO> acquisitionItemsInStage = new List<EquipmentSO>();

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
        newData.uid = System.Guid.NewGuid().ToString();
        newData.soData = itemData;
        newData.classType = EquipmentSO.EquipmentClassType.Normal;
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
        Debug.Log($"{itemData.equipmentName} 획득");
#endif
    }
    public InventoryItemData FindUid(string uid) => inventoryItems.Find(x => x.uid == uid);
    public void RemoveUid(string uid) => inventoryItems.RemoveAll(x => x.uid == uid);    
    public void ClearStageData()
    {
        acquisitionItemsInStage.Clear();
    }
}