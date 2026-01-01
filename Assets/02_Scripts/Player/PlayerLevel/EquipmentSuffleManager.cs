using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EquipmentSuffleManager : MonoBehaviour
{
    [SerializeField] private EquipmentDataBase equipmentDB;
    [SerializeField] private EvolutionDataBase evolutionDB;

    [SerializeField] private LevelUpUI levelUpUI;
    [SerializeField] private int optionCount = 3;

    private EquipmentService equipmentService;

    private Queue<int> levelUpQueue = new();
    private bool isProcessingLevelUp = false;


    private void Awake()
    {
#if UNITY_EDITOR
        if (equipmentDB == null)
            Debug.LogError("equipmentDB NULL");
        if (evolutionDB == null)
            Debug.LogError("evolutionDB NULL");
        if (levelUpUI == null)
            Debug.LogError("levelUpUI NULL");
#endif
        levelUpUI.OnClosed += OnLevelUpUIClose;
    }

    private void Start()
    {
        equipmentService = GameManager.Instance.GetComponent<EquipmentService>();

#if UNITY_EDITOR
        if (equipmentService == null)
            Debug.LogError("EquipmentService NULL");
#endif       
    }
    private void OnEnable()
    {
        var exp = GameManager.Instance.GetService<ExperienceService>();
        if (exp != null)
            exp.OnLevelUp += HandleLevelUp;
    }
    private void OnDisable()
    {
        var exp = GameManager.Instance.GetService<ExperienceService>();
        if (exp != null)
            exp.OnLevelUp -= HandleLevelUp;
    }
    private void HandleLevelUp(int level)
    {
        #if UNITY_EDITOR
        if (equipmentService == null || levelUpUI == null)
        {
            Debug.LogError("[HandleLevelUp] Missing reference");
            return;
        }
        Debug.Log($"[LevelUpService] HandleLevelUp {level}");
        #endif

        levelUpQueue.Enqueue(level);

        TryProcessNextLevelUp();
    }
    private void TryProcessNextLevelUp()
    {

        if (isProcessingLevelUp)
            return;

        if (levelUpQueue.Count == 0)
            return;

        isProcessingLevelUp = true;


        int level = levelUpQueue.Dequeue();
#if UNITY_EDITOR
        Debug.Log($"[LevelUp] Processing : {level}");
#endif

        var options = GenerateOptions();
        levelUpUI.Open(options);
    }
    private List<EquipmentOption> GenerateOptions()
    {
        List<EquipmentOption> result = new();

        List<EvolutionData> evolutions = new();

        // 1. 진화 장비 후보 수집
        foreach(var evo in evolutionDB.evolutionDataList)
        {
            if(equipmentService.IsCanEvolve(evo))
                evolutions.Add(evo);
        }
        // 2. 진화가 하나라도 있으면 -> 진화만 보여줌
        if(evolutions.Count > 0)
        {
            var pick = evolutions[Random.Range(0, evolutions.Count)];
            result.Add(EquipmentOption.Evolution(pick));
            return result;
        }

        // 3. 남은 슬롯 수 계산
        int remain = optionCount - result.Count;
        if(remain <= 0)
            return result;

        // 4. 기본 업그레이드 후보 수집
        List<EquipmentData> upgrades = new();
        foreach(var data in equipmentDB.equipmentDataList)
        { 
            if(equipmentService.IsCanUpgrade(data))
                upgrades.Add(data);
        }

        // 5. 업그레이드 셔플 후 채우기
        foreach (var data in WeightedShuffle(upgrades, 3))
        {
            result.Add(EquipmentOption.Upgrade(data));
        }

#if UNITY_EDITOR
        //디버그
        if (upgrades.Count == 0 && result.Count == 0)
        {
            Debug.LogWarning("[LevelUp] No available options");
        }
#endif

        return result;
    }

    private List<EquipmentData> WeightedShuffle(List<EquipmentData> source, int count)
    {
        List<EquipmentData> result = new();
        List<EquipmentData> pool = new(source);

        while (result.Count < count && pool.Count > 0)
        {
            EquipmentData pick = GetWeightedRandom(pool);
            result.Add(pick);
            pool.Remove(pick);
        }

        return result;
    }

    private EquipmentData GetWeightedRandom(List<EquipmentData> list)
    {
        int totalWeigth = 0;

        foreach (var skill in list)
            totalWeigth += RarityWeightTable.GetWeight(skill.rarity);

        int rand = Random.Range(0, totalWeigth);

        foreach (var skill in list)
        {
            rand -= RarityWeightTable.GetWeight(skill.rarity);
            if (rand < 0)
                return skill;
        }

        return list[0];
    }
    private void OnLevelUpUIClose()
    {
        isProcessingLevelUp = false;
        TryProcessNextLevelUp();
    }
}
