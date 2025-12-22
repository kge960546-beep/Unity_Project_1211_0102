using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EquipmentSuffleManager : MonoBehaviour
{
    [SerializeField] private EquipmentDataBase equipmentDB;
    [SerializeField] private EvolutionDataBase evolutionDB;

    [SerializeField] private int optionCount = 3;

    private EquipmentService equipmentService;
    private void Awake()
    {
        equipmentService = GameManager.Instance.GetService<EquipmentService>();
    }
    private List<EquipmentOption> GenerateOptions()
    {
        List<EquipmentOption> result = new();

        List<EvolutionData> evolutions = new();

        //진화 장비 후보 먼저 검사
        foreach(var evo in evolutionDB.evolutionDataList)
        {
            if(equipmentService.IsCanEvolve(evo))
                evolutions.Add(evo);
        }
        //진화가 하나라도 있으면 -> 진화만 보여줌
        if(evolutions.Count > 0)
        {
            var pick = evolutions[Random.Range(0, evolutions.Count)];
            result.Add(EquipmentOption.Evolution(pick));
            return result;
        }

        //기본 업그레이드 후보
        List<EquipmentData> upgrades = new();
        foreach(var data in equipmentDB.equipmentDataList)
        { 
            if(equipmentService.IsCanUpgrade(data))
                upgrades.Add(data);
        }


        foreach (var data in WeightedShuffle(upgrades, 3))
        {
            result.Add(EquipmentOption.Upgrade(data));
        }

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

        foreach(var skill in list)
        {
            rand -= RarityWeightTable.GetWeight(skill.rarity);
            if (rand < 0)
                return skill;
        }

        return list[0];
    }
}
