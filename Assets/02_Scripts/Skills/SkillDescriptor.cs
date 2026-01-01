using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillDescriptor : ScriptableObject
{
    public enum Type : byte
    {
        Active,
        ActiveEvo,
        Passive
    }

    [field: SerializeField] public int SkillID { get; private set; }
    [field: SerializeField] public Type SkillType { get; private set; }
    [field: SerializeField] public string SkillName { get; private set; }
    [field: SerializeField] public Sprite SkillThumbnail { get; private set; }
    [TextArea, SerializeField] private string skillDescription;
    public string SkillDescription => skillDescription;
}
