using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Image backGroundImage;

    [SerializeField] private Color[] classBackGroundColor;

    public void SetItem(ItemData item)
    {
        iconImage.sprite = item.icon;
        countText.text = "x" + item.count.ToString();

        int classIndex = (int)item.itemClass;

        if(classIndex >= 0 && classIndex < classBackGroundColor.Length)
        {
            backGroundImage.color = classBackGroundColor[classIndex];
        }

        gameObject.SetActive(true);
    }

    public void ClearSlot()
    {
        gameObject.SetActive(false);
    }
}
