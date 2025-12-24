using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SpawnPatternController : MonoBehaviour
{
    [SerializeField] private StagePatternSO stagePattern;
    [SerializeField] private EnemySpawner enemySpawner;

    TimeService ts;

    private void Awake()
    {     
        ts = GameManager.Instance.GetService<TimeService>();
#if UNITY_EDITOR
        if (ts == null)
            Debug.LogError("TimeService not found");
#endif
    }
    void Start()
    {
        RunStage();
    }
    private void RunStage()
    {
        foreach(var pattern in stagePattern.patterns)
        {
            enemySpawner.AddPattern(pattern);
        }
    }
    private void OnStaggComplete()
    {
        //º¸»ó, UI µî
    }
    public void StopStage()
    {
        enemySpawner.StopAllPatterns();
    }
}
