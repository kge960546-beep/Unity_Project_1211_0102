using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "9000-Skill", menuName = "Game/Skill/Passive Skill Descriptor")]
public sealed class PassiveSkillDescriptor : SkillDescriptor
{
    public enum Type : byte
    {
        HealthPoint,
        HealthPointRegen,       // Need HP regeneration feature in PlayerHP
        AttackDamage,
        CooldownReduction,
        MovementSpeed,
        ProjectileSpeed,
        SkillProjectileRange,   // Actually projectile lifetime? May need correction/fine tuning for each projectile.
        ExpGain,
        GoldGain,
        ReceivedDamageReduction,
        EffectDuration,
    }

    [field: SerializeField] public Type SkillType { get; private set; }
    [field: SerializeField] public int[] LeveledMultiplierTable { get; private set; }
}
