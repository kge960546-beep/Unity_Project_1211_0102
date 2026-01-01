using UnityEngine;
using UnityEngine.SceneManagement;

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

    private int stageTimerPauseRequestCount;
    private int gamePauseRequestCount;

    private void Awake()
    {
        SceneManager.sceneLoaded += (_, _) => ResetTime();
        stageTimerPauseRequestCount = 0;
        gamePauseRequestCount = 0;
    }

    private void FixedUpdate()
    {
        if (isResetRequestedForAccumulatedFixedDeltaTime)
        {
            accumulatedFixedDeltaTime = 0;
            isResetRequestedForAccumulatedFixedDeltaTime = false;
            return;
        }

        // why this do not consider pause?
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

        if (0 == stageTimerPauseRequestCount)
        {
            accumulatedDeltaTime += Time.deltaTime;
        }
    }

    public void ResetTime()
    {
        isResetRequestedForAccumulatedFixedDeltaTime = true;
        isResetRequestedForAccumulatedDeltaTime = true;
        stageTimerPauseRequestCount = 0;
    }

    public void PauseStageTimer()
    {
        stageTimerPauseRequestCount++;
    }
    public void ResumeStageTimer()
    {
        stageTimerPauseRequestCount = Mathf.Max(0, stageTimerPauseRequestCount - 1);
    }

    public void PauseGame()
    {
        if (0 == gamePauseRequestCount) Time.timeScale = 0;
        gamePauseRequestCount++;
    }

    public void ResumeGame()
    {
        gamePauseRequestCount = Mathf.Max(0, gamePauseRequestCount - 1);
        if (0 == gamePauseRequestCount) Time.timeScale = 1;
    }
}
