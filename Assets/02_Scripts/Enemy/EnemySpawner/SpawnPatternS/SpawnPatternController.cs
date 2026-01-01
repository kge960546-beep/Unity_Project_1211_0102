using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPatternController : MonoBehaviour
{
    [SerializeField] private EnemySpawner enemySpawner;

    [SerializeField] private StagePatternSO stagePattern;
    [SerializeField] private Transform player;

    private bool isBossSpawned = false;
    private bool isStagePaused = false;

    private void Start()
    {
        StartCoroutine(RunStagePatterns());
    }
    // 스테이지 패턴 실행
    public IEnumerator RunStagePatterns()
    {
        foreach (var pattern in stagePattern.patterns)
        {
            if (pattern == null)
                continue;

            //보스 페이즈 동안 정지
            while(isStagePaused)
                yield return null; 

            if (pattern.isAllowParallel)
                StartCoroutine(RunSpawnPattern(pattern));
            else
                yield return StartCoroutine(RunSpawnPattern(pattern));      
        }
    }
    // 스폰 패턴 실행
    private IEnumerator RunSpawnPattern(SpawnPatternSO pattern)
    {
        //패턴 시작 시간 까지 대기
        if(pattern.startTime > 0f)
            yield return new WaitForSeconds(pattern.startTime);

        float timer = 0f;

        while(timer < pattern.endTime)
        {
            //보스 페이즈 동안 정지
            while (isStagePaused)
                yield return null;

            SpawnByPattern(pattern);

            timer += pattern.tick;

            if(pattern.tick > 0)
                yield return new WaitForSeconds(pattern.tick);
            else
                yield return null;
        }
    }
    // 패턴에 의한 스폰 실행
    public void SpawnByPattern(SpawnPatternSO pattern)
    {
        // 보스 스폰 실행
        if (pattern.enemyData.enemyType == EnemyType.Boss)
        {
            if (isBossSpawned)
                return;

            isBossSpawned = true;

            BossPatternSO bossPattern = pattern as BossPatternSO;
            BossData bossData = pattern.enemyData as BossData;

            if (bossPattern == null || bossData == null)
            {
                Debug.LogError("BossPatternSO 또는 bossData 캐스팅 실패");
                return;
            }
            //스테이지 패턴 정지
            PauseStage();

            // 보스 루틴 실행 + 콜백 등록
            StartCoroutine(BossPhaseRoutine(bossPattern, bossData));
            return;
        }

        // 일반 몬스터 스폰 실행
        SpawnContext context = new SpawnContext
        {
            playerPosition = player.position,
            radius = pattern.radius,
            spawnCount = pattern.spawnCount,
            targetType = pattern.targetType
        };

        Vector3[] positions = pattern.shape.GetSpawnPositions(context);

        foreach (var pos in positions)
            enemySpawner.SpawnEnemy(pattern.enemyData, pos);
    }
    //보스 페이지 실행
    private IEnumerator BossPhaseRoutine(BossPatternSO bossPattern, BossData bossData)
    {
        //EmemySpawner가 실제 보스 생성 + 바리게이트 처리
        yield return StartCoroutine(enemySpawner.SpawnBossRoutine(bossPattern, bossData));

        //보스 사망 이후 도달
        while(bossData.currentHp > 0)
            yield return null;

        //마지막 보스라면
        if(bossPattern.isLastBoss)
        {
            //TODO: 클리어 ui 연동
            yield break;
        }

        //보스가 마지막이 아니라면 패턴 재개
        isBossSpawned = false;
        ResumeStage();
    }
    public void PauseStage()
    {
        isStagePaused = true;
    }
    public void ResumeStage()
    {
        isStagePaused = false;
    }
}
