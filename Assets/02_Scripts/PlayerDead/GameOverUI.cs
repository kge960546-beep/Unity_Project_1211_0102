using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject gameOverUI;
    
    private void OnEnable()
    {
        PlayerHp.onPlayerDeadEvent += OnPlayerDead;
    }
    private void OnDisable()
    {
        PlayerHp.onPlayerDeadEvent -= OnPlayerDead;
    }
    private void OnPlayerDead()
    {
        if(gameOverUI)
            gameOverUI.SetActive(true);
    }
}
