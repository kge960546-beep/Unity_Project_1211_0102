using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "0000-Skill", menuName = "Game/Skill/Active Skill Descriptor")]
public sealed class ActiveSkillDescriptor : SkillDescriptor
{
    public enum Type : byte
    {
        Active,
        ActiveEvo
    }

    [field: SerializeField] public Type SkillType { get; private set; }
    [field: SerializeField] public GameObject CommonSharedBaseProjectileSpawnerPrefab { get; private set; } // TODO: this is temporary thing. this is shared and should be eventually instantiated by all skills
    [field: SerializeField] public ProjectileLogicBase ProjectileLogic { get; private set; }
    [field: SerializeField] public List<LeveledProjectionTimingTable> LeveledProjectionTimingTables { get; private set; }
    [field: SerializeField] public float DefaultCooldown{ get; private set; }
}
