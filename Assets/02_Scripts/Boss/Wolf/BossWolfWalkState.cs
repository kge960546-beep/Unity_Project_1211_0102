using UnityEngine;

public class BossWolfWalkState : IBossWolfState
{
    public void EnterState(BossWolf boss)
    {
        boss.anim.Play("Wolf_Walk");
    }

    public void UpdateState(BossWolf boss)
    {
        if (boss.isDead) return;

        float distance = Vector2.Distance(boss.transform.position, boss.player.position);

        // 추적 범위 진입
        if (distance <= boss.chaseRange)
        {
            boss.ChangeState(new BossWolfChaseState());
            return;
        }

        // 플레이어 방향 이동
        Vector2 dir = (boss.player.position - boss.transform.position).normalized;

        boss.SetMove(dir, boss.walkSpeed);
    }

    public void ExitState(BossWolf boss)
    {
        boss.StopMove();
    }
}
