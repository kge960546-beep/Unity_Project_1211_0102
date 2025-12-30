using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillManagementBehaviour : MonoBehaviour
{
    [Header("Skill Database")]
    [SerializeField] private GameObject SharedCommonProjectilePrefab;
    [SerializeField] private ActiveSkillDescriptor[] activeSkillDescriptorInputTable;
    Dictionary<int, ActiveSkillDescriptor> activeSkillDescriptorDictionary;
    [SerializeField] private PassiveSkillDescriptor[] passiveSkillDescriptorInputTable;
    Dictionary<int, PassiveSkillDescriptor> passiveSkillDescriptorDictionary;
    [SerializeField] private SkillEvolutionRoute[] evoRouteInputTable;
    Dictionary<int, SkillEvolutionRoute> evoRouteDictionary;

    [Header("Settings")]
    [SerializeField] private int maxSkillLevel;

    private ActiveSkillStateControllerBehaviour[] activeSkills = new ActiveSkillStateControllerBehaviour[6];
    private PassiveSkillControllerBehaviour[] passiveSkills = new PassiveSkillControllerBehaviour[6];
    private int playerProjectileLayer;

    private DamageStatisticsService dss;

    private void Awake()
    {
        activeSkillDescriptorDictionary = activeSkillDescriptorInputTable.ToDictionary(desc => desc.SkillID);
        passiveSkillDescriptorDictionary = passiveSkillDescriptorInputTable.ToDictionary(desc => desc.SkillID);
        evoRouteDictionary = evoRouteInputTable.ToDictionary(r => r.EvolvedSkillID);
        playerProjectileLayer = GameManager.Instance.GetService<LayerService>().playerProjectileLayer;
        dss = GameManager.Instance.GetService<DamageStatisticsService>();
    }

    public List<(SkillDescriptor descriptor, int nextLevel, bool isPassive)> ListUpUpgradableSkills()
    {
        List<(SkillDescriptor descriptor, int nextLevel, bool isPassive)> result = new List<(SkillDescriptor descriptor, int nextLevel, bool isPassive)>();

        foreach (ActiveSkillStateControllerBehaviour behaviour in activeSkills)
            if (behaviour.context.level < maxSkillLevel)
                result.Add((activeSkillDescriptorDictionary[behaviour.skillID], behaviour.context.level + 1, false));

        foreach (PassiveSkillControllerBehaviour behaviour in passiveSkills)
            if (behaviour.level < maxSkillLevel)
                result.Add((passiveSkillDescriptorDictionary[behaviour.skillID], behaviour.level + 1, true));

        foreach (SkillEvolutionRoute route in evoRouteDictionary.Values)
        {
            if (activeSkills.Any(behaviour => behaviour.skillID == route.BaseSkillID1)
                && (activeSkills.Any(behaviour => behaviour.skillID == route.BaseSkillID2)
                    || passiveSkills.Any(behaviour => behaviour.skillID == route.BaseSkillID2)))
                result.Add((activeSkillDescriptorDictionary[route.EvolvedSkillID], 1, false));
        }

        return result;
    }

    #region Active Skill Spceific Methods
    public void EvolveActiveSkill(int skillID)
    {
        if (evoRouteDictionary.TryGetValue(skillID, out SkillEvolutionRoute route)) throw new System.InvalidOperationException("Cannot find the target skill.");

        // base skill 1 is always an active skill.
        if (null != dss) dss.OnActiveSkillEvolve(activeSkillDescriptorDictionary[route.BaseSkillID1], activeSkillDescriptorDictionary[skillID]);
        UnbindActiveSkill(route.BaseSkillID1);

        // base skill 2 can be either an active skill or a passive skill.
        if (TryFindTargetActiveSkillCell(route.BaseSkillID2, out _))
        {
            if (null != dss) dss.OnActiveSkillEvolve(activeSkillDescriptorDictionary[route.BaseSkillID2], activeSkillDescriptorDictionary[skillID]);
            UnbindActiveSkill(route.BaseSkillID2);
        }

        BindActiveSkill(skillID);
    }

    public void LevelUpActiveSkill(int skillID)
    {
        if (!TryFindTargetActiveSkillCell(skillID, out int cellIndex)) throw new System.InvalidOperationException("Cannot find the target skill.");
        activeSkills[cellIndex].context.level++;
    }

    public void BindActiveSkill(int skillID, bool isDefaultSkill = false)
    {
        if (activeSkillDescriptorDictionary.TryGetValue(skillID, out var descriptor)) throw new System.ArgumentNullException("Cannot find designated skill.");
        if (!TryFindEmptyActiveSkillCell(out int cellIndex)) throw new System.InvalidOperationException($"Cannot bind more than {activeSkills.Length} skills.");

        ActiveSkillStateControllerBehaviour skill = gameObject.AddComponent<ActiveSkillStateControllerBehaviour>();

        skill.skillID = descriptor.SkillID;

        skill.context.logic = descriptor.ProjectileLogic;
        skill.context.leveledTimingTables = descriptor.LeveledProjectionTimingTables.ToArray();
        skill.context.skillUserRB = GetComponent<Rigidbody2D>();
        skill.context.SharedCommonProjectilePrefab = SharedCommonProjectilePrefab;
        skill.context.period = descriptor.DefaultCooldown;

        skill.context.level = 1;
        skill.context.layer = playerProjectileLayer;

        activeSkills[cellIndex] = skill;

        if (null != dss) dss.OnActiveSkillBind(descriptor);
    }

    private void UnbindActiveSkill(int skillID)
    {
        if (activeSkillDescriptorDictionary.TryGetValue(skillID, out var descriptor)) throw new System.ArgumentNullException("Cannot find designated skill.");
        if (!TryFindTargetActiveSkillCell(skillID, out int cellIndex)) throw new System.InvalidOperationException("Cannot find the target skill.");
        Destroy(activeSkills[cellIndex]);
        activeSkills[cellIndex] = null;

        if (null != dss) dss.OnActiveSkillUnbind(descriptor);
    }

    private bool TryFindEmptyActiveSkillCell(out int cellIndex)
    {
        for (int i = 0; i < activeSkills.Length; i++)
        {
            if (null == activeSkills[i])
            {
                cellIndex = i;
                return true;
            }
        }
        cellIndex = -1;
        return false;
    }

    private bool TryFindTargetActiveSkillCell(int skillID, out int cellIndex)
    {
        for (int i = 0; i < activeSkills.Length; i++)
        {
            if (null == activeSkills[i]) continue;
            if (skillID == activeSkills[i].skillID)
            {
                cellIndex = i;
                return true;
            }
        }
        cellIndex = -1;
        return false;
    }
    #endregion

    #region Passive Skill Spceific Methods
    public void BindPassiveSkill(int skillID, bool isDefaultSkill = false)
    {
        if (passiveSkillDescriptorDictionary.TryGetValue(skillID, out var descriptor)) throw new System.ArgumentNullException("Cannot find designated skill.");
        if (!TryFindEmptyPassiveSkillCell(out int cellIndex)) throw new System.InvalidOperationException($"Cannot bind more than {activeSkills.Length} skills.");

        PassiveSkillControllerBehaviour skill = gameObject.AddComponent<PassiveSkillControllerBehaviour>();

        skill.skillID = descriptor.SkillID;

        // TODO: descriptor

        skill.level = 1;

        passiveSkills[cellIndex] = skill;
    }

    public void LevelUpPassiveSkill(int skillID)
    {
        if (!TryFindTargetPassiveSkillCell(skillID, out int cellIndex)) throw new System.InvalidOperationException("Cannot find the target skill.");
        passiveSkills[cellIndex].level++;
    }

    private bool TryFindEmptyPassiveSkillCell(out int cellIndex)
    {
        for (int i = 0; i < passiveSkills.Length; i++)
        {
            if (null == passiveSkills[i])
            {
                cellIndex = i;
                return true;
            }
        }
        cellIndex = -1;
        return false;
    }

    private bool TryFindTargetPassiveSkillCell(int skillID, out int cellIndex)
    {
        for (int i = 0; i < passiveSkills.Length; i++)
        {
            if (null == passiveSkills[i]) continue;
            if (skillID == passiveSkills[i].skillID)
            {
                cellIndex = i;
                return true;
            }
        }
        cellIndex = -1;
        return false;
    }
    #endregion
}
