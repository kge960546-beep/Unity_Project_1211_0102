using UnityEngine;
using UnityEngine.UI;

public class EquipmentInventory : MonoBehaviour
{
    [SerializeField] private Transform lobbyGrid;
    [SerializeField] private GameObject itemPrefab; 
    
    private void OnEnable()
    {
        RefreshLobbyUI();
    }
    public void RefreshLobbyUI()
    {
        if (lobbyGrid == null) return;

        foreach (Transform child in lobbyGrid)
        {
            Destroy(child.gameObject);
        }

        if(InventoryManager.Instance == null) return;

        var myItems = InventoryManager.Instance.GetInventoryList();

        foreach (var data in myItems)
        {
            GameObject newSlot = Instantiate(itemPrefab, lobbyGrid);

            EquipmentItem item = newSlot.GetComponent<EquipmentItem>();

            if (item != null)
            {
                item.Initialize(data.soData, data.classType, data.step);
                item.BindInventory(data.uid);
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(lobbyGrid.parent.GetComponent<RectTransform>());

        if(lobbyGrid.parent != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(lobbyGrid.parent.GetComponent<RectTransform>());
        }
    }
}