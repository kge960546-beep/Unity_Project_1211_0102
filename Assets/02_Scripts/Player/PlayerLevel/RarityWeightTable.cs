using UnityEngine;

// 스킬 레어도 가중치 테이블
public static class RarityWeightTable
{ 
    public static int GetWeight(Rarity rarity)
    {
        return rarity switch
        {
            Rarity.Common => 60,
            Rarity.Rare => 25,
            Rarity.Epic => 10,
            Rarity.Legandary => 5,
            _ => 1
        };
    }
}
