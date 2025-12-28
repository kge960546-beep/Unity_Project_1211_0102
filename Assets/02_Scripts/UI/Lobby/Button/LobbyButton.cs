using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyButton : MonoBehaviour
{
    [SerializeField] GameObject startButton;
    [SerializeField] GameObject lobbyStageSelectButton;
    [SerializeField] GameObject mainScrollView;

    public void OnClickButton()
    {
        startButton.SetActive(true);
        lobbyStageSelectButton.SetActive(true);
        mainScrollView.SetActive(false);
    }
}
