using System;
using UnityEngine;

public class BarricadeLineSpawner : MonoBehaviour
{
    [Header("Barricade Settings")]
    public GameObject barricadePrefab;

    public int cellCount = 20;
    public float cellSize = 1.0f;
    public float heightOffset = 5f;
    public Transform spawner;
    private Transform barricadeRoot;

    private event Action onBarricadeLineSpawnEvent;
    public void SubscribeBarricadeLineSpawnEvent(Action action)
    {
        onBarricadeLineSpawnEvent += action;
    }
    public void UnsubscribeBarricadeLineSpawnEvent(Action action)
    {
        onBarricadeLineSpawnEvent -= action;
    }

    public void CallLineBarricadeSpawn()
    {
        if (spawner == null)
        {
#if UNITY_EDITOR
            Debug.LogWarning("스폰할 위치를 선정해주세요");
#endif
            return;
        }

        SpawnLines();
        onBarricadeLineSpawnEvent?.Invoke();
    }
    public void ClearLineBarricade()
    {
        if (barricadeRoot != null)
        {
            Destroy(barricadeRoot.gameObject);
            barricadeRoot = null;
        }
    }
    void SpawnLines()
    {
        if (barricadePrefab == null) return;
        if (cellCount <= 0) return;

        if (barricadeRoot == null)
        {
            var root = new GameObject("Barricades");
            root.transform.position = spawner.position;
            barricadeRoot = root.transform;
        }

        float totalWidth = (cellCount - 1) * cellSize;

        for (int i = 0; i < cellCount; i++)
        {
            float xPos = spawner.position.x - (totalWidth * 0.5f) + (i * cellSize);

            // 위쪽 라인
            Instantiate(
                barricadePrefab,
                new Vector2(xPos, spawner.position.y + heightOffset),
                Quaternion.identity,barricadeRoot
            );

            // 아래쪽 라인
            Instantiate(
                barricadePrefab,
                new Vector2(xPos, spawner.position.y - heightOffset),
                Quaternion.identity,barricadeRoot
            );
        }
    }
}

