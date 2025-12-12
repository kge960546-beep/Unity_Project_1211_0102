using UnityEngine;

public class BossFlowerIdleState : IBossFlowerState
{
    public void EnterState(BossFlower boss)
    {
        boss.anim.Play("Flower_Idle");
    }   

    public void UpdateState(BossFlower boss)
    {
      
    }

    public void ExitState(BossFlower boss)
    {

    }
}
