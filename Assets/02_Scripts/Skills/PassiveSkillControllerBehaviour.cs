using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSkillControllerBehaviour : MonoBehaviour
{
    public int skillID;
    public int level;
    public PassiveSkillDescriptor descriptor;

    private void OnEnable()
    {
        // TODO: call a method to recalculate stats.
    }
}
