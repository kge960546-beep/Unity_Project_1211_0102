using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "1014-LandMine-ProjectileLogic", menuName = "Game/Projectile/1014 Land mine Projectile Logic")]
public class _1014_LandMine_ProjectileLogic : ProjectileLogicBase
{
    // TODO: apply player stat in the calculation
    [field: SerializeField] public GameObject RadialDistanceUIPrefab { set; private get; }
    [field: SerializeField] public GameObject DefaultExplosionDamageRadius { set; private get; }
    [field: SerializeField] public GameObject ExplosionEffectPrefab { set; private get; }
    [field: SerializeField] public float DefaultSummonDistance { set; private get; }
    [field: SerializeField] public float DefaultFuseDelayFrame { set; private get; }

    protected override bool IsTargetInRangeInternal(Vector2 projectorPosition, float projectorAzimuth)
    {
        return true;
    }

    protected override void CallbackAtOnEnableInternal(ref ProjectileInstanceContext instanceData, ProjectileInstanceInitializationData initData)
    {
        float azimuth = GameManager.Instance.GetService<RandomService>().Random.NextFloat(360f);
        // TODO: this does not work as random is separately calculated... need to put into ref initData


        azimuth += 360f * initData.sequenceNumber / initData.sequenceCount;
        Unity.Mathematics.math.sincos(azimuth * Mathf.Deg2Rad, out float sin, out float cos);

        instanceData.rb.position = initData.currentProjectorPosition + new Vector2(cos * DefaultSummonDistance, sin * DefaultSummonDistance);
        instanceData.rb.velocity = Vector2.zero;
        instanceData.rb.rotation = 0f;

        // TODO: radial distance UI
        //GameObject radialDistanceUI = GameManager.Instance.GetService<PoolingService>().GetOrCreateInactivatedGameObject(radialDistanceUIPrefab);
        //radialDistanceUI.transform.SetParent(instanceData.obj.transform);
        //radialDistanceUI.transform.SetLocalPositionAndRotation(Vector2.zero, Quaternion.identity);
        //radialDistanceUI.SetActive(true);
    }

    protected override void CallbackAtOnDisableInternal(ref ProjectileInstanceContext instanceData)
    {
        // TODO: radial distance UI and explosion effect
        // need refactor as projectiles have a child object to manage sprite
        //foreach (Transform child in instanceData.obj.transform)
        //{
        //    GameManager.Instance.GetService<PoolingService>().ReturnOrDestroyGameObject(child.gameObject);
        //}

        //GameObject explosionEffect = GameManager.Instance.GetService<PoolingService>().GetOrCreateInactivatedGameObject(explosionEffectPrefab);
        //explosionEffect.transform.SetLocalPositionAndRotation(Vector2.zero, Quaternion.identity);
        //explosionEffect.SetActive(true);
    }

    protected override void CallbackAtFixedUpdateInternal(ref ProjectileInstanceContext instanceData) { }

    protected override void CallbackAtOnTriggerEnter2DInternal(ref ProjectileInstanceContext instanceData, Collider2D collider)
    {
        instanceData.hitCount++;

        if (DefaultFuseDelayFrame == instanceData.hitCount)
        {
            // TODO: give damage. is it okay to call damage methods from this method? confirmation needed.

            GameManager.Instance.GetService<PoolingService>().ReturnOrDestroyGameObject(instanceData.obj);
        }
    }

    protected override void CallbackAtOnTriggerStay2DInternal(ref ProjectileInstanceContext instanceData, Collider2D collider)
    {
    }
}
