using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BossBearChaseState : IBossBearState
{
    public void EnterState(BossBear boss)
    {
        boss.anim.Play("Bear_Walk");
    }
    public void UpdateState(BossBear boss)
    {
        if (boss.isDead) return;

        float dist = Vector2.Distance(boss.transform.position, boss.player.position);

        // Rush 조건        
        if (Time.time >= boss.spawnTime + boss.rushDelay &&
            Time.time >= boss.lastRushTime + boss.rushCooldown &&
            dist < boss.rushRange && dist > boss.closeFlipStopRange)
        {
            boss.ChangeState(new BossBearRushState(boss.rushSpeed, boss.rushDuration));
            return;
        }

        // 플레이어와 가까우면 이동 멈춤
        if (dist <= boss.closeFlipStopRange)
        {
            boss.StopMove();
        }
        else
        {
            Vector2 dir = (boss.player.position - boss.transform.position).normalized;
            boss.SetMove(dir, boss.chaseSpeed);
        }
    }
    public void ExitState(BossBear boss)
    {
        boss.StopMove();
    }
}

