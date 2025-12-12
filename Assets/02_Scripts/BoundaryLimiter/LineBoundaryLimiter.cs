using UnityEngine;

public class LineBoundaryLimiter : MonoBehaviour, IBoundaryLimiter
{
    public Transform center;

    public float lineWidth = 20f;
    public float topY = 5f;
    public float bottomY = -5f;

    public void LimitPosition(Transform target)
    {
        Vector2 pos = target.position;
        Vector2 c = center.position;

        float halfW = lineWidth * 0.5f;

        pos.x = Mathf.Clamp(pos.x, c.x - halfW, c.x + halfW);
        pos.y = Mathf.Clamp(pos.y, bottomY, topY);

        target.position = pos;
    }
}
