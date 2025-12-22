using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentOptionUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descText;
    [SerializeField] private TextMeshProUGUI badgeText;
    [SerializeField] private GameObject evolutionBadge;
    [SerializeField] private Button button;

    private EquipmentOption option;
    private EquipmentService equipmentService;

    private void Awake()
    {
        equipmentService = GameManager.Instance.GetService<EquipmentService>();
    }

    public void Bind(
        EquipmentOption option, 
        EquipmentService equipmentService, 
        System.Action<EquipmentOption> onSelect)
    {
        this.option = option;

        icon.sprite = option.equipment.icon;
        nameText.text = option.equipment.name;

        descText.text = GetDescription(option, equipmentService);

        badgeText.text = GetBadge(option, equipmentService);
        badgeText.gameObject.SetActive(!string.IsNullOrEmpty(badgeText.text));

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onSelect(option));
    }

    private string GetBadge(EquipmentOption option, EquipmentService service)
    {
        if (option.type == EquipmentOptionType.Evolution)
            return "Evolution";

        int curLv = service.GetCurrentLevel(option.equipment);

        if (curLv == 0)
            return "New";

        return $"Lv {curLv} -> {curLv + 1}";
        ;
    }
    public string GetDescription(EquipmentOption option, EquipmentService service)
    {
        if (option.type == EquipmentOptionType.Evolution)
            return option.evolution.description;

        int nextLv = service.GetCurrentLevel(option.equipment) + 1;
        return option.equipment.levels[nextLv - 1].description;
    }
}
