using UnityEngine;
using UnityEngine.UI;

public class EquipmentItem : MonoBehaviour
{
    [SerializeField] private EquipmentSO data;
    [SerializeField] private EquipmentSO.EquipmentClassType type;    
    [Range(0, 2)] public int step;

    [SerializeField] private Image iconImage;
    [SerializeField] private Image frameImage;

    public EquipmentSO Data => data;
    public int EquipmentId { get { return data != null ? data.equipmentID : -1; } }
    public EquipmentSO.EquipmentClassType Type => type;    
    public int Step => step;
   
    public void Initialize(EquipmentSO newData, EquipmentSO.EquipmentClassType newtype,int newStep)
    {
        data = newData;
        type = newtype;
        step = Mathf.Clamp(newStep, 0, 2);
        
        UpdateItem();
    }
    public void UpdateItem()
    {
        if (data == null) return;
        if(iconImage != null)
        {
            iconImage.sprite = data.itemSprite;
        }
        if(frameImage != null)
        {
           SetColor(type);
        }
    }
    public void SetColor(EquipmentSO.EquipmentClassType type)
    {
        switch(type)
        {
            case (EquipmentSO.EquipmentClassType.Normal):
                frameImage.color =  Color.gray;
                break;
                
            case (EquipmentSO.EquipmentClassType.Good):
                frameImage.color = Color.green;
                break;

            case (EquipmentSO.EquipmentClassType.Better):
                frameImage.color = Color.blue;
                break;

            case (EquipmentSO.EquipmentClassType.Excellent):
                frameImage.color = new Color(0.6f, 0.0f, 0.8f);
                break;

            case (EquipmentSO.EquipmentClassType.Epic):
                frameImage.color = Color.yellow;
                break;

            case (EquipmentSO.EquipmentClassType.Legend):
                frameImage.color = Color.red;
                break;
        }
    }
}