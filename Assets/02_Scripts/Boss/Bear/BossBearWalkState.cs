using UnityEngine;

public class BossBearWalkState : IBossBearState
{
    public void EnterState(BossBear boss)
    {
        boss.anim.Play("Bear_Walk");
    }

    public void UpdateState(BossBear boss)
    {
        if (boss.isDead) return;

        float distance = Vector2.Distance(boss.transform.position, boss.player.position);

        // 추적 범위 안으로 들어오면 ChaseState로 전환
        if (distance <= boss.chaseRange)
        {
            boss.ChangeState(new BossBearChaseState());
            return;
        }

        Debug.Log("나 걸어갈게");

        // 플레이어 방향으로 이동
        Vector2 dir = (boss.player.position - boss.transform.position).normalized;

        boss.SetMove(dir, boss.walkSpeed);
    }

    public void ExitState(BossBear boss)
    {
        boss.StopMove();
    }
}
