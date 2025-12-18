public interface IBossTurretState
{
    void EnterState(BossTurret boss);
    void UpdateState(BossTurret boss);
    void ExitState(BossTurret boss);
}
