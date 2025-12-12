using UnityEngine;

public class BossBearRushState : IBossBearState
{
    private Vector2 rushDirection;
    private Vector2 startPosition;
    private float rushDistance;
    private float rushSpeed;

    public BossBearRushState(float speed, float distance)
    {
        rushSpeed = speed;
        rushDistance = distance;
    }

    public void EnterState(BossBear boss)
    {
        boss.anim.Play("Bear_Walk");

        // 시작 위치 저장
        startPosition = boss.transform.position;

        // 돌진 방향 결정 (예: 플레이어 위치 기준)
        rushDirection = ((Vector2)boss.player.position - startPosition).normalized;
    }

    public void UpdateState(BossBear boss)
    {
        if (boss.isDead) return;

        // 고정 방향으로 이동
        boss.SetMove(rushDirection, rushSpeed);

        // 목표 거리 도달 시 돌진 종료
        if (Vector2.Distance(startPosition, boss.transform.position) >= rushDistance)
        {
            boss.ChangeState(new BossBearChaseState());
        }
    }

    public void ExitState(BossBear boss)
    {
        boss.StopMove();
    }
}






