using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillDescriptor : ScriptableObject
{
    [field: SerializeField] public int SkillID { get; private set; }
    [field: SerializeField] public string SkillName { get; private set; }
    [field: SerializeField] public Texture2D SkillThumbnail { get; private set; }
    [TextArea, SerializeField] private string skillDescription;
    public string SkillDescription => skillDescription;
}
