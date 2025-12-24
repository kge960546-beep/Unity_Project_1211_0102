using System;
using UnityEngine;

public class ExperienceService : MonoBehaviour, IGameManagementService
{
    public int level;
    public int exp;
    public int nextLevelExp = 5;

    public event Action<int> OnLevelUp;

    public void GetExp(int amount)
    {
        exp += amount;

        if(exp >= nextLevelExp)
        {
            exp -= nextLevelExp;
            nextLevelExp = nextLevelExp * 2;
            LevelUp();
        }
    }
    private void LevelUp()
    {
        level++;
        Debug.Log("LEVEL UP INVOKED");
        OnLevelUp?.Invoke(level);
    }
}
