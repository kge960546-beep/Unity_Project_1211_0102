using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(fileName = "0001-Kunai-ProjectileLogic", menuName = "Game/Projectile/0001 Kunai Projectile Logic")]
public class _0001_Kunai_ProjectileLogic : ProjectileLogicBase
{
    protected override bool IsTargetInRangeInternal(Vector2 projectorPosition, float projectorAzimuth)
    {
        return null != GetTargetPosition(projectorPosition);
    }

    private Vector2? GetTargetPosition(Vector2 projectorPosition)
    {
        LayerService ls = GameManager.Instance.GetService<LayerService>();
        int enemyLayer = ls.enemyLayer;
        int bossLayer = ls.bossLayer;
        int destructibleItemLayer = ls.destructibleItemLayer;
        LayerMask mask = (1 << enemyLayer) | (1 << bossLayer) | (1 << destructibleItemLayer);

        float minimumDistance = float.MaxValue;
        Vector2? targetPosition = null;

        Collider2D[] candidates = Physics2D.OverlapCircleAll(projectorPosition, DefaultSearchDistance, mask);
        // TODO: may use NonAlloc to minimize GC

        foreach (Collider2D candidate in candidates)
        {
            float distance = Vector2.Distance(projectorPosition, candidate.transform.position);
            if (distance < minimumDistance)
            {
                minimumDistance = distance;
                targetPosition = candidate.transform.position;
            }
        }

        return targetPosition;
    }

    protected override void CallbackAtOnEnableInternal(ref ProjectileInstanceContext instanceData, ProjectileInstanceInitializationData initData)
    {
        instanceData.rb.position = initData.currentProjectorPosition;

        Vector2 targetPosition = GetTargetPosition(initData.currentProjectorPosition) ?? initData.currentProjectorPosition;

        Vector2 dir = (targetPosition - initData.currentProjectorPosition).normalized;
        instanceData.rb.velocity = dir * DefaultSpeed;
        instanceData.rb.rotation = Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg;
    }

    protected override void CallbackAtOnDisableInternal(ref ProjectileInstanceContext instanceData) { }
    protected override void CallbackAtFixedUpdateInternal(ref ProjectileInstanceContext instanceData) { }
    protected override void CallbackAtOnTriggerEnter2DInternal(ref ProjectileInstanceContext instanceData, Collider2D collider)
    {
        GameManager.Instance.GetService<PoolingService>().ReturnOrDestroyGameObject(instanceData.obj);
        // duplicated returning occurs
        // do we need to logically assure only one trigger occurs for a kunai projectile?
    }

    protected override void CallbackAtOnTriggerStay2DInternal(ref ProjectileInstanceContext instanceData, Collider2D collider) { }
}
