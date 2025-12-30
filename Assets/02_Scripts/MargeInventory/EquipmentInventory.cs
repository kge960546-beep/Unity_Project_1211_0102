using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentInventory : MonoBehaviour
{
    public enum SortMode { Parts, Class }
    [SerializeField] private SortMode sortMode;

    [SerializeField] private Transform lobbyGrid;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private TextMeshProUGUI sortModeText;

    
    private Coroutine refreshCo;

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
    public void RefreshLobbyUI()
    {
        if (lobbyGrid == null) return;
       
        if(InventoryManager.Instance == null) return;

        var myItems = InventoryManager.Instance.GetInventoryList();

        var sorted = (sortMode == SortMode.Parts) ? InventorySortComparer.SortByPart(myItems) :
                                                    InventorySortComparer. SortDescendingOrderByRank(myItems);

        int dataCount = sorted.Count;
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
                var data = sorted[i];
                EItem.Initialize(data.scriptableObjectData, data.classType, data.step);
                EItem.BindInventory(data.uid);

                var slotItem = EItem;                
                var btn = EItem.GetComponent<Button>();
                if (btn != null)
                {
                    btn.onClick.RemoveAllListeners();
                    btn.onClick.AddListener(() => GetEquipped(slotItem));
                }
                else
                {
                    EItem.SetOnClickAction(() => GetEquipped(slotItem));
                }
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
        refreshCo = StartCoroutine(RefreshTiming());
    }
    public void GetEquipped(EquipmentItem item)
    {
        //TODO: 장비 작착 및 데이터 합치기
    }
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
    public void ToggleSort()
    {
        Debug.Log($"클릭했음 sortMode = {sortMode}");

        sortMode = (sortMode == SortMode.Parts) ? SortMode.Class : SortMode.Parts;
        if(sortModeText != null)
        {
            sortModeText.text = (sortMode == SortMode.Parts) ? "부위별" : "등급별";
        }
        RefreshLobbyUI();
    }    
}