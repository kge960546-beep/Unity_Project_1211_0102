using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "1014-LandMine-ProjectileLogic", menuName = "Game/Projectile/1014 Land mine Projectile Logic")]
public class _1014_LandMine_ProjectileLogic : ProjectileLogicBase
{
    // TODO: apply player stat in the calculation
    [field: SerializeField] public GameObject RadialDistanceUIPrefab { set; private get; }
    [field: SerializeField] public GameObject ExplosionFXPrefab { set; private get; }
    [field: SerializeField] public float DefaultSummonDistance { set; private get; }
    [field: SerializeField] public float DefaultFuseDelayFrame { set; private get; }

    protected override bool IsTargetInRangeInternal(Vector2 projectorPosition, float projectorAzimuth)
    {
        return true;
    }

    protected override void CallbackAtOnEnableInternal(ref ProjectileInstanceContext instanceData, ProjectileInstanceInitializationData initData)
    {
        float azimuth = GameManager.Instance.GetService<RandomService>().random.NextFloat(360f);
        // TODO: this does not work as random is separately calculated... need to put into ref initData


        azimuth += 360f * initData.sequenceNumber / initData.sequenceCount;
        Unity.Mathematics.math.sincos(azimuth * Mathf.Deg2Rad, out float sin, out float cos);

        instanceData.rb.position = initData.currentProjectorPosition + new Vector2(cos * DefaultSummonDistance, sin * DefaultSummonDistance);
        instanceData.rb.velocity = Vector2.zero;
        instanceData.rb.rotation = 0f;

        //GameObject radialDistanceUI = GameManager.Instance.GetService<PoolingService>().GetOrCreateInactivatedGameObject(RadialDistanceUIPrefab);
        //radialDistanceUI.name = "FuseRadius";
        //radialDistanceUI.transform.SetParent(instanceData.obj.transform);
        //radialDistanceUI.transform.SetLocalPositionAndRotation(Vector2.zero, Quaternion.identity);
        //radialDistanceUI.transform.localScale = new Vector3(DefaultSearchDistance, DefaultSearchDistance, DefaultSearchDistance);
        //radialDistanceUI.SetActive(true);
    }

    protected override void CallbackAtOnDisableInternal(ref ProjectileInstanceContext instanceData)
    {
        //GameObject fuseRadius = instanceData.obj.transform.Find("FuseRadius").gameObject;
        //if (null != fuseRadius) GameManager.Instance.GetService<PoolingService>().ReturnOrDestroyGameObject(fuseRadius);

        // TODO: check possibility for pooling on vfx as it may be self-destroyed.
        GameObject explosionEffect = Instantiate(ExplosionFXPrefab);
        explosionEffect.transform.SetLocalPositionAndRotation(instanceData.obj.transform.position, Quaternion.identity);

        LayerService ls = GameManager.Instance.GetService<LayerService>();
        int enemyLayer = ls.enemyLayer;
        int bossLayer = ls.bossLayer;
        int destructibleItemLayer = ls.destructibleItemLayer;
        LayerMask mask = (1 << enemyLayer) | (1 << bossLayer) | (1 << destructibleItemLayer);

        var targets = Physics2D.OverlapCircleAll(instanceData.obj.transform.position, DefaultColliderRadius, mask);
        foreach (var target in targets)
        {
            if (target.TryGetComponent(out IDamageable damageable)) InflictDamage(ref instanceData, damageable);
        }
    }

    protected override void CallbackAtFixedUpdateInternal(ref ProjectileInstanceContext instanceData)
    {
        int hitFrame = instanceData.hitCount >> 1;
        bool isHitInPreviousFrame = (0 != (instanceData.hitCount & 1));

        if (isHitInPreviousFrame) hitFrame++;
        else hitFrame--;

        instanceData.hitCount = hitFrame << 1;

        if (DefaultFuseDelayFrame <= hitFrame) GameManager.Instance.GetService<PoolingService>().ReturnOrDestroyGameObject(instanceData.obj);

        //GameObject fuseRadius = instanceData.obj.transform.Find("FuseRadius").gameObject;
        //if (null != fuseRadius && fuseRadius.TryGetComponent(out SpriteRenderer sr))
        //{
        //    float h, s, v;
        //    Color.RGBToHSV(sr.color, out h, out s, out v);
        //    float hNext = 0f;
        //    float sNext = Mathf.Lerp(0f, 1f, hitFrame / (float)DefaultFuseDelayFrame);
        //    float vNext = v;
        //    Color colorNext = Color.HSVToRGB(hNext, sNext, vNext);
        //    colorNext.a = sr.color.a;
        //    sr.color = colorNext;
        //}
    }

    protected override void CallbackAtOnTriggerEnter2DInternal(ref ProjectileInstanceContext instanceData, Collider2D collider)
    {
        // hitCount hack: real hit count is saved with 31 bits and remaining 1 bit stores whether any collision occured or not in this frame
        instanceData.hitCount = instanceData.hitCount | 1;
    }

    protected override void CallbackAtOnTriggerStay2DInternal(ref ProjectileInstanceContext instanceData, Collider2D collider)
    {
        // hitCount hack: real hit count is saved with 31 bits and remaining 1 bit stores whether any collision occured or not in this frame
        instanceData.hitCount = instanceData.hitCount | 1;
    }

    private void InflictDamage(ref ProjectileInstanceContext instanceData, IDamageable target)
    {
        RandomService rs = GameManager.Instance.GetService<RandomService>();
        bool isCritical = rs.random.NextFloat(0f, 1f) < CriticalRate;
        float randomRatio = GameManager.Instance.GetService<RandomService>().random.NextFloat(1f - damageVariationRatio, 1f + damageVariationRatio);
        float damageCeofficient = OrdinaryDamageCoefficientTable[instanceData.level - 1] * (isCritical ? CriticalDamageMultiplier : 1f);
        // TODO: get player attack value
        int damage = (int)(/*PLAYERATTACK*/ 100 * damageCeofficient * randomRatio);
        Debug.Log(randomRatio);
        GameManager.Instance.GetService<DamageManagementService>().QueueDamage(damage, instanceData.obj, target, isCritical);
    }
}
