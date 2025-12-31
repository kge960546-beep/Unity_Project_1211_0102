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

    //수정 결과보고 판단할 변수들
    private string selectedUid;

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
        if (InventoryManager.Instance == null) return;
        isOpenSlot = false;

        selectedItem = item;
        selectedUid = item.inventoryUid;
        if (string.IsNullOrEmpty(selectedUid)) return;

        var inv = InventoryManager.Instance.FindUid(selectedUid);
        if (inv == null || inv.scriptableObjectData == null) return;

        var so = inv.scriptableObjectData;

        if (icon != null) icon.sprite = so.itemSprite;
        if (nameText != null) nameText.text = so.equipmentName;
        
        if (stepText != null)
        {
            bool showStep = (inv.classType >= EquipmentSO.EquipmentClassType.Excellent) && (inv.step > 0);
            stepText.gameObject.SetActive(showStep);

            if(showStep)
                stepText.text = inv.step.ToString();
        }
        int finalHp, finalAttack;
        so.GetFinalStats(inv.classType, inv.step, out finalHp, out finalAttack);

        if (statText != null)
        {
           
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
            switch (inv.classType)
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
        bool isEquipped = equipmentInventory != null && equipmentInventory.IsEquippedUid(selectedUid);
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
    //아이템 정보창 비활성화
    public void Hide()
    {
        if (root != null) root.SetActive(false);
        root.SetActive(false);
        selectedItem = null;
    }
    //아이템 장착버튼
    private void OnClickEquip()
    {
        if (string.IsNullOrEmpty(selectedUid)) return;
        if (equipmentInventory == null) return;
        if (selectedItem == null) return;

        equipmentInventory.EquipFromUid(selectedUid);
        if(isOpenSlot)
        {
            Hide();
            return;
        }
        ShowFromInventory(selectedItem); // 버튼 상태 갱신
           
    }
    //아이템 해제버튼
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