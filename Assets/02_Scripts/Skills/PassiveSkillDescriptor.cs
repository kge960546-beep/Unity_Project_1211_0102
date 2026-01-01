using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "9000-Skill", menuName = "Game/Skill/Passive Skill Descriptor")]
public sealed class PassiveSkillDescriptor : SkillDescriptor
{
    public enum ModifiableStat : byte
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

    [field: SerializeField] public ModifiableStat AffectedStat { get; private set; }
    [field: SerializeField] public int[] LeveledMultiplierTable { get; private set; }

    // TODO: apply stat data into actual player data
}
