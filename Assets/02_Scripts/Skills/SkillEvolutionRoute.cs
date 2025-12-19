using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "0000-SkillEvolutionRoute", menuName = "Game/Skill/Skill Evolution Route")]
public class SkillEvolutionRoute : ScriptableObject
{
    [SerializeField] private SkillDescriptor baseSkill;       public int BaseSkillID => baseSkill.SkillID;
    [SerializeField] private SkillDescriptor additionalSkill; public int AdditionalSkillID => additionalSkill.SkillID;
    [SerializeField] private SkillDescriptor evolvedSkill;    public int EvolvedSkillID => evolvedSkill.SkillID;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (null == baseSkill || null == additionalSkill || null == evolvedSkill) Debug.LogError("Skill fields shall not be null.", this);
    }
#endif
}
