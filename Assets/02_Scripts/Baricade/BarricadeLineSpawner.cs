using UnityEngine;

public class BarricadeLineSpawner : MonoBehaviour
{
    [Header("Barricade Settings")]
    public GameObject barricadePrefab;

    public int cellCount = 20;
    public float cellSize = 1.0f;
    public float heightOffset = 5f;
    public Transform spawner;

    private void Start()
    {
        SpawnLines();
    }

    void SpawnLines()
    {
        float totalWidth = (cellCount - 1) * cellSize;

        for (int i = 0; i < cellCount; i++)
        {
            float xPos = spawner.position.x - (totalWidth * 0.5f) + (i * cellSize);

            // 위쪽 라인
            Instantiate(
                barricadePrefab,
                new Vector2(xPos, spawner.position.y + heightOffset),
                Quaternion.identity
            );

            // 아래쪽 라인
            Instantiate(
                barricadePrefab,
                new Vector2(xPos, spawner.position.y - heightOffset),
                Quaternion.identity
            );
        }
    }
}

