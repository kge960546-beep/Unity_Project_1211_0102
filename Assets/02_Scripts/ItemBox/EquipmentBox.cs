using UnityEngine;
using System.Collections.Generic;

public class EquipmentBox : MonoBehaviour
{
    [SerializeField] private List<EquipmentSO> dropItem;

    [SerializeField] EquipmentSO itemData;
    [Range(0, 1)] public float drop = 1f;

    void DestroyBoxAndRewardItem()
    {
        if (dropItem != null && dropItem.Count > 0)
        {
            if (Random.value <= drop)
            {
                int index = Random.Range(0, dropItem.Count);
                itemData = dropItem[index];

                if(InventoryManager.Instance != null)
                    InventoryManager.Instance.AcquisitionItem(itemData);
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
