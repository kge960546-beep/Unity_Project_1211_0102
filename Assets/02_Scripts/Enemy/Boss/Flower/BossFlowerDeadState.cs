using UnityEngine;

public class BossFlowerDeadState : IBossFlowerState
{
    private bool initialized = false;

    public void EnterState(BossFlower boss)
    {
        if (initialized) return;
        initialized = true;

        boss.anim.SetTrigger("isDead");

        if (boss.rb != null)
        {
            boss.rb.velocity = Vector2.zero;
            boss.rb.isKinematic = true;
        }

        Collider2D col = boss.GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
    }
    public void UpdateState(BossFlower boss)
    {
        
    }
    public void ExitState(BossFlower boss)
    {

    }
}