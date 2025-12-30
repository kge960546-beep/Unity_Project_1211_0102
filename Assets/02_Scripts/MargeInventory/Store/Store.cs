using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    [SerializeField] private StoreResultUI resultUI;
    [SerializeField] private List<EquipmentSO> items = new List<EquipmentSO>();

    [SerializeField] private int[] gradWeights;
    [SerializeField] private int totalWeight = 0;
    [SerializeField] private int drawPrice = 100;

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

        EquipmentSO orgItem = items[Random.Range(0, items.Count)];
        EquipmentSO resultItems = Instantiate(orgItem);

        resultItems.classType = classType;
        resultItems.name = orgItem.name;

        return resultItems;
    }
    public void OnClickDrawOneButton()
    {
        if(GoldService.instance.UseGold(drawPrice))
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
        int totalPrice = drawPrice * 10;
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