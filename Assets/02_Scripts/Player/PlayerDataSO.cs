using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PlayerData", menuName = "Player")]
public class PlayerDataSO : ScriptableObject
{
    public int playerMaxHp;
    public int playerAttack;
}
