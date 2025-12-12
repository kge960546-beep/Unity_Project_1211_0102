using UnityEngine;

public class CircleBoundaryLimiter : MonoBehaviour, IBoundaryLimiter
{
    public Transform center;
    public float radius = 20f;

    public void LimitPosition(Transform target)
    {
        Vector2 pos = target.position;
        Vector2 c = center.position;

        float dist = Vector2.Distance(pos, c);
        float max = radius * 0.98f;

        if (dist > max)
        {
            Vector2 dir = (pos - c).normalized;
            target.position = c + dir * max;
        }
    }
}

