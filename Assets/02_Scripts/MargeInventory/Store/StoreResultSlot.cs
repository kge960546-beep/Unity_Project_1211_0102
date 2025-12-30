using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StoreResultSlot : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Image iconBackGround;
    [SerializeField] private TextMeshProUGUI nameText;

    public void SetUp(EquipmentSO item)
    {
        if(item == null) return;

        if (iconImage != null)
            iconImage.sprite = item.itemSprite;
        if(nameText != null)
            nameText.text = item.equipmentName;

        if(iconBackGround != null)
        {
            iconBackGround.color = GetClolrBackGround(item.classType);
        }
    }
    public Color GetClolrBackGround(EquipmentSO.EquipmentClassType classType)
    {
        switch(classType)
        {
            case (EquipmentSO.EquipmentClassType.Normal):
                return Color.gray;
                
            case (EquipmentSO.EquipmentClassType.Good):
                return Color.green;
                
            case (EquipmentSO.EquipmentClassType.Better):
                return Color.blue;
                
            case (EquipmentSO.EquipmentClassType.Excellent):
                return new Color(0.5f, 0.0f, 0.5f);
                
            case (EquipmentSO.EquipmentClassType.Epic):
                return Color.yellow;
                
            case (EquipmentSO.EquipmentClassType.Legend):
                return Color.red;

            default: return Color.white;
                
        }
    }
}