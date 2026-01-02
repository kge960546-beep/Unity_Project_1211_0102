using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ProjectileInstanceInitializationData
{
    public Vector2 initialProjectorPositionSnapshot;
    public Vector2 currentProjectorPosition;
    public float initialProjectorAzimuthSnapshot;
    public float currentProjectorAzimuth;
    public int sequenceCount;
    public int sequenceNumber;
    public int layer;
    public int level;
}

public abstract class ProjectileLogicBase : ScriptableObject
{
    // TODO: apply player stat in the calculation
    [field: SerializeField] protected RuntimeAnimatorController AnimationController { get; private set; }
    [field: SerializeField] protected float DefaultSpeed { get; private set; }
    [field: SerializeField] public    float DefaultSearchDistance { get; private set; }
    [field: SerializeField] protected float DefaultColliderRadius { get; private set; }
    [field: SerializeField] protected float LifeTime { get; private set; }
    [field: SerializeField] protected float KnockBackForce { get; private set; }

    [field: SerializeField] protected bool IsInflictingDamageOnTriggerEnter { set; get; }
    [field: SerializeField] protected bool IsInflictingDamageOnTriggerStay { set; get; }
    [field: SerializeField] protected float CriticalRate { set; get; }
    [Tooltip("Random value with uniform distribution from (1 - ratio) to (1 + ratio)")][SerializeField] protected float damageVariationRatio;
    [field: SerializeField] protected float[] OrdinaryDamageCoefficientTable { set; get; }
    [field: SerializeField] protected float CriticalDamageMultiplier { set; get; }

    // TODO: add more common values
    // TODO: consider storing implementation-specific stagePattern and do not require context stagePattern store
    //         - store stagePattern in packed form, like dots system?

    public void CallbackOnDrawGizmos(ref ProjectileInstanceContext context)
    {
        Gizmos.DrawWireSphere(context.rb.position, DefaultColliderRadius);
    }

    public bool IsTargetInRange(Vector2 projectorPosition, float projectorAzimuth) => IsTargetInRangeInternal(projectorPosition, projectorAzimuth);

    public void CallbackAtOnEnable(ref ProjectileInstanceContext context, ProjectileInstanceInitializationData initData)
    {
        context.obj.layer = initData.layer;
        context.anim.runtimeAnimatorController = AnimationController;
        context.cc.radius = DefaultColliderRadius;
        context.timer = 0f;
        context.hitCount = 0;
        context.level = initData.level;
        context.sequenceCount = initData.sequenceCount;
        context.sequenceNumber = initData.sequenceNumber;

        if (context.obj.TryGetComponent(out ProjectileCollisionDamageBehaviour pcdb))
        {
            pcdb.IsInflictingDamageOnTriggerEnter = IsInflictingDamageOnTriggerEnter;
            pcdb.IsInflictingDamageOnTriggerStay = IsInflictingDamageOnTriggerStay;
            pcdb.CriticalRate = CriticalRate;
            pcdb.DamageVariationRatio = damageVariationRatio;
            float baseDamage = 100f; // TODO: calculate damage from context.obj.{STAT_RELATED_COMPONENT}
            foreach (PlayerStat stat in context.obj.GetComponents<PlayerStat>()) // not good but we don't have enough time to cache this...
            {
                if (PlayerStatType.Attack ==  stat.StatType)
                {
                    baseDamage = stat.CalculatedValue;
                    break;
                }
            }
            int ordinaryDamage = (int)(baseDamage * OrdinaryDamageCoefficientTable[context.level - 1]);
            pcdb.OrdinaryDamage = ordinaryDamage;
            pcdb.CriticalDamage = (int)(ordinaryDamage * CriticalDamageMultiplier);
        }

        context.rb.rotation = 0f;
        context.obj.transform.localScale = Vector3.one;

        CallbackAtOnEnableInternal(ref context, initData);
    }

    public void CallbackAtOnDisable(ref ProjectileInstanceContext context)
    {
        CallbackAtOnDisableInternal(ref context);
    }

    public void CallbackAtFixedUpdate(ref ProjectileInstanceContext context)
    {
        context.timer += Time.fixedDeltaTime;
        if (context.timer >= LifeTime) GameManager.Instance.GetService<PoolingService>().ReturnOrDestroyGameObject(context.obj);
        CallbackAtFixedUpdateInternal(ref context);
    }

    public void CallbackAtOnTriggerEnter2D(ref ProjectileInstanceContext context, Collider2D collider) => CallbackAtOnTriggerEnter2DInternal(ref context, collider);
    public void CallbackAtOnTriggerStay2D(ref ProjectileInstanceContext context, Collider2D collider) => CallbackAtOnTriggerStay2DInternal(ref context, collider);


    protected abstract bool IsTargetInRangeInternal(Vector2 projectorPosition, float projectorAzimuth);

    protected abstract void CallbackAtOnEnableInternal(ref ProjectileInstanceContext context, ProjectileInstanceInitializationData initData);
    protected abstract void CallbackAtOnDisableInternal(ref ProjectileInstanceContext context);
    protected abstract void CallbackAtFixedUpdateInternal(ref ProjectileInstanceContext context);

    protected abstract void CallbackAtOnTriggerEnter2DInternal(ref ProjectileInstanceContext context, Collider2D collider);
    protected abstract void CallbackAtOnTriggerStay2DInternal(ref ProjectileInstanceContext context, Collider2D collider);

#if UNITY_EDITOR
    // use Debug.LogError / LogWarning / UnityEngine.Assertions.Assert if conditions are not met
    protected virtual void ExecuteOnValidate() { }
#endif
}
