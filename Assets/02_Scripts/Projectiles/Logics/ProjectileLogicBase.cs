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
}

public abstract class ProjectileLogicBase : ScriptableObject
{
    // TODO: apply player stat in the calculation
    [field: SerializeField] protected RuntimeAnimatorController AnimationController { get; private set; }
    [field: SerializeField] protected float DefaultSpeed { get; private set; }
    [field: SerializeField] public    float DefaultSearchDistance { get; private set; }
    [field: SerializeField] protected float ColliderRadius { get; private set; }
    [field: SerializeField] protected float LifeTime { get; private set; }
    [field: SerializeField] protected float KnockBackForce { get; private set; }

    [field: SerializeField] protected bool IsInflictingDamageOnTriggerStay { set; private get; }
    [field: SerializeField] protected float CriticalRate { set; private get; }
    [field: SerializeField] protected int OrdinaryDamage { set; private get; }
    [field: SerializeField] protected int CriticalDamage { set; private get; }

    // TODO: add more common values
    // TODO: consider storing implementation-specific data and do not require context data store
    //         - store data in packed form, like dots system?

    public void CallbackOnDrawGizmos(ref ProjectileInstanceContext context)
    {
        Gizmos.DrawWireSphere(context.rb.position, ColliderRadius);
    }

    public bool IsTargetInRange(Vector2 projectorPosition, float projectorAzimuth) => IsTargetInRangeInternal(projectorPosition, projectorAzimuth);

    public void CallbackAtOnEnable(ref ProjectileInstanceContext context, ProjectileInstanceInitializationData initData)
    {
        context.obj.layer = initData.layer;
        context.anim.runtimeAnimatorController = AnimationController;
        context.cc.radius = ColliderRadius;
        context.timer = 0f;
        context.hitCount = 0;

        if (context.obj.TryGetComponent(out ProjectileCollisionDamageBehaviour pcdb))
        {
            pcdb.IsInflictingDamageOnTriggerStay = IsInflictingDamageOnTriggerStay;
            pcdb.CriticalRate = CriticalRate;
            pcdb.OrdinaryDamage = OrdinaryDamage;
            pcdb.CriticalDamage = CriticalDamage;
        }

        CallbackAtOnEnableInternal(ref context, initData);
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
