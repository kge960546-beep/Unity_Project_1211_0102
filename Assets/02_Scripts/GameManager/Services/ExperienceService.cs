using UnityEngine;

public class ExperienceService : MonoBehaviour, IGameManagementService
{
    public int level;
    public int exp;
    public int nextLevelExp = 5;

    public LevelUpUI levelUpUI;

    public void GetExp(int amount)
    {
        exp += amount;

        if(exp >= nextLevelExp)
        {
            exp -= nextLevelExp;
            level++;
            nextLevelExp = nextLevelExp * 2;

            levelUpUI.ShowLevelUp();
        }
    }
}
