using UnityEngine;

public class LineBoundaryLimiter : MonoBehaviour
{
    public Transform center;

    public float lineWidth = 20f;
    public float topY = 5f;
    public float bottomY = -5f;

    private void FixedUpdate()
    {
        if (GameManager.Instance == null) return;
        GameContextService gcs = GameManager.Instance.GetService<GameContextService>();

        if (gcs.Player != null)
            LimitPosition(gcs.Player.transform);
        if(gcs.BossMonster != null)
            LimitPosition(gcs.BossMonster.transform);
    }

    private void LimitPosition(Transform target)
    {
        if (center == null || target == null) return;

        Vector2 pos = target.position;
        Vector2 c = center.position;

        float halfW = lineWidth * 0.5f;

        pos.x = Mathf.Clamp(pos.x, c.x - halfW, c.x + halfW);
        pos.y = Mathf.Clamp(pos.y, bottomY, topY);

        Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
        if (rb != null) rb.position = pos;
        else { target.position = pos; }            
    }
}