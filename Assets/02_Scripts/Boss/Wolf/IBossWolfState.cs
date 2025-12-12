public interface IBossWolfState
{
    void EnterState(BossWolf boss);
    void UpdateState(BossWolf boss);
    void ExitState(BossWolf boss);
}
