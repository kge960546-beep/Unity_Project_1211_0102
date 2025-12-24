using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "1006-Brick-ProjectileLogic", menuName = "Game/Projectile/1006 Brick Projectile Logic")]
public class _1006_Brick_ProjectileLogic : ProjectileLogicBase
{
    [field: SerializeField] public float DefaultGravity { set; private get; } // TODO: apply player stat in the calculation
    [field: SerializeField] public float DefaultSearchDistanceAspectRatio { set; private get; } // TODO: apply player stat in the calculation
    [field: SerializeField] public float DefaultSearchAreaVerticalOffsetRatio { set; private get; } // TODO: apply player stat in the calculation

    protected override bool IsTargetInRangeInternal(Vector2 projectorPosition, float projectorAzimuth)
    {
        return null != GetTargetRB(projectorPosition, 0);
    }

    protected override void CallbackAtOnEnableInternal(ref ProjectileInstanceContext instanceData, ProjectileInstanceInitializationData initData)
    {
        Vector2 projectorPosition = initData.currentProjectorPosition;
        instanceData.rb.position = projectorPosition;
        instanceData.rb.rotation = 0f;

        Rigidbody2D targetRB = GetTargetRB(projectorPosition, initData.sequenceNumber);

        float velocityX = 0f;

        if (null != targetRB)
        {
            var solution =
                MathSolverUtility.SolveQuadraticEquation(
                    0.5f * DefaultGravity,
                    DefaultSpeed - targetRB.velocity.y,
                    projectorPosition.y - targetRB.position.y);

            float impactTime = Mathf.Max(solution.Item1 ?? -1f, solution.Item2 ?? -1f);
            Vector2 targetPosition = (impactTime > 0f) ? targetRB.position + targetRB.velocity * impactTime : projectorPosition;
            // TODO: this calculation is not working because the enemy is not controlled with velocity

            velocityX = (targetPosition.x - projectorPosition.x) / impactTime;
        }

        instanceData.rb.velocity = new Vector2(velocityX, DefaultSpeed);

        // TODO: do we need to rotate bricks?
    }

    protected override void CallbackAtOnDisableInternal(ref ProjectileInstanceContext instanceData) { }

    protected override void CallbackAtFixedUpdateInternal(ref ProjectileInstanceContext instanceData)
    {
        instanceData.rb.velocity = instanceData.rb.velocity + new Vector2(0, DefaultGravity) * Time.fixedDeltaTime;
    }

    protected override void CallbackAtOnTriggerEnter2DInternal(ref ProjectileInstanceContext instanceData, Collider2D collider)
    {
        instanceData.hitCount++;

        if (3 == instanceData.hitCount) GameManager.Instance.GetService<PoolingService>().ReturnOrDestroyGameObject(instanceData.obj);
    }

    protected override void CallbackAtOnTriggerStay2DInternal(ref ProjectileInstanceContext instanceData, Collider2D collider) { }

    private Rigidbody2D GetTargetRB(Vector2 projectorPosition, int sequenceNumber)
    {
        float searchBoxWidth = DefaultSearchDistance;
        float searchBoxHeight = searchBoxWidth * DefaultSearchDistanceAspectRatio;
        float searchBoxVerticalOffset = searchBoxHeight * DefaultSearchAreaVerticalOffsetRatio;

        LayerService ls = GameManager.Instance.GetService<LayerService>();
        int enemyLayer = ls.enemyLayer;
        int bossLayer = ls.bossLayer;
        int destructibleItemLayer = ls.destructibleItemLayer;
        LayerMask mask = (1 << enemyLayer) | (1 << bossLayer) | (1 << destructibleItemLayer);

        Collider2D[] hits = Physics2D.OverlapBoxAll(
            new Vector2(projectorPosition.x, projectorPosition.y + searchBoxVerticalOffset),
            new Vector2(searchBoxWidth, searchBoxHeight),
            0f,
            mask);

        if (null == hits || 0 == hits.Length) return null;

        Rigidbody2D result =
            hits
                .OrderBy(hit => (null != hit.attachedRigidbody) ? Mathf.Abs(hit.attachedRigidbody.position.x - projectorPosition.x) : float.MaxValue)
                .ElementAt(Mathf.Min(sequenceNumber, hits.Length - 1))
                .attachedRigidbody;

        return result;
    }
}
