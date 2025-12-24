using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        GameContextService gcs = GameManager.Instance.GetService<GameContextService>(); // TODO: rework gcs
        GameObject player = gcs.Player;
        if (null != player)
        {
            Vector2 dir = ((Vector2)player.transform.position - instanceData.rb.position).normalized;
            instanceData.rb.velocity = dir * DefaultSpeed;
            instanceData.rb.rotation = Mathf.Atan2(dir.y, dir.x);
        }
        else
        {
            instanceData.rb.velocity = Vector2.zero;
            instanceData.rb.rotation = 0f;
        }
    }

    protected override void CallbackAtOnDisableInternal(ref ProjectileInstanceContext instanceData) { }
    protected override void CallbackAtFixedUpdateInternal(ref ProjectileInstanceContext instanceData) { }
    protected override void CallbackAtOnTriggerEnter2DInternal(ref ProjectileInstanceContext instanceData, Collider2D collider)
    {
        GameManager.Instance.GetService<PoolingService>().ReturnOrDestroyGameObject(instanceData.obj);
    }

    protected override void CallbackAtOnTriggerStay2DInternal(ref ProjectileInstanceContext instanceData, Collider2D collider) { }
}
