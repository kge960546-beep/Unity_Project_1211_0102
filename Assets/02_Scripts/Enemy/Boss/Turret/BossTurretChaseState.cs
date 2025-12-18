using System.Collections.Generic;
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

        float dist = Vector2.Distance(boss.transform.position, boss.player.position);

        if(dist <= boss.closeFlipStopRange)
        {
            boss.StopMove();
        }
        else
        {
            Vector2 dir = (boss.player.position - boss.transform.position).normalized;

            boss.SetMove(dir, boss.chaseSpeed);
        }

        if (Mathf.Abs(boss.moveDirection.x) > 0.05f && dist > boss.closeFlipStopRange)
        {
            boss.transform.localScale = new Vector3(boss.moveDirection.x > 0 ? 1 : -1, 1, 1);
        }
    }
    public void ExitState(BossTurret boss)
    {
        boss.StopMove();
    }
}
