using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SlotData
{
    public EquipmentSO.EquipmentPart partType;
    public bool isEmpty = true;
    public GameObject slotObj;

    public string equippedUid;
    public EquipmentItem view;
}
public class EquipmentInventory : MonoBehaviour
{
    [SerializeField] private GameObject inventoryRoot;
    public enum SortMode { Parts, Class }
    [SerializeField] private SortMode sortMode;

    [SerializeField] private Transform lobbyGrid;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private TextMeshProUGUI sortModeText;


    [SerializeField] private List<SlotData> slots = new List<SlotData>();    
    [SerializeField] private RectTransform[] slotAnchors;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private EquipmentInfoPanel infoPanel;

    private bool isHideLobbyList = true;

    private Coroutine refreshCo;

    private event Action OnEquipmentChange;
    private void Start()
    {
        int partCount = Enum.GetValues(typeof(EquipmentSO.EquipmentPart)).Length;
        if (slotAnchors == null || slotAnchors.Length != partCount) return;

        slots.Clear();        

        for (int i = 0; i < partCount; i++)
        {
            var part = (EquipmentSO.EquipmentPart)i;
            var anchor = slotAnchors[i];                        

            var view = anchor.GetComponentInChildren<EquipmentItem>(true);
            if (view == null) continue;
           
            var capturedPart = part;
            view.SetOnClickAction(() => OnClickEquipSlot(capturedPart));
            view.ClearEquipmentSlot();

            slots.Add(new SlotData
            {
                partType = part,
                slotObj = anchor.gameObject,
                view = view,
                isEmpty = true,
                equippedUid = null
            });
        }
        RestoreSavedEquipment();
        
        inventoryRoot.SetActive(false);        
    }
    public void SubscribeOnEquipmentChange(Action action)
    {
        OnEquipmentChange -= action;
        OnEquipmentChange += action;
    }
    public void UnsubscribeOnEquipmentChange(Action action)
    {
        OnEquipmentChange -= action;
    }
    private void IsChangeAction()
    {
        OnEquipmentChange?.Invoke();
    }
    private void OnEnable()
    {
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.SubscribeOnInventoryChanged(RefreshLobbyUI);
        RefreshLobbyUI();

    }
    private void OnDisable()
    {
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.UnsubscribeOnInventoryChanged(RefreshLobbyUI);

        if (refreshCo != null)
        {
            StopCoroutine(refreshCo);
            refreshCo = null;
        }
    }

    //장착슬롯 찾기
    public SlotData FindSlot(EquipmentSO.EquipmentPart part)
    {
        for(int i = 0; i < slots.Count; i++)
        {
            var sl = slots[i];
            if (sl == null) continue;
            if (sl.partType == part) return sl;
        }
        return null;
    }
    //Uid 가지고있기
    public string GetInventoryEquipmentUid(EquipmentSO.EquipmentPart part)
    {
        var slot = FindSlot(part);
        if(slot == null) return null;
        if(slot.isEmpty) return null;

        return slot.equippedUid;
    }

    //장비창 UI 갱신
    public void RefreshLobbyUI()
    {
        if (lobbyGrid == null) return;       
        if(InventoryManager.Instance == null) return;

        var myItems = InventoryManager.Instance.GetInventoryList();

        var sorted = (sortMode == SortMode.Parts) ? InventorySortComparer.SortByPart(myItems) :
                                                    InventorySortComparer. SortDescendingOrderByRank(myItems);

        List<InventoryItemData> visible = new List<InventoryItemData>();

        int equipmentFilter = 0;

        for(int i = 0; i< sorted.Count; i++)
        {
            var d = sorted[i];
            if (d == null) return;

            if (isHideLobbyList && IsEquippedUid(d.uid)) 
            {
                equipmentFilter++;
                continue;
            }
            
            visible.Add(d);
        }

        int dataCount = visible.Count;
        int currentSlotCount = lobbyGrid.childCount;

        for(int i = 0; i<dataCount; i++)
        {
            EquipmentItem EItem = null;

            if(i < currentSlotCount)
            {
                Transform child = lobbyGrid.GetChild(i);
                child.gameObject.SetActive(true);
                EItem = child.GetComponent<EquipmentItem>();
            }
            else
            {
                GameObject newSlot = Instantiate(itemPrefab, lobbyGrid);
                EItem = newSlot.GetComponent<EquipmentItem>();
            }

            if(EItem != null)
            {
                var data = visible[i];
                EItem.Initialize(data.scriptableObjectData, data.classType, data.step);
                EItem.BindInventory(data.uid);

                var slotItem = EItem;
                EItem.SetOnClickAction(() => OnClickInventoryItem(slotItem));
            }            
        }

        for(int i = dataCount; i < currentSlotCount; i++)
        {
            lobbyGrid.GetChild(i).gameObject.SetActive(false);
        }

        if(refreshCo != null)
        {            
            StopCoroutine(refreshCo);
        }
        if (inventoryRoot.activeSelf == false) return;

        if (inventoryRoot != null && inventoryRoot.activeSelf == false)
            return;
        
        refreshCo = StartCoroutine(RefreshTiming());
    }
    //저장된장비불러오기
    private void RestoreSavedEquipment()
    {
        if (InventoryManager.Instance == null) return;

        for(int i = 0; i< slots.Count; i++)
        {
            var slot = slots[i];
            if (slot == null || slot.view == null) continue;
            
            string uid = InventoryManager.Instance.GetSaveEquipmentUid(slot.partType);            

            if (string.IsNullOrEmpty(uid)) continue;

            var data = InventoryManager.Instance.FindUid(uid);
            if(data == null || data.scriptableObjectData == null) continue;

            slot.view.Initialize(data.scriptableObjectData, data.classType, data.step);
            slot.view.BindInventory(uid);

            slot.isEmpty = false;
            slot.equippedUid = uid;
        }
        RefreshLobbyUI();
    }
    //장착된 장비 가지고있기
    public void GetEquipped(EquipmentItem item)
    {        
        if(item == null || item.Data == null) return;
        EquipmentSO so = item.Data;
        string uid = item.inventoryUid;
        if (string.IsNullOrEmpty(uid)) return;
         

        var slot = FindSlot(so.partType);
        if (slot == null || slot.view == null)
        {
            return;
        }
        
        if (!slot.isEmpty && slot.equippedUid == uid)
        {
            Unequipment(so.partType);
            return;
        }
        
        slot.view.Initialize(so, item.ClassType, item.Step);
        slot.view.BindInventory(uid);

        slot.isEmpty = false;
        slot.equippedUid = uid;

        RefreshLobbyUI();
    }
    //이건 장착한건지 판단
    public bool IsEquippedUid(string uid)
    {
        if (string.IsNullOrEmpty(uid)) return false;
        for (int i = 0; i < slots.Count; i++)
            if (!slots[i].isEmpty && slots[i].equippedUid == uid) return true;
        return false;
    }
    //장비창 새로고침 실행순서 조절용
    IEnumerator RefreshTiming()
    {
        if (lobbyGrid == null || itemPrefab == null) yield break;
        if(InventoryManager.Instance == null) yield break;

        yield return new WaitForEndOfFrame();

        Canvas.ForceUpdateCanvases();

        var gridRect = lobbyGrid as RectTransform;
        if(gridRect != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(gridRect);
        }

        if(lobbyGrid.parent != null)
        {
            var parentRect = lobbyGrid.parent as RectTransform;
            if (parentRect != null)
                LayoutRebuilder.ForceRebuildLayoutImmediate(parentRect);
        }
        Canvas.ForceUpdateCanvases();        
    }

    //아이템 장착
    public void EquipFromInfo(EquipmentItem item)
    {
        Debug.Log($"[EQUIP_FROM_INFO] item.inventoryUid={item?.inventoryUid} so={item?.Data?.name} soClass={item?.Data?.classType}");
        var inv = InventoryManager.Instance.FindUid(item.inventoryUid);
        Debug.Log($"[EQUIP_FROM_INFO] invClass={inv?.classType} invStep={inv?.step}");

        GetEquipped(item);

        InventoryManager.Instance.SetEquippedUid(item.Data.partType, item.inventoryUid);
        IsChangeAction();
    }

    //아이템 해제
    public void Unequipment(EquipmentSO.EquipmentPart part)
    {
        var slot = FindSlot(part);
        if (slot.view == null) return;
        if (slot.isEmpty) return;

        slot.view.ClearEquipmentSlot();
        slot.isEmpty = true;
        slot.equippedUid = null;

        InventoryManager.Instance.SetEquippedUid(part,null);

        RefreshLobbyUI();
        IsChangeAction();
    }

    //장비창 슬롯 아이템 클릭
    public void OnClickInventoryItem(EquipmentItem item)
    {
        if (item == null) return;
        if (infoPanel != null) infoPanel.ShowFromInventory(item);
    }

    //아이템 정보창
    public void OnClickEquipSlot(EquipmentSO.EquipmentPart part)
    {        
        var slot = FindSlot(part);
        if (slot == null || slot.view == null) return;
        if (slot.isEmpty || slot.view.Data == null) return;

        if (infoPanel != null)
            infoPanel.ShowSlot(slot.view);
    }
    //장착 아이템 Uid 갱신 인벤토리 기준으로
    public void EquipFromUid(string uid)
    {
        if (InventoryManager.Instance == null) return;
        if (string.IsNullOrEmpty(uid)) return;

        var inv = InventoryManager.Instance.FindUid(uid);
        if (inv == null || inv.scriptableObjectData == null) return;

        var part = inv.scriptableObjectData.partType;
        
        var slot = FindSlot(part);
        if (slot != null && slot.view != null)
        {
            slot.view.Initialize(inv.scriptableObjectData, inv.classType, inv.step);
            slot.view.BindInventory(uid);

            slot.isEmpty = false;
            slot.equippedUid = uid;
        }
        
        InventoryManager.Instance.SetEquippedUid(part, uid);

        RefreshLobbyUI();
        IsChangeAction();
    }

    //정렬
    public void ToggleSort()
    {
        sortMode = (sortMode == SortMode.Parts) ? SortMode.Class : SortMode.Parts;
        if(sortModeText != null)
        {
            sortModeText.text = (sortMode == SortMode.Parts) ? "부위별" : "등급별";
        }
        RefreshLobbyUI();
    }    
}