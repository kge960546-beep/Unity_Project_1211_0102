using System.Collections.Generic;
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

        float dist = Vector2.Distance(boss.transform.position, boss.player.position);

        // Rush 조건        
        if (Time.time >= boss.spawnTime + boss.rushDelay &&
            Time.time >= boss.lastRushTime + boss.rushCooldown &&
            dist < boss.rushRange && dist > boss.closeFlipStopRange)
        {
            boss.ChangeState(new BossBearRushState(boss.rushSpeed, boss.rushDuration));
            return;
        }

        // 추적 범위 안으로 들어오면 ChaseState로 전환
        if (dist <= boss.chaseRange)
        {
            boss.ChangeState(new BossBearChaseState());
            return;
        }        

        // 플레이어 방향으로 이동
        Vector2 dir = (boss.player.position - boss.transform.position).normalized;
        boss.SetMove(dir, boss.walkSpeed);

        //좌우 반전
        if (Mathf.Abs(boss.moveDirection.x) > 0.05f && dist > boss.closeFlipStopRange)
        {
            boss.transform.localScale = new Vector3(boss.moveDirection.x > 0 ? 1 : -1, 1, 1);
        }
    }
    public void ExitState(BossBear boss)
    {
        boss.StopMove();
    }
}
