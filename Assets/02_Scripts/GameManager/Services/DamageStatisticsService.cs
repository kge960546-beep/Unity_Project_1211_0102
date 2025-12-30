using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(int.MaxValue)] // TODO: move this hardcoded order into configuration
public class DamageStatisticsService : MonoBehaviour, IGameManagementService
{
    public Dictionary<int, int> DamageStatisticsTable { get; private set; }

    private void OnEnable()
    {
        ResetTable();
        SceneManager.sceneLoaded += OnSceneLoaded;
        GameManager.Instance.GetService<DamageManagementService>().SubscribeOnDamageEvent(OnDamageEvent);
        // TODO: subscribe skill change event
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        GameManager.Instance.GetService<DamageManagementService>().UnsubscribeOnDamageEvent(OnDamageEvent);
        // TODO: unsubscribe skill change event
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetTable();
    }

    private void ResetTable()
    {
        DamageStatisticsTable = new Dictionary<int, int>();
    }

    private void OnDamageEvent(int damage, GameObject source, IDamageable target, bool isCritical)
    {
        if (!source.TryGetComponent(out ProjectileLogicRunner runner)) return;
        int skillID = runner.SkillID;
        if (DamageStatisticsTable.ContainsKey(skillID)) DamageStatisticsTable[skillID] += damage;
        else DamageStatisticsTable[skillID] = damage;
    }

    private void OnSkillChange(ActiveSkillDescriptor source1, ActiveSkillDescriptor source2, ActiveSkillDescriptor destination)
    {
        if (null == destination.ProjectileLogic) return;
        int sum = 0;
        if (DamageStatisticsTable.TryGetValue(source1.SkillID, out int damage1)) sum += damage1;
        if (DamageStatisticsTable.TryGetValue(source2.SkillID, out int damage2)) sum += damage2;
        DamageStatisticsTable.Remove(source1.SkillID);
        DamageStatisticsTable.Remove(source2.SkillID);
        DamageStatisticsTable[destination.SkillID] = sum;
    }
}
