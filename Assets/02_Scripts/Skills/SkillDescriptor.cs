using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "0000-Skill", menuName = "Game/Skill/Skill Descriptor")]
public class SkillDescriptor : ScriptableObject
{
    public enum Type : byte
    {
        Active,
        Passive,
        Evo
    }

    [field: SerializeField] public int SkillID { get; private set; }
    [field: SerializeField] public string SkillName { get; private set; }
    [field: SerializeField] public Texture2D SkillThumbnail { get; private set; }
    [TextArea, SerializeField] private string skillDescription;
    public string SkillDescription => skillDescription;

    [field: SerializeField] public Type SkillType { get; private set; }
    [field: SerializeField] public GameObject CommonSharedBaseProjectileSpawnerPrefab { get; private set; } // TODO: this is temporary thing. this is shared and should be eventually instantiated by all skills
    [field: SerializeField] public ProjectileLogicBase ProjectileLogic { get; private set; }
    [field: SerializeField] public List<LeveledProjectionTimingTable> LeveledProjectionTimingTables { get; private set; }
    [field: SerializeField] public float DefaultCooldown{ get; private set; }
}
