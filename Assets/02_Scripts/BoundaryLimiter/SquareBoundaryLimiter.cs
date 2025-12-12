using UnityEngine;

public class SquareBoundaryLimiter : MonoBehaviour, IBoundaryLimiter
{
    public Transform center;
    public float width = 50f;
    public float height = 30f;

    public void LimitPosition(Transform target)
    {
        Vector2 pos = target.position;
        Vector2 c = center.position;

        float halfW = width * 0.5f;
        float halfH = height * 0.5f;

        pos.x = Mathf.Clamp(pos.x, c.x - halfW, c.x + halfW);
        pos.y = Mathf.Clamp(pos.y, c.y - halfH, c.y + halfH);

        target.position = pos;
    }
}


