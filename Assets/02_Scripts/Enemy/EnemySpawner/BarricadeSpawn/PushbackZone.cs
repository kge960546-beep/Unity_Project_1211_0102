using Unity.VisualScripting;
using UnityEngine;

public class PushbackZone : MonoBehaviour
{
    public Transform center;
    public float radius = 5f;
    public float pushStrenght = 10f;

    private void OnTriggerStay2D(Collider2D other)
    {
        if(!other.CompareTag("Player")) return;

        Rigidbody2D rb = other.attachedRigidbody;
        if(rb == null) return;

        Vector2 dir = (other.transform.position - center.position).normalized;

        Vector3 boundaryPos = (Vector2)center.position + dir * radius;

        rb.MovePosition(Vector2.Lerp(other.transform.position, boundaryPos, Time.deltaTime * pushStrenght));
    }
}
