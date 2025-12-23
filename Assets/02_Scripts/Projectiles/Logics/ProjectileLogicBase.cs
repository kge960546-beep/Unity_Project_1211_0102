using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ProjectileInstanceInitializationData
{
    public Vector2 initialProjectorPositionSnapshot;
    public Vector2 curretnProjectorPosition;
    public float initialProjectorAzimuthSnapshot;
    public float curretnProjectorAzimuth;
    public int sequenceCount;
    public int sequenceNumber;
    //public int level; // TODO: This may not required. Remove this appropriately.
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
        CallbackAtOnEnableInternal(ref context, initData);
    }

    public void CallbackAtFixedUpdate(ref ProjectileInstanceContext context)
    {
        context.timer += Time.fixedDeltaTime;
        if (context.timer >= LifeTime)
        {
            GameManager.Instance.GetService<PoolingService>().ReturnOrDestroyGameObject(context.obj);
            // TODO: refine destruction, defer it and make some destruction animation etc.
        }
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
