using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverKillCount : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gameOverKillCount;

    [SerializeField] KillCount killCount;
    void Start()
    {
        if(gameOverKillCount == null)
        {
            gameOverKillCount = GetComponent<TextMeshProUGUI>();
        }

        if(killCount == null)
        {
            killCount = FindAnyObjectByType<KillCount>();
        }
        GameOverKillCounted();
    }
    public void GameOverKillCounted()
    {
        if (gameOverKillCount == null) return;
        if (killCount == null) return;

        Debug.Log($"[GameOverRead] {killCount.name} id={killCount.GetInstanceID()} count={killCount.GetKillCount()}");

        gameOverKillCount.text = killCount.GetKillCount().ToString();
    }
}
