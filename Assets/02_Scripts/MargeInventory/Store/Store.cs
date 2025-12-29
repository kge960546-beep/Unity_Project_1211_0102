using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;


public class Store : MonoBehaviour
{
    [SerializeField] private StoreResultUI resultUI;
    [SerializeField] private List<EquipmentSO> items = new List<EquipmentSO>();

    [SerializeField] private int[] gradWeights;
    [SerializeField] private int totalWeight = 0;
    [SerializeField] private int drawPeice = 100;

    private void Awake()
    {
        totalWeight = 0;
        Recalculate();
    }
    private void Recalculate()
    {
        totalWeight = 0;        

        for (int i = 0; i < gradWeights.Length; i++)
        {            
            totalWeight += gradWeights[i];
        }
    }
    public EquipmentSO DrawEquipment()
    {
        if (items == null || items.Count == 0)
        {
            Debug.LogError("[Store] items 리스트가 비어있습니다.");
            return null;
        }

        if (totalWeight <= 0)
        {
            Debug.LogError("[Store] total이 0입니다. weight 설정을 확인하세요.");
            return null;
        }

        int randomValue = Random.Range(0, totalWeight);
        int currentSum = 0;

        EquipmentSO.EquipmentClassType classType = EquipmentSO.EquipmentClassType.Normal;

        for (int i = 0; i < gradWeights.Length; i++)
        {
            currentSum += gradWeights[i];
            if (randomValue < currentSum)
            {
                classType = (EquipmentSO.EquipmentClassType)i;
                break;
            }
        }

        List<EquipmentSO> resultItems = new List<EquipmentSO>();
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] != null && items[i].classType == classType)
            {
                resultItems.Add(items[i]);
            }
        }

        if(resultItems.Count > 0)
        {
            return resultItems[Random.Range(0, resultItems.Count)];
        }

        return null;
    }
    public void OnClickDrawOneButton()
    {
        if(GoldService.instance.UseGold(drawPeice))
        {
            var reward = DrawEquipment();

            if (reward != null)
            {
                InventoryManager.Instance.AddItem(reward);
            
                List<EquipmentSO> resultList = new List<EquipmentSO>
                {
                    reward
                };
            
                if (resultUI != null)
                    resultUI.ShowResult(resultList);
            }
        }
        else
        {
            Debug.Log("골드부족");
        }
    }
    public void OnClickDrawTenButton()
    {
        int totalPrice = drawPeice * 10;
        if(GoldService.instance.UseGold(totalPrice))
        {
            List<EquipmentSO> resultList = new List<EquipmentSO>();

            for (int i = 0; i < 10; i++)
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
        else
        {
            Debug.Log("골드부족");
        }
    }
}