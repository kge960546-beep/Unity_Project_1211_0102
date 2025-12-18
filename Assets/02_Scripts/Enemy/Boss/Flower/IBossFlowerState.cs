public interface IBossFlowerState 
{
    void EnterState(BossFlower boss);
    void UpdateState(BossFlower boss);
    void ExitState(BossFlower boss);
}