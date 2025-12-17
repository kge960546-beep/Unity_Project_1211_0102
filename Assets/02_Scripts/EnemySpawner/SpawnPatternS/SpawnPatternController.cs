using System;
using System.Collections;
using UnityEngine;

public class SpawnPatternController : MonoBehaviour
{
    [SerializeField] private StagePatternSO stagePattern;
    [SerializeField] private EnemySpawner enemySpawner;

    private Coroutine stageRoutine;
    TimeService ts;

    private void Awake()
    {     
        ts = GameManager.Instance.GetService<TimeService>();

        if (ts == null)
            Debug.LogError("TimeService not found");
    }
    void Start()
    {
        stageRoutine = StartCoroutine(RunStage());
    }
    IEnumerator RunStage()
    {
        foreach(var pattern in stagePattern.patterns)
        {
            if (pattern.isAllowParallel)
            {
                enemySpawner.RunPattern(pattern);
            }
            else
            {
                yield return enemySpawner.RunPattern(pattern);
            }
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
