using UnityEngine;

public class BoundaryLimiterRunner : MonoBehaviour
{
    private IBoundaryLimiter boundaryLimiter;

    private void Awake()
    {
        boundaryLimiter = GetComponent<IBoundaryLimiter>();
    }

    private void LateUpdate()
    {
        boundaryLimiter = GetComponent<IBoundaryLimiter>();
        boundaryLimiter.LimitPosition(transform);
    }
}
