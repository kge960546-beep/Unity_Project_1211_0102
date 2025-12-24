using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkillStateControllerBehaviour : MonoBehaviour
{
    [SerializeField] private ActiveSkillStateContext context;
    public ActiveSkillStateContext Context => context;

    public PlayerCoolTimeSlider coolTime;

    [SerializeField] public bool isDefaultSkill;

    private void OnEnable()
    {
        // TODO: init context dynamically with skill input

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
