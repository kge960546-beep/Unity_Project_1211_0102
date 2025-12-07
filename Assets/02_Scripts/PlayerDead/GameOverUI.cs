using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour, IPlayerDead
{
    [SerializeField] private GameObject gameOverUI;

    private PlayerDeadSubject deadSubject;
   
    private void Start()
    {
        if(gameOverUI)
            gameOverUI.SetActive(false);

        deadSubject = FindObjectOfType<PlayerDeadSubject>();

        if(deadSubject != null)
            deadSubject.RegisterObserver(this);
    }
    public void OnDestroy()
    {
        if(deadSubject != null)
            deadSubject.UnregisterObserver(this);
    }
    public void OnPlayerDead()
    {
        if(gameOverUI)
            gameOverUI.SetActive(true);
    }   
}
