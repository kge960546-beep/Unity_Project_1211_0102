using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGold : MonoBehaviour
{
    [SerializeField] private int goldAmount = 10000;

    [ContextMenu("Å×½ºÆ®¿ë °ñµå")]
    public void AmountGold()
    {
        if(GoldService.instance != null)
        {
            GoldService.instance.AddGold(goldAmount);
        }
    }
    [ContextMenu("Å×½ºÆ®¿ë °ñµå »©±â")]
    public void ReverseAmountGold()
    {
        if (GoldService.instance != null)
        {
            GoldService.instance.ReverseAddGold(goldAmount);
        }
    }
}
