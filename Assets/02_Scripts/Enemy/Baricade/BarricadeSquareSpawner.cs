
using UnityEngine;

public class BarricadeSquareSpawner : MonoBehaviour
{
    [Header("Barricade Settings")]
    public GameObject barricadePrefab;
    public int count = 60;
    public float width = 50f;
    public float height = 30f;
    public Transform spawner;

    private void Start()
    {
        SpawnSquare();
    }

    void SpawnSquare()
    {
        float left = spawner.position.x - width / 2;
        float right = spawner.position.x + width / 2;
        float top = spawner.position.y + height / 2;
        float bottom = spawner.position.y - height / 2;

        // 위쪽에서 아래 라인
        for (int i = 0; i < count; i++)
        {
            float t = (float)i / (count - 1);
            float x = Mathf.Lerp(left, right, t);

            Instantiate(barricadePrefab, new Vector2(x, top), Quaternion.identity);
            Instantiate(barricadePrefab, new Vector2(x, bottom), Quaternion.identity);
        }

        // 왼쪽에서 오른쪽 라인
        for (int i = 0; i < count; i++)
        {
            float t = (float)i / (count - 1);
            float y = Mathf.Lerp(bottom, top, t);

            Instantiate(barricadePrefab, new Vector2(left, y), Quaternion.identity);
            Instantiate(barricadePrefab, new Vector2(right, y), Quaternion.identity);
        }
    }
}






