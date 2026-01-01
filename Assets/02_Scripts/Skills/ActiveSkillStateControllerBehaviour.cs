using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkillStateControllerBehaviour : MonoBehaviour
{
    public int skillID;
    public ActiveSkillStateContext context;
    public PlayerCoolTimeSlider coolTime;
    public bool isDefaultSkill;

    private void OnEnable()
    {
        context.nextState = ActiveSkillStateUtility.GetStateType<ActiveSkillWaitingState>();
        context.isStateChanged = true;
        if (coolTime == null)
            coolTime = FindAnyObjectByType<PlayerCoolTimeSlider>();
    }

    private void FixedUpdate()
    {
        ActiveSkillStateBase currentState = context.nextState;

        if (context.isStateChanged)
        {
            currentState.OnEnterState(ref context);
            context.isStateChanged = false;
            if (currentState == ActiveSkillStateUtility.GetStateType<ActiveSkillWaitingState>())
            {
                if (isDefaultSkill == true)
                {
                    coolTime.StartCoolTime();
                }
            }
        }

        currentState.Tick(ref context);
        context.timer += Time.fixedDeltaTime;
       

        if (currentState != context.nextState)
        {
            context.isStateChanged = true;
            currentState.OnExitState(ref context);
           
        }
    }
}
