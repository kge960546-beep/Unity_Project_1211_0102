using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(fileName = "0002-RevolverBullet-ProjectileLogic", menuName = "Game/Projectile/0002 Revolver bullet Projectile Logic")]
public class _0002_RevolverBullet_ProjectileLogic : ProjectileLogicBase
{
    [field: SerializeField] public float DefaultShotAngle { set; private get; }

    protected override bool IsTargetInRangeInternal(Vector2 projectorPosition, float projectorAzimuth)
    {
        return true;
    }

    protected override void CallbackAtOnEnableInternal(ref ProjectileInstanceContext instanceData, ProjectileInstanceInitializationData initData)
    {
        instanceData.rb.position = initData.currentProjectorPosition;
        // TODO: start position would be the end of a gun barrel instead of the character itself

        Rigidbody2D targetRB = GetTargetLocation(initData.currentProjectorPosition, initData.currentProjectorAzimuth, initData.sequenceNumber);

        float azimuth = initData.currentProjectorAzimuth;
        Unity.Mathematics.math.sincos(azimuth * Mathf.Deg2Rad, out float sin, out float cos);
        Vector2 diff = new Vector2(cos, sin);

        if (null != targetRB)
        {
            diff = targetRB.position - instanceData.rb.position;
            azimuth = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        }

        instanceData.rb.velocity = diff.normalized * DefaultSpeed;
        instanceData.rb.rotation = azimuth;
    }

    protected override void CallbackAtOnDisableInternal(ref ProjectileInstanceContext instanceData) { }
    protected override void CallbackAtFixedUpdateInternal(ref ProjectileInstanceContext instanceData) { }

    protected override void CallbackAtOnTriggerEnter2DInternal(ref ProjectileInstanceContext instanceData, Collider2D collider)
    {
        GameManager.Instance.GetService<PoolingService>().ReturnOrDestroyGameObject(instanceData.obj);
    }

    protected override void CallbackAtOnTriggerStay2DInternal(ref ProjectileInstanceContext instanceData, Collider2D collider) { }

    private Rigidbody2D GetTargetLocation(Vector2 projectorLocation, float azimuth, int sequenceNumber)
    {

        LayerService ls = GameManager.Instance.GetService<LayerService>();
        int enemyLayer = ls.enemyLayer;
        int bossLayer = ls.bossLayer;
        int destructibleItemLayer = ls.destructibleItemLayer;
        LayerMask mask = (1 << enemyLayer) | (1 << bossLayer) | (1 << destructibleItemLayer);

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            projectorLocation,
            DefaultSearchDistance,
            mask);

        if (null == hits || 0 == hits.Length) return null;

        Collider2D candidate =
            hits
                .Select(
                    hit =>
                    {
                        Vector2 direction = (Vector2)hit.transform.position - projectorLocation;
                        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                        float angleDiff = Mathf.Abs(angle - azimuth);
                        return new { angleDiff, hit };
                    })
                .Where(hitInfo => hitInfo.angleDiff <= DefaultShotAngle && null != hitInfo.hit.attachedRigidbody)
                .OrderBy(hitInfo => hitInfo.angleDiff)
                .Select(hitInfo => hitInfo.hit)
                .ElementAtOrDefault(Mathf.Min(sequenceNumber, hits.Length - 1));

        Rigidbody2D result = null;
        if (null != candidate) result = candidate.attachedRigidbody;

        return result;
    }
}
