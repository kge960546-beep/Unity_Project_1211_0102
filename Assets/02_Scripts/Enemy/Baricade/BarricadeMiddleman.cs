using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarricadeMiddleman : MonoBehaviour
{
    [Header("Circle")]
    [SerializeField] private BarricadeCircleSpawner circleSpawner;
    [SerializeField] private CircleBoundaryLimiter circleLimiter;

    [Header("Line")]
    [SerializeField] private BarricadeLineSpawner lineSpawner;
    [SerializeField] private LineBoundaryLimiter lineLimiter;

    [Header("Square")]
    [SerializeField] private BarricadeSquareSpawner squareSpawner;
    [SerializeField] private SquareBoundaryLimiter squareLimiter;

    private void OnEnable()
    {
        if(circleSpawner != null)
            circleSpawner.SubscribeBarricadeCircleSpawnEvent(OnCircleBarricadeSpawned);
        if (lineSpawner != null)
            lineSpawner.SubscribeBarricadeLineSpawnEvent(OnLineBarricadeSpawned);
        if (squareSpawner != null)
            squareSpawner.SubscribeBarricadeSquareSpawnEvent(OnSquareBarricadeSpawned);
    }
    private void OnDisable()
    {
        if (circleSpawner != null)
            circleSpawner.UnsubscribeBarricadeCircleSpawnEvent(OnCircleBarricadeSpawned);
        if (lineSpawner != null)
            lineSpawner.UnsubscribeBarricadeLineSpawnEvent(OnLineBarricadeSpawned);
        if (squareSpawner != null)
            squareSpawner.UnsubscribeBarricadeSquareSpawnEvent(OnSquareBarricadeSpawned);
    }
    private void AllDisable()
    {
        if (circleLimiter != null)
            circleLimiter.enabled = false;
        if (lineLimiter != null)
            lineLimiter.enabled = false;
        if (squareLimiter != null)
            squareLimiter.enabled = false;
    }
    private void OnCircleBarricadeSpawned(Transform center)
    {
        AllDisable();

        if (circleLimiter == null) return;
        if (center == null) return;

        circleLimiter.center = center;
        circleLimiter.enabled = true;
    }
    private void OnLineBarricadeSpawned(Transform center)
    {
        AllDisable();

        if (lineLimiter == null) return;
        if (center == null) return;

        lineLimiter.center = center;        
        lineLimiter.enabled = true;
    }
    private void OnSquareBarricadeSpawned(Transform center)
    {
        AllDisable();

        if (squareLimiter == null) return;
        if (center == null) return;

        squareLimiter.center = center;        
        squareLimiter.enabled = true;
    }
}