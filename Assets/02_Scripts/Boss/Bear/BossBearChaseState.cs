using UnityEngine;

public class BossBearChaseState : IBossBearState
{
    public void EnterState(BossBear boss)
    {
        boss.anim.Play("Bear_Walk");
    }

    public void UpdateState(BossBear boss)
    {
        if (boss.isDead) return;
     
        Vector2 dir = (boss.player.position - boss.transform.position).normalized;

        boss.SetMove(dir, boss.chaseSpeed);

    }

    public void ExitState(BossBear boss)
    {
        boss.StopMove();
    }
}

