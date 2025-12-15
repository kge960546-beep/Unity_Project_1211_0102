using UnityEngine;

public class BossWolfRushState : IBossWolfState
{
    private Vector2 rushDirection;
    private Vector2 startPosition;
    private float rushDuration;
    private float rushSpeed;
    private float rushStartTime;

    public BossWolfRushState(float speed, float duration)
    {
        rushSpeed = speed;
        rushDuration = duration;
    }

    public void EnterState(BossWolf boss)
    {
        boss.anim.Play("Wolf_Walk");

        // 시작 위치 저장
        startPosition = boss.transform.position;

        // 돌진 방향 결정 (플레이어 위치 기준)
        rushDirection = ((Vector2)boss.player.position - startPosition).normalized;

        // 돌진 시작 시각 저장 
        rushStartTime = GameManager.Instance.GetService<TimeService>().accumulatedFixedDeltaTime;
    }

    public void UpdateState(BossWolf boss)
    {
        if (boss.isDead) return;

        // 고정 방향으로 이동
        boss.SetMove(rushDirection, rushSpeed);

        // 목표 시간 도달 시 돌진 종료
        float now = GameManager.Instance.GetService<TimeService>().accumulatedFixedDeltaTime;
        if (now - rushStartTime >= rushDuration)
        {
            boss.ChangeState(new BossWolfChaseState());
        }

    }

    public void ExitState(BossWolf boss)
    {
        boss.StopMove();
    }
}

