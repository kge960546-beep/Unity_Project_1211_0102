using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageInformation : IComparable<DamageInformation>
{
    public int damage;
    public GameObject source;
    public IDamageable target;
    public bool isCritical;

    public int CompareTo(DamageInformation other)
    {
        int comparison;

        comparison = damage.CompareTo(other.damage);
        if (0 != comparison) return comparison;

        comparison = source.GetInstanceID().CompareTo(other.source.GetInstanceID());
        if (0 != comparison) return comparison;

        MonoBehaviour targetMonoBehaviour = target as MonoBehaviour;
        MonoBehaviour comparedTargetMonoBehaviour = target as MonoBehaviour;
        if (null == targetMonoBehaviour) return -1;
        if (null == comparedTargetMonoBehaviour) return 1;
        comparison = targetMonoBehaviour.gameObject.GetInstanceID().CompareTo(comparedTargetMonoBehaviour.gameObject.GetInstanceID());
        return comparison;
    }
}

[DefaultExecutionOrder(int.MaxValue)]
public class DamageManagementService : MonoBehaviour, IGameManagementService
{
    private SimplePriorityQueue<DamageInformation> damageInfoQueue;

    /// <summary>
    /// int: damage value
    /// GameObject: damage source
    /// bool: boolean whether the damage was critical damage
    /// </summary>
    private event Action<int, GameObject, bool> OnDamageEvent;

    private void OnEnable()
    {
        damageInfoQueue = new SimplePriorityQueue<DamageInformation>();
    }

    public void QueueDamage(int damage, GameObject source, IDamageable target, bool isCritical)
    {
        damageInfoQueue.Enqueue(new DamageInformation { damage = damage, source = source, target = target });
    }

    private void FixedUpdate()
    {
        while (0 != damageInfoQueue.Count)
        {
            DamageInformation damageInfo = damageInfoQueue.Dequeue();
            Debug.Log((damageInfo.target as MonoBehaviour).gameObject.name);
            damageInfo.target.TakeDamage(damageInfo.damage, damageInfo.source, damageInfo.isCritical);
            OnDamageEvent?.Invoke(damageInfo.damage, damageInfo.source, damageInfo.isCritical);
        }
    }

    public void SubscribeOnDamageEvent(Action<int, GameObject, bool> action)
    {
        OnDamageEvent += action;
    }

    public void UnsubscribeOnDamageEvent(Action<int, GameObject, bool> action)
    {
        OnDamageEvent -= action;
    }
}
