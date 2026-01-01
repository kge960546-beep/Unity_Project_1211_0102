using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "0005-KatanaSlash-ProjectileLogic", menuName = "Game/Projectile/0005 Katana slash Projectile Logic")]
public class _0005_KatanaSlash_ProjectileLogic : ProjectileLogicBase
{
    [field: SerializeField] public float DefaultExpansionTime { set; private get; }
    [field: SerializeField] public float DefaultExpandedDisplacementMagnitude { set; private get; }
    [field: SerializeField] private GameObject SharedCommonProjectilePrefab { set; get; }
    [field: SerializeField] public _0005_KatanaAura_ProjectileLogic CatanaAuraLogic { set; private get; }

    protected override bool IsTargetInRangeInternal(Vector2 projectorPosition, float projectorAzimuth)
    {
        return true;
    }

    protected override void CallbackAtOnEnableInternal(ref ProjectileInstanceContext instanceData, ProjectileInstanceInitializationData initData)
    {
        bool isReverse = 0 != initData.sequenceNumber % 2;

        instanceData.rb.position = initData.currentProjectorPosition;
        instanceData.rb.rotation = initData.initialProjectorAzimuthSnapshot + (isReverse ? 180f : 0f);
        ProjectileSpawnUtility.Spawn(SharedCommonProjectilePrefab, CatanaAuraLogic, initData);
    }

    protected override void CallbackAtFixedUpdateInternal(ref ProjectileInstanceContext instanceData)
    {
        float timer = instanceData.timer;

        PlayerFeedService pfs = GameManager.Instance.GetService<PlayerFeedService>();
        Vector2 playerPosition = pfs.playerPosition;

        Unity.Mathematics.math.sincos(instanceData.rb.rotation * Mathf.Deg2Rad, out float sin, out float cos);
        Vector2 direction = new Vector2(cos, sin);

        float scale = Mathf.Clamp01(timer / DefaultExpansionTime);
        instanceData.rb.transform.localScale = new Vector3(scale, scale, scale);
        instanceData.rb.position = playerPosition + direction * (DefaultExpandedDisplacementMagnitude * scale);
    }

    protected override void CallbackAtOnDisableInternal(ref ProjectileInstanceContext instanceData) { }

    protected override void CallbackAtOnTriggerEnter2DInternal(ref ProjectileInstanceContext instanceData, Collider2D collider) { }
    protected override void CallbackAtOnTriggerStay2DInternal(ref ProjectileInstanceContext instanceData, Collider2D collider) { }
}
