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

        if(exp >= nextLevelExp)
        {
            exp -= nextLevelExp;
            level++;
            nextLevelExp = nextLevelExp * 2;
        }
    }
}
