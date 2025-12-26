using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//바리케이드 타임
public enum BarricadeType
{
    Circle,
    Line,
    Square,
    None
}
// 보스 페이즈별 데이터
[System.Serializable]
public class BossPhaseData
{
    public string phaseName;
    public float spawnTime;
    public GameObject bossPrefab;
    public BarricadeType barricadeType;
}
public class BossManager : MonoBehaviour
{     
    private TimeService timeService;

    [Header("Boundary 정보")]
    [SerializeField] private CircleBoundaryLimiter circleLimiter;
    [SerializeField] private LineBoundaryLimiter lineLimiter;
    [SerializeField] private SquareBoundaryLimiter squareLimiter;

    [Header("Barricade 정보")]
    [SerializeField] private BarricadeCircleSpawner circleSpawner;
    [SerializeField] private BarricadeLineSpawner lineSpawner;
    [SerializeField] private BarricadeSquareSpawner squareSpawner;
    [SerializeField] private float barricadeSpawnDelay = 1.5f;    

    [Header("Boss Spawn")]    
    [SerializeField] private int currentPhaseIndex = 0;
    [SerializeField] private bool isBossPhase = false;

    //언제(몇초후에) 어떤 보스가(Prefab) 어떤 바리케이드로 스폰될지 직접 지정하는 리스트
    [SerializeField] private List<BossPhaseData> bossPhases = new List<BossPhaseData>();
    
    void Start()
    {
        if(GameManager.Instance != null)
        {
            timeService = GameManager.Instance.GetService<TimeService>();
            timeService.ResetTime();
        }        
    }
    void Update()
    {
        if (isBossPhase || currentPhaseIndex >= bossPhases.Count) return;       

        float currentTime = timeService.accumulatedDeltaTime;

        if(currentTime >= bossPhases[currentPhaseIndex].spawnTime)
        {
            StartCoroutine(BossSpawnRoutine(bossPhases[currentPhaseIndex]));
        }
    }
    IEnumerator BossSpawnRoutine(BossPhaseData phaseData)
    {
        isBossPhase = true;
        timeService.PauseStageTimer();
        //보스 스폰 경고문용 시간은 수정해야할 수 있음
        yield return new WaitForSeconds(3.0f);

        timeService.PauseStageTimer();
        SpawnBoundary(phaseData.barricadeType);
        yield return new WaitForSeconds(barricadeSpawnDelay);

        SpawnBarrcade(phaseData.barricadeType);
        SpawnBoss(phaseData.bossPrefab);

        currentPhaseIndex++;
    }
    //리미터 스크립트가 FixedUpdate로 실행이되서 해당 오브젝트를 끄고 필요한 경계선만 킬수 있는 사전 실행 메서드
    private void AllBoundaryOff()
    {
        if(squareLimiter != null)
            squareLimiter.gameObject.SetActive(false);
        
        if(circleLimiter != null)
            circleLimiter.gameObject.SetActive(false);

        if (lineLimiter != null)
            lineLimiter.gameObject.SetActive(false);
    }
    //경계선 먼저 스폰시키는 메서드
    private void SpawnBoundary(BarricadeType type)
    {
        AllBoundaryOff();

        switch (type)
        {
            case (BarricadeType.Square):
                if (squareLimiter != null) squareLimiter.gameObject.SetActive(true);

                break;
            case (BarricadeType.Circle):
                if (circleLimiter != null) circleLimiter.gameObject.SetActive(true);
                break;
            case (BarricadeType.Line):
                if (lineLimiter != null) lineLimiter.gameObject.SetActive(true);
                break;
            case (BarricadeType.None):
                break;
        }
    }
    // 경계선의 위치로 스폰시키는 메서드
    private void SpawnBarrcade(BarricadeType type)
    {
        switch(type)
        {
            case (BarricadeType.Square):
                if (squareSpawner != null) squareSpawner.CallSquareBarricadeSpawn();
                break;
            case (BarricadeType.Circle):
                if(circleSpawner != null) circleSpawner.CallCircleBarricadeSpawn();
                break;
            case (BarricadeType.Line):
                if(lineSpawner != null) lineSpawner.CallLineBarricadeSpawn();
                break;
            case (BarricadeType.None):
                break;
        }
    }
    //보스 스폰 메서드
    //TODO: 보스 스폰위치는 수정해서 어디서 스폰될지 결정해야함
    private void SpawnBoss(GameObject bossPrefab)
    {
        if(bossPrefab != null)
        {
            Instantiate(bossPrefab, Vector3.zero, Quaternion.identity);
        }
    }
}