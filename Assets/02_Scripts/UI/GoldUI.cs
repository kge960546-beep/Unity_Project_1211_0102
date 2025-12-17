using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI savedGoldText;
    [SerializeField] private TextMeshProUGUI earnedGoldText;

    void Start()
    {
        if(GoldService.instance != null)
        {
            UpdateUI(GoldService.instance.SavedGold, GoldService.instance.EarnedGold);

            GoldService.instance.SubscribeGoldAmountChanged(UpdateUI);
        }
    }
    private void OnDestroy()
    {
        if(GoldService.instance != null)
        {
            GoldService.instance.UnsubscribeGoldAmountChanged(UpdateUI);
        }
    }
    private void UpdateUI(int saved, int earned)
    {
        if(savedGoldText != null)
        {
            savedGoldText.text = ChangeNumber(saved);            
        }
        
        if(earnedGoldText != null)
        {
            earnedGoldText.text = earned.ToString();
        }
    }
    public static string ChangeNumber(int number)
    {
        // 1억2천3백만 = 1.23B
        if (number >= 1000000000)
            return (number / 1000000000f).ToString("0.##") + "B";
        //1백2십만 = 1.2M
        if(number >= 1000000)
            return (number / 1000000f).ToString("0.##") + "M";
        // 1,200 = 1.2K
        if (number >= 1000)
            return (number / 1000f).ToString("0.##") + "K";

        return number.ToString();
    }
}
