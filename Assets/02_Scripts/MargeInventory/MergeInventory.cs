using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MergeInventory : MonoBehaviour
{    
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

        foreach (Transform child in mergeGrid)
        {
            Destroy(child.gameObject);
        }

        if (InventoryManager.Instance == null) return;

        var myItems = InventoryManager.Instance.GetInventoryList();

        foreach (var data in myItems)
        {
            GameObject newSlot = Instantiate(itemPrefab, mergeGrid);

            EquipmentItem item = newSlot.GetComponent<EquipmentItem>();

            if (item != null)
            {
                item.Initialize(data.soData, data.classType, data.step);
                item.BindInventory(data.uid);
                
                item.SetOnClickAction(() =>
                {
                    if (mergeWindow != null)
                        mergeWindow.OnItemSelected(item);
                });
            } 
        }

        if(refreshCo != null)
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
}