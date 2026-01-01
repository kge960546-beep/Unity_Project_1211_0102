using System.Collections;
using UnityEngine;

// ½ÇÇà°´Ã¼
public class SpawnPatternRunner : MonoBehaviour
{
    [SerializeField] private StagePatternSO stagePattern;
    [SerializeField] private EnemySpawner spawner;
    [SerializeField] private Transform player;

    private bool stopped;

    TimeService ts;

    private void AWake()
    {
        ts = GameManager.Instance.GetService<TimeService>();
    }
    public IEnumerator RunStagePatterns()
    {
        foreach(var pattern in stagePattern.patterns)
        {
            if(pattern == null) continue;

            if(pattern.isAllowParallel)
                StartCoroutine(RunSpawnPattern(pattern));
            else 
                yield return StartCoroutine(RunSpawnPattern(pattern));
        }
    }
    public IEnumerator RunSpawnPattern(SpawnPatternSO pattern)
    {
        stopped = false;
        float time = 0;

        while (time < pattern.endTime)
        {
            SpawnOnce(pattern);

            time += pattern.tick;

            if(pattern.tick > 0f)
                yield return new WaitForSeconds(pattern.tick);
            else
                yield return null;
        }
    }
    public void Stop()
    {
        stopped = true;
    }
    public void SpawnOnce(SpawnPatternSO pattern)
    {
        var context = new SpawnContext
        {
            playerMoveDir = player.position,
            spawnCount = pattern.spawnCount,
            radius = pattern.radius
        };

        Vector3[] positions = pattern.shape.GetSpawnPositions(context);

        foreach(var pos in positions)
        {
            spawner.SpawnEnemy(pattern.enemyData, pos);
        }
    }
}
