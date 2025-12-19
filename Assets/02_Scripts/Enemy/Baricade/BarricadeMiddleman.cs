using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarricadeMiddleman : MonoBehaviour
{
    [Header("Circle")]
    private BarricadeCircleSpawner circleSpawner;
    private CircleBoundaryLimiter circleLimiter;

    [Header("Line")]
    private BarricadeLineSpawner lineSpawner;
    private LineBoundaryLimiter lineLimiter;

    [Header("Square")]
    private BarricadeSquareSpawner squareSpawner;
    private LineBoundaryLimiter squareLimiter;

   //private void OnEnable()
   //{
   //    
   //    if(circleSpawner != null && circleLimiter != null)
   //    {
   //        circleSpawner.SubscribeBarricadeCircleSpawnEvent();
   //    }
   //}
   //private void OnDisable()
   //{
   //    if (circleSpawner != null && circleLimiter != null)
   //    {
   //        circleSpawner.SubscribeBarricadeCircleSpawnEvent();
   //    }
   //}
}
