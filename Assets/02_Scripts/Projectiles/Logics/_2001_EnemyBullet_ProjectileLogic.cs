using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "2001-EnemyBullet-ProjectileLogic", menuName = "Game/Projectile/2001 Enemy bullet Projectile Logic")]
public class _2001_EnemyBullet_ProjectileLogic : ProjectileLogicBase
{
    protected override bool IsTargetInRangeInternal(Vector2 projectorPosition, float projectorAzimuth)
    {
        int mask = 1 << GameManager.Instance.GetService<LayerService>().playerLayer;
        Collider2D c = Physics2D.OverlapCircle(projectorPosition, DefaultSearchDistance, mask);
        return null == c;
    }

    protected override void CallbackAtOnEnableInternal(ref ProjectileInstanceContext instanceData, ProjectileInstanceInitializationData initData)
    {
        PlayerFeedService pfs = GameManager.Instance.GetService<PlayerFeedService>();

        Vector2 dir = (pfs.playerPosition - instanceData.rb.position).normalized;
        instanceData.rb.velocity = dir * DefaultSpeed;
        instanceData.rb.rotation = Mathf.Atan2(dir.y, dir.x);
    }

    protected override void CallbackAtOnDisableInternal(ref ProjectileInstanceContext instanceData) { }
    protected override void CallbackAtFixedUpdateInternal(ref ProjectileInstanceContext instanceData) { }
    protected override void CallbackAtOnTriggerEnter2DInternal(ref ProjectileInstanceContext instanceData, Collider2D collider)
    {
        GameManager.Instance.GetService<PoolingService>().ReturnOrDestroyGameObject(instanceData.obj);
    }

    protected override void CallbackAtOnTriggerStay2DInternal(ref ProjectileInstanceContext instanceData, Collider2D collider) { }
}
