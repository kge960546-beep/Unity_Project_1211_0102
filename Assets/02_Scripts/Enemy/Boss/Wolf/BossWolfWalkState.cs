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

        float dist = Vector2.Distance(boss.transform.position, boss.player.position);

        // Rush 조건        
        if (Time.time >= boss.spawnTime + boss.rushDelay &&
            Time.time >= boss.lastRushTime + boss.rushCooldown &&
            dist < boss.rushRange && dist > boss.closeFlipStopRange)
        {
            boss.ChangeState(new BossWolfRushState(boss.rushSpeed, boss.rushDuration));
            return;
        }

        // 추적 범위 진입
        if (dist <= boss.chaseRange)
        {
            boss.ChangeState(new BossWolfChaseState());
            return;
        }

        // 플레이어 방향 이동
        Vector2 dir = (boss.player.position - boss.transform.position).normalized;
        boss.SetMove(dir, boss.walkSpeed);

        //좌우 반전
        if (Mathf.Abs(boss.moveDirection.x) > 0.05f && dist > boss.closeFlipStopRange)
        {
            boss.transform.localScale = new Vector3(boss.moveDirection.x > 0 ? 1 : -1, 1, 1);
        }
    }
    public void ExitState(BossWolf boss)
    {
        boss.StopMove();
    }
}
