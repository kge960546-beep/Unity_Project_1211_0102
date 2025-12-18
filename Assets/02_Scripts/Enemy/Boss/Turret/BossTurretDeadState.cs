using UnityEngine;

public class BossTurretDeadState : IBossTurretState
{
    private bool initialized = false;

    public void EnterState(BossTurret boss)
    {
        if (initialized) return;
        initialized = true;

        boss.StopMove();
        boss.anim.SetTrigger("isDead");

        if (boss.rb != null)
        {
            boss.rb.velocity = Vector2.zero;
            boss.rb.isKinematic = true;
        }

        Collider2D col = boss.GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
    }

    public void UpdateState(BossTurret boss) { }
    public void ExitState(BossTurret boss) { }
}
