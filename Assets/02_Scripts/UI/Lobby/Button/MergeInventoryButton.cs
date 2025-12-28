using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeInventoryButton : MonoBehaviour
{
    [SerializeField] GameObject startButton;
    [SerializeField] GameObject lobbyStageSelectButton;
    [SerializeField] GameObject mainScrollView;
    

    public void OnClickButton()
    {
        startButton.SetActive(false);
        lobbyStageSelectButton.SetActive(false);
        mainScrollView.SetActive(true);        
    }
}
