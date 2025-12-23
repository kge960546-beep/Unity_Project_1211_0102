using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum EquipmentBadgeType
{
    None,
    New,
    Upgrade,
    Evolution
}
public class EquipmentOptionUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private GameObject newBadge;
    [SerializeField] private GameObject evolutionBadge;
    [SerializeField] private Button button;

    private EquipmentOption option;
    private EquipmentService equipmentService;

    public void Bind(EquipmentOption option, EquipmentService equipmentService, System.Action<EquipmentOption> onSelect)
    {
        var badgeType = GetBadgeType(option, equipmentService);
        //ÀüºÎ ²ô±â
        newBadge.SetActive(false);
        evolutionBadge.SetActive(false);
        levelText.gameObject.SetActive(false);

        switch(badgeType)
        {
            case EquipmentBadgeType.New:
                newBadge.SetActive(true);
                break;

            case EquipmentBadgeType.Evolution:
                evolutionBadge.SetActive(true);
                break;

            case EquipmentBadgeType.Upgrade:
                levelText.gameObject.SetActive(true);
                levelText.text = $"Lv {equipmentService.GetCurrentLevel(option.equipment)} -> {equipmentService.GetCurrentLevel(option.equipment) + 1}";
                break;

        }
        this.option = option;

        nameText.text = GetName(option);
        icon.sprite = GetIcon(option);

        descText.text = GetDescription(option, equipmentService);

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onSelect(option));
    }

    private string GetName(EquipmentOption option)
    {
        return option.type == EquipmentOptionType.Evolution
            ? option.evolution.result.name
            : option.equipment.name;
    }
    private Sprite GetIcon(EquipmentOption option)
    {
        return option.type == EquipmentOptionType.Evolution
            ? option.evolution.result.icon
            : option.equipment.icon;
    }
    private EquipmentBadgeType GetBadgeType(EquipmentOption option, EquipmentService service)
    {
        if (option.type == EquipmentOptionType.Evolution)
            return EquipmentBadgeType.Evolution;

        int curLv = service.GetCurrentLevel(option.equipment);

        if (curLv == 0)
            return EquipmentBadgeType.New;

            return EquipmentBadgeType.Upgrade;
    }
    public string GetDescription(EquipmentOption option, EquipmentService service)
    {
        if (option.type == EquipmentOptionType.Evolution)
            return option.evolution.description;

        int nextLv = service.GetCurrentLevel(option.equipment) + 1;
        return option.equipment.levels[nextLv - 1].description;
    }
}
