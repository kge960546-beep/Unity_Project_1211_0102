using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingService : MonoBehaviour, IGameManagementService
{
    private GameObject pooler;

    /// <summary>
    /// Map from tracked GameObject instances to their prefabs.
    /// </summary>
    private Dictionary<GameObject, GameObject> trackingMap;

    /// <summary>
    /// Map from prefabs to stacks for their pooled instances available to use.
    /// </summary>
    private Dictionary<GameObject, Stack<GameObject>> poolMap;

    private void OnEnable()
    {
        pooler = new GameObject("Pooler");
        pooler.SetActive(false);

        trackingMap = new Dictionary<GameObject, GameObject>();
        poolMap = new Dictionary<GameObject, Stack<GameObject>>();
    }

    private void OnDisable()
    {
        GameObject.Destroy(pooler);
    }

    public GameObject GetOrCreateInactivatedGameObject(GameObject prefab)
    {
        Stack<GameObject> pool = GetOrCreatePool(prefab);
        if (null == pool) return null;
        if (!pool.TryPop(out GameObject result) || null == result)
        {
            result = GameObject.Instantiate(prefab, pooler.transform);
            if (null == result) return null;
            trackingMap[result] = prefab;
        }
        result.SetActive(false);
        result.transform.SetParent(null);
        return result;
    }

    public void ReturnOrDestroyGameObject(GameObject obj)
    {
        if (null == obj) return;
        if (!trackingMap.TryGetValue(obj, out GameObject prefab) || !poolMap.TryGetValue(prefab, out Stack<GameObject> pool))
        {
            trackingMap.Remove(obj);
            GameObject.Destroy(obj);
            return;
        }
        obj.transform.SetParent(pooler.transform);
        pool.Push(obj);
    }

    public void Shrink()
    {
        foreach(GameObject prefab in poolMap.Keys) ShrinkPool(prefab);
    }

    public void ShrinkPool(GameObject prefab)
    {
        if (!poolMap.TryGetValue(prefab, out Stack<GameObject> pool)) return;
        while (pool.Count > 0)
        {
            GameObject obj = pool.Pop();
            trackingMap.Remove(obj);
            GameObject.Destroy(obj);
        }
    }

    private Stack<GameObject> GetOrCreatePool(GameObject prefab)
    {
        if (!poolMap.TryGetValue(prefab, out Stack<GameObject> result))
        {
            result = new Stack<GameObject>();
            if (null == result || !poolMap.TryAdd(prefab, result)) return null;
            // TODO: reserve object?
        }
        return result;
    }

    // TODO:
    // if pool should persist regardless of scene unloading, it needs to move the pooler object to the newly created scene with additive loading.
    // can it use DDOL? run a test if needed
}
