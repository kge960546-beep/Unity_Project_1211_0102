using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MergeInventory : MonoBehaviour
{
    public enum SortMode { Parts, Class }
    [SerializeField] private SortMode sortMode;
    [SerializeField] private TextMeshProUGUI sortModeText;

    [SerializeField] Transform mergeGrid;
    [SerializeField] GameObject itemPrefab;

    [SerializeField] MergeUIWindow mergeWindow;
    private Coroutine refreshCo;
    
    private void OnEnable()
    {
        if (mergeWindow == null)
            mergeWindow = FindObjectOfType<MergeUIWindow>();

        if (InventoryManager.Instance != null)
            InventoryManager.Instance.SubscribeOnInventoryChanged(RefreshMergeUI);

        RefreshMergeUI();
    }
    private void OnDisable()
    {
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.UnsubscribeOnInventoryChanged(RefreshMergeUI);

        if (refreshCo != null)
        {
            StopCoroutine(refreshCo);
            refreshCo = null;
        }
    }
    public void RefreshMergeUI()
    {
        if (mergeGrid == null) return;

        //foreach (Transform child in mergeGrid)
        //{
        //    Destroy(child.gameObject);
        //}

        if (InventoryManager.Instance == null) return;

        var myItems = InventoryManager.Instance.GetInventoryList();

        var sorted = (sortMode == SortMode.Parts) ? InventorySortComparer.SortByPart(myItems) :
                                                    InventorySortComparer.SortDescendingOrderByRank(myItems);

        int dataCount = sorted.Count;
        int currentSlotCount = mergeGrid.childCount;

        for (int i = 0; i < dataCount; i++)
        {
            EquipmentItem EItem = null;

            if (i < currentSlotCount)
            {
                Transform child = mergeGrid.GetChild(i);
                child.gameObject.SetActive(true);
                EItem = child.GetComponent<EquipmentItem>();
            }
            else
            {
                GameObject newSlot = Instantiate(itemPrefab, mergeGrid);
                EItem = newSlot.GetComponent<EquipmentItem>();
            }

            if (EItem != null)
            {
                var data = sorted[i];
                EItem.Initialize(data.soData, data.classType, data.step);
                EItem.BindInventory(data.uid);
            }

        }

        for (int i = dataCount; i < currentSlotCount; i++)
        {
            mergeGrid.GetChild(i).gameObject.SetActive(false);
        }

        //foreach (var data in sorted)
        //{
        //    GameObject newSlot = Instantiate(itemPrefab, mergeGrid);
        //
        //    EquipmentItem item = newSlot.GetComponent<EquipmentItem>();
        //
        //    if (item != null)
        //    {
        //        item.Initialize(data.soData, data.classType, data.step);
        //        item.BindInventory(data.uid);
        //        
        //        item.SetOnClickAction(() =>
        //        {
        //            if (mergeWindow != null)
        //                mergeWindow.OnItemSelected(item);
        //        });
        //    } 
        //}

        if (refreshCo != null)
        {
            StopCoroutine(refreshCo);
        }
        refreshCo = StartCoroutine(RefreshTiming());
    }
    IEnumerator RefreshTiming()
    {
        if (mergeGrid == null || itemPrefab == null) yield break;
        if (InventoryManager.Instance == null) yield break;

        yield return new WaitForEndOfFrame();

        Canvas.ForceUpdateCanvases();

        var gridRect = mergeGrid as RectTransform;
        if (gridRect != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(gridRect);
        }

        if (mergeGrid.parent != null)
        {
            var parentRect = mergeGrid.parent as RectTransform;
            if (parentRect != null)
                LayoutRebuilder.ForceRebuildLayoutImmediate(parentRect);
        }
        Canvas.ForceUpdateCanvases();
    }
    public void ToggleSort()
    {
        sortMode = (sortMode == SortMode.Parts) ? SortMode.Class : SortMode.Parts;
        if (sortModeText != null)
        {
            sortModeText.text = (sortMode == SortMode.Parts) ? "부위별" : "등급별";
        }
        RefreshMergeUI();
    }
}