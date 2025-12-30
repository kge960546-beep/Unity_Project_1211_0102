using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "1118-DeathRay-ProjectileLogic", menuName = "Game/Projectile/1118 Death ray Projectile Logic")]
public class _1118_DeathRay_ProjectileLogic : ProjectileLogicBase
{
    // TODO: apply player stat in the calculation
    [field: SerializeField] public float InitialOrbitalAngle { set; private get; }
    [field: SerializeField] public float DefaultOrbitRadius { set; private get; }

    protected override bool IsTargetInRangeInternal(Vector2 projectorPosition, float projectorAzimuth)
    {
        return true;
    }

    protected override void CallbackAtOnEnableInternal(ref ProjectileInstanceContext instanceData, ProjectileInstanceInitializationData initData) { }

    protected override void CallbackAtOnDisableInternal(ref ProjectileInstanceContext instanceData) { }

    protected override void CallbackAtFixedUpdateInternal(ref ProjectileInstanceContext instanceData)
    {
        Vector2 displacement = CalculatePosition(instanceData.timer) - CalculatePosition(instanceData.timer - instanceData.timer);
        instanceData.rb.velocity = displacement / Time.fixedDeltaTime;
    }

    protected override void CallbackAtOnTriggerEnter2DInternal(ref ProjectileInstanceContext instanceData, Collider2D collider) { }
    protected override void CallbackAtOnTriggerStay2DInternal(ref ProjectileInstanceContext instanceData, Collider2D collider) { }

    Vector2 CalculatePosition(float time)
    {
        float orbitalAngularVelocity = DefaultSpeed / DefaultOrbitRadius * Mathf.Rad2Deg;
        float orbitingAngle = InitialOrbitalAngle + orbitalAngularVelocity * time;
        float orbitRadius = DefaultOrbitRadius * Mathf.Clamp01((LifeTime - time) / LifeTime);
        Unity.Mathematics.math.sincos(orbitingAngle * Mathf.Deg2Rad, out float sin, out float cos);
        Vector2 result = new Vector2(cos * orbitRadius, sin * orbitRadius);
        return result;
    }
}
