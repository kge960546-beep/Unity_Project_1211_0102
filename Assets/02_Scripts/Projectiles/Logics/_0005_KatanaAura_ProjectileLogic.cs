using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "0005-KatanaAura-ProjectileLogic", menuName = "Game/Projectile/0005 Katana aura Projectile Logic")]
public class _0005_KatanaAura_ProjectileLogic : ProjectileLogicBase
{
    protected override bool IsTargetInRangeInternal(Vector2 projectorPosition, float projectorAzimuth)
    {
        return true;
    }

    protected override void CallbackAtOnEnableInternal(ref ProjectileInstanceContext instanceData, ProjectileInstanceInitializationData initData)
    {
        Unity.Mathematics.math.sincos(initData.initialProjectorAzimuthSnapshot * Mathf.Deg2Rad, out float sin, out float cos);

        Vector2 direction = new Vector2(cos, sin);

        instanceData.rb.position = initData.initialProjectorPositionSnapshot;
        instanceData.rb.velocity = direction * DefaultSpeed;
        instanceData.rb.rotation = initData.initialProjectorAzimuthSnapshot;
    }

    protected override void CallbackAtOnDisableInternal(ref ProjectileInstanceContext instanceData) { }
    protected override void CallbackAtFixedUpdateInternal(ref ProjectileInstanceContext instanceData) { }
    protected override void CallbackAtOnTriggerEnter2DInternal(ref ProjectileInstanceContext instanceData, Collider2D collider) { }
    protected override void CallbackAtOnTriggerStay2DInternal(ref ProjectileInstanceContext instanceData, Collider2D collider) { }
}
