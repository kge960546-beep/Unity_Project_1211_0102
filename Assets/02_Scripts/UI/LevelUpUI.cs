using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using Unity.Content;

public class LevelUpUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private EquipmentOptionUI optionPrefab;
    [SerializeField] private Transform optionRoot;
    [SerializeField] private Button skipButton;

    private EquipmentService equipmentService;

    public event Action OnClosed;
    private void Awake()
    {
        equipmentService = GameManager .Instance.GetService<EquipmentService>();
        HideInstant();
    }
    public void Open(List<EquipmentOption> options)
    {
        Debug.Log("[LevelUpUI] Open 호출됨");
        Show();

        // 기존 스킬 옵션 제거
        foreach(Transform child in optionRoot)
        {
            Destroy(child.gameObject);
        }

        // 스킬 옵션 생성
        foreach(var option in options)
        {
            var ui = Instantiate(optionPrefab);
            ui.transform.SetParent(optionRoot, false); //추가
            ui.transform.localScale = Vector3.one;
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

        Hide();

        OnClosed?.Invoke();
    }
    private void Show()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        //Todo: timeScale 추후 수정 바람
        Time.timeScale = 0f;
    }
    private void Hide()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        //Todo: timeScale 추후 수정 바람
        Time.timeScale = 1f;
    }
    private void HideInstant()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
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
