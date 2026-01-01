using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum StatModType
{
    Flat,
    PercentAdd,
    PercentMult
}
/// <summary>
/// 보정값
/// Value: 보정값
/// Type: 보정 방식
/// Source : 보정 출처 일괄처리용(지금은 안쓸듯)
/// </summary>
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
    public float baseValue; //캐릭터 기본 스탯
    public float Value { get; protected set; }

    private readonly List<StatModifier> statModifiers = new List<StatModifier>();

    //보정 1개 추가 후 재계산
    public void AddModifier(StatModifier mod)
    {
        if (mod == null) return;
        statModifiers.Add(mod);
        UpdateStats();
    }

    //보정 1개 제거후 재계산
    public void RemoveModifier(StatModifier mod)
    {
        if (mod == null) return;
        statModifiers.Remove(mod);
        UpdateStats();
    }

    //모든 보정 출처 제거후 재계산
    public void RemoveAllModifier(object source)
    {
        if (source == null) return;
        statModifiers.RemoveAll(m => Equals( m.Source, source));
        UpdateStats();
    }

    //현재 최종 Value 계산
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
                // 장비옵션 계산
                case StatModType.Flat:
                    flatSum += mod.Value;
                    break;
                    
                    //인게임 패시브 스킬 체력10% 증가
                case StatModType.PercentAdd:
                    percentAddSum += mod.Value; // 1f = +100%
                    break;

                    //치명타 계산
                case StatModType.PercentMult:
                    percentMult *= (1f + mod.Value);
                    break;
            }
        }

        Value = (baseValue + flatSum) * (1f + percentAddSum) * percentMult;

        
    }
}

