using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    [SerializeField] private BossTimerSO currentStateData;
    private TimeService timeService;
    
    [SerializeField] private bool isFirstBoss = false;
    [SerializeField] private bool isLastBoss = false;
    void Start()
    {
        timeService = GameManager.Instance.GetService<TimeService>();  
        timeService.ResetTime();
    }
    void Update()
    {
        float currentTime = timeService.accumulatedDeltaTime;

        if (!isFirstBoss && currentTime >= currentStateData.stageFirstBossTime)
        {
            SpawnFirstBoss();
        }
        if(!isLastBoss && currentTime >= currentStateData.stageMaxTime)
        {
            SpawnLastBoss();
        }
#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.Space))
        {            
            timeService.ResumeStageTimer();
        }
#endif        
    }
    private void OnValidate()
    {
        
    }
    void SpawnFirstBoss()
    {
        isFirstBoss = true;
        timeService.PauseStageTimer();
#if UNITY_EDITOR
        Debug.Log("중간보스 출현");
#endif
    }
    void SpawnLastBoss()
    {
        isLastBoss = true;
        timeService.PauseStageTimer();
#if UNITY_EDITOR
        Debug.Log("라스트 보스 출현");
#endif
    }
}