using UnityEngine;

[CreateAssetMenu(fileName = "CircleBarricadePattern", menuName = "Game/Stage/Barricade/CircleSpawn SO")]
public class CircleBarricadePatternSO : BarricadePatternSO
{
    public float radius = 10f;
    public int count = 30;
    public Vector3[] GetPositions(Vector3 center)
    {
        Vector3[] result = new Vector3[count];

        for(int i = 0; i< count; i++)
        {
            float t = i / (float)count;
            float angle = Mathf.PI * 2f * t;

            result[i] = center + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0f);
        }
        return result;
    }
}
