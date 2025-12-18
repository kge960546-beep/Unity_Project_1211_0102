using UnityEngine;

public class BarricadeCircleSpawner : MonoBehaviour
{
    [Header("Barricade Settings")]
    public GameObject barricadePrefab;
    public int count = 110;
    public float radius = 20f;
    public Transform spawner;

    private void Start()
    {
        SpawnCircle();
    }

    void SpawnCircle()
    {
        for (int i = 0; i < count; i++)
        {
            float angle = i * Mathf.PI * 2f / Mathf.Max(1, count);

            Vector2 pos = new Vector2(
                spawner.position.x + Mathf.Cos(angle) * radius,
                spawner.position.y + Mathf.Sin(angle) * radius
            );

            Instantiate(barricadePrefab, pos, Quaternion.identity);
        }
    }
}


