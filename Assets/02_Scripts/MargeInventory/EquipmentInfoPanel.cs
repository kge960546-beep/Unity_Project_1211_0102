using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentInfoPanel : MonoBehaviour
{
    [SerializeField] GameObject root;
    [SerializeField] private Image icon;
    [SerializeField] private Image frame;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI statText;
    [SerializeField] private TextMeshProUGUI stepText;
    [SerializeField] private Button equipButton;
    [SerializeField] private Button unequipButton;
    
    [SerializeField] private EquipmentInventory equipmentInventory;
    private EquipmentItem selectedItem;

    private bool isOpenSlot = false;

    private void Awake()
    {
        if (root == null) root = gameObject;
        if (equipButton != null) equipButton.onClick.AddListener(OnClickEquip);
        if (unequipButton != null) unequipButton.onClick.AddListener(OnClickUnequip);
        root.SetActive(false);
    }
    public void ShowFromInventory(EquipmentItem item)
    {
        if (item == null || item.Data == null) return;
        isOpenSlot = false;
        selectedItem = item;        

        var so = item.Data;

        if (icon != null) icon.sprite = so.itemSprite;
        if (nameText != null) nameText.text = so.equipmentName;
        
        if (stepText != null)
        {
            bool showStep = (item.ClassType >= EquipmentSO.EquipmentClassType.Excellent) && (item.Step > 0);
            stepText.gameObject.SetActive(showStep);

            if(showStep)
                stepText.text = item.Step.ToString();
        }
        if (statText != null)
        {
            int finalHp, finalAttack;
            so.GetFinalStats(item.ClassType, item.step, out finalHp, out finalAttack);
            switch (so.partType)
            {
                case (EquipmentSO.EquipmentPart.Weapon):
                    statText.text = $"ATK : {finalAttack}";
                    break;
                case (EquipmentSO.EquipmentPart.Armor):
                    statText.text = $"HP : {finalHp}";
                    break;
                case (EquipmentSO.EquipmentPart.Necklace):
                    statText.text = $"ATK : {finalAttack}";
                    break;
                case (EquipmentSO.EquipmentPart.Belt):
                    statText.text = $"HP : {finalHp}";
                    break;
                case (EquipmentSO.EquipmentPart.Gloves):
                    statText.text = $"ATK : {finalAttack}";
                    break;
                case (EquipmentSO.EquipmentPart.Shoes):
                    statText.text = $"HP : {finalHp}";
                    break;
            }
        }
        if(frame != null)
        {
            switch (item.ClassType)
            {
                case (EquipmentSO.EquipmentClassType.Normal):
                    frame.color = Color.gray;
                    break;
                case (EquipmentSO.EquipmentClassType.Good):
                    frame.color = Color.green;
                    break;
                case (EquipmentSO.EquipmentClassType.Better):
                    frame.color = Color.blue;
                    break;
                case (EquipmentSO.EquipmentClassType.Excellent):
                    frame.color = new Color(0.6f, 0f, 0.8f);
                    break;
                case (EquipmentSO.EquipmentClassType.Epic):
                    frame.color = Color.yellow;
                    break;
                case (EquipmentSO.EquipmentClassType.Legend):
                    frame.color = Color.red;
                    break;
            }
        }      

        // 이미 장착된 아이템이면 해제 버튼 활성
        bool isEquipped = equipmentInventory != null && equipmentInventory.IsEquippedUid(item.inventoryUid);
        if (equipButton != null) equipButton.gameObject.SetActive(!isEquipped);
        if (unequipButton != null) unequipButton.gameObject.SetActive(isEquipped);

        root.SetActive(true);
    }
    public void ShowSlot(EquipmentItem slotItem)
    {
        if (slotItem == null || slotItem.Data == null) return;
        isOpenSlot = true;

        selectedItem = slotItem;
        var so = slotItem;
        ShowFromInventory(slotItem);
        isOpenSlot = true;
    }
    public void Hide()
    {
        if (root != null) root.SetActive(false);
        root.SetActive(false);
        selectedItem = null;
    }
    private void OnClickEquip()
    {
        if (equipmentInventory == null) return;
        if (selectedItem == null) return;

        equipmentInventory.EquipFromInfo(selectedItem);
        ShowFromInventory(selectedItem); // 버튼 상태 갱신
    }
    private void OnClickUnequip()
    {        
        if (equipmentInventory == null) return;
        if (selectedItem == null || selectedItem.Data == null) return;

        equipmentInventory.Unequipment(selectedItem.Data.partType);

        if (isOpenSlot)
        {
            Hide();
            return;
        }

        ShowFromInventory(selectedItem); // 버튼 상태 갱신
    }    
}