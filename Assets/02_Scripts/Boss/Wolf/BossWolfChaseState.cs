using UnityEngine;

public class BossWolfChaseState : IBossWolfState
{
    public void EnterState(BossWolf boss)
    {
        boss.anim.Play("Wolf_Walk");
    }

    public void UpdateState(BossWolf boss)
    {
        if (boss.isDead) return;

        Vector2 dir = (boss.player.position - boss.transform.position).normalized;

        boss.SetMove(dir, boss.chaseSpeed);
    }

    public void ExitState(BossWolf boss)
    {
        boss.StopMove();
    }
}
