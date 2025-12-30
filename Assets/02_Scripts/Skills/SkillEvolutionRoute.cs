using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "0000-SkillEvolutionRoute", menuName = "Game/Skill/Skill Evolution Route")]
public class SkillEvolutionRoute : ScriptableObject
{
    [SerializeField] private SkillDescriptor baseSkill1;    public int BaseSkillID1 => baseSkill1.SkillID;
    [SerializeField] private SkillDescriptor baseSkill2;    public int BaseSkillID2 => baseSkill2.SkillID;
    [SerializeField] private SkillDescriptor evolvedSkill;  public int EvolvedSkillID => evolvedSkill.SkillID;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (null == baseSkill1 || null == baseSkill2 || null == evolvedSkill) Debug.LogError("Skill fields shall not be null.", this);
    }
#endif
}
