using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelUpUI : MonoBehaviour
{
    [SerializeField] private EquipmentOptionUI optionPrefab;
    [SerializeField] private Transform optionRoot;
    [SerializeField] private Button skipButton;

    private EquipmentService equipmentService;

    public void Open(List<EquipmentOption> options)
    {
        gameObject.SetActive(true);
        // 테스트용 시간 정지. 추후 교체 필요
        Time.timeScale = 0f;

        foreach(Transform child in optionRoot)
            Destroy(child.gameObject);

        foreach(var option in options)
        {
            var ui = Instantiate(optionPrefab, optionRoot);
            ui.Bind(option, equipmentService, OnSelectOption);
        }

        skipButton.onClick.RemoveAllListeners();
        skipButton.onClick.AddListener(Skip);
    }
    public void OnSelectOption(EquipmentOption option)
    {
        ApplyOption(option);
        Close();
    }
    public void Skip()
    {
        Close();
    }
    public void Close()
    {

        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
    public void ApplyOption(EquipmentOption option)
    {
        switch(option.type)
        {
            case EquipmentOptionType.Upgrade:
                equipmentService.ApplyUpgrade(option.equipment);
                break;

            case EquipmentOptionType.Evolution:
                equipmentService.ApplyEvolution(option.evolution);
                break;
        }
    }
}
