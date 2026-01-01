
using System;
using UnityEngine;

public class BarricadeSquareSpawner : MonoBehaviour
{
    [Header("Barricade Settings")]
    public GameObject barricadePrefab;
    public int count = 60;
    public float width = 50f;
    public float height = 30f;
    public Transform spawner;
    private Transform barricadeRoot;

    private event Action<Transform> onBarricadeSquareSpawnEvent;
    public void SubscribeBarricadeSquareSpawnEvent(Action<Transform> action)
    {
        onBarricadeSquareSpawnEvent += action;
    }
    public void UnsubscribeBarricadeSquareSpawnEvent(Action<Transform> action)
    {
        onBarricadeSquareSpawnEvent -= action;
    }
    public void CallSquareBarricadeSpawn(bool enableBoundary = true, bool spawnBarricade = true)
    {
        if (spawner == null)
        {
#if UNITY_EDITOR
            Debug.LogWarning("스폰할 위치를 선정해주세요");
#endif
            return;
        }
        if (enableBoundary)
            onBarricadeSquareSpawnEvent?.Invoke(spawner);

        if (spawnBarricade)
            SpawnSquare();
    }
    public void ClearSquareBarricade()
    {
        if (barricadeRoot != null)
        {
            Destroy(barricadeRoot.gameObject);
            barricadeRoot = null;
        }
    }
    void SpawnSquare()
    {
        if (barricadePrefab == null) return;
        if (count < 2) return;

        if (barricadeRoot == null)
        {
            var root = new GameObject("Barricades");
            root.transform.position = spawner.position;
            barricadeRoot = root.transform;
        }

        float left = spawner.position.x - width / 2;
        float right = spawner.position.x + width / 2;
        float top = spawner.position.y + height / 2;
        float bottom = spawner.position.y - height / 2;

        // 위쪽에서 아래 라인
        for (int i = 0; i < count; i++)
        {
            float t = (float)i / (count - 1);
            float x = Mathf.Lerp(left, right, t);

            Instantiate(barricadePrefab, new Vector2(x, top), Quaternion.identity, barricadeRoot);
            Instantiate(barricadePrefab, new Vector2(x, bottom), Quaternion.identity, barricadeRoot);
        }

        // 왼쪽에서 오른쪽 라인
        for (int i = 0; i < count; i++)
        {
            float t = (float)i / (count - 1);
            float y = Mathf.Lerp(bottom, top, t);

            Instantiate(barricadePrefab, new Vector2(left, y), Quaternion.identity, barricadeRoot);
            Instantiate(barricadePrefab, new Vector2(right, y), Quaternion.identity, barricadeRoot);
        }
    }
}