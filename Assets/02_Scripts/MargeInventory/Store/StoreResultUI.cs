using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreResultUI : MonoBehaviour
{
    [SerializeField] private GameObject storeResultPanel;
    [SerializeField] private Transform slot;
    [SerializeField] private GameObject slotPrefab;

    private List<GameObject> spawnSlots = new List<GameObject>();

    public void ShowResult(List<EquipmentSO> so )
    {
        ClearSlots();

        storeResultPanel.SetActive(true);

        foreach(var item in so)
        {
            GameObject sp = Instantiate(slotPrefab, slot);
            StoreResultSlot rSlot = sp.GetComponent<StoreResultSlot>();

            if(slot != null)
            {
                rSlot.SetUp(item);
            }
            spawnSlots.Add(sp);
        }
    }
    public void ClosestoreResultPanel()
    {
        storeResultPanel.SetActive(false);
        ClearSlots();
    }
    public void ClearSlots()
    {
        foreach(var slot in spawnSlots)
        {
            Destroy(slot);
        }
        spawnSlots.Clear();
    }
}
