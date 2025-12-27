using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeInventory : MonoBehaviour
{    
    [SerializeField] Transform mergeGrid;
    [SerializeField] GameObject itemPrefab;

    [SerializeField] MergeUIWindow mergeWindow;


    void Start()
    {
        if(mergeWindow == null)
            mergeWindow = FindObjectOfType<MergeUIWindow>();

        RefreshMergeUI();
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
                item.Initialize(data.soData, data.type, data.step);
                item.SetOnClickAction(() =>
                {
                    if (mergeWindow != null)
                        mergeWindow.OnItemSelected(item);
                });
            } 
        }
    }   
}