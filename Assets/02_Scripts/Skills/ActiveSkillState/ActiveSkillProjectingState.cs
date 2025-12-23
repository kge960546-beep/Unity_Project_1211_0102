using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class ActiveSkillProjectingState : ActiveSkillStateBase
{
    public override void Tick(ref ActiveSkillStateContext context)
    {
        float prevTimer = context.timer - Time.fixedDeltaTime;
        float currentTimer = context.timer;

        List<float> targetTable = GetTimingTable(context.leveledTimingTables, context.level);
        // TODO: order by time at the OnEnable time.
        //       The lines below this assume that the times are sorted in ascending order.
        int count = targetTable.Count;

        Rigidbody2D rb = context.skillUserRB;

        if (0f == currentTimer)
        {
            context.cachedInitData = new ProjectileInstanceInitializationData();
            context.cachedInitData.initialProjectorPositionSnapshot
                = context.cachedInitData.curretnProjectorPosition
                = (null == rb) ? Vector2.zero : rb.position;
            context.cachedInitData.initialProjectorAzimuthSnapshot
                = context.cachedInitData.curretnProjectorAzimuth
                = (null == rb) ? 0f : Mathf.Atan2(rb.velocity.y, rb.velocity.x);
            // TODO: change to get latest direction instead of velocity which may be zero.
            context.cachedInitData.sequenceCount = targetTable.Count;
            context.cachedInitData.layer = context.layer;
        }

        context.cachedInitData.curretnProjectorPosition = (null == rb) ? Vector2.zero : rb.position;
        context.cachedInitData.curretnProjectorAzimuth = (null == rb) ? 0f : Mathf.Atan2(rb.velocity.y, rb.velocity.x);

        for (int i = 0; i < targetTable.Count; i++)
        {
            float time = targetTable[i];
            if (time <= prevTimer) continue;
            if (currentTimer < time) break;
            context.cachedInitData.sequenceNumber = i;
            ProjectileSpawnUtility.Spawn(context.tempSharedCommonProjectilePrefab, context.logic, context.cachedInitData);
        }

        if (0 == count || targetTable[count - 1] <= currentTimer)
            context.nextState = ActiveSkillStateUtility.GetStateType<ActiveSkillWaitingState>();
    }

    public override void OnEnterState(ref ActiveSkillStateContext context)
    {
        context.timer = 0;
    }

    public override void OnExitState(ref ActiveSkillStateContext context)
    {
    }

    private List<float> GetTimingTable(LeveledProjectionTimingTable[] tables, int currentLevel)
    {
        return tables
            .Where(t => t.RequiredLevel <= currentLevel)
            .Aggregate((tLeft, tRight) => tLeft.RequiredLevel < tRight.RequiredLevel ? tLeft : tRight)
            .ProjectionTimingTable;
    }
}
