using UnityEngine;

public class CircleBoundaryLimiter : MonoBehaviour
{
    public Transform center;
    public float radius = 20f;

    private void FixedUpdate()
    {
        GameContextService gcs = GameManager.Instance.GetService<GameContextService>();

        LimitPosition(gcs.Player.transform);
        LimitPosition(gcs.BossMonster.transform);
    }

    private void LimitPosition(Transform target)
    {
        Vector2 pos = target.position;
        Vector2 c = center.position;

        float maxRadius = radius * 0.98f;

        Vector2 offset = pos - c;
        float dist = offset.magnitude;

        if (dist > maxRadius)
        {
            Vector2 clampedPos = c + offset.normalized * maxRadius;
            target.position = clampedPos;
        }
    }
}