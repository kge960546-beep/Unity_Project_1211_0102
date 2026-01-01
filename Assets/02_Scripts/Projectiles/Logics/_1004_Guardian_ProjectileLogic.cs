using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "1004-Guardian-ProjectileLogic", menuName = "Game/Projectile/1004 Guardian Projectile Logic")]
public class _1004_Guardian_ProjectileLogic : ProjectileLogicBase
{
    // TODO: apply player stat in the calculation
    [field: SerializeField] public float InitialOrbitalAngle { set; private get; }
    [field: SerializeField] public float DefaultOrbitRadius { set; private get; }
    [field: SerializeField] public float DefaultPreDestructionTime { set; private get; }
    [field: SerializeField] public float InitialRotationalAngle { set; private get; }
    [field: SerializeField] public float DefaultRotationalAngularVelocity { set; private get; }

    protected override bool IsTargetInRangeInternal(Vector2 projectorPosition, float projectorAzimuth)
    {
        return true;
    }

    protected override void CallbackAtOnEnableInternal(ref ProjectileInstanceContext instanceData, ProjectileInstanceInitializationData initData)
    {
        instanceData.rb.angularVelocity = DefaultRotationalAngularVelocity;
    }

    protected override void CallbackAtOnDisableInternal(ref ProjectileInstanceContext instanceData) { }

    protected override void CallbackAtFixedUpdateInternal(ref ProjectileInstanceContext instanceData)
    {
        float orbitalAngularVelocity = DefaultSpeed / DefaultOrbitRadius * Mathf.Rad2Deg;
        float perSequenceInitialOrbitalAngle = InitialOrbitalAngle + (360f * (float)instanceData.sequenceNumber / (float)instanceData.sequenceCount);
        float orbitingAngle = perSequenceInitialOrbitalAngle + orbitalAngularVelocity * instanceData.timer;
        Unity.Mathematics.math.sincos(orbitingAngle * Mathf.Deg2Rad, out float sin, out float cos);

        PlayerFeedService pfs = GameManager.Instance.GetService<PlayerFeedService>();
        instanceData.rb.velocity = (pfs.playerPosition + new Vector2(cos * DefaultOrbitRadius, sin * DefaultOrbitRadius) - instanceData.rb.position) / Time.fixedDeltaTime;

        float preDestructionTimerRate = Mathf.Clamp01((instanceData.timer + DefaultPreDestructionTime - LifeTime) / DefaultPreDestructionTime);
        float remainingPreDestructionTimerRate = 1 - preDestructionTimerRate;

        instanceData.obj.transform.localScale = new Vector3(remainingPreDestructionTimerRate, remainingPreDestructionTimerRate, remainingPreDestructionTimerRate);
    }

    protected override void CallbackAtOnTriggerEnter2DInternal(ref ProjectileInstanceContext instanceData, Collider2D collider) { }
    protected override void CallbackAtOnTriggerStay2DInternal(ref ProjectileInstanceContext instanceData, Collider2D collider) { }
}
