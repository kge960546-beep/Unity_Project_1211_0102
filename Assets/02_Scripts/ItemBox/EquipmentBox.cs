using UnityEngine;
using System.Collections.Generic;

public class EquipmentBox : MonoBehaviour
{
    [SerializeField] private List<EquipmentSO> dropItems;
    [Range(0, 1)] public float drop = 1f;
    [SerializeField] int rewardItems = 2;

    EquipmentSO itemData;

    void DestroyBoxAndRewardItem()
    {
        if (dropItems != null && dropItems.Count > 0)
        {
            if (dropItems == null || dropItems.Count == 0) { Destroy(gameObject); return; }

            int count = Mathf.Max(1, rewardItems);

            for(int i = 0; i< count; i++)
            {
                if (Random.value <= drop)
                {
                    int index = Random.Range(0, dropItems.Count);
                    itemData = dropItems[index];

                    if (InventoryManager.Instance != null)
                        InventoryManager.Instance.AcquisitionItem(itemData);
                }
            }           
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        DestroyBoxAndRewardItem();
    }
}
