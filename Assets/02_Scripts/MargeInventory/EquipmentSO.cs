using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentSO", menuName = "Game/Equipment/EquipmentSO")]
public class EquipmentSO : ScriptableObject
{
    //장비의 등급
    public enum EquipmentClassType
    {
        Normal = 0,    // 회색
        Good = 1,      // 초록
        Better = 2,    // 파랑
        Excellent = 3, // 보라
        Epic = 4,      // 노랑
        Legend = 5     // 빨강
    }

    //장비 부위
    public enum EquipmentPart
    {
        Weapon,
        Armor,
        Necklace,
        Belt,
        Gloves,
        Shoes
    }
    //강화 적용 스탯
    public enum PercentTarget
    {
        Hp,
        Attack
    }

    [Header("Identity")]
    public string equipmentName;
    public int equipmentID;
    public EquipmentClassType classType;

    [Header("Stats")]
    public int plusEquimentHP;
    public int plusEquimentAttack;
    public EquipmentPart partType;
    public Sprite itemSprite;
    public int weight;    

    [Header("SkillID")]
    public int skillId;

    [Header("Upgrade")]
    [SerializeField] private PercentTarget percentTargetStat = PercentTarget.Attack;
    public GameObject[] upgradePrefabs = new GameObject[18];
    [SerializeField] private float[] precentUpgradeStep = new float[18];


    //테이블 인덱스 변환 함수
    public int GetUpgradeStep(EquipmentClassType type, int step)
    {
        if(type <= EquipmentClassType.Better)
        {
            step = 0;
        }

        step = Mathf.Clamp(step, 0, 2);

        return ((int)type * 3) + step;
    }

    //등급, 단계 업그래이드  반환
    public GameObject GetUpgradePrefab(EquipmentClassType type, int step)
    {        
        int index = GetUpgradeStep(type, step);

        if (upgradePrefabs == null) return null;
        if (index < 0 || index >= upgradePrefabs.Length) return null;

        return upgradePrefabs[index];
    }

    //등급에 따른 퍼센트 증가량 반환
    private float GetPercent(EquipmentClassType type, int step)
    {
        int index = GetUpgradeStep(type, step);

        if (precentUpgradeStep == null || precentUpgradeStep.Length < 18)
            return 0;

        if (index < 0 || index >= precentUpgradeStep.Length)
            return 0;

        return precentUpgradeStep[index];        
    }

    //최종 스탯 계산
    public void GetFinalStats(EquipmentClassType type, int step, out int finalHp, out int fianAttack)
    {
        float per = GetPercent(type, step);

        if(percentTargetStat == PercentTarget.Hp)
        {
            finalHp = Mathf.RoundToInt(plusEquimentHP * (1.0f + per));
            fianAttack = plusEquimentAttack;
        }
        else
        {
            finalHp = plusEquimentHP;
            fianAttack = Mathf.RoundToInt(plusEquimentAttack * (1.0f + per));
        }
    }
}