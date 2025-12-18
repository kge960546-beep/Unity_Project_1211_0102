using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ProjectileInstanceInitializationData
{
    public Vector2 projectorPosition;
    public Vector2 targetingDirection;
    public int level;
    public int sequenceNumber;
    public int layer;
}

public abstract class ProjectileLogicBase : ScriptableObject
{
    [field: SerializeField] protected RuntimeAnimatorController AnimationController { get; private set; }
    // TODO: add more common values
    [field: SerializeField] public float DefaultSearchRadius { get; private set; } // TODO: apply player stat in the calculation
    [field: SerializeField] protected float ColliderRadius { get; private set; }
    [field: SerializeField] protected float LifeTime { get; private set; }
    [field: SerializeField] protected float NuckBackForce { get; private set; }

    public void CallbackOnDrawGizmos(ref ProjectileInstanceContext context)
    {
        Gizmos.DrawWireSphere(context.rb.position, ColliderRadius);
    }

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
            Destroy(context.obj); // TODO: refine destruction, defer it and make some destruction animation etc. and use pooling
        }
        CallbackAtFixedUpdateInternal(ref context);
    }

    public void CallbackAtOnTriggerEnter2D(ref ProjectileInstanceContext context, Collider2D collider) { CallbackAtOnTriggerEnter2DInternal(ref context, collider); }
    public void CallbackAtOnTriggerStay2D(ref ProjectileInstanceContext context, Collider2D collider) { CallbackAtOnTriggerStay2DInternal(ref context, collider); }

    protected abstract void CallbackAtOnEnableInternal(ref ProjectileInstanceContext context, ProjectileInstanceInitializationData initData);
    protected abstract void CallbackAtOnDisableInternal(ref ProjectileInstanceContext context);
    protected abstract void CallbackAtFixedUpdateInternal(ref ProjectileInstanceContext context);

    // Common logics such as damage and destruction on collision are not processed here.
    // only projectile-specific logic goes here.
    protected abstract void CallbackAtOnTriggerEnter2DInternal(ref ProjectileInstanceContext context, Collider2D collider);
    protected abstract void CallbackAtOnTriggerStay2DInternal(ref ProjectileInstanceContext context, Collider2D collider);

#if UNITY_EDITOR
    // use Debug.LogError / LogWarning / UnityEngine.Assertions.Assert if conditions are not met
    protected virtual void ExecuteOnValidate() { }
#endif
}
