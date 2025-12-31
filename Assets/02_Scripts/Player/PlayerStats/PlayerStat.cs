using System.Collections.Generic;
using UnityEngine;

public enum StatModType
{
    Flat,
    PercentAdd,
    PercentMult
}
public class StatModifier
{
    public float Value;
    public StatModType Type;
    public object Source;

    public StatModifier(float value, StatModType type, object source = null)
    {
        Value = value;
        Type = type;
        Source = source;
    }
}
public class PlayerStat : MonoBehaviour, IGameManagementService
{
    public float baseValue;
    public float Value { get; protected set; }

    private readonly List<StatModifier> statModifiers = new List<StatModifier>();

    public void AddModifier(StatModifier mod)
    {
        if (mod == null) return;
        statModifiers.Add(mod);
        UpdateStats();
    }

    public void RemoveModifier(StatModifier mod)
    {
        if (mod == null) return;
        statModifiers.Remove(mod);
        UpdateStats();
    }

    public void RemoveAllModifier(object source)
    {
        if (source == null) return;
        statModifiers.RemoveAll(m => Equals( m.Source, source));
        UpdateStats();
    }

    public void UpdateStats()
    {
        float flatSum = 0f;
        float percentAddSum = 0f;
        float percentMult = 1f;

        for (int i = 0; i < statModifiers.Count; i++)
        {
            var mod = statModifiers[i];
            switch (mod.Type)
            {
                case StatModType.Flat:
                    flatSum += mod.Value;
                    break;

                case StatModType.PercentAdd:
                    percentAddSum += mod.Value; // 1f = +100%
                    break;

                case StatModType.PercentMult:
                    percentMult *= (1f + mod.Value);
                    break;
            }
        }

        Value = (baseValue + flatSum) * (1f + percentAddSum) * percentMult;
    }
}

