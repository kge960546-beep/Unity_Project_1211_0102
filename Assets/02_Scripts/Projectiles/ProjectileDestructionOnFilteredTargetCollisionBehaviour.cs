using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDestructionOnFilteredTargetCollisionBehaviour : MonoBehaviour
{
    [SerializeField] private LayerMask selectiveFilter;
    public enum PrefabFilteringMode
    {
        Exclusive,
        Inclusive,
    }
    [SerializeField] private PrefabFilteringMode perfabFilteringMode;
    [SerializeField] private GameObject[] filteringPrefabs;

    private HashSet<GameObject> runtimeFilteringPrefabsCache;

    private void Awake()
    {
        CacheExcludedPrefabs();
    }

    private void OnValidate()
    {
        CacheExcludedPrefabs();
    }

    private void CacheExcludedPrefabs()
    {
        runtimeFilteringPrefabsCache = new HashSet<GameObject>(filteringPrefabs);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (0 != (selectiveFilter & (1 << collision.gameObject.layer)))
        {
            bool filterResult = perfabFilteringMode switch
                {
                    PrefabFilteringMode.Inclusive => runtimeFilteringPrefabsCache.Contains(collision.gameObject),
                    PrefabFilteringMode.Exclusive => !runtimeFilteringPrefabsCache.Contains(collision.gameObject),
                    _ => false
                };

            if (filterResult) Destroy(gameObject);
        }
    }
}
