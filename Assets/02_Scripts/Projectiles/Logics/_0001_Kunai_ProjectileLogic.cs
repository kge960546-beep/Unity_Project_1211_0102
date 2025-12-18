using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "0001-Kunai-ProjectileLogic", menuName = "Game/Projectile/0001 Kunai Projectile Logic")]
public class _0001_Kunai_ProjectileLogic : ProjectileLogicBase
{
    [field: SerializeField] public float DefaultSpeed { set; private get; } // TODO: apply player stat in the calculation

    protected override void CallbackAtOnEnableInternal(ref ProjectileInstanceContext instanceData, ProjectileInstanceInitializationData initData)
    {
        GameObject target = GetNearstEnemy(initData.projectorPosition);
        instanceData.rb.position = initData.projectorPosition;
        if (null != target)
        {
            Vector2 dir = ((Vector2)target.transform.position - initData.projectorPosition).normalized;
            instanceData.rb.velocity = dir * DefaultSpeed;
            instanceData.rb.rotation = Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg;
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
        Destroy(instanceData.obj);
    }

    protected override void CallbackAtOnTriggerStay2DInternal(ref ProjectileInstanceContext instanceData, Collider2D collider) { }

    private GameObject GetNearstEnemy(Vector2 position)
    {
        LayerService ls = GameManager.Instance.GetService<LayerService>();
        int enemyLayer = ls.enemyLayer;
        int bossLayer = ls.bossLayer;
        LayerMask mask = (1 << enemyLayer) | (1 << bossLayer);
        Collider2D[] candidates = Physics2D.OverlapCircleAll(position, DefaultSearchRadius, mask);
        float minimumDistance = float.MaxValue;
        GameObject target = null;
        foreach (var candidate in candidates)
        {
            float distance = Vector2.Distance(position, candidate.transform.position);
            if (distance < minimumDistance)
            {
                minimumDistance = distance;
                target = candidate.gameObject;
            }
        }
        return target;
    }
}
