using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public sealed class ActiveSkillWaitingState : ActiveSkillStateBase
{
    public override void OnEnterState(ref ActiveSkillStateContext context)
    {
        context.timer = 0;
    }

    public override void Tick(ref ActiveSkillStateContext context)
    {
        if (context.isStateChanged) OnEnterState(ref context);

        Vector2 pos = Vector2.zero;
        float azimuth = 0f;

        if (null != context.skillUserRB)
        {
            pos = context.skillUserRB.position;
            azimuth = Mathf.Atan2(context.skillUserRB.velocity.y, context.skillUserRB.velocity.x);
        }

        if (context.timer > context.period
            && context.logic.IsTargetInRange(pos, azimuth))
            context.nextState = ActiveSkillStateUtility.GetStateType<ActiveSkillProjectingState>();

        // TODO: update UI?
    }

    public override void OnExitState(ref ActiveSkillStateContext context)
    {
    }
}
