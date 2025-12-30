using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(int.MaxValue)] // TODO: move this hardcoded order into configuration
public class DamageStatisticsService : MonoBehaviour, IGameManagementService
{
    public Dictionary<int, int> DamageStatisticsTable { get; private set; }
    public Dictionary<int, int> SkillEvolutionLog { get; private set; }

    private void OnEnable()
    {
        ResetTable();
        SceneManager.sceneLoaded += OnSceneLoaded;
        GameManager.Instance.GetService<DamageManagementService>().SubscribeOnDamageEvent(OnDamageEventCallback);
        // TODO: subscribe skill change event
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        GameManager.Instance.GetService<DamageManagementService>().UnsubscribeOnDamageEvent(OnDamageEventCallback);
        // TODO: unsubscribe skill change event
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetTable();
    }

    private void ResetTable()
    {
        DamageStatisticsTable = new Dictionary<int, int>();
        SkillEvolutionLog = new Dictionary<int, int>();
    }

    private void OnDamageEventCallback(int damage, GameObject source, IDamageable target, bool isCritical)
    {
        if (!source.TryGetComponent(out ProjectileLogicRunner runner)) return;
        int skillID = runner.SkillID;
        if (DamageStatisticsTable.ContainsKey(skillID))                             DamageStatisticsTable[skillID]          += damage;
        else if (SkillEvolutionLog.TryGetValue(skillID, out int evolvedSkillID))    DamageStatisticsTable[evolvedSkillID]   += damage;
    }

    public void OnActiveSkillBind(ActiveSkillDescriptor skill)
    {
        DamageStatisticsTable.TryAdd(skill.SkillID, 0);
    }

    public void OnActiveSkillUnbind(ActiveSkillDescriptor skill)
    {
        DamageStatisticsTable.Remove(skill.SkillID);
    }

    public void OnActiveSkillEvolve(ActiveSkillDescriptor skillFrom, ActiveSkillDescriptor skillTo)
    {
        DamageStatisticsTable[skillTo.SkillID] += DamageStatisticsTable[skillFrom.SkillID];
        DamageStatisticsTable.Remove(skillFrom.SkillID);
        SkillEvolutionLog[skillFrom.SkillID] = skillTo.SkillID;
    }
}
