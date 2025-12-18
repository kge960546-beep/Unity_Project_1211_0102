using UnityEngine;

public class ExperienceService : MonoBehaviour, IGameManagementService
{
    public ExperienceService instance { get; private set; }

    public int level;
    public int exp;
    public int nextLevelExp = 5;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
            
        instance = this;
    }
    public void GetExp(int amount)
    {
        exp += amount;
        Debug.Log($"Current Exp : {exp}");

        if(exp >= nextLevelExp)
        {
            exp -= nextLevelExp;
            level++;
            nextLevelExp = nextLevelExp * 2;

            Debug.Log($"Level Up! Lv.{level}");
        }
    }
}
