using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "1002-Boomerang-ProjectileLogic", menuName = "Game/Projectile/1002 Boomerang Projectile Logic")]
public class _1002_Boomerang_ProjectileLogic : ProjectileLogicBase
{
    // TODO: apply player stat in the calculation
    [field: SerializeField] public float DefaultAcceleration { set; private get; }
    [field: SerializeField] public float DefaultRotationSpeed { set; private get; }

    protected override bool IsTargetInRangeInternal(Vector2 projectorPosition, float projectorAzimuth)
    {
        return null != GetTargetRB(projectorPosition);
    }

    private Rigidbody2D GetTargetRB(Vector2 projectorPosition)
    {
        LayerService ls = GameManager.Instance.GetService<LayerService>();
        int enemyLayer = ls.enemyLayer;
        int bossLayer = ls.bossLayer;
        int destructibleItemLayer = ls.destructibleItemLayer;
        LayerMask mask = (1 << enemyLayer) | (1 << bossLayer) | (1 << destructibleItemLayer);

        Collider2D[] hits = Physics2D.OverlapCircleAll(projectorPosition, DefaultSearchDistance, mask);
        // TODO: may use NonAlloc to minimize GC

        if (null == hits || 0 == hits.Length) return null;

        Rigidbody2D result =
            hits
            .OrderBy(hit => Vector2.Distance(hit.attachedRigidbody.position, projectorPosition))
            .ElementAt(0)
            .attachedRigidbody;

        return result;
    }

    protected override void CallbackAtOnEnableInternal(ref ProjectileInstanceContext instanceData, ProjectileInstanceInitializationData initData)
    {
        Rigidbody2D targetRB = GetTargetRB(initData.currentProjectorPosition);
        Vector2 dir = (targetRB.position - initData.currentProjectorPosition).normalized;

        instanceData.rb.position = initData.currentProjectorPosition;
        instanceData.rb.velocity = dir * DefaultSpeed;
        instanceData.rb.rotation = 0f;
        instanceData.rb.angularVelocity = DefaultRotationSpeed;
    }

    protected override void CallbackAtOnDisableInternal(ref ProjectileInstanceContext instanceData) { }

    protected override void CallbackAtFixedUpdateInternal(ref ProjectileInstanceContext instanceData)
    {

    }

    protected override void CallbackAtOnTriggerEnter2DInternal(ref ProjectileInstanceContext instanceData, Collider2D collider) { }
    protected override void CallbackAtOnTriggerStay2DInternal(ref ProjectileInstanceContext instanceData, Collider2D collider) { }
}
