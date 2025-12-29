using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;

public class Store : MonoBehaviour
{
    [SerializeField] private StoreResultUI resultUI;

    [SerializeField] private List<EquipmentSO> items = new List<EquipmentSO>();
    [SerializeField] private int total = 0;

    private void Awake()
    {
        Recalculate();
    }
    private void Recalculate()
    {
        total = 0;
        if (items == null) return;

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null) continue;
            total += Mathf.Max(0, items[i].weight);
        }
    }
    public EquipmentSO DrawEquipment()
    {
        if (items == null || items.Count == 0)
        {
            Debug.LogError("[Store] items 리스트가 비어있습니다.");
            return null;
        }

        if (total <= 0)
        {
            Debug.LogError("[Store] total이 0입니다. weight 설정을 확인하세요.");
            return null;
        }

        int roll = Random.Range(0, total);
        int cumulative = 0;

        for (int i = 0; i < items.Count; i++)
        {
            var so = items[i];
            if (so == null) continue;

            cumulative += Mathf.Max(0, so.weight);
            if (roll < cumulative)
            {                
                return so;               
            }
        }
        
        for (int i = items.Count - 1; i >= 0; i--)
            if (items[i] != null) return items[i];                 

        return null;
    }
    public void OnClickDrawOneButton()
    {
        var reward = DrawEquipment();


        if (reward != null)
        {
            InventoryManager.Instance.AddItem(reward);

            List<EquipmentSO> resultList = new List<EquipmentSO>();
            if (resultUI != null)
                resultUI.ShowResult(resultList);
        }
    }
    public void OnClickDrawTenButton()
    {
        List<EquipmentSO> resultList = new List<EquipmentSO>();

        for (int i = 0; i <10; i++)
        {
            var rewards = DrawEquipment();

            if (rewards != null)
                InventoryManager.Instance.AddItem(rewards);

            if (resultList != null)
                resultList.Add(rewards);
        }

        if (resultUI != null && resultList.Count > 0)
            resultUI.ShowResult(resultList);
    }
}