using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExperienceService : MonoBehaviour, IGameManagementService
{
    public int level = 1;
    public int exp = 0;
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

    private void Awake()
    {
        SceneManager.sceneLoaded += (_, _) => Reset();
    }

    private void Reset()
    {
        level = 1;
        exp = 0;
        nextLevelExp = 5;
    }
}
