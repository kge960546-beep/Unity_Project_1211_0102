using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPatternController : MonoBehaviour
{
    [SerializeField] private EnemySpawner enemySpawner;

    [SerializeField] private StagePatternSO stagePattern;
    [SerializeField] private Transform player;

    private Coroutine stageRoutine;

    private void Start()
    {
        stageRoutine = StartCoroutine(RunStagePatterns());
    }
    private IEnumerator RunStagePatterns()
    {
        foreach (var pattern in stagePattern.patterns)
        {
            if (pattern == null)
                continue;

            if(pattern.isAllowParallel)
            {
                StartCoroutine(RunSpawnPattern(pattern));
            }
            else
            {
                yield return StartCoroutine(RunSpawnPattern(pattern));
            }
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
        var context = new SpawnContext
        {
            playerPosition = player.position,
            radius = pattern.radius,
            spawnCount = pattern.spawnCount,
            targetType = pattern.targetType
        };

        Vector3[] positions = pattern.shape.GetSpawnPositions(context);

        foreach(var pos in positions)
        {
            enemySpawner.SpawnEnemy(pattern.enemyData, pos);
        }
    }
}
