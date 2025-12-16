using System;
using System.Collections;
using UnityEngine;

public class SpawnPatternController : MonoBehaviour
{
    [SerializeField] private StagePatternSO stagePattern;
    [SerializeField] private EnemySpawner enemySpawner;
    private DifficultyEvaluator difficulty = new DifficultyEvaluator();

    private Coroutine stageRoutine;
    TimeService ts;

    void Start()
    {
        stageRoutine = StartCoroutine(RunStage());
    }
    IEnumerator RunStage()
    {
        float startTime = ts.accumulatedFixedDeltaTime;

        foreach(var pattern in stagePattern.patterns)
        {
            float playTime = ts.accumulatedDeltaTime - startTime;

            pattern.spawnCount = Mathf.CeilToInt(pattern.spawnCount * difficulty.CurrentDifficulty);

            if (pattern.isAllowParallel)
            {
                enemySpawner.RunPattern(pattern);
            }
            else
                yield return enemySpawner.RunPattern(pattern);
        }
    }
    void OnStageComplete()
    {
        //보상, 스테이지 클리어 UI
    }
    public void StopStage()
    {
        if (stageRoutine != null)
        { 
            StopCoroutine(stageRoutine);
        }

        enemySpawner.StopAllPatterns();
    }
}
