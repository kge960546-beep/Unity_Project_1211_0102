using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentItem : MonoBehaviour
{
    [SerializeField] private EquipmentSO data;
    [SerializeField] private EquipmentSO.EquipmentClassType classType;
    [SerializeField] private EquipmentSO.EquipmentPart partType;
    [Range(0, 2)] public int step;

    [SerializeField] private Image iconImage;
    [SerializeField] private Image frameImage;

    [SerializeField] private TextMeshProUGUI mergeLevel;

    private Action onClickItemAction;
    [SerializeField] private Button btn;

    public string inventoryUid {  get; private set; }
    public EquipmentSO Data => data;
    public int EquipmentId { get { return data != null ? data.equipmentID : -1; } }
    public EquipmentSO.EquipmentClassType ClassType => classType;   
    public EquipmentSO.EquipmentPart PartType => partType;
    public int Step => step;

    private void Awake()
    {
        btn.onClick.AddListener(OnButtonClicked);
    }
    private void OnButtonClicked()
    {
        if (onClickItemAction != null)
            onClickItemAction.Invoke();
    }
    public void Initialize(EquipmentSO newData, EquipmentSO.EquipmentClassType newtype,int newStep)
    {
        data = newData;
        classType = newtype;
        step = Mathf.Clamp(newStep, 0, 2);
        
        UpdateItem();
    }
    public void BindInventory(string uid)
    {
        inventoryUid = uid;
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
           SetColor(classType);
        }
        UpdateMergeLevel();
    }
    void UpdateMergeLevel()
    {
        if (mergeLevel == null) return;

        if(step > 0)
        {
            mergeLevel.gameObject.SetActive(true);
            mergeLevel.text = step.ToString();
        }
        else
        {
            mergeLevel.gameObject.SetActive(false);            
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
    public void SetOnClickAction(Action action)
    {
        onClickItemAction = action;
    }
   
}