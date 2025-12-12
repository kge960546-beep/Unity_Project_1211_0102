using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject gameOverUI;
    PlayerHp playerHp;

    private void OnEnable()
    {
        playerHp.SubscribePlayerDeadEvent(OnPlayerDead);
    }
    private void OnDisable()
    {
        playerHp.UnsubscribePlayerDeadEvent(OnPlayerDead);
    }
    private void OnPlayerDead()
    {
        if(gameOverUI)
            gameOverUI.SetActive(true);
    }
}
