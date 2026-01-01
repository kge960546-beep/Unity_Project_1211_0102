using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "0005-KatanaAura-ProjectileLogic", menuName = "Game/Projectile/0005 Katana aura Projectile Logic")]
public class _0005_KatanaAura_ProjectileLogic : ProjectileLogicBase
{
    [field: SerializeField] public float DefaultPreDestructionAnimationDuration { set; private get; }
    protected override bool IsTargetInRangeInternal(Vector2 projectorPosition, float projectorAzimuth)
    {
        return true;
    }

    protected override void CallbackAtOnEnableInternal(ref ProjectileInstanceContext instanceData, ProjectileInstanceInitializationData initData)
    {
        bool isReverse = 0 != initData.sequenceNumber % 2;
        Unity.Mathematics.math.sincos(initData.initialProjectorAzimuthSnapshot * Mathf.Deg2Rad, out float sin, out float cos);

        Vector2 direction = new Vector2(cos, sin);

        PlayerFeedService pfs = GameManager.Instance.GetService<PlayerFeedService>();
        Vector2 playerPosition = pfs.playerPosition;

        instanceData.rb.position = playerPosition;
        instanceData.rb.velocity = direction * (isReverse ? -DefaultSpeed : DefaultSpeed);
        instanceData.rb.rotation = initData.initialProjectorAzimuthSnapshot + (isReverse ? 180f : 0f);

        instanceData.anim.speed = 0f;
    }

    protected override void CallbackAtOnDisableInternal(ref ProjectileInstanceContext instanceData) { }
    protected override void CallbackAtFixedUpdateInternal(ref ProjectileInstanceContext instanceData)
    {
        if (instanceData.timer + DefaultPreDestructionAnimationDuration >= LifeTime) instanceData.anim.speed = 1f;
    }
    protected override void CallbackAtOnTriggerEnter2DInternal(ref ProjectileInstanceContext instanceData, Collider2D collider) { }
    protected override void CallbackAtOnTriggerStay2DInternal(ref ProjectileInstanceContext instanceData, Collider2D collider) { }
}
