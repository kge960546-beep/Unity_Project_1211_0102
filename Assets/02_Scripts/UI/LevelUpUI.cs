using System;
using System.Collections.Generic;
using Unity.Content;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class LevelUpUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject skillCardPrefab;
    [SerializeField] private Transform optionRoot;
    [SerializeField] private Button skipButton;
    [SerializeField] private int buttonCount;
    
    private SkillManagementBehaviour smb;

    public event Action OnClosed;

    private void Awake()
    {
        HideInstant();
        smb = FindAnyObjectByType<SkillManagementBehaviour>(FindObjectsInactive.Include);
    }

    public void Open(List<EquipmentOption> options)
    {
        Debug.Log("[LevelUpUI] Open 호출됨");
        Show();

        foreach(Transform child in optionRoot)
        {
            Destroy(child.gameObject);
        }

        #region pick random skills
        var candidates = smb.ListUpUpgradableAndBindableSkills();
        var candidatesOfEvo     = candidates.FindAll(e => e.descriptor.SkillType == SkillDescriptor.Type.ActiveEvo);
        var candidatesOfNonEvo  = candidates.FindAll(e => e.descriptor.SkillType != SkillDescriptor.Type.ActiveEvo);
        int evoPickCount = Mathf.Min(buttonCount, candidatesOfEvo.Count);
        int nonEvoPickCount = Mathf.Min(buttonCount, Mathf.Max(0, buttonCount - evoPickCount));

        Debug.Log($"evoPickCount {evoPickCount}, nonEvoPickCount {nonEvoPickCount}");
        var pickedSkillDescriptors = ShuffleUtility.FisherYatesShuffle(candidatesOfEvo, evoPickCount);
        pickedSkillDescriptors.AddRange(ShuffleUtility.FisherYatesShuffle(candidatesOfNonEvo, nonEvoPickCount));
        #endregion

        foreach (var target in pickedSkillDescriptors)
        {
            var ui = Instantiate(skillCardPrefab);
            ui.transform.SetParent(optionRoot, false);
            //ui.transform.localScale = Vector3.one;
            if (ui.TryGetComponent(out SkillCardLayout layoutBehaviour)) layoutBehaviour.Initialize(target.descriptor, target.nextLevel, smb, OnSelectSkill);
            else Destroy(ui);
        }

        skipButton.onClick.RemoveAllListeners();
        skipButton.onClick.AddListener(Skip);
    }
    public void OnSelectSkill()
    {
        foreach (Transform child in optionRoot) child.gameObject.SetActive(false);
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
}
