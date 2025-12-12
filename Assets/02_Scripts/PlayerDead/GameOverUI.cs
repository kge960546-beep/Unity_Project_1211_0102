using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private PlayerHp playerHp;


    private void Awake()
    {
        if(playerHp == null)
            playerHp = FindObjectOfType<PlayerHp>();

        if(gameOverUI != null)
            gameOverUI.SetActive(false);
    }

    //TODO: 부활 기능을 넣을려면 클래스 이름변경과, 새로운 구독 해지 생성
    /// <summary>
    /// 플레이어가 게임에서 완전사망시 실패 패널로 넘어기는 구독 / 해지
    /// </summary>    
    private void OnEnable()
    {
        if(playerHp != null)
            playerHp.SubscribePlayerDeadEvent(OnPlayerDead);
    }
    private void OnDisable()
    {
        if (playerHp != null)
            playerHp.UnsubscribePlayerDeadEvent(OnPlayerDead);
    }
    private void OnPlayerDead()
    {
        if(gameOverUI)
            gameOverUI.SetActive(true);
    }
}
