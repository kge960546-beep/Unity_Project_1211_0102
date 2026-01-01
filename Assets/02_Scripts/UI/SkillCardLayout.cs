using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum SkillBadgeType
{
    None,
    New,
    Upgrade,
    Evolution
}

public class SkillCardLayout : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private GameObject newBadge;
    [SerializeField] private GameObject evolutionBadge;
    [SerializeField] private Button button;

    public void Initialize(SkillDescriptor descriptor, int nextLevel, SkillManagementBehaviour smb, Action onClickCallbackFromExternal)
    {
        if (null == descriptor) throw new System.ArgumentNullException("Skill descriptor was null.");

        newBadge.SetActive(false);
        evolutionBadge.SetActive(false);
        levelText.gameObject.SetActive(false);

        switch(GetBadgeType(descriptor, nextLevel))
        {
            case SkillBadgeType.New:
                newBadge.SetActive(true);
                break;

            case SkillBadgeType.Evolution:
                evolutionBadge.SetActive(true);
                break;

            case SkillBadgeType.Upgrade:
                levelText.gameObject.SetActive(true);
                levelText.text = $"Lv {nextLevel - 1} -> {nextLevel}";
                break;
        }

        nameText.text = descriptor.name;
        icon.sprite = descriptor.SkillThumbnail;
        descText.text = descriptor.SkillDescription;

        int skillID = descriptor.SkillID;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClickCallbackFromExternal?.Invoke());
        button.onClick.AddListener(
            descriptor.SkillType switch
            {
                SkillDescriptor.Type.ActiveEvo => () => smb.EvolveActiveSkill(skillID),
                SkillDescriptor.Type.Active => nextLevel == 1 ? () => smb.BindActiveSkill(skillID) : () => smb.LevelUpActiveSkill(skillID),
                SkillDescriptor.Type.Passive => nextLevel == 1 ? () => smb.BindPassiveSkill(skillID) : () => smb.LevelUpPassiveSkill(skillID),
                _ => throw new NotImplementedException(),
            });
    }

    private SkillBadgeType GetBadgeType(SkillDescriptor descriptor, int nextLevel)
    {
        if (descriptor.SkillType == SkillDescriptor.Type.ActiveEvo) return SkillBadgeType.Evolution;
        if (1 == nextLevel) return SkillBadgeType.New;
        else return SkillBadgeType.Upgrade;
    }

    private void OnDisable()
    {
        button.onClick.RemoveAllListeners();
    }
}
