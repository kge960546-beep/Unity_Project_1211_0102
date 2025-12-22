using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentService : MonoBehaviour,IEquipmentService, IGameManagementService
{
    private Dictionary<EquipmentData, EquipmentRuntime> ownedEquipments = new();

    //이벤트 옵저버 패턴
    public event Action<EquipmentData, int> OnEquipmentUpgraded;
    public event Action<EquipmentData> OnEquipmentAdded;
    public event Action<EquipmentData> OnEquipmentEvolved;

    //상태 조회
    public int GetCurrentLevel(EquipmentData data)
    {
        if (!ownedEquipments.TryGetValue(data, out var runtime))
            return 0;

        return runtime.level;
    }
    //업그레이드
    public bool IsCanUpgrade(EquipmentData data)
    {
        // 신규 획득
        if (!ownedEquipments.TryGetValue(data, out var runtime))
            return true;

        return runtime.level < data.levels.Length;
    }
    public void ApplyUpgrade(EquipmentData data)
    {
        //신규장비
        if (!ownedEquipments.TryGetValue(data, out var runtime))
        {
            runtime = new EquipmentRuntime(data);
            ownedEquipments[data] = runtime;

            OnEquipmentAdded?.Invoke(data);
        }
        else
        {
            runtime.level++;
            OnEquipmentUpgraded?.Invoke(data, runtime.level);
        }
    }
    //진화 조건 검사
    public bool IsCanEvolve(EvolutionData evo)
    {

        if (!ownedEquipments.TryGetValue(evo.baseEquipment, out var runtime))
            return false;

        if (runtime.level < evo.baseEquipment.levels.Length)
            return false;
        if(ownedEquipments.ContainsKey(evo.result))
            return false;

        return true;
    }
    public void ApplyEvolution(EvolutionData evo)
    {
        ownedEquipments.Remove(evo.baseEquipment);
        OnEquipmentEvolved?.Invoke(evo.baseEquipment);

        ownedEquipments[evo.result] = new EquipmentRuntime(evo.result);
        OnEquipmentAdded?.Invoke(evo.result);
    }
}
