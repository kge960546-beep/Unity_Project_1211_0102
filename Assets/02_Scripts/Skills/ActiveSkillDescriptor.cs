using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "0000-Skill", menuName = "Game/Skill/Active Skill Descriptor")]
public sealed class ActiveSkillDescriptor : SkillDescriptor
{
    [field: SerializeField] public ProjectileLogicBase ProjectileLogic { get; private set; }
    [field: SerializeField] public List<LeveledProjectionTimingTable> LeveledProjectionTimingTables { get; private set; }
    [field: SerializeField] public float DefaultCooldown{ get; private set; }
}
