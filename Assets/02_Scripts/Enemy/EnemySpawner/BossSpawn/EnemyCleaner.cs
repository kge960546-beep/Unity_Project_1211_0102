using UnityEngine;

public static class EnemyCleaner
{
    public static void ClearAllNormalEnemies()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach(var enemy in enemies)
        {
            //일반 몬스터 void kill()생성시 생성할 것
        }
    }
}