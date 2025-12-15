using UnityEngine;

public class BossTurretChaseState : IBossTurretState
{
    public void EnterState(BossTurret boss)
    {
        boss.anim.Play("Turret_Walk");
    }

    public void UpdateState(BossTurret boss)
    {
        if (boss.isDead) return;

        Vector2 dir = (boss.player.position - boss.transform.position).normalized;

        boss.SetMove(dir, boss.chaseSpeed);

    }

    public void ExitState(BossTurret boss)
    {
        boss.StopMove();
    }
}
