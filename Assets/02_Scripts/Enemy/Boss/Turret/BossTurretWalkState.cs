using UnityEngine;

public class BossTurretWalkState : IBossTurretState
{
    public void EnterState(BossTurret boss)
    {
        boss.anim.Play("Turret_Walk");
    }
    public void UpdateState(BossTurret boss)
    {
        if (boss.isDead) return;

        float distance = Vector2.Distance(boss.transform.position, boss.player.position);

        // 추적 상태로 전환
        if (distance <= boss.chaseRange)
        {
            boss.ChangeState(new BossTurretChaseState());
            return;
        }

        // 플레이어 방향으로 이동
        Vector2 dir = (boss.player.position - boss.transform.position).normalized;

        boss.SetMove(dir, boss.walkSpeed);

        if(Mathf.Abs(boss.moveDirection.x) > 0.05f && distance > boss.closeFlipStopRange)
        {
            boss.transform.localScale = new Vector3(boss.moveDirection.x > 0 ? 1 : -1, 1, 1);
        }
    }
    public void ExitState(BossTurret boss)
    {
        boss.StopMove();
    }
}
