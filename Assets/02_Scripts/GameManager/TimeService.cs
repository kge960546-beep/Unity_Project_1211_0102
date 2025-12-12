using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is for only one global timer.
/// If multiple timers are needed, modify this class to store multiple timers in a dictionary.
/// </summary>
[DefaultExecutionOrder(int.MaxValue)]
public class TimeService : MonoBehaviour, IGameManagementService
{
    public float accumulatedFixedDeltaTime { get; private set; }
    public float accumulatedDeltaTime { get; private set; }
    private bool isResetRequestedForAccumulatedFixedDeltaTime = false;
    private bool isResetRequestedForAccumulatedDeltaTime = false;

    private void FixedUpdate()
    {
        if (isResetRequestedForAccumulatedFixedDeltaTime)
        {
            accumulatedFixedDeltaTime = 0;
            isResetRequestedForAccumulatedFixedDeltaTime = false;
            return;
        }
        accumulatedFixedDeltaTime += Time.fixedDeltaTime;
    }

    private void Update()
    {
        if (isResetRequestedForAccumulatedDeltaTime)
        {
            accumulatedDeltaTime = 0;
            isResetRequestedForAccumulatedDeltaTime = false;
            return;
        }
        accumulatedDeltaTime += Time.deltaTime;
    }

    public void ResetTime()
    {
        isResetRequestedForAccumulatedFixedDeltaTime = true;
        isResetRequestedForAccumulatedDeltaTime = true;
    }
}
