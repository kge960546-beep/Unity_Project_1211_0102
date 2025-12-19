using System;
using UnityEngine;

public class BarricadeCircleSpawner : MonoBehaviour
{
    [Header("Barricade Settings")]
    public GameObject barricadePrefab;
    public int count = 110;
    public float radius = 20f;
    public Transform spawner;
    private Transform barricadeRoot;

    private event Action onBarricadeCircleSpawnEvent;

    public void SubscribeBarricadeCircleSpawnEvent(Action action)
    {
        onBarricadeCircleSpawnEvent += action;
    }
    public void UnsubscribeBarricadeCircleSpawnEvent(Action action)
    {
        onBarricadeCircleSpawnEvent -= action;
    }
    public void CallCircleBarricadeSpawn()
    {
        if (spawner == null)
        {
#if UNITY_EDITOR
            Debug.LogWarning("스폰할 위치를 선정해주세요");
#endif
            return;
        }
        
        SpawnCircle();
        onBarricadeCircleSpawnEvent?.Invoke();
    }
    public void ClearCircleBarricade()
    {
        if(barricadeRoot != null)
        {
            Destroy(barricadeRoot.gameObject);
            barricadeRoot = null;
        }
    }
    void SpawnCircle()
    {
        if (barricadePrefab == null) return;
        if (count <= 0) return;

        if(barricadeRoot == null)
        {
            var root = new GameObject("Barricades");
            root.transform.position = spawner.position;
            barricadeRoot = root.transform;
        }

        for (int i = 0; i < count; i++)
        {
            float angle = i * Mathf.PI * 2f / Mathf.Max(1, count);

            Vector2 pos = new Vector2(
                spawner.position.x + Mathf.Cos(angle) * radius,
                spawner.position.y + Mathf.Sin(angle) * radius );

            Instantiate(barricadePrefab, pos, Quaternion.identity,barricadeRoot);
        }
    }
}