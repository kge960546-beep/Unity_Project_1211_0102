using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPatternController : MonoBehaviour
{
    [SerializeField] private EnemySpawner enemySpawner;

    [SerializeField] private StagePatternSO stagePattern;
    [SerializeField] private Transform player;

    private bool isBossSpawned = false;

    private void Start()
    {
        StartCoroutine(RunStagePatterns());
    }
    public IEnumerator RunStagePatterns()
    {
        foreach (var pattern in stagePattern.patterns)
        {
            if (pattern == null)
                continue;

            if (pattern.isAllowParallel)
                StartCoroutine(RunSpawnPattern(pattern));
            else
                yield return StartCoroutine(RunSpawnPattern(pattern));      
        }
    }
    private IEnumerator RunSpawnPattern(SpawnPatternSO pattern)
    {
        //패턴 시작 시간 까지 대기
        if(pattern.startTime > 0f)
            yield return new WaitForSeconds(pattern.startTime);

        float timer = 0f;

        while(timer < pattern.endTime)
        {
            SpawnByPattern(pattern);

            timer += pattern.tick;

            if(pattern.tick > 0)
                yield return new WaitForSeconds(pattern.tick);
            else
                yield return null;
        }
    }
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
                Debug.LogError("BossPatternSO 또는 BossData 캐스팅 실패");
                return;
            }

            // 반드시 StartCoroutine 으로 실행해야 함
            StartCoroutine(enemySpawner.SpawnBossRoutine(bossPattern, bossData));
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
}
