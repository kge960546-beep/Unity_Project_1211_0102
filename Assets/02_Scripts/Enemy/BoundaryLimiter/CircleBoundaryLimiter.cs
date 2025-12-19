using Unity.VisualScripting;
using UnityEngine;

public class CircleBoundaryLimiter : MonoBehaviour
{
    public Transform center;
    public float radius = 20f;
    
    private void FixedUpdate()
    {
        if (GameManager.Instance == null) return;

        GameContextService gcs = GameManager.Instance.GetService<GameContextService>();
        if (gcs == null) return;

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

            Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
            if (rb != null) rb.position = clampedPos;
            else { target.position = clampedPos; }                
        }
    }
}