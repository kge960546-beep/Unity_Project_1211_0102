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

    public void CallbackAtOnCollisionEnter2D(ref ProjectileInstanceContext context, Collision2D collision) { CallbackAtOnCollisionEnter2DInternal(ref context, collision); }
    public void CallbackAtOnCollisionStay2D(ref ProjectileInstanceContext context, Collision2D collision) { CallbackAtOnCollisionStay2DInternal(ref context, collision); }

    protected abstract void CallbackAtOnEnableInternal(ref ProjectileInstanceContext context, ProjectileInstanceInitializationData initData);
    protected abstract void CallbackAtOnDisableInternal(ref ProjectileInstanceContext context);
    protected abstract void CallbackAtFixedUpdateInternal(ref ProjectileInstanceContext context);

    // Common logics such as damage and destruction on collision are not processed here.
    // only projectile-specific logic goes here.
    protected abstract void CallbackAtOnCollisionEnter2DInternal(ref ProjectileInstanceContext context, Collision2D collision);
    protected abstract void CallbackAtOnCollisionStay2DInternal(ref ProjectileInstanceContext context, Collision2D collision);

#if UNITY_EDITOR
    // use Debug.LogError / LogWarning / UnityEngine.Assertions.Assert if conditions are not met
    protected virtual void ExecuteOnValidate() { }
#endif
}
