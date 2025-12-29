using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StoreResultSlot : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;

    public void SetUp(EquipmentSO item)
    {
        if(item == null) return;

        if (iconImage != null)
            iconImage.sprite = item.itemSprite;
        if(nameText != null)
            nameText.text = item.equipmentName;
    }
}
