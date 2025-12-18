using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BossFlowerIdleState : IBossFlowerState
{
    public void EnterState(BossFlower boss)
    {
        boss.anim.Play("Flower_Idle");
    }   

    public void UpdateState(BossFlower boss)
    {
        if (boss.isDead) return;

        if (boss.player == null) return;

        // 플레이어의 이동경로 쪽으로 플립 
        if (boss.player != null)
        {
            float dir = boss.player.position.x - boss.transform.position.x;

            if (Mathf.Abs(dir) > 0.1f)
            {
                boss.transform.localScale = new Vector3(dir > 0 ? 1 : -1, 1, 1);
            }
        }
    }
    public void ExitState(BossFlower boss)
    {

    }
}
